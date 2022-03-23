using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum EnemyType  { MINOTAUR, WOLF, WORM }
public class Game_Manager : MonoBehaviour
{
    static Game_Manager mInstance;

    static public Game_Manager Instance
    {
        get { return mInstance; }
        private set { }
    }

    public UserInterface m_inventoryDisplay;
    public UserInterface m_GrodnarEquipmentDisplay;
    public UserInterface m_LanstarEquipmentDisplay;
    public UserInterface m_SigfridEquipmentDisplay;
    public UserInterface m_currentEquipmentInterface;

    public Button[] equipmentButtons;

    public GameObject m_playerPrefab;

    public GameObject m_minotaurPrefab;
    public GameObject m_wolfPrefab;

    private GameObject m_player;
    private GameObject m_pointToGo;

    private GameObject m_canvasToCombat;

  

    Vector3 playerPosition;

    Vector3 enemy1_respawnPosition = new Vector3(-237f, 33f, 0f);
    Vector3 enemy2_respawnPosition = new Vector3(370f, -100f, 0f);

    public GameObject enemyOnCombat;
    public EnemyType enemyOnCombatType;

    public int enemyIndex;
    public int totalEnemies;
    public bool[] areEnemiesAlive;

    public bool enemyEngaged;
    public bool combatIsOver;

    bool inventoryOnScreen = false;

