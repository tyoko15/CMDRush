using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public enum GameMode
{
    None,
    TwoMatch,
    Tapping,
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameMode gameMode;

    public string[] playerNames = new string[2];

    public NotesManager[] notesManagers = new NotesManager[2];
    public PlayerInput[] playerInputs = new PlayerInput[2];

    public bool gameFlag;
    public bool[] readyFlags = new bool[2];
    [SerializeField] public float gameLimitSecond;

    public bool setUpPracticeNotesFlag;
    public bool setupFlag;

    public bool playerNameChangeFlag;
    public int playerNameChange;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
