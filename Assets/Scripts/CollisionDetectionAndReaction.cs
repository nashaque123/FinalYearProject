using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetectionAndReaction : MonoBehaviour
{
    [SerializeField]
    private Ball _ball;

    [SerializeField]
    private Plane[] _planes;

    [SerializeField]
    private Net[] _nets;

    [SerializeField]
    private Renderer[] _goalPosts;

    [SerializeField]
    private Renderer _goalkeeper;

    public ListGameObjectsScriptableObject WallList;

    // Update is called once per frame
    void Update()
    {
        float ballVelocityMagnitude = MyMathsFunctions.CalculateVectorMagnitude(_ball.LinearVelocity);

        //if ball is moving, check for collisions against all collidable objects
        if (ballVelocityMagnitude > 0f)
        {
            foreach (GameObject opp in WallList.List)
            {
                if (IsBallCollidingWithNet(opp.GetComponent<MeshRenderer>()))
                {
                    BallToCapsuleResponse();
                }
            }

            if (IsBallCollidingWithNet(_goalkeeper))
            {
                BallToCapsuleResponse();
            }

            foreach (Net net in _nets)
            {
                if (IsBallCollidingWithNet(net.GetComponent<Renderer>()))
                {
                    BallToNetResponse(net);
                }
            }

            foreach (Renderer post in _goalPosts)
            {
                if (IsBallCollidingWithNet(post))
                {
                    BallToCapsuleResponse();
                }
            }

            foreach (Plane plane in _planes)
            {
                if (IsBallMovingTowardsPlane(plane))
                {
                    float contactPointMagnitude = CalculateDistanceFromBallToCollisionPointOnPlane(plane);

                    if (ballVelocityMagnitude >= contactPointMagnitude)
                    {
                        BallToPlaneReaction(plane, contactPointMagnitude);
                    }
                }
            }
        }
    }

    //checks angle is less than 90 degrees
    private bool IsBallMovingTowardsPlane(Plane plane)
    {
        Vector3 negativeVelocity = -_ball.LinearVelocity;
        float angleInRadiansBetweenNormalAndNegativeVelocity = MyMathsFunctions.CalculateAngleInRadiansBetweenVectors(plane.NormalToSurface, negativeVelocity);
        return angleInRadiansBetweenNormalAndNegativeVelocity < 1.5707f;
    }

    private float CalculateClosestDistanceBetweenBallAndPlane(Plane plane)
    {
        Vector3 pointOnPlane = plane.AnyPoint;
        Vector3 vectorFromPointOnPlaneToBall = _ball.transform.position - pointOnPlane;
        float angleInRadiansBetweenNormalAndPointToBallVector = MyMathsFunctions.CalculateAngleInRadiansBetweenVectors(plane.NormalToSurface, vectorFromPointOnPlaneToBall);
        float angleInRadiansBetweenPlaneAndPointToBallVector = 1.5708f - angleInRadiansBetweenNormalAndPointToBallVector;

        return Mathf.Sin(angleInRadiansBetweenPlaneAndPointToBallVector) * MyMathsFunctions.CalculateVectorMagnitude(vectorFromPointOnPlaneToBall);
    }

    private float CalculateDistanceFromBallToCollisionPointOnPlane(Plane plane)
    {
        Vector3 negativeNormal = -plane.NormalToSurface;
        float angleInRadiansBetweenVelocityAndNegativeNormal = MyMathsFunctions.CalculateAngleInRadiansBetweenVectors(_ball.LinearVelocity, negativeNormal);
        float closestDistance = CalculateClosestDistanceBetweenBallAndPlane(plane);

        return (closestDistance - _ball.Radius) / Mathf.Cos(angleInRadiansBetweenVelocityAndNegativeNormal);
    }

    private void BallToPlaneReaction(Plane plane, float contactPointMagnitude)
    {
        //move ball to collision point before setting post collision velocity
        float velocityMagnitudePreCollision = MyMathsFunctions.CalculateVectorMagnitude(_ball.LinearVelocity);
        Vector3 unitVectorPreCollision = _ball.LinearVelocity / velocityMagnitudePreCollision;
        Vector3 vCol = unitVectorPreCollision * contactPointMagnitude;
        _ball.transform.position += vCol;
        float dotProductOfNormalAndNegativeUnitVector = MyMathsFunctions.CalculateDotProduct(plane.NormalToSurface, -unitVectorPreCollision);
        //only change to overall magnitude is due to coefficient of restitution
        Vector3 unitVectorPostCollision = (2f * dotProductOfNormalAndNegativeUnitVector * plane.NormalToSurface) + unitVectorPreCollision;
        Vector3 velocityPostCollision = plane.CoefficientOfRestitution * velocityMagnitudePreCollision * unitVectorPostCollision;
        _ball.LinearVelocity = velocityPostCollision;
    }

    private bool IsBallCollidingWithNet(Renderer net)
    {
        Bounds bounds = net.bounds;
        Vector3 unitVectorBallVelocity = _ball.LinearVelocity / MyMathsFunctions.CalculateVectorMagnitude(_ball.LinearVelocity);
        MyRay ray = new MyRay(_ball.transform.position, unitVectorBallVelocity, _ball.Radius);

        //create raycast to see if ball will collide
        //returns distance to collision
        if (ray.IntersectsWithNet(bounds, out float distance, out Vector3 collisionPoint))
        {
            if (distance - _ball.Radius <= MyMathsFunctions.CalculateVectorMagnitude(_ball.LinearVelocity))
            {
                _ball.transform.position = collisionPoint - (unitVectorBallVelocity * _ball.Radius);
                return true;
            }
        }

        return false;
    }

    //handle as plane response - look into updating with impulse
    private void BallToNetResponse(Net net)
    {
        float velocityMagnitudePreCollision = MyMathsFunctions.CalculateVectorMagnitude(_ball.LinearVelocity);
        Vector3 unitVectorPreCollision = _ball.LinearVelocity / velocityMagnitudePreCollision;
        Vector3 collisionPoint = _ball.transform.position + (unitVectorPreCollision * _ball.Radius);
        float dotProductOfNormalAndNegativeUnitVector = MyMathsFunctions.CalculateDotProduct(net.GetNormalOfCollisionFace(collisionPoint), -unitVectorPreCollision);
        Vector3 unitVectorPostCollision = (2f * dotProductOfNormalAndNegativeUnitVector * net.GetNormalOfCollisionFace(collisionPoint)) + unitVectorPreCollision;
        Vector3 velocityPostCollision = net.CoefficientOfRestitution * velocityMagnitudePreCollision * unitVectorPostCollision;
        _ball.LinearVelocity = velocityPostCollision;
    }

    private void BallToCapsuleResponse()
    {
        float velocityMagnitudePreCollision = MyMathsFunctions.CalculateVectorMagnitude(_ball.LinearVelocity);
        Vector3 unitVectorPreCollision = _ball.LinearVelocity / velocityMagnitudePreCollision;
        Vector3 pos = _ball.transform.position + (_ball.Radius * unitVectorPreCollision);
        ImpulseCalculation(pos);
    }

    //use inertia tensor to calculate total inertia of collision
    private void ImpulseCalculation(Vector3 collisionPoint)
    {
        Vector3 relativePosition = collisionPoint - _ball.transform.position;
        Vector3 collisionNormal = relativePosition / MyMathsFunctions.CalculateVectorMagnitude(relativePosition);
        Vector3 totalInertia = MyMathsFunctions.CalculateCrossProduct(_ball.InertiaTensor.Inverse * MyMathsFunctions.CalculateCrossProduct(relativePosition, collisionNormal), relativePosition);
        float impulse = -(1f + 0.2f) * MyMathsFunctions.CalculateDotProduct(-(_ball.LinearVelocity + MyMathsFunctions.CalculateCrossProduct(_ball.AngularVelocity, relativePosition)), collisionNormal) / ((1f / _ball.Mass) + MyMathsFunctions.CalculateDotProduct(totalInertia, collisionNormal));
        _ball.LinearVelocity += -(impulse * collisionNormal) / _ball.Mass;
        _ball.AngularVelocity += _ball.InertiaTensor.Inverse * MyMathsFunctions.CalculateCrossProduct(relativePosition, -(impulse * collisionNormal));
    }
}
