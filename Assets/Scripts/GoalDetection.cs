using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalDetection : MonoBehaviour
{
    public Ball Ball;
    public BoxCollider TargetGoalCollider;
    private Bounds bounds;
    public BooleanScriptableObject BallInMotion;

    // Start is called before the first frame update
    void Start()
    {
        bounds = TargetGoalCollider.bounds;
    }

    // Update is called once per frame
    void Update()
    {
        if (BallInMotion && HasBallCrossedLine())
        {
            //goooooooooaaaaaaaaaaaaaaaalllllllllll
            Debug.Log("goal scored");
        }
    }

    private bool HasBallCrossedLine()
    {
        //check if ball is within bounds and z + radius > goal collider z
        if (Ball.transform.position.x > bounds.min.x && Ball.transform.position.y > bounds.min.y && Ball.transform.position.x < bounds.max.x
            && Ball.transform.position.y < bounds.max.y && Ball.transform.position.z + Ball.Radius > bounds.max.z)
        {
            return true;
        }

        return false;
    }
}
