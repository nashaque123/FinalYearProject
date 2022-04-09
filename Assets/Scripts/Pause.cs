using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    public BooleanScriptableObject GamePlaying;
    private GameObject _pauseMenuUI;
    private GameObject _shotPlacementMenuUI;
    private Image[] _pauseMenuButtons = new Image[3];
    private Image[] _shotPlacementMenuButtons = new Image[2];
    private List<Image> _activeButtons = new List<Image>();
    private int _currentButton = 0;
    private bool _scrollingThroughMenu = false;
    private GameObject _resultText;
    private GameObject _ball;
    public BooleanScriptableObject BallInMotion;
    private readonly float kMenuSpeedBuffer = 0.15f;
    public GameObject BallPlacementCursorUI;
    public Vector3ScriptableObject BallStartingPosition;
    private Bounds _pitchBounds;

    // Start is called before the first frame update
    void Start()
    {
        GamePlaying.Value = true;
        _pauseMenuUI = GameObject.Find("Pause Menu");
        _shotPlacementMenuUI = GameObject.Find("Shot Placement Menu");
        _pauseMenuButtons[0] = GameObject.Find("Resume Button").GetComponent<Image>();
        _pauseMenuButtons[1] = GameObject.Find("Restart Button").GetComponent<Image>();
        _pauseMenuButtons[2] = GameObject.Find("Shot Placement Button").GetComponent<Image>();
        _shotPlacementMenuButtons[0] = GameObject.Find("Confirm Button").GetComponent<Image>();
        _shotPlacementMenuButtons[1] = GameObject.Find("Discard Button").GetComponent<Image>();
        _resultText = GameObject.Find("Result Text");

        _pauseMenuUI.SetActive(false);
        _shotPlacementMenuUI.SetActive(false);
        _resultText.SetActive(false);
        SetMenuButtonsActive(_pauseMenuButtons);

        _ball = GameObject.Find("Ball");
        _pitchBounds = GameObject.Find("Pitch").GetComponent<MeshRenderer>().bounds;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("joystick button 7") || Input.GetKeyDown("p"))
        {
            if (GamePlaying.Value)
            {
                PauseGame();
            }
            else
            {
                StartCoroutine(ResumeGame());
            }
        }

        if (!GamePlaying.Value)
        {
            if (!_scrollingThroughMenu)
            {
                ProcessMenuInput();
            }

            ProcessMenuSelectInput();
        }
    }

    private void ProcessMenuInput()
    {
        if (Input.GetAxis("Vertical") >= 0.7f || Input.GetKeyDown(KeyCode.UpArrow))
        {
            StartCoroutine(PreviousButton());
        }
        else if (Input.GetAxis("Vertical") <= -0.7f || Input.GetKeyDown(KeyCode.DownArrow))
        {
            StartCoroutine(NextButton());
        }
    }

    private void ProcessMenuSelectInput()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.Return))
        {
            if (_pauseMenuUI.activeInHierarchy)
            {
                ProcessPauseMenuSelectInput();
            }
            else
            {
                ProcessShotPlacementMenuSelectInput();
            }
        }
    }

    private void ProcessPauseMenuSelectInput()
    {
        if (_currentButton == 0)
        {
            StartCoroutine(ResumeGame());
        }
        else if (_currentButton == 1)
        {
            RestartGame();
        }
        else if (_currentButton == 2)
        {
            //go to place ball menu
            _pauseMenuUI.SetActive(false);
            _shotPlacementMenuUI.SetActive(true);
            SetMenuButtonsActive(_shotPlacementMenuButtons);
        }
    }

    private void ProcessShotPlacementMenuSelectInput()
    {
        _shotPlacementMenuUI.SetActive(false);
        if (_currentButton == 0)
        {
            //place ball where cursor is
            BallStartingPosition.Value = CalculateBallPositionFromCursor();
            RestartGame();
        }
        else if (_currentButton == 1)
        {
            _pauseMenuUI.SetActive(true);
            SetMenuButtonsActive(_pauseMenuButtons);
        }
    }

    private IEnumerator ResumeGame()
    {
        _pauseMenuUI.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        GamePlaying.Value = true;
    }

    private void PauseGame()
    {
        _pauseMenuUI.SetActive(true);
        SetMenuButtonsActive(_pauseMenuButtons);
        GamePlaying.Value = false;
    }

    private void RestartGame()
    {
        BallInMotion.Value = false;
        _ball.GetComponent<AdamsMoultonSolver>().Reset();
        _ball.GetComponent<TrailRenderer>().Clear();
        gameObject.GetComponent<TakeShot>().Reset();
        _resultText.SetActive(false);
        StartCoroutine(ResumeGame());
    }

    private IEnumerator NextButton()
    {
        _scrollingThroughMenu = true;
        _activeButtons[_currentButton].color = Color.white;
        _currentButton++;
        _currentButton %= _activeButtons.Count;
        _activeButtons[_currentButton].color = Color.magenta;
        yield return new WaitForSeconds(kMenuSpeedBuffer);
        _scrollingThroughMenu = false;
    }

    private IEnumerator PreviousButton()
    {
        _scrollingThroughMenu = true;
        _activeButtons[_currentButton].color = Color.white;
        _currentButton--;

        if (_currentButton < 0)
            _currentButton = _activeButtons.Count - 1;

        _activeButtons[_currentButton].color = Color.magenta;
        yield return new WaitForSeconds(kMenuSpeedBuffer);
        _scrollingThroughMenu = false;
    }

    public void GameOver(bool didScore)
    {
        _resultText.GetComponent<Text>().text = didScore ? "You scored!!" : "You missed :(";
        _resultText.SetActive(true);
        PauseGame();
    }

    private void SetMenuButtonsActive(Image[] menuButtons)
    {
        _activeButtons.Clear();
        _currentButton = 0;

        foreach (Image button in menuButtons)
        {
            button.color = Color.white;
            _activeButtons.Add(button);
        }

        _activeButtons[0].color = Color.magenta;
    }

    private Vector3 CalculateBallPositionFromCursor()
    {
        Rect parentRect = BallPlacementCursorUI.transform.parent.GetComponent<RectTransform>().rect;

        //pitch size / pitch image size = scale
        float horizontalScale = _pitchBounds.size.x / parentRect.width;
        float verticalScale = _pitchBounds.size.z / 2f / parentRect.height;

        float xPos = BallPlacementCursorUI.transform.localPosition.x * horizontalScale;
        //add quarter of pitch size for z as image is only half the pitch
        float zPos = (BallPlacementCursorUI.transform.localPosition.y * verticalScale) + (_pitchBounds.size.z / 4);

        return new Vector3(xPos, BallStartingPosition.Value.y, zPos);
    }
}
