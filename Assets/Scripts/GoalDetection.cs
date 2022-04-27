using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalDetection : MonoBehaviour
{
    public Ball Ball;
    public BoxCollider TargetGoalCollider;
    private Bounds bounds;
    public BooleanScriptableObject BallInMotion;
    private bool _goalScored = false;

    // Start is called before the first frame update
    void Start()
    {
        bounds = TargetGoalCollider.bounds;
    }

    // Update is called once per frame
    void Update()
    {
        if (BallInMotion.Value && !_goalScored)
        {
            if (HasBallCrossedLine())
            {
                //goooooooooaaaaaaaaaaaaaaaalllllllllll
                _goalScored = true;
                StartCoroutine(DisplayResult(true));
            }
            else if (MyMathsFunctions.CalculateVectorMagnitude(Ball.LinearVelocity) < 0.09f)
            {
                StartCoroutine(DisplayResult(false));
            }
        }
    }

    private bool HasBallCrossedLine()
    {
        //check if ball is within bounds and z + radius > goal collider z
        if (Ball.transform.position.x > bounds.min.x && Ball.transform.position.y > bounds.min.y && Ball.transform.position.x < bounds.max.x
            && Ball.transform.position.y < bounds.max.y && Ball.transform.position.z - Ball.Radius > bounds.min.z + 0.01f && Ball.transform.position.z + Ball.Radius < bounds.max.z)
        {
            return true;
        }

        return false;
    }

    public IEnumerator DisplayResult(bool didScore)
    {
        yield return new WaitForSeconds(1f);
        BallInMotion.Value = false;
        _goalScored = false;
        gameObject.GetComponent<Pause>().GameOver(didScore);
    }
}
