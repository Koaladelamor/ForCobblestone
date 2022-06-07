using UnityEngine;

public enum EnemyType { NONE, SPIDER, WORM }
public class PatrolAI : MonoBehaviour
{
    public float speed;
    public GameObject[] patrolPoints;
    public GameObject patrolPointPrefab;
    private int currentPatrolPoint;

    private bool patrolling;
    private bool alive;

    private GameObject m_player;

    private int enemyID;

    Vector2 previousPosition;
    Vector2 currentPosition;

    public EnemyType enemyType;

    // Start is called before the first frame update
    private void Awake()
    {
        alive = true;
        currentPatrolPoint = 0;
        patrolling = true;
        m_player = GameObject.FindGameObjectWithTag("PlayerMap");
    }

    // Update is called once per frame
    private void Update()
    {

        previousPosition = currentPosition;
        currentPosition = transform.position;

        /*if (patrolling)
        {
            FlipSprites();
        }*/
    }

    void FixedUpdate()
    {
        Patrol();
        MoveTowardsPlayer();
    }

    void MoveTowardsPlayer() {
        if (GetComponentInChildren<VisionRange>().GetPlayerDetected())
        {
            patrolling = false;
            transform.position = Vector3.MoveTowards(transform.position, m_player.transform.position, speed * 1.2f *Time.fixedDeltaTime);
        }
        else {
            patrolling = true;
        }
    }

    void Patrol() {
        if(patrolling) { 
            if (transform.position != patrolPoints[currentPatrolPoint].transform.position)
            {
                transform.position = Vector2.MoveTowards(transform.position, patrolPoints[currentPatrolPoint].transform.position, speed * Time.fixedDeltaTime);
            }
            else
            {
                currentPatrolPoint++;
            }

            if (transform.position == patrolPoints[patrolPoints.Length-1].transform.position)
            {
                currentPatrolPoint = 0;
            }
        }
    }

    public GameObject InstantiatePatrolPoint(float offset_x, float offset_y) {

        GameObject patrolPoint = Instantiate(patrolPointPrefab, new Vector3(transform.position.x + offset_x, transform.position.y + offset_y, 0), transform.rotation);


        return patrolPoint;
    }

    public void FlipSprites()
    {
        if (currentPosition.x > previousPosition.x)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (currentPosition.x < previousPosition.x)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    public void SetEnemyID(int ID) { enemyID = ID; }

    public int GetEnemyID() { return enemyID; }

    public EnemyType GetEnemyType() { return enemyType; }

    public bool IsAlive() { return alive; }

    public void SetAlive(bool _alive) { alive = _alive; }
}
