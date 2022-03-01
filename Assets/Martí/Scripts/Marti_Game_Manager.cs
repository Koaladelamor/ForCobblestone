using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Marti_Game_Manager : MonoBehaviour
{
    static Marti_Game_Manager m_gameManager = null;

    private GameObject m_combatManager;

    public GameObject m_playerPrefab;
    public GameObject m_enemyPrefab;


    private GameObject m_canvasToCombat;
    private GameObject m_canvasToMap;


   


    private GameObject m_player;
    private GameObject m_pointToGo;

    Vector3 playerPosition;

    Vector3 enemy1_respawnPosition = new Vector3(-270f, -150f, 0f);
    Vector3 enemy2_respawnPosition = new Vector3(-500f, 250f, 0f);

    public GameObject enemyOnCombat;
    GameObject[] m_enemies;

    public int enemyIndex;
    public int totalEnemies;
    public bool[] areEnemiesAlive;

    public bool enemyEngaged;
    public bool combatIsOver;


    // Start is called before the first frame update
    void Start()
    {
        //Singleton
        if (m_gameManager == null)
        {
            m_gameManager = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

        m_player = GameObject.FindGameObjectWithTag("PlayerMap");
        enemyEngaged = false;

        m_canvasToCombat = GameObject.FindGameObjectWithTag("CanvasToBattle");
        m_canvasToCombat.SetActive(false);

        m_canvasToMap = GameObject.FindGameObjectWithTag("CanvasToMap");
        m_canvasToMap.SetActive(false);

        


        RespawnEnemy0();
        RespawnEnemy1();


    }

    // Update is called once per frame
    void Update()
    {

        if (enemyEngaged)
        {

            //Save stats
            playerPosition = m_player.transform.position;

            m_player = null;

            m_canvasToCombat.SetActive(true);

            enemyEngaged = false;


            //Enemy info
            enemyIndex = enemyOnCombat.GetComponent<Marti_PatrolAI>().enemyID;
            totalEnemies = enemyOnCombat.GetComponent<Marti_PatrolAI>().totalEnemies;

            //Load scene
            //SceneManager.LoadScene("CombatScene");

            



        }


        if (combatIsOver)
        {
            //SceneManager.LoadScene("MapScene");

            m_canvasToMap.SetActive(true);//Después de esto se necesita parar el juego
            /*
            areEnemiesAlive[enemyIndex] = false;
            //RespawnPlayer();
            Invoke("SetupPlayer", 0.2f);
            Invoke("EnemiesRespawner", 0.3f);

            combatIsOver = false;
            */
        }
    }

    void SetupPlayer()
    {
        m_player = GameObject.FindGameObjectWithTag("PlayerMap");
        m_pointToGo = GameObject.FindGameObjectWithTag("PointToGo");
        m_player.transform.position = playerPosition;
        m_pointToGo.transform.position = playerPosition;
    }

    GameObject RespawnEnemy0()
    {
        GameObject enemy = Instantiate(m_enemyPrefab, enemy1_respawnPosition, Quaternion.identity);

        enemy.name = "Enemy0";
        enemy.GetComponent<Marti_PatrolAI>().patrolPoints[0] = GameObject.Find("Enemy1PatrolPoint1");
        enemy.GetComponent<Marti_PatrolAI>().patrolPoints[1] = GameObject.Find("Enemy1PatrolPoint2");
        enemy.GetComponent<Marti_PatrolAI>().patrolPoints[2] = GameObject.Find("Enemy1PatrolPoint3");
        enemy.GetComponent<Marti_PatrolAI>().patrolPoints[3] = GameObject.Find("Enemy1PatrolPoint4");

        enemy.GetComponent<Marti_PatrolAI>().enemyID = 0;

        return enemy;
    }

    GameObject RespawnEnemy1()
    {
        GameObject enemy = Instantiate(m_enemyPrefab, enemy2_respawnPosition, Quaternion.identity);

        enemy.name = "Enemy1";
        enemy.GetComponent<Marti_PatrolAI>().patrolPoints[0] = GameObject.Find("Enemy2PatrolPoint1");
        enemy.GetComponent<Marti_PatrolAI>().patrolPoints[1] = GameObject.Find("Enemy2PatrolPoint2");
        enemy.GetComponent<Marti_PatrolAI>().patrolPoints[2] = GameObject.Find("Enemy2PatrolPoint3");
        enemy.GetComponent<Marti_PatrolAI>().patrolPoints[3] = GameObject.Find("Enemy2PatrolPoint4");

        enemy.GetComponent<Marti_PatrolAI>().enemyID = 1;

        return enemy;
    }

    void EnemiesRespawner()
    {
        if (areEnemiesAlive[0])
        {
            RespawnEnemy0();
        }

        if (areEnemiesAlive[1])
        {
            RespawnEnemy1();
        }
    }

    public void clickOnBattleButton() {
        Debug.Log("prueba");
        SceneManager.LoadScene("CombatScene_Marti");

    }

    public void clickOnExploreButton()
    {
        Debug.Log("prueba");
        SceneManager.LoadScene("UpdatedMap");

        areEnemiesAlive[enemyIndex] = false;
        //RespawnPlayer();
        Invoke("SetupPlayer", 0.2f);
        Invoke("EnemiesRespawner", 0.3f);

        combatIsOver = false;

    }

}
