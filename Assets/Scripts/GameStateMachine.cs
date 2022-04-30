using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    eReadyToShoot,
    eShotTaken,
    eBallInMotion,
    ePaused,
    eShotFinished
}

[CreateAssetMenu(fileName = "GameStateMachine", menuName = "GameStateMachine")]
public class GameStateMachine : ScriptableObject
{
    public GameState Value;
}
