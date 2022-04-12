using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OppositionController : MonoBehaviour
{
    public Vector3ScriptableObject BallStartingPosition;
    private GameObject _targetGoal;
    private readonly float kWallDistanceToBall = 10f;

    // Start is called before the first frame update
    void Start()
    {
        _targetGoal = GameObject.Find("Target Goal");
    }

    public void GenerateWall(bool isBallInBox)
    {
        //if penalty then no wall required
        if (isBallInBox)
        {
            return;
        }

        Vector3 centreOfPitchPosition = new Vector3(0f, BallStartingPosition.Value.y, 0f);
        Vector3 targetGoalPosition = _targetGoal.transform.position;
        targetGoalPosition.y = BallStartingPosition.Value.y;

        Vector3 vectorFromGoalToCentreOfPitch = targetGoalPosition - centreOfPitchPosition;
        Vector3 vectorFromGoalToBall = targetGoalPosition - BallStartingPosition.Value;

        float angleInRadians = MyMathsFunctions.CalculateAngleInRadiansBetweenVectors(vectorFromGoalToCentreOfPitch, vectorFromGoalToBall);
        int wallSize = CalculateNumberOfPlayersInWall(vectorFromGoalToBall, angleInRadians);
    }

    private int CalculateNumberOfPlayersInWall(Vector3 vectorFromGoalToBall, float angleInRadians)
    {
        float freeKickDistance = MyMathsFunctions.CalculateVectorMagnitude(vectorFromGoalToBall);

        //check angle of free kick to decide how many players are in the wall
        if (angleInRadians >= 0.825f)
        {
            return freeKickDistance <= 30f ? 2 : 1;
        }

        switch (freeKickDistance)
        {
            case float f when f <= 30f:
                return 4;
            case float f when f <= 35f:
                return 3;
            case float f when f <= 45f:
                return 2;
            default:
                return 1;
        }
    }
}
