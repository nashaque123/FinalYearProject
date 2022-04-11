using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBallStartingPosition : MonoBehaviour
{
    public Vector3ScriptableObject BallStartingPosition;
    private GameObject _targetGoal;
    private Vector3 _previousPosition;

    // Start is called before the first frame update
    void Start()
    {
        _targetGoal = GameObject.Find("Target Goal");
    }

    // Update is called once per frame
    void Update()
    {
        if (HasStartingPositionChanged())
        {
            SetPositionAndRotation();
        }
    }

    public void SetPositionAndRotation()
    {
        Vector3 centreOfPitchPosition = new Vector3(0f, BallStartingPosition.Value.y, 0f);
        Vector3 targetGoalPosition = _targetGoal.transform.position;
        targetGoalPosition.y = BallStartingPosition.Value.y;

        Vector3 vectorFromGoalToCentreOfPitch = targetGoalPosition - centreOfPitchPosition;
        Vector3 vectorFromGoalToBall = targetGoalPosition - BallStartingPosition.Value;

        float angleInRadians = MyMathsFunctions.CalculateAngleInRadiansBetweenVectors(vectorFromGoalToCentreOfPitch, vectorFromGoalToBall);
        float xPos, yRot;

        if (BallStartingPosition.Value.x >= targetGoalPosition.x)
        {
            xPos = BallStartingPosition.Value.x + (5f * Mathf.Sin(angleInRadians));
            yRot = 360f - (Mathf.Rad2Deg * angleInRadians);
        }
        else
        {
            xPos = BallStartingPosition.Value.x - (5f * Mathf.Sin(angleInRadians));
            yRot = Mathf.Rad2Deg * angleInRadians;
        }

        transform.position = new Vector3(xPos, transform.position.y, BallStartingPosition.Value.z - (5f * Mathf.Cos(angleInRadians)));
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, yRot);
    }

    private bool HasStartingPositionChanged()
    {
        bool valueChanged = false;

        if (!_previousPosition.Equals(BallStartingPosition.Value))
        {
            valueChanged = true;
        }

        _previousPosition = BallStartingPosition.Value;

        return valueChanged;
    }
}
