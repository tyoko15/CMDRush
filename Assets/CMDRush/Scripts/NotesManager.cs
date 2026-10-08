using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public enum NoteType
{
    Up,
    Down,
    Left,
    Right
}

public class NotesData
{
    public int number;
    public NoteType noteType;
}

public class NotesManager : MonoBehaviour
{
    public static NotesManager Instance;

    int round;
    int count;
    public List<NotesData> roundNotes;
    public List<NotesData> currentNotes = new List<NotesData>(4);

    public GameObject notesPrefab;

    [SerializeField] Sprite[] noteSprites;

    bool spawnFlag;
    [SerializeField] float spawnTime;
    float spawnTimer;

    void Start()
    {
        Instance = this;

        spawnFlag = true;
    }

    void Update()
    {

    }

    void SetUpRoundNotes()
    {
        roundNotes.Clear();
        roundNotes = new List<NotesData>(round + 1); 
        for (int i = 0; i < roundNotes.Count; i++)
        {
            NotesData n = new NotesData();
            int r = Random.Range(0, 4);
            n.number = i;
            n.noteType = (NoteType)r;
            roundNotes.Add(n);
        }
        round++;
    }

    void InstatiateNotes()
    {
        Instantiate(notesPrefab, Vector3.zero, Quaternion.identity);
    }
}
