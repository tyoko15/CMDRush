using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

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
        SetUpController();
        
    }

    void Update()
    {
        if (!gameFlag) SetUpController();
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
    }

    void Init()
    {
        inputTexts[0] = titlePlayerUIs[0].transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        inputTexts[1] = titlePlayerUIs[1].transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        instructionTexts[0] = titlePlayerUIs[0].transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        instructionTexts[1] = titlePlayerUIs[1].transform.GetChild(2).GetComponent<TextMeshProUGUI>();

        startCountDownText = gameUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        gameTimerText = gameUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        gameTimerText.text = $"03:00";
    }

    void SetUpController()
    {
        var gamepads = Gamepad.all;

        // コントローラーの接続なし
        if (gamepads.Count == 0)
        {
            notesManagers[0].playerNumber = 0;
            notesManagers[0].gamepad = null;
            notesManagers[1].playerNumber = 1;
            notesManagers[1].gamepad = null;

            inputTexts[0].text = $"Input : Keyboard\n(WASD)";
            instructionTexts[0].text = $"pless W";
            inputTexts[1].text = $"Input : Keyboard\n(Arrows)";
            instructionTexts[1].text = $"pless UpArrow";

            if (readyFlags[0]) instructionTexts[0].color = Color.green;
            else instructionTexts[0].color = Color.white;
            if (readyFlags[1]) instructionTexts[1].color = Color.green;
            else instructionTexts[1].color = Color.white;
        }
        // 　コントローラー1つ目
        if (gamepads.Count >= 1)
        {
            notesManagers[0].playerNumber = 0;
            notesManagers[0].gamepad = gamepads[0];
            playerInputs[0].user.UnpairDevices();
            InputUser.PerformPairingWithDevice(gamepads[0], playerInputs[0].user);
            notesManagers[1].playerNumber = 1;
            notesManagers[1].gamepad = null;
            playerInputs[1].user.UnpairDevices();
            InputUser.PerformPairingWithDevice(Keyboard.current, playerInputs[1].user);

            inputTexts[0].text = $"Input : Controller\n(ABXY)";
            instructionTexts[0].text = $"pless X";
            inputTexts[1].text = $"Input : Keyboard\n(WASD)";
            instructionTexts[1].text = $"pless W";
            
            if (readyFlags[0]) instructionTexts[0].color = Color.green;
            else instructionTexts[0].color = Color.white;
            if (readyFlags[1]) instructionTexts[1].color = Color.green;
            else instructionTexts[1].color = Color.white;
        }
        // 　コントローラー2つ接続の場合
        if (gamepads.Count >= 2)
        {
            notesManagers[0].playerNumber = 0;
            notesManagers[0].gamepad = gamepads[0];
            playerInputs[0].user.UnpairDevices();
            InputUser.PerformPairingWithDevice(gamepads[0], playerInputs[0].user);
            notesManagers[1].playerNumber = 1;
            notesManagers[1].gamepad = gamepads[1];
            playerInputs[1].user.UnpairDevices();
            InputUser.PerformPairingWithDevice(gamepads[1], playerInputs[1].user);

            inputTexts[0].text = $"Input : Controller\n(ABXY)";
            instructionTexts[0].text = $"pless X"; 
            inputTexts[1].text = $"Input : Controller\n(ABXY)";
            instructionTexts[1].text = $"pless X";
            
            if (readyFlags[0]) instructionTexts[0].color = Color.green;
            else instructionTexts[0].color = Color.white;
            if (readyFlags[1]) instructionTexts[1].color = Color.green;
            else instructionTexts[1].color = Color.white;
        }
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
                    titleUI.SetActive(false);
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
            int m = 2 - gameMinute;
            gameTimerText.text = $"{m.ToString("00")}:{s.ToString("00")}";
        }
    }
}
