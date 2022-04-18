using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matrix3x3
{
    private Vector3[] columns = new Vector3[3];

    public Matrix3x3(Vector3 column0, Vector3 column1, Vector3 column2)
    {
        columns[0] = column0;
        columns[1] = column1;
        columns[2] = column2;
    }

    public Vector3[] Matrix
    {
        get
        {
            return columns;
        }
    }

    public static Vector3 operator *(Matrix3x3 matrix, Vector3 vector)
    {
        float x = (matrix.columns[0].x * vector.x) + (matrix.columns[0].y * vector.y) + (matrix.columns[0].z * vector.z);
        float y = (matrix.columns[1].x * vector.x) + (matrix.columns[1].y * vector.y) + (matrix.columns[1].z * vector.z);
        float z = (matrix.columns[2].x * vector.x) + (matrix.columns[2].y * vector.y) + (matrix.columns[2].z * vector.z);

        return new Vector3(x, y, z);
    }
}
