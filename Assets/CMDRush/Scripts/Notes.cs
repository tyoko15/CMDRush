using UnityEngine;

public enum NotesType
{
    Right, Left, Up, Down
}

public class NotesData
{
    public int number;
    public NotesType type;
}

public class Notes : MonoBehaviour
{
    // ノーツのデータ
    public NotesManager notesManager;
    public NotesData data;
    SpriteRenderer sr;
    Rigidbody rb;

    // 
    bool spawnFlag;
    [SerializeField] float spawnTime;
    float spawnTimer;

    public bool moveFlag;
    float currentX;
    float nextX;
    [SerializeField] float moveTime;
    float moveTimer;

    public bool successFlag;
    [SerializeField] float successTime;
    float successTimer;
    public bool practiceFailureFlag;
    [SerializeField] float practiceFailureTime;
    float practiceFailureTimer;
    [Header("震えの設定")]
    [SerializeField] private float shakeAmount;
    private Vector3 initialPosition;

    public bool failureFlag;
    

    void Start()
    {        
        rb = GetComponent<Rigidbody>();
        spawnFlag = true;
        CalculationX();
    }

    void Update()
    {
        if (spawnFlag) Spawn();
        if (moveFlag) Move();
        if (successFlag) Success();
        if (practiceFailureFlag) PracticeFailure();
        if (failureFlag) Failure();
    }

    void Spawn()
    {
        if (spawnTimer > spawnTime)
        {
            spawnTimer = 0;
            spawnFlag = false;
        }
        else
        {
            spawnTimer += Time.deltaTime;
            float size = Mathf.Lerp(0f, 0.25f, spawnTimer / spawnTime);
            transform.localScale = new Vector2(size, size);
        }
    }

    public void CalculationX()
    {
        currentX = notesManager.positions[data.number].x;
        if (data.number + 1 > 4) return;
        else nextX = notesManager.positions[data.number + 1].x;                           
    }

    void Move()
    {
        if (moveTimer > moveTime)
        {
            moveTimer = 0;
            moveFlag = false;
        }
        else
        {
            moveTimer += Time.deltaTime;
            float x = Mathf.Lerp(currentX, nextX, moveTimer / moveTime);
            transform.position = new Vector2(x, transform.position.y);
        }
    }

    void Success()
    {
        if (successTimer > successTime)
        {

            Destroy(gameObject);
            successTimer = 0;
            successFlag = false;
        }
        else
        {
            successTimer += Time.deltaTime;
            float a = Mathf.Lerp(1f, 0f, successTimer / successTime);
            float s = Mathf.Lerp(0.15f, 0.4f, successTimer / successTime);
            Color color = transform.GetComponent<SpriteRenderer>().color;
            color.a = a;
            transform.GetComponent<SpriteRenderer>().color = color;
            transform.localScale = new Vector3(s, s, s);
        }

    }

    void PracticeFailure()
    {
        if (practiceFailureTimer > practiceFailureTime)
        {
            transform.localPosition = initialPosition;

            practiceFailureTimer = 0f;
            practiceFailureFlag = false;
        }
        else
        {
            if (practiceFailureTimer == 0f) initialPosition = transform.localPosition;
            practiceFailureTimer += Time.deltaTime;
            Vector2 randomOffset = Random.insideUnitCircle * shakeAmount;
            transform.localPosition = initialPosition + new Vector3(randomOffset.x, randomOffset.y, 0f);
        }
    }

    void Failure()
    {
        float x = Random.Range(-0.5f, 0.5f);
        rb.AddForce(x, 4f, 0f, ForceMode.Impulse);
        rb.useGravity = true;
        Destroy(gameObject, 2f);
        failureFlag = false;
    }
}
