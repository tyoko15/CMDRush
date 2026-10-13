using UnityEngine;

public enum NotesType
{
    Right, Left, Up, Down
}

[System.Serializable]
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
            float size = Mathf.Lerp(0f, 0.15f, spawnTimer / spawnTime);
            transform.localScale = new Vector2(size, size);
        }
    }

    public void CalculationX()
    {
        currentX = notesManager.positions[data.number].x;
        if (data.number + 1 > 4) Debug.Log($"over");
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
            float s = Mathf.Lerp(0.15f, 0.35f, successTimer / successTime);
            Color color = transform.GetComponent<SpriteRenderer>().color;
            color.a = a;
            transform.GetComponent<SpriteRenderer>().color = color;
            transform.localScale = new Vector3(s, s, s);
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
