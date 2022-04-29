using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OppositionController : MonoBehaviour
{
    public Vector3ScriptableObject BallStartingPosition;
    private GameObject _targetGoal;
    private readonly float kWallDistanceToBall = 10f;
    public GameObject OppositionPlayerPrefab;
    private readonly float kDistanceBetweenPlayersInWall = 0.72f;
    public ListGameObjectsScriptableObject WallList;

    // Start is called before the first frame update
    void Start()
    {
        _targetGoal = GameObject.Find("Target Goal");
        GenerateWall(false);
    }

    public void GenerateWall(bool isBallInBox)
    {
        ClearWall();

        //if penalty then no wall required
        if (isBallInBox)
        {
            return;
        }

        //get shot angle to goal
        Vector3 centreOfPitchPosition = new Vector3(0f, BallStartingPosition.Value.y, 0f);
        Vector3 targetGoalPosition = _targetGoal.transform.position;
        targetGoalPosition.y = BallStartingPosition.Value.y;
        Vector3 vectorFromGoalToCentreOfPitch = targetGoalPosition - centreOfPitchPosition;
        Vector3 vectorFromGoalToBall = targetGoalPosition - BallStartingPosition.Value;
        float angleInRadians = MyMathsFunctions.CalculateAngleInRadiansBetweenVectors(vectorFromGoalToCentreOfPitch, vectorFromGoalToBall);
        int wallSize = CalculateNumberOfPlayersInWall(vectorFromGoalToBall, angleInRadians);

        //make wall face ball
        Vector3 perpendicularLine = new Vector3(vectorFromGoalToBall.z, BallStartingPosition.Value.y, -vectorFromGoalToBall.x);
        Vector3 unitVectorPerpendicular = perpendicularLine / MyMathsFunctions.CalculateVectorMagnitude(perpendicularLine);
        Vector3 startingPositionOfWall = CalculateStartingPointOfWall(vectorFromGoalToBall, wallSize, unitVectorPerpendicular);

        for (float i = 0f; i < wallSize; i++)
        {
            Vector3 position = startingPositionOfWall + (kDistanceBetweenPlayersInWall * i * unitVectorPerpendicular);
            //adjust y value for 0.5 * height of prefab
            position.y = 0.8f;
            GameObject obj = Instantiate(OppositionPlayerPrefab, position, Quaternion.identity);
            WallList.List.Add(obj);
        }
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

    private void ClearWall()
    {
        for (int i = WallList.List.Count - 1; i >= 0; i--)
        {
            GameObject obj = WallList.List[i];
            WallList.List.RemoveAt(i);
            Destroy(obj);
        }
    }

    //keep centre of wall in line with ball
    private Vector3 CalculateStartingPointOfWall(Vector3 vectorFromGoalToBall, int wallSize, Vector3 unitVectorPerpendicular)
    {
        Vector3 unitVectorFromGoalToBall = vectorFromGoalToBall / MyMathsFunctions.CalculateVectorMagnitude(vectorFromGoalToBall);
        Vector3 centrePointOfWall = BallStartingPosition.Value + (unitVectorFromGoalToBall * kWallDistanceToBall);
        Vector3 startingPoint = centrePointOfWall - (kDistanceBetweenPlayersInWall * ((wallSize / 2f) - 0.5f) * unitVectorPerpendicular);

        return startingPoint;
    }
}
