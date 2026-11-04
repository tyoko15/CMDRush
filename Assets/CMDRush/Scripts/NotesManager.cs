using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class NotesManager : MonoBehaviour
{
    public int playerNumber;
    public ControllerKind controllerKind;
    public Gamepad gamepad;
    [SerializeField] GameObject notesPrefab;
    [SerializeField] Sprite[] notesSprites;
    public Vector3[] positions = new Vector3[5];
    [HideInInspector] List<Notes> notesList = new List<Notes>();

    [SerializeField] GameObject playerUI;
    TextMeshProUGUI roundText;
    TextMeshProUGUI scoreText;
    TextMeshProUGUI correctAnswerRateText;

    public int score;
    public int notesCount;
    public int roundCount;
    public int correctCount;
    public int missCount;
    int correctAnswerRate;
    [SerializeField] int totalCorrectAnswer;
    [SerializeField] int addQuestion;

    public bool setUpPracticeNotesFlag;
    int setUpPracticeNotesCount;
    [SerializeField] float setUpPracticeNotesDelayTime;
    float setUpPracticeNotesDelayTimer;

    bool setUpNotesFlag;
    int notesSetUpCount;
    bool setupDelayFlag;
    [SerializeField] float setUpDelayTime;
    float setUpDelayTimer;

    bool addNotesFlag;
    bool addDelayFlag;
    [SerializeField] float addDelayTime;
    float addDelayTimer;

    bool resetNotesFlag;
    bool resetDelayFlag;
    [SerializeField] float resetDelayTime;
    float resetDelayTimer;

    bool practiceCommandFlag;
    int practiceNotesCount;

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
        StartPracticeNotesSetUp();
        setUpPracticeNotesFlag = true;
        //setUpNotesFlag = true;

        roundCount++;
    }

    void Update()
    {
        UpdateUI();
        if (setUpPracticeNotesFlag) SetupPracticeNotes();
        //if (setUpNotesFlag) SetUpNotes();
        if (setUpPracticeNotesFlag) CommandControl();
        if (TwoPMatchGameManager.Instance.gameFlag)
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
        roundText = playerUI.transform.GetChild(5).GetChild(3).GetComponent<TextMeshProUGUI>();
        scoreText = playerUI.transform.GetChild(5).GetChild(4).GetComponent<TextMeshProUGUI>();
        correctAnswerRateText = playerUI.transform.GetChild(5).GetChild(5).GetComponent<TextMeshProUGUI>();
    }

    void InstantiateDesignationNotes(int i)
    {
        GameObject n = Instantiate(notesPrefab, Vector2.zero, Quaternion.identity);
        NotesData d = new NotesData();
        d.number = 0;
        d.type = (NotesType)i;
        n.GetComponent<Notes>().notesManager = this;
        n.GetComponent<Notes>().data = d;
        n.GetComponent<SpriteRenderer>().sprite = notesSprites[(int)d.type + (int)controllerKind * 4];
        n.transform.SetParent(transform, true);
        n.transform.position = positions[0];
        n.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
        notesList.Add(n.GetComponent<Notes>());
        notesCount++;
    }

    void InstantiateNotes()
    {
        GameObject n = Instantiate(notesPrefab, Vector2.zero, Quaternion.identity);
        NotesData d = new NotesData();
        d.number = 0;
        d.type = (NotesType)Random.Range(0, 4);
        n.GetComponent<Notes>().notesManager = this;
        n.GetComponent<Notes>().data = d;
        n.GetComponent<SpriteRenderer>().sprite = notesSprites[(int)d.type + (int)controllerKind * 4];
        n.transform.SetParent(transform, true);
        n.transform.position = positions[0];
        n.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
        notesList.Add(n.GetComponent<Notes>());
        notesCount++;
    }

    public void ControllerStop()
    {
        if (gamepad != null) gamepad.SetMotorSpeeds(0f, 0f);
    }

    public void SetupPracticeNotes()
    {
        if (!setUpPracticeNotesFlag)
        {
            InstantiateDesignationNotes(setUpPracticeNotesCount);
            setUpPracticeNotesFlag = true;
        }
        else
        {
            if (setUpPracticeNotesDelayTimer > setUpPracticeNotesDelayTime)
            {
                if (setUpPracticeNotesCount != 3)
                {
                    for (int i = 0; i < notesList.Count; i++)
                    {
                        notesList[i].CalculationX();
                        notesList[i].moveFlag = true;
                        notesList[i].data.number++;
                    }
                }
            }
        }
    }


    public void StartPracticeNotesSetUp()
    {
        StartCoroutine(SetUpPracticeNotesCoroutine());
    }

    public void StartNotesSetup()
    {
        StartCoroutine(SetUpNotesCoroutine());        
    }

    private IEnumerator SetUpPracticeNotesCoroutine()
    {
        for (int setUpPracticeNotesCount = 0; setUpPracticeNotesCount < 4; setUpPracticeNotesCount++)
        {
            InstantiateDesignationNotes(setUpPracticeNotesCount);

            float timer = 0f;
            while (timer < setUpPracticeNotesDelayTime)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            if (setUpPracticeNotesCount != 4)
            {
                for (int i = 0; i < notesList.Count; i++)
                {
                    notesList[i].CalculationX();
                    notesList[i].moveFlag = true;
                    notesList[i].data.number++;
                }
            }
        }

        //setUpPracticeNotesFlag = false;
    }

    private IEnumerator SetUpNotesCoroutine()
    {
        for (int notesSetUpCount = 0; notesSetUpCount < 5; notesSetUpCount++)
        {
            InstantiateNotes();

            float timer = 0f;
            while (timer < setUpDelayTime)
            {
                timer += Time.deltaTime;

                float amount = Mathf.Lerp(
                    notesSetUpCount * 0.2f,
                    (notesSetUpCount * 0.2f) + 0.2f,
                    timer / setUpDelayTime
                );
                countDownImage.fillAmount = amount;

                yield return null;
            }

            if (notesSetUpCount != 4)
            {
                for (int i = 0; i < notesList.Count; i++)
                {
                    notesList[i].CalculationX();
                    notesList[i].moveFlag = true;
                    notesList[i].data.number++;
                }
            }
        }

        TwoPMatchGameManager.Instance.setupFlag = true;
        setUpNotesFlag = false;
    }


    void SetUpNotes()
    {
        if (!setupDelayFlag)
        {
            InstantiateNotes();
            setupDelayFlag = true;
        }
        else
        {
            if (setUpDelayTimer > setUpDelayTime)
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
                    TwoPMatchGameManager.Instance.setupFlag = true;
                    notesSetUpCount = 0;
                    setUpNotesFlag = false;
                }
                setUpDelayTimer = 0f;
                setupDelayFlag = false;
            }
            else
            {
                setUpDelayTimer += Time.deltaTime;

                float amount = Mathf.Lerp(notesSetUpCount * 0.2f, (notesSetUpCount * 0.2f) + 0.2f, setUpDelayTimer / setUpDelayTime);
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
            if (gamepad != null) gamepad.SetMotorSpeeds(1f, 1f);
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
                StartNotesSetup();
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
        StartNotesSetup();
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
                correctCount++;
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
                missCount++;
                notesCount = 0;
                resetNotesFlag = true;
            }
            countDownTimer = 0;
            commandFlag = false;
            inputAllowedFlag = false;
        }
        else if (practiceCommandFlag)
        {
            if (notesList.Count == 0) return;
            if ((int)notesList[0].data.type == selectCommand)
            {
                notesList[0].successFlag = true;
                for (int i = 0; i < notesList.Count; i++)
                {
                    notesList[i].CalculationX();
                    notesList[i].moveFlag = true;
                    notesList[i].data.number++;
                }
                notesList.RemoveAt(0);

                if (notesList.Count == 0)
                {
                    notesCount = 0;
                    TwoPMatchGameManager.Instance.readyFlags[playerNumber] = true;
                    setUpPracticeNotesFlag = false;
                }
            }
            else
            {
                notesList[0].practiceFailureFlag = true;

            }
            practiceCommandFlag = false;
        }
    }

    void UpdateUI()
    {
        roundText.text = $"Round {roundCount.ToString("00")}";
        scoreText.text = $"Score {score.ToString("000000")}";
        correctAnswerRateText.text = $"{correctAnswerRate.ToString("00")} / {totalCorrectAnswer.ToString("00")}";
    }

    public void PracticeSelectCommand(int i)
    {
        selectCommand = i;
        practiceCommandFlag = true;
    }

    public void SetSelectCommand(int i)
    {
        selectCommand = i;
        commandFlag = true;
    }

    private void OnDisable()
    {
        if (gamepad != null) gamepad.SetMotorSpeeds(0f, 0f);
    }
}
