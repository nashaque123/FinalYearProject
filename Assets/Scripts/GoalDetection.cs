using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalDetection : MonoBehaviour
{
    public Ball Ball;
    public BoxCollider TargetGoalCollider;
    private Bounds bounds;

    // Start is called before the first frame update
    void Start()
    {
        bounds = TargetGoalCollider.bounds;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private bool HasBallCrossedLine()
    {
        //check if ball is within bounds and z + radius > goal collider z

        return false;
    }
}
