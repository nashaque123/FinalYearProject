using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetectionAndReaction : MonoBehaviour
{
    [SerializeField]
    private GameObject _ball;

    [SerializeField]
    private Plane[] _planes;

    [Range(0.0f, 1.0f)]
    public float CoefficientOfRestitution;

    // Update is called once per frame
    void Update()
    {
        float ballVelocityMagnitude = MyMathsFunctions.CalculateVectorMagnitude(_ball.GetComponent<AdamsMoultonSolver>().Velocity);

        foreach (Plane plane in _planes)
        {
            float contactPointMagnitude = CalculateDistanceFromBallToCollisionPointOnPlane(plane);

            if (ballVelocityMagnitude >= contactPointMagnitude)
            {
                Debug.Log("collision detected");
            }
        }
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
        float angleInRadiansBetweenVelocityAndNegativeNormal = MyMathsFunctions.CalculateAngleInRadiansBetweenVectors(_ball.GetComponent<AdamsMoultonSolver>().Velocity, negativeNormal);
        float closestDistance = CalculateClosestDistanceBetweenBallAndPlane(plane);

        return (closestDistance - _ball.GetComponent<SphereCollider>().radius) / Mathf.Cos(angleInRadiansBetweenVelocityAndNegativeNormal);
    }
}
