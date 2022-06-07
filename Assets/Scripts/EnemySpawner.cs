using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject spiderPrefab;
    [SerializeField] private GameObject wormPrefab;
    private GameObject enemyToSpawn;
    private GameObject enemyOnSpawn;
    private int spawnerID;
    [SerializeField] private bool linearPatrol;
    [SerializeField] private bool horizontal;
    [SerializeField] private bool vertical;

    private void Awake()
    {
        //enemyToSpawn = null;
        //enemyOnSpawn = null;
        //linearPatrol = false;
        spawnerID = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetPatrolAI().IsAlive()) {
            //Destroy Enemy + PatrolPoints and Instantiate a new one when spawner is out of camera
            for (int i = 0; i < GetPatrolAI().patrolPoints.Length; i++)
            {
                GetPatrolAI().patrolPoints[i].SetActive(false);
            }
            enemyOnSpawn.SetActive(false);
        }
    }

    private void OnBecameInvisible()
    {
        if (!GetPatrolAI().IsAlive())
        {
            for (int i = 0; i < GetPatrolAI().patrolPoints.Length; i++)
            {
                GetPatrolAI().patrolPoints[i].SetActive(true);
            }
            enemyOnSpawn.SetActive(true);
            GetPatrolAI().SetAlive(true);
        }
    }

    public void RespawnEnemy(Vector3 enemySpawnPos, int ID)
    {
        int randomInt = Random.Range(0, 2);
        if (randomInt == 0)
        {
            enemyToSpawn = spiderPrefab;
        }
        else if (randomInt == 1)
        {
            enemyToSpawn = wormPrefab;
        }
        else { Debug.Log("randomInt > 1"); }


        GameObject enemy = Instantiate(enemyToSpawn, enemySpawnPos, transform.rotation);
        PatrolAI AI = enemy.GetComponent<PatrolAI>();
        if (AI == null) {
            Debug.Log("ERROR SPAWNER PATROL AI NULL");
            return;
        }
        if (enemy == null)
        {
            Debug.Log("ERROR SPAWNER Enemy is null");
            return;
        }


        if (!linearPatrol)
        {
            AI.patrolPoints[0] = AI.InstantiatePatrolPoint(75f, 75f);
            AI.patrolPoints[1] = AI.InstantiatePatrolPoint(-75f, 75f);
            AI.patrolPoints[2] = AI.InstantiatePatrolPoint(-75f, -75f);
            AI.patrolPoints[3] = AI.InstantiatePatrolPoint(75f, -75f);
        }
        else {
            if (horizontal)
            {
                AI.patrolPoints[0] = AI.InstantiatePatrolPoint(75f, 0);
                AI.patrolPoints[1] = AI.InstantiatePatrolPoint(-75f, 0);
                AI.patrolPoints[2] = AI.InstantiatePatrolPoint(125f, 0);
                AI.patrolPoints[3] = AI.InstantiatePatrolPoint(-125f, 0);
            }
            else if (vertical)
            {
                AI.patrolPoints[0] = AI.InstantiatePatrolPoint(0, 75f);
                AI.patrolPoints[1] = AI.InstantiatePatrolPoint(0, -75f);
                AI.patrolPoints[2] = AI.InstantiatePatrolPoint(0, 125f);
                AI.patrolPoints[3] = AI.InstantiatePatrolPoint(0, -125f);
            }
        }

        enemy.GetComponent<PatrolAI>().SetEnemyID(ID);
        spawnerID = ID;

        enemy.name = "EnemyParty " + ID.ToString();

        SetEnemy(enemy);
    }

    public void SetEnemy(GameObject enemy) { enemyOnSpawn = enemy; }

    public GameObject GetEnemy() { return enemyOnSpawn; }

    public PatrolAI GetPatrolAI() { return enemyOnSpawn.GetComponent<PatrolAI>(); }

    public void SetSpawnerID(int ID) { spawnerID = ID; }

    public int GetSpawnerID() { return spawnerID; }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 50);
    }
}
