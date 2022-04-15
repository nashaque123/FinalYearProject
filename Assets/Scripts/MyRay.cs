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