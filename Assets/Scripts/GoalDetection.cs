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
        if (BallInMotion.Value)
        {
            //Debug.Log("vel mag " + MyMathsFunctions.CalculateVectorMagnitude(Ball.LinearVelocity));
            if (HasBallCrossedLine())
            {
                //goooooooooaaaaaaaaaaaaaaaalllllllllll
                StartCoroutine(DisplayResult(true));
            }
            else if (MyMathsFunctions.CalculateVectorMagnitude(Ball.LinearVelocity) < 0.08f)
            {
                StartCoroutine(DisplayResult(false));
            }
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

    public IEnumerator DisplayResult(bool didScore)
    {
        BallInMotion.Value = false;
        yield return new WaitForSeconds(3f);
        Debug.Log(didScore ? "winner!" : "loser!");
    }
}
