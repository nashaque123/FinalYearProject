using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    public BooleanScriptableObject GamePlaying;
    private GameObject _pauseMenuUI;
    private Image[] _menuButtons = new Image[3];
    private int _currentButton = 0;
    private bool _scrollingThroughMenu = false;
    private GameObject _resultText;
    private GameObject _ball;
    public BooleanScriptableObject BallInMotion;

    // Start is called before the first frame update
    void Start()
    {
        GamePlaying.Value = true;
        _pauseMenuUI = GameObject.Find("Pause Menu");
        _menuButtons[0] = GameObject.Find("Resume Button").GetComponent<Image>();
        _menuButtons[1] = GameObject.Find("Restart Button").GetComponent<Image>();
        _menuButtons[2] = GameObject.Find("Menu Button").GetComponent<Image>();
        _resultText = GameObject.Find("Result Text");

        _pauseMenuUI.SetActive(false);
        _resultText.SetActive(false);
        _menuButtons[0].color = Color.magenta;
        _menuButtons[1].color = Color.white;
        _menuButtons[2].color = Color.white;

        _ball = GameObject.Find("Ball");
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

        if (!GamePlaying.Value && !_scrollingThroughMenu)
        {
            ProcessPauseMenuInput();
        }
    }

    private void ProcessPauseMenuInput()
    {
        if (Input.GetAxis("Vertical") >= 0.7f || Input.GetKeyDown(KeyCode.UpArrow))
        {
            StartCoroutine(PreviousButton());
        }
        else if (Input.GetAxis("Vertical") <= -0.7f || Input.GetKeyDown(KeyCode.DownArrow))
        {
            StartCoroutine(NextButton());
        }
        else if (Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.Return))
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
                //back to menu
                Debug.Log("back to menu");
            }
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
        _menuButtons[_currentButton].color = Color.white;
        _currentButton++;
        _currentButton %= _menuButtons.Length;
        _menuButtons[_currentButton].color = Color.magenta;
        yield return new WaitForSeconds(0.2f);
        _scrollingThroughMenu = false;
    }

    private IEnumerator PreviousButton()
    {
        _scrollingThroughMenu = true;
        _menuButtons[_currentButton].color = Color.white;
        _currentButton--;

        if (_currentButton < 0)
            _currentButton = _menuButtons.Length - 1;

        _menuButtons[_currentButton].color = Color.magenta;
        yield return new WaitForSeconds(0.2f);
        _scrollingThroughMenu = false;
    }

    public void GameOver(bool didScore)
    {
        _resultText.GetComponent<Text>().text = didScore ? "You scored!!" : "You missed :(";
        _resultText.SetActive(true);
        PauseGame();
    }
}
