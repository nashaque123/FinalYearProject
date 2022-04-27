using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Net : MonoBehaviour
{
    private Bounds _bounds;
    private readonly float kRoomForError = 0.0001f;

    // Start is called before the first frame update
    void Start()
    {
        _bounds = gameObject.GetComponent<Renderer>().bounds;
    }

    public Vector3 GetNormalOfCollisionFace(Vector3 collisionPoint)
    {
        string collisionFace = GetFace(collisionPoint);
        switch (collisionFace)
        {
            case "min0":
                //minx,miny,minz - minx,miny,maxz, minx,maxy,maxz - minx,miny,maxz
                {
                    Vector3 basePoint = new Vector3(_bounds.min.x, _bounds.min.y, _bounds.max.z);
                    Vector3 a = new Vector3(_bounds.min.x, _bounds.min.y, _bounds.min.z);
                    Vector3 b = new Vector3(_bounds.min.x, _bounds.max.y, _bounds.max.z);
                    Vector3 cross = GetCrossProductFromVectors(basePoint, a, b);
                    return cross / MyMathsFunctions.CalculateVectorMagnitude(cross);
                }
            case "max0":
                //maxx,maxy,minz - maxx,miny,minz, maxx,miny,maxz - maxx,miny,minz
                {
                    Vector3 basePoint = new Vector3(_bounds.max.x, _bounds.min.y, _bounds.min.z);
                    Vector3 a = new Vector3(_bounds.max.x, _bounds.max.y, _bounds.min.z);
                    Vector3 b = new Vector3(_bounds.max.x, _bounds.min.y, _bounds.max.z);
                    Vector3 cross = GetCrossProductFromVectors(basePoint, a, b);
                    return cross / MyMathsFunctions.CalculateVectorMagnitude(cross);
                }
            case "min1":
                //minx,miny,maxz - minx,miny,minz, maxx,miny,minz - minx,miny,minz
                {
                    Vector3 basePoint = new Vector3(_bounds.min.x, _bounds.min.y, _bounds.min.z);
                    Vector3 a = new Vector3(_bounds.min.x, _bounds.min.y, _bounds.max.z);
                    Vector3 b = new Vector3(_bounds.max.x, _bounds.min.y, _bounds.min.z);
                    Vector3 cross = GetCrossProductFromVectors(basePoint, a, b);
                    return cross / MyMathsFunctions.CalculateVectorMagnitude(cross);
                }
            case "max1":
                //minx,maxy,minz - maxx,maxy,minz, maxx,maxy,maxz - maxx,maxy,minz
                {
                    Vector3 basePoint = new Vector3(_bounds.max.x, _bounds.max.y, _bounds.min.z);
                    Vector3 a = new Vector3(_bounds.min.x, _bounds.max.y, _bounds.min.z);
                    Vector3 b = new Vector3(_bounds.max.x, _bounds.max.y, _bounds.max.z);
                    Vector3 cross = GetCrossProductFromVectors(basePoint, a, b);
                    return cross / MyMathsFunctions.CalculateVectorMagnitude(cross);
                }
            case "min2":
                //maxx,maxy,minz - minx,maxy,minz, minx,miny,minz - minx,maxy,minz
                {
                    Vector3 basePoint = new Vector3(_bounds.min.x, _bounds.max.y, _bounds.min.z);
                    Vector3 a = new Vector3(_bounds.max.x, _bounds.max.y, _bounds.min.z);
                    Vector3 b = new Vector3(_bounds.min.x, _bounds.min.y, _bounds.min.z);
                    Vector3 cross = GetCrossProductFromVectors(basePoint, a, b);
                    return cross / MyMathsFunctions.CalculateVectorMagnitude(cross);
                }
            case "max2":
                //minx,maxy,maxz - maxx,maxy,maxz, maxx,miny,maxz - maxx,maxy,maxz
                {
                    Vector3 basePoint = new Vector3(_bounds.max.x, _bounds.max.y, _bounds.max.z);
                    Vector3 a = new Vector3(_bounds.min.x, _bounds.max.y, _bounds.max.z);
                    Vector3 b = new Vector3(_bounds.max.x, _bounds.min.y, _bounds.max.z);
                    Vector3 cross = GetCrossProductFromVectors(basePoint, a, b);
                    return cross / MyMathsFunctions.CalculateVectorMagnitude(cross);
                }
            default:
                return new Vector3(-1f, -1f, -1f);
        }
    }

    private Vector3 GetCrossProductFromVectors(Vector3 basePoint, Vector3 a, Vector3 b)
    {
        return Vector3.Cross(a - basePoint, b - basePoint);
    }

    private string GetFace(Vector3 collisionPoint)
    {
        for (int i = 0; i < 3; i++)
        {
            if (collisionPoint[i] <= _bounds.max[i] + kRoomForError
                && collisionPoint[i] >= _bounds.max[i] - kRoomForError)
            {
                return "max" + i;
            }

            if (collisionPoint[i] <= _bounds.min[i] + kRoomForError
                && collisionPoint[i] >= _bounds.min[i] - kRoomForError)
            {
                return "min" + i;
            }
        }

        return "null";
    }

    public Bounds Bounds
    {
        get
        {
            return _bounds;
        }
    }
}
