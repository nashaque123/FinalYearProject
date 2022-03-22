using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetectionAndReaction : MonoBehaviour
{
    [SerializeField]
    private Ball _ball;

    [SerializeField]
    private Plane[] _planes;

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

        return (closestDistance - _ball.GetComponent<SphereCollider>().radius) / Mathf.Cos(angleInRadiansBetweenVelocityAndNegativeNormal);
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
}
