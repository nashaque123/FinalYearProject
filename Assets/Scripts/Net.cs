using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Net : MonoBehaviour
{
    private Bounds _bounds;

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
                //minx,maxy,maxz - minx,miny,maxz, minx,miny,minz - minx,miny,maxz
                {
                    Vector3 basePoint = new Vector3(_bounds.min.x, _bounds.min.y, _bounds.max.z);
                    Vector3 a = new Vector3(_bounds.min.x, _bounds.max.y, _bounds.max.z);
                    Vector3 b = new Vector3(_bounds.min.x, _bounds.min.y, _bounds.min.z);
                    return GetCrossProductFromVectors(basePoint, a, b);
                }
            case "max0":
                //maxx,miny,maxz - maxx,miny,minz, maxx,maxy,minz - maxx,miny,minz
                {
                    Vector3 basePoint = new Vector3(_bounds.max.x, _bounds.min.y, _bounds.min.z);
                    Vector3 a = new Vector3(_bounds.max.x, _bounds.min.y, _bounds.max.z);
                    Vector3 b = new Vector3(_bounds.max.x, _bounds.max.y, _bounds.min.z);
                    return GetCrossProductFromVectors(basePoint, a, b);
                }
            case "min1":
                //maxx,miny,minz - minx,miny,minz, minx,miny,maxz - minx,miny,minz
                {
                    Vector3 basePoint = new Vector3(_bounds.min.x, _bounds.min.y, _bounds.min.z);
                    Vector3 a = new Vector3(_bounds.max.x, _bounds.min.y, _bounds.min.z);
                    Vector3 b = new Vector3(_bounds.min.x, _bounds.min.y, _bounds.max.z);
                    return GetCrossProductFromVectors(basePoint, a, b);
                }
            case "max1":
                //maxx,maxy,maxz - maxx,maxy,minz, minx,maxy,minz - maxx,maxy,minz
                {
                    Vector3 basePoint = new Vector3(_bounds.max.x, _bounds.max.y, _bounds.min.z);
                    Vector3 a = new Vector3(_bounds.max.x, _bounds.max.y, _bounds.max.z);
                    Vector3 b = new Vector3(_bounds.min.x, _bounds.max.y, _bounds.min.z);
                    return GetCrossProductFromVectors(basePoint, a, b);
                }
            case "min2":
                //minx,miny,minz - minx,maxy,minz, maxx,maxy,minz - minx,maxy,minz
                {
                    Vector3 basePoint = new Vector3(_bounds.min.x, _bounds.max.y, _bounds.min.z);
                    Vector3 a = new Vector3(_bounds.min.x, _bounds.min.y, _bounds.min.z);
                    Vector3 b = new Vector3(_bounds.max.x, _bounds.max.y, _bounds.min.z);
                    return GetCrossProductFromVectors(basePoint, a, b);
                }
            case "max2":
                //maxx,miny,maxz - maxx,maxy,maxz, minx,maxy,maxz - maxx,maxy,maxz
                {
                    Vector3 basePoint = new Vector3(_bounds.max.x, _bounds.max.y, _bounds.max.z);
                    Vector3 a = new Vector3(_bounds.max.x, _bounds.min.y, _bounds.max.z);
                    Vector3 b = new Vector3(_bounds.min.x, _bounds.max.y, _bounds.max.z);
                    return GetCrossProductFromVectors(basePoint, a, b);
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
        Debug.Log("collision point " + collisionPoint);
        for (int i = 0; i < 3; i++)
        {
            Debug.Log("max " + i + " " + _bounds.max[i]);
            Debug.Log("min " + i + " " + _bounds.min[i]);
            if (collisionPoint[i].Equals(_bounds.max[i]))
            {
                return "max" + i;
            }

            if (collisionPoint[i].Equals(_bounds.min[i]))
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
