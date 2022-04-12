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
        Vector3 unitVectorPreCollision = _ball.LinearVelocity / velocityMagnitudePreCollision;
        Vector3 vCol = unitVectorPreCollision * contactPointMagnitude;
        _ball.transform.position += vCol;

        float dotProductOfNormalAndNegativeUnitVector = MyMathsFunctions.CalculateDotProduct(plane.NormalToSurface, -unitVectorPreCollision);
        Vector3 unitVectorPostCollision = (2f * dotProductOfNormalAndNegativeUnitVector * plane.NormalToSurface) + unitVectorPreCollision;
        Vector3 velocityPostCollision = plane.CoefficientOfRestitution * velocityMagnitudePreCollision * unitVectorPostCollision;
        _ball.LinearVelocity = velocityPostCollision;
    }

    private bool IsBallCollidingWithNet(Renderer net)
    {
        Bounds bounds = net.bounds;
        Vector3 unitVectorBallVelocity = _ball.LinearVelocity / MyMathsFunctions.CalculateVectorMagnitude(_ball.LinearVelocity);
        Ray ray = new Ray(_ball.transform.position, unitVectorBallVelocity);
        
        if (bounds.IntersectRay(ray, out float distance))
        {
            if (distance <= MyMathsFunctions.CalculateVectorMagnitude(_ball.LinearVelocity))
            {
                Vector3 vCol = unitVectorBallVelocity * distance;
                _ball.transform.position += vCol;
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

    /*private void ImpulseCalculation()
    {
        Vector3 collisionNormal = _ball.LinearVelocity / MyMathsFunctions.CalculateVectorMagnitude(_ball.LinearVelocity);
        float totalInertia = Vector3.Cross(_ball.InertiaTensor.Inverse * Vector3.Cross(_ball.Radius, collisionNormal)
        float impulse = (-(1f + 0.9f) * Vector3.Dot(_ball.LinearVelocity, collisionNormal) / ((1f / _ball.Mass) + (1f / _ball.Mass) + ));
    }*/
}
