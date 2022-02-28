using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Air", menuName = "Air")]
public class Air : ScriptableObject
{
    public Vector3 Velocity;
    public float Density;
}
