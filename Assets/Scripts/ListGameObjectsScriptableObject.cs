using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ListGameObjectsScriptableObject", menuName = "ListGameObjectsScriptableObject")]
public class ListGameObjectsScriptableObject : ScriptableObject
{
    public List<GameObject> List;
}

