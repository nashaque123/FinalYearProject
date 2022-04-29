using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyRay
{
    public MyRay(Vector3 position, Vector3 direction, float ballRadius)
    {
        _position = position;
        _direction = direction;
        kBallRadius = ballRadius;
    }

    private Vector3 _position;
    private Vector3 _direction;
    private readonly float kBallRadius;
    private readonly float kRoomForError = 0.0001f;

    public bool IntersectsWithNet(Bounds bounds, out float collisionPointDistance, out Vector3 collisionPoint)
    {
        Vector3 intersectionPoints = new Vector3
        {
            x = GetClosestIntersectionPointAtAxis(bounds, 0),
            y = GetClosestIntersectionPointAtAxis(bounds, 1),
            z = GetClosestIntersectionPointAtAxis(bounds, 2)
        };

        //get furthest collision point distance to get collision on all axes
        collisionPointDistance = Mathf.Max(intersectionPoints.x, Mathf.Max(intersectionPoints.y, intersectionPoints.z));
        collisionPoint = CalculateCollisionPointOnAxes(collisionPointDistance);

        if (collisionPointDistance < 0f)
        {
            return false;
        }

        return IsCollisionPointOnNet(collisionPoint, bounds);
    }

    //use [0], [1], [2] to access x, y, z components
    private float GetClosestIntersectionPointAtAxis(Bounds bounds, int index)
    {
        //if moving right, check left side of net for collision e.g. o --> |===|
        if (_direction[index] > 0f)
        {
            return (bounds.min[index] - _position[index]) / _direction[index];
        }
        else if (_direction[index] < 0f)
        {
            return (bounds.max[index] - _position[index]) / _direction[index];
        }

        //default to negative value if ray is parallel to net
        return -1;
    }

    private Vector3 CalculateCollisionPointOnAxes(float collisionPointDistance)
    {
        return _position + (_direction * collisionPointDistance);
    }

    private bool IsCollisionPointOnNet(Vector3 collisionPoint, Bounds bounds)
    {
        for (int i = 0; i < 3; i++)
        {
            //add radius of ball in case centre doesn't pass through but edge of ball does
            //give room for error in case of rounding float values issues
            if (collisionPoint[i] + kBallRadius + kRoomForError < bounds.min[i] ||
                collisionPoint[i] - kBallRadius - kRoomForError > bounds.max[i])
            {
                return false;
            }
        }

        return true;
    }

    //following techniques from Real Time Collision Detection by Christer Ericson
    //checks against cylinder section of capsule
    public bool IntersectsWithCapsule(Transform capsule, Vector3 ballVelocity, out float t)
    {
        t = -1f;
        float cylinderRadius = capsule.lossyScale.x;
        Vector3 cylinderTopFacePosition = capsule.position + new Vector3(0, capsule.lossyScale.y, 0);
        Vector3 cylinderBottomFacePosition = capsule.position - new Vector3(0, capsule.lossyScale.y, 0);
        Vector3 vectorFromPlaneToPlane = cylinderTopFacePosition - cylinderBottomFacePosition;
        Vector3 vectorFromRayToBottomFace = _position - cylinderBottomFacePosition;

        float md = MyMathsFunctions.CalculateDotProduct(vectorFromRayToBottomFace, vectorFromPlaneToPlane);
        float nd = MyMathsFunctions.CalculateDotProduct(ballVelocity, vectorFromPlaneToPlane);
        float dd = MyMathsFunctions.CalculateDotProduct(vectorFromPlaneToPlane, vectorFromPlaneToPlane);

        //check if ray misses either end of cylinder
        //lower than bottom face
        if (md < 0f && md + nd < 0f)
        {
            return false;
        }

        //higher than top face
        if (md > dd && md + nd > dd)
        {
            return false;
        }

        float nn = MyMathsFunctions.CalculateDotProduct(ballVelocity, ballVelocity);
        float mn = MyMathsFunctions.CalculateDotProduct(vectorFromRayToBottomFace, ballVelocity);

        //quadratic formula
        float a = (dd * nn) - (nd * nd);
        float factorisedPartOfC = Vector3.Dot(vectorFromRayToBottomFace, vectorFromRayToBottomFace) - (cylinderRadius * cylinderRadius);
        float c = (dd * factorisedPartOfC) - (md * md);

        //check if vectorFromPlaneToPlane and ray direction are parallel
        if (Mathf.Abs(a) < kRoomForError)
        {
            //check if a is inside the cylinder
            if (c > 0f)
            {
                return false;
            }

            //intersects against p
            if (md < 0f)
            {
                t = -mn / nn;
            }
            else if (md > 0f)
            {
                //intersects against q;
                t = (nd - mn) / nn;
            }
            else
            {
                //a is inside cylinder
                t = 0;
            }

            return true;
        }

        float b = (dd * mn) - (nd * md);

        if ((b * b) < (a * c))
        {
            //no values for t
            //ray moving away from cylinder
            return false;
        }

        float t1 = (-b + Mathf.Sqrt((b * b) - (4f * a * c))) / (2f * a);
        float t2 = (-b - Mathf.Sqrt((b * b) - (4f * a * c))) / (2f * a);

        t = Mathf.Min(t1, t2);

        return t >= 0f && t < 1f;
    }

    public Vector3 Position
    {
        get
        {
            return _position;
        }
        set
        {
            _position = value;
        }
    }

    public Vector3 Direction
    {
        get
        {
            return _direction;
        }
        set
        {
            _direction = value;
        }
    }
}