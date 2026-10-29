using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public NotesManager[] notesManagers = new NotesManager[2];
    public PlayerInput[] playerInputs = new PlayerInput[2];

    [SerializeField] GameObject titleUI;
    [SerializeField] GameObject[] titlePlayerUIs;
    TextMeshProUGUI[] inputTexts = new TextMeshProUGUI[2];
    TextMeshProUGUI[] instructionTexts = new TextMeshProUGUI[2];

    [SerializeField] GameObject gameUI;
    TextMeshProUGUI startCountDownText;
    TextMeshProUGUI gameTimerText;

    [SerializeField] GameObject resultUI;
    TextMeshProUGUI[] resultRoundsTexts = new TextMeshProUGUI[2];
    TextMeshProUGUI[] resultCorrectTexts = new TextMeshProUGUI[2];
    TextMeshProUGUI[] resultMissesTexts = new TextMeshProUGUI[2];
    TextMeshProUGUI[] resultScoreTexts = new TextMeshProUGUI[2];

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
    void Start()
    {
        Instance = this;
        Init(); 
        //SetUpController();
        readyFlags[0] = true;
        readyFlags[1] = true;
    }

    void Update()
    {
        //if (!gameFlag) SetUpController();
        NotesManagerSetUp();
        GameStart();
        if (readyFlags[0] && readyFlags[1])
        {
            if (readyTimer > readyTime)
            {
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
                SceneManager.LoadScene("2PMatchScene");
            }
        }
    }

    void Init()
    {
        //inputTexts[0] = titlePlayerUIs[0].transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        //inputTexts[1] = titlePlayerUIs[1].transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        //instructionTexts[0] = titlePlayerUIs[0].transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        //instructionTexts[1] = titlePlayerUIs[1].transform.GetChild(2).GetComponent<TextMeshProUGUI>();
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

    void SetUpController()
    {
        var gamepads = Gamepad.all;
        if (gamepads.Count == 0)
        {
            inputTexts[0].text = $"Input : Keyboard\n(WASD)";
            instructionTexts[0].text = $"pless W";
            inputTexts[1].text = $"Input : Keyboard\n(Arrows)";
            instructionTexts[1].text = $"pless UpArrow";
        }
        else if (gamepads.Count == 1)
        {
            inputTexts[0].text = $"Input : Controller\n(ABXY)";
            instructionTexts[0].text = $"pless X";
            inputTexts[1].text = $"Input : Keyboard\n(WASD)";
            instructionTexts[1].text = $"pless W";
        }
        else if (gamepads.Count == 2)
        {
            inputTexts[0].text = $"Input : Controller\n(ABXY)";
            instructionTexts[0].text = $"pless X";
            inputTexts[1].text = $"Input : Controller\n(ABXY)";
            instructionTexts[1].text = $"pless X";
        }

        if (readyFlags[0]) instructionTexts[0].color = Color.green;
        else instructionTexts[0].color = Color.white;
        if (readyFlags[1]) instructionTexts[1].color = Color.green;
        else instructionTexts[1].color = Color.white;


        // コントローラーの接続なし
        //if (gamepads.Count == 0)
        //{
        //    notesManagers[0].playerNumber = 0;
        //    notesManagers[1].playerNumber = 1;



        //    if (readyFlags[0]) instructionTexts[0].color = Color.green;
        //    else instructionTexts[0].color = Color.white;
        //    if (readyFlags[1]) instructionTexts[1].color = Color.green;
        //    else instructionTexts[1].color = Color.white;
        //}
        //// 　コントローラー1つ目
        //if (gamepads.Count >= 1)
        //{
        //    notesManagers[0].playerNumber = 0;
        //    notesManagers[0].gamepad = gamepads[0];
        //    InputUser.PerformPairingWithDevice(gamepads[0], playerInputs[0].user);
        //    notesManagers[1].playerNumber = 1;
        //    InputUser.PerformPairingWithDevice(Keyboard.current, playerInputs[1].user);



        //    if (readyFlags[0]) instructionTexts[0].color = Color.green;
        //    else instructionTexts[0].color = Color.white;
        //    if (readyFlags[1]) instructionTexts[1].color = Color.green;
        //    else instructionTexts[1].color = Color.white;
        //}
        //// 　コントローラー2つ接続の場合
        //if (gamepads.Count >= 2)
        //{
        //    notesManagers[0].playerNumber = 0;
        //    notesManagers[0].gamepad = gamepads[0];
        //    playerInputs[0].user.UnpairDevices();
        //    InputUser.PerformPairingWithDevice(gamepads[0], playerInputs[0].user);
        //    notesManagers[1].playerNumber = 1;
        //    notesManagers[1].gamepad = gamepads[1];
        //    playerInputs[1].user.UnpairDevices();
        //    InputUser.PerformPairingWithDevice(gamepads[1], playerInputs[1].user);

        //    inputTexts[0].text = $"Input : Controller\n(ABXY)";
        //    instructionTexts[0].text = $"pless X"; 
        //    inputTexts[1].text = $"Input : Controller\n(ABXY)";
        //    instructionTexts[1].text = $"pless X";

        //    if (readyFlags[0]) instructionTexts[0].color = Color.green;
        //    else instructionTexts[0].color = Color.white;
        //    if (readyFlags[1]) instructionTexts[1].color = Color.green;
        //    else instructionTexts[1].color = Color.white;
        //}
    }

    void NotesManagerSetUp()
    {
        if (setupFlag && startFlag)
        {
            //gameFlag = true;
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
                    startCountDownText.gameObject.SetActive(true);
                }
                startTimer += Time.deltaTime;
                if (startTimer < 1) startCountDownText.text = $"3";
                else if (startTimer < 2) startCountDownText.text = "2";
                else if (startTimer < 3) startCountDownText.text = "1";
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
        }
        else
        {
            gameTimer += Time.deltaTime;
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
}
