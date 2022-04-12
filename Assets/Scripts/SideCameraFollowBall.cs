using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideCameraFollowBall : MonoBehaviour
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
            transform.position = new Vector3(transform.position.x, transform.position.y, (_targetGoal.transform.position.z + BallStartingPosition.Value.z) / 2f);
        }
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
