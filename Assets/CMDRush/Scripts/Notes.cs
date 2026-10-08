using UnityEngine;

public class Notes : MonoBehaviour
{
    RectTransform rect;
    int number;


    bool spawnFlag;
    [SerializeField] float spawnTime;
    float spawnTimer;

    bool moveFlag;
    float currentX;
    float nextX;
    [SerializeField] float moveTime;
    float moveTimer;

    void Start()
    {
        rect = transform.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(-350f, 0);
        spawnFlag = true;
    }

    void Update()
    {
        SpawnNotes();
        MoveNotes();
    }

    void CalculationX()
    {

    }

    void SpawnNotes()
    {
        if (spawnFlag)
        {
            if (spawnTimer > spawnTime)
            {
                spawnTimer = 0;
                spawnFlag = false;
                moveFlag = true;
                number = 0;
            }
            else
            {
                spawnTimer += Time.deltaTime;
                float s = Mathf.Lerp(0f, 100f, spawnTimer / spawnTime);
                transform.GetComponent<RectTransform>().sizeDelta = new Vector3(s, s, s);
            }
        }
    }

    void MoveNotes()
    {
        if (moveFlag)
        {
            if (moveTimer > moveTime)
            {
                moveTimer = 0;
                number++;
                moveFlag = false;
            }
            else
            {
                moveTimer += Time.deltaTime;
                float x = Mathf.Lerp(currentX, nextX, moveTimer / moveTime);
                transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, 0);
            }
        }
    }
}
