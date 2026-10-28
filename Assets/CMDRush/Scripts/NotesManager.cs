using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class NotesManager : MonoBehaviour
{
    public int playerNumber;
    public Gamepad gamepad;
    [SerializeField] GameObject notesPrefab;
    [SerializeField] Sprite[] notesSprites;
    public Vector3[] positions = new Vector3[5];
    [HideInInspector] List<Notes> notesList = new List<Notes>();

    [SerializeField] GameObject playerUI;
    TextMeshProUGUI roundText;
    TextMeshProUGUI scoreText;
    TextMeshProUGUI correctAnswerRateText;

    int score;
    int notesCount;
    int roundCount;
    int correctAnswerRate;
    [SerializeField] int totalCorrectAnswer;
    [SerializeField] int addQuestion;

    bool setUpNotesFlag;
    int notesSetUpCount;
    bool delayFlag;
    [SerializeField] float delayTime;
    float delayTimer;

    bool addNotesFlag;
    bool addDelayFlag;
    [SerializeField] float addDelayTime;
    float addDelayTimer;

    bool resetNotesFlag;
    bool resetDelayFlag;
    [SerializeField] float resetDelayTime;
    float resetDelayTimer;

    public bool inputAllowedFlag;
    bool commandFlag;
    int selectCommand;

    Image countDownImage;
    float currentAmount;
    [SerializeField] float countDownTime;
    float countDownTimer;

    void Start()
    {
        Init();
        setUpNotesFlag = true;

        roundCount++;
    }

    void Update()
    {
        UpdateUI();
        if (setUpNotesFlag) SetUpNotes();
        if (GameManager.Instance.gameFlag)
        {
            CommandControl();
            if (addNotesFlag) AddNotes();
            if (resetNotesFlag) ResetNotes();
            if (inputAllowedFlag) CountDown();
            if (!setUpNotesFlag == !addNotesFlag && !resetNotesFlag) inputAllowedFlag = true;
        }        
    }

    void Init()
    {
        positions = new Vector3[5];
        for (int i = 0; i < positions.Length; i++) positions[i] = transform.GetChild(0).GetChild(i).position;
        notesList = new List<Notes>();
        countDownImage = playerUI.transform.GetChild(4).GetChild(1).GetComponent<Image>();
        countDownImage.fillAmount = 0;
        roundText = playerUI.transform.GetChild(5).GetChild(1).GetComponent<TextMeshProUGUI>();
        scoreText = playerUI.transform.GetChild(5).GetChild(2).GetComponent<TextMeshProUGUI>();
        correctAnswerRateText = playerUI.transform.GetChild(5).GetChild(3).GetComponent<TextMeshProUGUI>();
    }

    void InstantiateNotes()
    {
        GameObject n = Instantiate(notesPrefab, Vector2.zero, Quaternion.identity);
        NotesData d = new NotesData();
        d.number = 0;
        d.type = (NotesType)Random.Range(0, 4);
        n.GetComponent<Notes>().notesManager = this;
        n.GetComponent<Notes>().data = d;
        n.GetComponent<SpriteRenderer>().sprite = notesSprites[(int)d.type];
        n.transform.SetParent(transform, true);
        n.transform.position = positions[0];
        n.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
        notesList.Add(n.GetComponent<Notes>());
        notesCount++;
    }

    void SetUpNotes()
    {
        if (!delayFlag)
        {
            InstantiateNotes();
            delayFlag = true;
        }
        else
        {
            if (delayTimer > delayTime)
            {
                if (notesSetUpCount != 4)
                {
                    for (int i = 0; i < notesList.Count; i++)
                    {
                        notesList[i].CalculationX();
                        notesList[i].moveFlag = true;
                        notesList[i].data.number++;
                    }
                }
                notesSetUpCount++;
                if (notesSetUpCount == 5)
                {
                    GameManager.Instance.setupFlag = true;
                    notesSetUpCount = 0;
                    setUpNotesFlag = false;
                }
                delayTimer = 0f;
                delayFlag = false;
            }
            else
            {
                delayTimer += Time.deltaTime;

                float amount = Mathf.Lerp(notesSetUpCount * 0.2f, (notesSetUpCount * 0.2f) + 0.2f, delayTimer / delayTime);
                countDownImage.fillAmount = amount;
            }
        }
    }

    void AddNotes()
    {
        if (!addDelayFlag)
        {
            if (notesCount != totalCorrectAnswer) 
            {
                InstantiateNotes(); 
            }
            addNotesFlag = false;
        }
        else
        {
            if (addDelayTimer > addDelayTime)
            {
                for (int i = 0; i < notesList.Count; i++)
                {
                    notesList[i].CalculationX();
                    notesList[i].moveFlag = true;
                    notesList[i].data.number++;
                }
                addDelayTimer = 0f;
                addDelayFlag = false;
            }
            else
            {
                addDelayTimer += Time.deltaTime;
                float amount = Mathf.Lerp(currentAmount, 1f, addDelayTimer / addDelayTime);
                countDownImage.fillAmount = amount;
            }
        }
    }

    void ResetNotes()
    {
        if (!resetDelayFlag)
        {
            if (gamepad != null) gamepad.SetMotorSpeeds(0.1f, 0.1f);
            for (int i = 0; i < notesList.Count; i++)
            {
                notesList[i].failureFlag = true;
            }
            notesList.Clear();
            resetDelayFlag = true;
        }
        else
        {
            if (resetDelayTimer > resetDelayTime)
            {
                if (gamepad != null) gamepad.SetMotorSpeeds(0f, 0f);
                correctAnswerRate = 0;
                notesCount = 0; 
                setUpNotesFlag = true;
                resetNotesFlag = false;
                resetDelayTimer = 0f;
                resetDelayFlag = false;
            }
            else
            {
                resetDelayTimer += Time.deltaTime;
                float amount = Mathf.Lerp(currentAmount, 1f, resetDelayTimer / resetDelayTime);
                countDownImage.fillAmount = amount;
            }
        }
    }

    void CountDown()
    {
        if (countDownTimer > countDownTime)
        {
            currentAmount = 0f;
            countDownTimer = 0f;
            inputAllowedFlag = false;
            resetNotesFlag = true;
        }
        else
        {
            countDownTimer += Time.deltaTime;
            float amount = Mathf.Lerp(1f, 0f, countDownTimer / countDownTime);
            countDownImage.fillAmount = amount;
        }
    }

    void RoundUp()
    {
        totalCorrectAnswer += addQuestion;
        roundCount++;
        score += 1000;
        correctAnswerRate = 0;
        notesCount = 0;
        setUpNotesFlag = true;
    }

    void CommandControl()
    {
        if (commandFlag)
        {
            if (notesList.Count == 0) return;
            if ((int)notesList[0].data.type == selectCommand)
            {
                currentAmount = countDownImage.fillAmount;
                notesList[0].successFlag = true;
                score += 100;
                correctAnswerRate++;
                if (totalCorrectAnswer == correctAnswerRate) RoundUp();
                else
                {
                    addNotesFlag = true;
                    addDelayFlag = true;
                }
                notesList.RemoveAt(0);
            }
            else
            {
                currentAmount = countDownImage.fillAmount;
                score -= 50;
                correctAnswerRate = 0;
                notesCount = 0;
                resetNotesFlag = true;
            }
            countDownTimer = 0;
            commandFlag = false;
            inputAllowedFlag = false;
        }
    }

    void UpdateUI()
    {
        roundText.text = $"Round {roundCount.ToString("00")}";
        scoreText.text = $"Score {score.ToString("000000")}";
        correctAnswerRateText.text = $"{correctAnswerRate.ToString("00")} / {totalCorrectAnswer.ToString("00")}";
    }

    public void SetSelectCommand(int i)
    {
        selectCommand = i;
        commandFlag = true;
    }

    //public void ButtonA(InputAction.CallbackContext context)
    //{
    //    if (gamepad == null)
    //    {
    //        if (context.started && inputAllowedFlag)
    //        {
    //            if (playerNumber == 0)
    //            {
    //                if (context.control.path == "/Keyboard/d")
    //                {
    //                    selectCommand = 0;
    //                    commandFlag = true;
    //                }
    //            }
    //            else if (playerNumber == 1)
    //            {
    //                if (context.control.path == "/Keyboard/rightArrow")
    //                {
    //                    selectCommand = 0;
    //                    commandFlag = true;
    //                }
    //            }
    //        }
    //    }
    //    else
    //    {
    //        if (context.started && inputAllowedFlag)
    //        {
    //            selectCommand = 0;
    //            commandFlag = true;
    //        }
    //    }
    //}
    //public void ButtonB(InputAction.CallbackContext context)
    //{
    //    if (gamepad == null)
    //    {
    //        if (context.started && inputAllowedFlag)
    //        {
    //            if (playerNumber == 0)
    //            {
    //                if (context.control.path == "/Keyboard/s")
    //                {
    //                    selectCommand = 3;
    //                    commandFlag = true;
    //                }
    //            }
    //            else if (playerNumber == 1)
    //            {
    //                if (context.control.path == "/Keyboard/downArrow")
    //                {
    //                    selectCommand = 3;
    //                    commandFlag = true;
    //                }
    //            }
    //        }
    //    }
    //    else
    //    {
    //        if (context.started && inputAllowedFlag)
    //        {
    //            selectCommand = 3;
    //            commandFlag = true;
    //        }
    //    }
    //}
    //public void ButtonX(InputAction.CallbackContext context)
    //{
    //    if (!GameManager.Instance.startFlag && !GameManager.Instance.gameFlag)
    //    {
    //        if (context.started)
    //        {
    //            if (playerNumber == 0)
    //            {
    //                if (context.control.path == "/Keyboard/w")
    //                {
    //                    GameManager.Instance.readyFlags[playerNumber] = true;
    //                }
    //                else if (context.control.path == "/XInputControllerWindows/buttonNorth")
    //                {
    //                    GameManager.Instance.readyFlags[playerNumber] = true;
    //                }
    //            }
    //            else if (playerNumber == 1)
    //            {
    //                if (context.control.path == "/Keyboard/upArrow")
    //                {
    //                    GameManager.Instance.readyFlags[playerNumber] = true;
    //                }
    //                else if (context.control.path == "/XInputControllerWindows/buttonNorth")
    //                {
    //                    GameManager.Instance.readyFlags[playerNumber] = true;
    //                }
    //            }
    //        }
    //        else if (context.canceled)
    //        {

    //            if (playerNumber == 0)
    //            {
    //                if (context.control.path == "/Keyboard/w")
    //                {
    //                    GameManager.Instance.readyFlags[playerNumber] = false;
    //                }
    //                else if (context.control.path == "/XInputControllerWindows/buttonNorth")
    //                {
    //                    GameManager.Instance.readyFlags[playerNumber] = false;
    //                    Debug.Log($"{gameObject.name}");
    //                }
    //            }
    //            else if (playerNumber == 1)
    //            {
    //                if (context.control.path == "/Keyboard/upArrow")
    //                {
    //                    GameManager.Instance.readyFlags[playerNumber] = false;
    //                }
    //                else if (context.control.path == "/XInputControllerWindows/buttonNorth")
    //                {
    //                    GameManager.Instance.readyFlags[playerNumber] = false;
    //                }
    //            }

    //        }            
    //    }
    //    if (gamepad == null)
    //    {
    //        if (context.started && inputAllowedFlag)
    //        {
    //            if (playerNumber == 0)
    //            {
    //                if (context.control.path == "/Keyboard/w")
    //                {
    //                    selectCommand = 2;
    //                    commandFlag = true;
    //                }
    //            }
    //            else if (playerNumber == 1)
    //            {
    //                if (context.control.path == "/Keyboard/upArrow")
    //                {
    //                    selectCommand = 2;
    //                    commandFlag = true;
    //                }
    //            }
    //        }
    //    }
    //    else
    //    {
    //        if (context.started && inputAllowedFlag)
    //        {
    //            selectCommand = 2;
    //            commandFlag = true;
    //        }
    //    }
    //}
    //public void ButtonY(InputAction.CallbackContext context)
    //{
    //    if (gamepad == null)
    //    {
    //        if (context.started && inputAllowedFlag)
    //        {
    //            if (playerNumber == 0)
    //            {
    //                if (context.control.path == "/Keyboard/a")
    //                {
    //                    selectCommand = 1;
    //                    commandFlag = true;
    //                }
    //            }
    //            else if (playerNumber == 1)
    //            {
    //                if (context.control.path == "/Keyboard/leftArrow")
    //                {
    //                    selectCommand = 1;
    //                    commandFlag = true;
    //                }
    //            }
    //        }
    //    }
    //    else
    //    {
    //        if (context.started && inputAllowedFlag)
    //        {
    //            selectCommand = 1;
    //            commandFlag = true;
    //        }
    //    }
    //}
}
