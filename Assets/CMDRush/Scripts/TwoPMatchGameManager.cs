using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TwoPMatchGameManager : MonoBehaviour
{
    public static TwoPMatchGameManager Instance;

    public NotesManager[] notesManagers = new NotesManager[2];
    public PlayerInput[] playerInputs = new PlayerInput[2];

    [SerializeField] TextMeshProUGUI[] readyTexts = new TextMeshProUGUI[2];
    [SerializeField] TextMeshProUGUI[] okTexts = new TextMeshProUGUI[2];

    [SerializeField] GameObject gameUI;
    TextMeshProUGUI startCountDownText;
    TextMeshProUGUI gameTimerText;

    [SerializeField] GameObject resultUI;
    TextMeshProUGUI[] resultRoundsTexts = new TextMeshProUGUI[2];
    TextMeshProUGUI[] resultCorrectTexts = new TextMeshProUGUI[2];
    TextMeshProUGUI[] resultMissesTexts = new TextMeshProUGUI[2];
    TextMeshProUGUI[] resultScoreTexts = new TextMeshProUGUI[2];

    public bool setUpPracticeNotesFlag;
    public bool startFlag;
    public bool[] readyFlags = new bool[2];
    [SerializeField] float readyTime;
    float readyTimer;
    [SerializeField] float startTime;
    float startTimer;

    public bool gameFlag;
    [SerializeField] float gameLimitMinute;
    int gameMinute;
    float gameTimer;

    public bool setupFlag;

    Action getFade;
    Action moveScene;
    void Start()
    {
        Instance = this;
        Init();
        setUpPracticeNotesFlag = true;
        readyTexts[0].gameObject.SetActive(true);
        readyTexts[1].gameObject.SetActive(true);

        FadeManager.Instance.fade.StartFadeOut(1.0f);

        getFade += FadeGet;
        moveScene += SceneMove;

        StartCoroutine(GetFadeManager());
    }

    void Update()
    {
        NotesManagerSetUp();
        GameStart();

        for (int i = 0; i < 2; i++)
        {
            if (readyFlags[i])
            {
                readyTexts[i].gameObject.SetActive(false);
                okTexts[i].gameObject.SetActive(true);
            }
        }

        if (readyFlags[0] && readyFlags[1])
        {
            if (readyTimer > readyTime)
            {
                setUpPracticeNotesFlag = false;
                okTexts[0].gameObject.SetActive(false);
                okTexts[1].gameObject.SetActive(false);
                readyTimer = 0f;
                readyFlags[0] = false;
                readyFlags[1] = false;
                startFlag = true;
            }
            else readyTimer += Time.deltaTime;
        }

        if (gameFlag) GameTime();
        else
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                FadeManager.Instance.fade.StartFadeIn(1.0f, moveScene);
            }
        }
    }

    void Init()
    {
        resultRoundsTexts[0] = resultUI.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        resultRoundsTexts[1] = resultUI.transform.GetChild(0).GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
        resultCorrectTexts[0] = resultUI.transform.GetChild(0).GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>();
        resultCorrectTexts[1] = resultUI.transform.GetChild(0).GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>();
        resultMissesTexts[0] = resultUI.transform.GetChild(0).GetChild(0).GetChild(3).GetComponent<TextMeshProUGUI>();
        resultMissesTexts[1] = resultUI.transform.GetChild(0).GetChild(1).GetChild(3).GetComponent<TextMeshProUGUI>();
        resultScoreTexts[0] = resultUI.transform.GetChild(0).GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>();
        resultScoreTexts[1] = resultUI.transform.GetChild(0).GetChild(1).GetChild(4).GetComponent<TextMeshProUGUI>();   

        startCountDownText = gameUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        gameTimerText = gameUI.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        gameTimerText.text = $"01:00";
    }

    IEnumerator GetFadeManager()
    {
        bool end = false;
        while (end)
        {
            if (FadeManager.Instance != null) end = true;
            else yield return null;
        }
        getFade();
    }

    void NotesManagerSetUp()
    {
        if (setupFlag && startFlag)
        {
            setupFlag = false;
        }
    }

    void GameStart()
    {
        if (startFlag)
        {
            if (startTimer > startTime)
            {
                startCountDownText.gameObject.SetActive(false);
                gameFlag = true;               
                startTimer = 0f;
                startFlag = false;
            }
            else
            {
                if (startTimer == 0f)
                {
                    //titleUI.SetActive(false);
                    notesManagers[0].StartNotesSetup();
                    notesManagers[1].StartNotesSetup();
                    startCountDownText.gameObject.SetActive(true);

                    // Œø‰Ê‰¹
                    AudioManager.Instance.PlaySE(9);
                }
                startTimer += Time.deltaTime;
                if (startTimer < 1) startCountDownText.text = $"3";
                else if (startTimer < 2) startCountDownText.text = "2";
                else if (startTimer < 3) startCountDownText.text = "1";
                else if (startTimer < 4) startCountDownText.text = "GO";

                Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * 5f;
                startCountDownText.transform.localPosition = Vector3.zero + new Vector3(randomOffset.x, randomOffset.y, 0f);
            }
        }
    }

    void GameTime()
    {
        if (gameMinute == gameLimitMinute)
        {
            gameTimerText.text = $"00:00";
            gameMinute = 0;
            gameTimer = 0f;
            gameFlag = false;
            notesManagers[0].ControllerStop();
            notesManagers[1].ControllerStop();
            Result();

            AudioManager.Instance.PlaySE(11);
        }
        else
        {
            gameTimer += Time.deltaTime;
            // Œø‰Ê‰¹
            if (gameLimitMinute - 1 == gameMinute && gameTimer > 55f) if (!AudioManager.Instance.sources[10].isPlaying) AudioManager.Instance.PlaySE(10);

            if (gameTimer > 60f)
            {
                gameTimer = 0f;
                gameMinute++;
            }
            float s = 60f - gameTimer;
            if (s > 59f) s = 59f;
            int m = (int)gameLimitMinute - gameMinute;
            gameTimerText.text = $"{0}:{s.ToString("00")}";
        }
    }

    void Result()
    {
        resultUI.SetActive(true);
        resultRoundsTexts[0].text = $"{notesManagers[0].roundCount}";
        resultRoundsTexts[1].text = $"{notesManagers[1].roundCount}";
        resultCorrectTexts[0].text = $"{notesManagers[0].correctCount}";
        resultCorrectTexts[1].text = $"{notesManagers[1].correctCount}";
        resultMissesTexts[0].text = $"{notesManagers[0].missCount}";
        resultMissesTexts[1].text = $"{notesManagers[1].missCount}";
        resultScoreTexts[0].text = $"{notesManagers[0].score}";
        resultScoreTexts[1].text = $"{notesManagers[1].score}";
    }

    void FadeGet()
    {
        FadeManager.Instance.fade.StartFadeOut(1.0f);
    }

    void SceneMove()
    {
        SceneManager.LoadScene("2PMatchScene");
    }
}
