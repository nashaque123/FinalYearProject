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
    private Renderer[] _nets;

    // Update is called once per frame
    void Update()
    {
        float ballVelocityMagnitude = MyMathsFunctions.CalculateVectorMagnitude(_ball.LinearVelocity);

        if (ballVelocityMagnitude > 0f)
        {
            foreach (Renderer net in _nets)
            {
                if (IsBallCollidingWithNet(net))
                {
                    BallToNetResponse(net);
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

        Vector3 pos = _ball.transform.position + (_ball.Radius * unitVectorPreCollision);
        Debug.Log("collision point " + pos);
        ImpulseCalculation(pos, plane);
        /*float dotProductOfNormalAndNegativeUnitVector = MyMathsFunctions.CalculateDotProduct(plane.NormalToSurface, -unitVectorPreCollision);
        Vector3 unitVectorPostCollision = (2f * dotProductOfNormalAndNegativeUnitVector * plane.NormalToSurface) + unitVectorPreCollision;
        Vector3 velocityPostCollision = plane.CoefficientOfRestitution * velocityMagnitudePreCollision * unitVectorPostCollision;
        _ball.LinearVelocity = velocityPostCollision;*/
    }

    private bool IsBallCollidingWithNet(Renderer net)
    {
        Bounds bounds = net.bounds;
        Vector3 unitVectorBallVelocity = _ball.LinearVelocity / MyMathsFunctions.CalculateVectorMagnitude(_ball.LinearVelocity);
        MyRay ray = new MyRay(_ball.transform.position, unitVectorBallVelocity, _ball.Radius);

        if (ray.IntersectsWithNet(bounds, out float distance, out Vector3 collisionPoint))
        {
            if (distance - _ball.Radius <= MyMathsFunctions.CalculateVectorMagnitude(_ball.LinearVelocity))
            {
                _ball.transform.position = collisionPoint;
                return true;
            }
        }

        return false;
    }

    private void BallToNetResponse(Renderer net)
    {
        //ball rebounds off net
        Debug.Log("net " + net.name);
        //TODO: add real response - only for testing
        _ball.LinearVelocity = -_ball.LinearVelocity;
    }

    private void ImpulseCalculation(Vector3 collisionPoint, Plane plane)
    {
        Vector3 relativePosition = collisionPoint - _ball.transform.position;
        Vector3 collisionNormal = relativePosition / MyMathsFunctions.CalculateVectorMagnitude(relativePosition);
        Vector3 totalInertia = Vector3.Cross(_ball.InertiaTensor.Inverse * Vector3.Cross(relativePosition, collisionNormal), relativePosition);
        float impulse = -(1f + plane.CoefficientOfRestitution) * MyMathsFunctions.CalculateDotProduct(-(_ball.LinearVelocity + Vector3.Cross(_ball.AngularVelocity, relativePosition)), collisionNormal) / ((1f / _ball.Mass) + MyMathsFunctions.CalculateDotProduct(totalInertia, collisionNormal));
        Debug.Log("impulse " + impulse);
        Debug.Log("old vel " + _ball.LinearVelocity + ", new vel " + (_ball.LinearVelocity + (-(impulse * collisionNormal) / _ball.Mass)));
        Debug.Log("old ang vel " + _ball.AngularVelocity + ", new ang vel " + (_ball.AngularVelocity + (Vector3.Cross(relativePosition, -(impulse * collisionNormal)) / _ball.InertiaTensor.Inertia)));
        _ball.LinearVelocity += -(impulse * collisionNormal) / _ball.Mass;
        _ball.AngularVelocity += _ball.InertiaTensor.Inverse * Vector3.Cross(relativePosition, -(impulse * collisionNormal));
    }
}