    private void Awake()
    {
        //Singleton
        if (mInstance == null)
        {
            mInstance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_canvasToCombat = GameObject.FindGameObjectWithTag("CanvasToBattle");
        m_canvasToCombat.SetActive(false);

        m_player = GameObject.FindGameObjectWithTag("PlayerMap");
        enemyEngaged = false;

        RespawnEnemy0();
        RespawnEnemy1();

        m_currentEquipmentInterface = m_GrodnarEquipmentDisplay;
        foreach (Button button in equipmentButtons)
        {
            button.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (InputManager.Instance.InventoryButtonPressed && inventoryOnScreen)
        {
            HideInventories();
        }
        else if (InputManager.Instance.InventoryButtonPressed && !inventoryOnScreen) 
        {
            //m_GrodnarEquipmentDisplay.ShowInventory();
            //m_LanstarEquipmentDisplay.ShowInventory();
            //m_SigfridEquipmentDisplay.ShowInventory();
            ShowButtons();
            m_currentEquipmentInterface.ShowInventory();
            m_inventoryDisplay.ShowInventory();
            inventoryOnScreen = true;
        }

        if (enemyEngaged) {
            //Save stats
            playerPosition = m_player.transform.position;

            //Enemy info
            enemyIndex = enemyOnCombat.GetComponent<PatrolAI>().enemyID;
            totalEnemies = enemyOnCombat.GetComponent<PatrolAI>().totalEnemies;
            enemyOnCombatType = enemyOnCombat.GetComponent<PatrolAI>().enemyType;

            m_canvasToCombat.SetActive(true);

            //Load scene
            //SceneManager.LoadScene("CombatScene", LoadSceneMode.Single);
            //SceneManager.LoadScene("CombatScene", LoadSceneMode.Additive);

            enemyEngaged = false;

        }


        if (combatIsOver) {
            combatIsOver = false;

            SceneManager.LoadScene("MapScene", LoadSceneMode.Single);

            areEnemiesAlive[enemyIndex] = false;
            //RespawnPlayer();
            Invoke("disableCanvas", 0.03f);
            Invoke("SetupPlayer", 0.04f);
            Invoke("EnemiesRespawner", 0.1f);



        }
    }

    public void HideInventories() {
        HideButtons();
        m_GrodnarEquipmentDisplay.HideInventory();
        m_LanstarEquipmentDisplay.HideInventory();
        m_SigfridEquipmentDisplay.HideInventory();
        m_inventoryDisplay.HideInventory();
        inventoryOnScreen = false;
    }

    public void GrodnarEquipmentDisplay() {
        if (m_currentEquipmentInterface == m_GrodnarEquipmentDisplay) { return; }
        m_currentEquipmentInterface.HideInventory();
        m_currentEquipmentInterface = m_GrodnarEquipmentDisplay;
        m_currentEquipmentInterface.ShowInventory();
    }

    public void LanstarEquipmentDisplay()
    {
        if (m_currentEquipmentInterface == m_LanstarEquipmentDisplay) { return; }
        m_currentEquipmentInterface.HideInventory();
        m_currentEquipmentInterface = m_LanstarEquipmentDisplay;
        m_currentEquipmentInterface.ShowInventory();
    }

    public void SigfridEquipmentDisplay()
    {
        if (m_currentEquipmentInterface == m_SigfridEquipmentDisplay) { return; }
        m_currentEquipmentInterface.HideInventory();
        m_currentEquipmentInterface = m_SigfridEquipmentDisplay;
        m_currentEquipmentInterface.ShowInventory();
    }

    public void HideButtons() {
        foreach (Button button in equipmentButtons)
        {
            button.gameObject.SetActive(false);
        }
    }

    public void ShowButtons()
    {
        foreach (Button button in equipmentButtons)
        {
            button.gameObject.SetActive(true);
        }
    }

    void SetupPlayer() {
        m_player = GameObject.FindGameObjectWithTag("PlayerMap");
        m_pointToGo = GameObject.FindGameObjectWithTag("PointToGo");
        m_player.transform.position = playerPosition;
        m_pointToGo.transform.position = playerPosition;
    }

    GameObject RespawnEnemy0() {
        GameObject enemy = Instantiate(m_minotaurPrefab, enemy1_respawnPosition, Quaternion.identity);

        enemy.name = "Enemy0";
        enemy.GetComponent<PatrolAI>().patrolPoints[0] = GameObject.Find("Enemy1PatrolPoint1");
        enemy.GetComponent<PatrolAI>().patrolPoints[1] = GameObject.Find("Enemy1PatrolPoint2");
        enemy.GetComponent<PatrolAI>().patrolPoints[2] = GameObject.Find("Enemy1PatrolPoint3");
        enemy.GetComponent<PatrolAI>().patrolPoints[3] = GameObject.Find("Enemy1PatrolPoint4");

        enemy.GetComponent<PatrolAI>().enemyID = 0;

        enemy.GetComponent<PatrolAI>().enemyType = EnemyType.MINOTAUR;

        return enemy;
    }

    GameObject RespawnEnemy1()
    {
        GameObject enemy = Instantiate(m_wolfPrefab, enemy2_respawnPosition, Quaternion.identity);

        enemy.name = "Enemy1";
        enemy.GetComponent<PatrolAI>().patrolPoints[0] = GameObject.Find("Enemy2PatrolPoint1");
        enemy.GetComponent<PatrolAI>().patrolPoints[1] = GameObject.Find("Enemy2PatrolPoint2");
        enemy.GetComponent<PatrolAI>().patrolPoints[2] = GameObject.Find("Enemy2PatrolPoint3");
        enemy.GetComponent<PatrolAI>().patrolPoints[3] = GameObject.Find("Enemy2PatrolPoint4");

        enemy.GetComponent<PatrolAI>().enemyID = 1;

        enemy.GetComponent<PatrolAI>().enemyType = EnemyType.WOLF;

        return enemy;
    }

    void EnemiesRespawner() {
        if (areEnemiesAlive[0])
        {
            RespawnEnemy0();
        }

        if (areEnemiesAlive[1])
        {
            RespawnEnemy1();
        }
    }

    public void loadCombatScene() {
        SceneManager.LoadScene("CombatScene", LoadSceneMode.Single);
    }

    public void loadMapScene()
    {
        SceneManager.LoadScene("MapScene", LoadSceneMode.Single);


    }

    public void disableCanvas() {
        m_canvasToCombat = GameObject.FindGameObjectWithTag("CanvasToBattle");
        m_canvasToCombat.SetActive(false);
    }



}
