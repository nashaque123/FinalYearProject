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

        foreach (Renderer net in _nets)
        {
            if (IsBallCollidingWithNet(net))
            {
                BallToNetResponse(net);
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
        Vector3 vCol = (_ball.LinearVelocity / velocityMagnitudePreCollision) * contactPointMagnitude;
        _ball.transform.position += vCol;

        Vector3 unitVectorPreCollision = _ball.LinearVelocity / velocityMagnitudePreCollision;
        float dotProductOfNormalAndNegativeUnitVector = MyMathsFunctions.CalculateDotProduct(plane.NormalToSurface, -unitVectorPreCollision);
        Vector3 unitVectorPostCollision = (2f * dotProductOfNormalAndNegativeUnitVector * plane.NormalToSurface) + unitVectorPreCollision;
        Vector3 velocityPostCollision = plane.CoefficientOfRestitution * velocityMagnitudePreCollision * unitVectorPostCollision;
        _ball.LinearVelocity = velocityPostCollision;
    }

    private bool IsBallCollidingWithNet(Renderer net)
    {
        Bounds bounds = net.bounds;

        //get closest point of net to ball
        Vector3 closestPoint = new Vector3(Mathf.Max(bounds.min.x, Mathf.Min(_ball.transform.position.x, bounds.max.x)),
                    Mathf.Max(bounds.min.y, Mathf.Min(_ball.transform.position.y, bounds.max.y)),
                    Mathf.Max(bounds.min.z, Mathf.Min(_ball.transform.position.z, bounds.max.z)));



        //check if point is colliding with ball
        Vector3 vectorBetweenClosestPointAndBall = closestPoint - _ball.transform.position;
        float distance = MyMathsFunctions.CalculateVectorMagnitude(vectorBetweenClosestPointAndBall);
        if (net.name.Equals("Net"))
        {
            Debug.Log("test " + closestPoint);
            Debug.Log("test dist " + distance);
        }
        return distance <= _ball.Radius;
    }

    private void BallToNetResponse(Renderer net)
    {
        //ball rebounds off net
        Debug.Log("net " + net.name);
    }

    /*private void ImpulseCalculation()
    {
        Vector3 collisionNormal = _ball.LinearVelocity / MyMathsFunctions.CalculateVectorMagnitude(_ball.LinearVelocity);
        float totalInertia = Vector3.Cross(_ball.InertiaTensor.Inverse * Vector3.Cross(_ball.Radius, collisionNormal)
        float impulse = (-(1f + 0.9f) * Vector3.Dot(_ball.LinearVelocity, collisionNormal) / ((1f / _ball.Mass) + (1f / _ball.Mass) + ));
    }*/
}
