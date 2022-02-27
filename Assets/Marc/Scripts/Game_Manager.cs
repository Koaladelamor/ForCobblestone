using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_Manager : MonoBehaviour
{
    static Game_Manager m_gameManager = null;

    private GameObject m_combatManager;

    public GameObject m_playerPrefab;
    public GameObject m_enemyPrefab;

    private GameObject m_player;
    private GameObject m_pointToGo;

    Vector3 playerPosition;

    Vector3 enemy1_respawnPosition = new Vector3(-270f, -150f, 0f);
    Vector3 enemy2_respawnPosition = new Vector3(-500f, 250f, 0f);

    GameObject enemy1;
    GameObject enemy2;


    public bool enemyEngaged;
    public bool combatIsOver;

    // Start is called before the first frame update
    void Start()
    {
        //Singleton
        if (m_gameManager == null) { 
            m_gameManager = this;
            DontDestroyOnLoad(this);
        }
        else { Destroy(this.gameObject); }

        m_player = GameObject.FindGameObjectWithTag("PlayerMap");
        enemyEngaged = false;

        enemy1 = RespawnEnemy1();
        enemy2 = RespawnEnemy2();
    }

    // Update is called once per frame
    void Update()
    {

        if (enemyEngaged) {

            //Save stats
            playerPosition = m_player.transform.position;

            m_player = null;
            enemyEngaged = false;

            //Load scene
            SceneManager.LoadScene("CombatScene");

        }
        

        if (combatIsOver) {
            SceneManager.LoadScene("MapScene");

            //RespawnPlayer();
            Invoke("SetupPlayer", 1f);

            combatIsOver = false;
        }
    }

    void RespawnPlayer() {
        GameObject playerInstantiate = Instantiate(m_playerPrefab, playerPosition, Quaternion.identity);
        m_player = playerInstantiate;
    }

    void SetupPlayer() {
        m_player = GameObject.FindGameObjectWithTag("PlayerMap");
        m_pointToGo = GameObject.FindGameObjectWithTag("PointToGo");
        m_player.transform.position = playerPosition;
        m_pointToGo.transform.position = playerPosition;
    }

    GameObject RespawnEnemy1() {
        GameObject enemy1 = Instantiate(m_enemyPrefab, enemy1_respawnPosition, Quaternion.identity);

        enemy1.name = "Enemy1";
        enemy1.GetComponent<PatrolAI>().patrolPoints[0] = GameObject.Find("Enemy1PatrolPoint1");
        enemy1.GetComponent<PatrolAI>().patrolPoints[1] = GameObject.Find("Enemy1PatrolPoint2");
        enemy1.GetComponent<PatrolAI>().patrolPoints[2] = GameObject.Find("Enemy1PatrolPoint3");
        enemy1.GetComponent<PatrolAI>().patrolPoints[3] = GameObject.Find("Enemy1PatrolPoint4");

        return enemy1;
    }

    GameObject RespawnEnemy2()
    {
        GameObject enemy1 = Instantiate(m_enemyPrefab, enemy2_respawnPosition, Quaternion.identity);

        enemy1.name = "Enemy2";
        enemy1.GetComponent<PatrolAI>().patrolPoints[0] = GameObject.Find("Enemy2PatrolPoint1");
        enemy1.GetComponent<PatrolAI>().patrolPoints[1] = GameObject.Find("Enemy2PatrolPoint2");
        enemy1.GetComponent<PatrolAI>().patrolPoints[2] = GameObject.Find("Enemy2PatrolPoint3");
        enemy1.GetComponent<PatrolAI>().patrolPoints[3] = GameObject.Find("Enemy2PatrolPoint4");

        return enemy1;
    }
}
