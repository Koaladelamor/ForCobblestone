using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyToSpawn;
    private GameObject enemyOnSpawn;
    private int spawnerID;


    // Update is called once per frame
    void Update()
    {
        if (enemyOnSpawn != null && !GetPatrolAI().IsAlive()) {
            //Destroy Enemy + PatrolPoints and Instantiate a new one when spawner is out of camera
            for (int i = 0; i < GetPatrolAI().patrolPoints.Length; i++)
            {
                Destroy(GetPatrolAI().patrolPoints[i]);
            }
            Destroy(enemyOnSpawn);
        }
    }

    private void OnBecameInvisible()
    {
        if (enemyOnSpawn == null) {
            RespawnEnemy(transform.position, spawnerID);
        }
    }

    public void RespawnEnemy(Vector3 enemySpawnPos, int ID)
    {
        GameObject enemy = Instantiate(enemyToSpawn, enemySpawnPos, transform.rotation);
        PatrolAI AI = enemy.GetComponent<PatrolAI>();
        if (AI == null) {
            Debug.Log("ERROR SPAWNER PATROL AI NULL");
            return;
        }

        AI.patrolPoints[0] = AI.InstantiatePatrolPoint(50f, 50f);
        AI.patrolPoints[1] = AI.InstantiatePatrolPoint(-50f, 50f);
        AI.patrolPoints[2] = AI.InstantiatePatrolPoint(-50f, -50f);
        AI.patrolPoints[3] = AI.InstantiatePatrolPoint(50f, -50f);

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
