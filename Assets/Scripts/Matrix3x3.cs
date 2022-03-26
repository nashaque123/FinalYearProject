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
}
