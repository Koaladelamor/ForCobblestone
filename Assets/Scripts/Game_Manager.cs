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

    public StatsScreen m_statsScreen;

    public InventoryObject m_inventory;

    public InventoryObject m_GrodnarEquipmentInventory;
    public InventoryObject m_LanstarEquipmentInventory;
    public InventoryObject m_SigfridEquipmentInventory;

    public Button[] equipmentButtons;

    public readonly List<Stat> GrodnarStats = new List<Stat>();
    public readonly List<Stat> LanstarStats = new List<Stat>();
    public readonly List<Stat> SigfridStats = new List<Stat>();


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

        SetPlayerStats(GrodnarStats, 25, 40, 5, 1);
        SetPlayerStats(LanstarStats, 38, 30, 15, 15);
        SetPlayerStats(SigfridStats, 15, 20, 40, 30);

        for (int i = 0; i < m_GrodnarEquipmentInventory.GetSlots.Length; i++)
        {
            m_GrodnarEquipmentInventory.GetSlots[i].OnBeforeUpdate += OnBeforeSlotUpdate;
            m_GrodnarEquipmentInventory.GetSlots[i].OnAfterUpdate += OnAfterSlotUpdate;
            m_LanstarEquipmentInventory.GetSlots[i].OnBeforeUpdate += OnBeforeSlotUpdate;
            m_LanstarEquipmentInventory.GetSlots[i].OnAfterUpdate += OnAfterSlotUpdate;
            m_SigfridEquipmentInventory.GetSlots[i].OnBeforeUpdate += OnBeforeSlotUpdate;
            m_SigfridEquipmentInventory.GetSlots[i].OnAfterUpdate += OnAfterSlotUpdate;
        }


        Invoke("HideInventories", 0.02f);
    }
    public void OnBeforeSlotUpdate(InventorySlot _slot)
    {
        if(_slot.ItemObject == null) { return; }

        switch (_slot.parent.mInventory.type)
        {
            case InventoryType.MAIN:
                break;
            case InventoryType.GRODNAR:
                print(string.Concat("Removed ", _slot.ItemObject, " on ", _slot.parent.mInventory.type));
                RemoveItemModifier(_slot.Item, GrodnarStats);
                m_statsScreen.DisplayGrodnarStats();
                break;
            case InventoryType.LANSTAR:
                print(string.Concat("Removed ", _slot.ItemObject, " on ", _slot.parent.mInventory.type));
                RemoveItemModifier(_slot.Item, LanstarStats);
                m_statsScreen.DisplayLanstarStats();
                break;
            case InventoryType.SIGFRID:
                print(string.Concat("Removed ", _slot.ItemObject, " on ", _slot.parent.mInventory.type));
                RemoveItemModifier(_slot.Item, SigfridStats);
                m_statsScreen.DisplaySigfridStats();
                break;
            case InventoryType.TRADE:
                break;
            case InventoryType.CHEST:
                break;
            case InventoryType.LAST_NO_USE:
                break;
            default:
                break;
        }
    }

    public void OnAfterSlotUpdate(InventorySlot _slot)
    {
        if (_slot.ItemObject == null) { return; }

        switch (_slot.parent.mInventory.type)
        {
            case InventoryType.MAIN:
                break;
            case InventoryType.GRODNAR:
                print(string.Concat("Placed ", _slot.ItemObject, " on ", _slot.parent.mInventory.type));
                AddItemModifier(_slot.Item, GrodnarStats);
                m_statsScreen.DisplayGrodnarStats();
                break;
            case InventoryType.LANSTAR:
                print(string.Concat("Placed ", _slot.ItemObject, " on ", _slot.parent.mInventory.type));
                AddItemModifier(_slot.Item, LanstarStats);
                m_statsScreen.DisplayLanstarStats();
                break;
            case InventoryType.SIGFRID:
                print(string.Concat("Placed ", _slot.ItemObject, " on ", _slot.parent.mInventory.type));
                AddItemModifier(_slot.Item, SigfridStats);
                m_statsScreen.DisplaySigfridStats();
                break;
            case InventoryType.TRADE:
                break;
            case InventoryType.CHEST:
                break;
            case InventoryType.LAST_NO_USE:
                break;
            default:
                break;
        }
    }

    public void AttributeModified(Stat _stat) {
        Debug.Log(string.Concat(_stat.attribute, " was updated. Value is ", _stat.value));
    }

    public void ShowStats(List<Stat> playerStats) {
        for (int i = 0; i < playerStats.Count; i++)
        {
            print(string.Concat(playerStats[i].attribute, " ", playerStats[i].value));
        }
    }
    public void AddItemModifier(Item _item, List<Stat> _stat)
    {
        for (int i = 0; i < _stat.Count; i++)
        {
            for (int j = 0; j < _item.Buffs.Length; j++)
            {
                if (_stat[i].attribute == _item.Buffs[j].attribute)
                {
                    _stat[i].value += _item.Buffs[j].value;
                    AttributeModified(_stat[i]);
                }
            }
        }
    }

    public void RemoveItemModifier(Item _item, List<Stat> _stat)
    {
        for (int i = 0; i < _stat.Count; i++)
        {
            for (int j = 0; j < _item.Buffs.Length; j++)
            {
                if (_stat[i].attribute == _item.Buffs[j].attribute)
                {
                    _stat[i].value -= _item.Buffs[j].value;
                    AttributeModified(_stat[i]);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            m_inventory.Save();
            m_GrodnarEquipmentInventory.Save();
            m_LanstarEquipmentInventory.Save();
            m_SigfridEquipmentInventory.Save();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            m_inventory.Load();
            m_GrodnarEquipmentInventory.Load();
            m_LanstarEquipmentInventory.Load();
            m_SigfridEquipmentInventory.Load();
        }

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
            m_currentEquipmentInterface.gameObject.SetActive(true);
            m_inventoryDisplay.ShowInventory();
            m_statsScreen.ShowDisplay();
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

    public List<Stat> GetGrodnarStats() { return GrodnarStats; }
    public List<Stat> GetLanstarStats() { return LanstarStats; }
    public List<Stat> GetSigfridStats() { return SigfridStats; }
    public void SetPlayerStats(List<Stat> statsList, int strenghtInt, int staminaInt, int agilityInt, int intelligenceInt) {
        statsList.Add(new Stat(Attributes.STRENGHT, strenghtInt));
        statsList.Add(new Stat(Attributes.STAMINA, staminaInt));
        statsList.Add(new Stat(Attributes.AGILITY, agilityInt));
        statsList.Add(new Stat(Attributes.INTELLIGENCE, intelligenceInt));
    }

    public void HideInventories() {
        HideButtons();
        m_GrodnarEquipmentDisplay.gameObject.SetActive(false);
        m_LanstarEquipmentDisplay.gameObject.SetActive(false);
        m_SigfridEquipmentDisplay.gameObject.SetActive(false);
        m_inventoryDisplay.HideInventory();
        m_statsScreen.HideDisplay();
        inventoryOnScreen = false;
    }

    public void GrodnarEquipmentDisplay() {
        if (m_currentEquipmentInterface == m_GrodnarEquipmentDisplay) {
            m_currentEquipmentInterface.gameObject.SetActive(true);
            return; 
        }
        m_currentEquipmentInterface.gameObject.SetActive(false);
        m_currentEquipmentInterface = m_GrodnarEquipmentDisplay;
        m_currentEquipmentInterface.gameObject.SetActive(true);
    }

    public void LanstarEquipmentDisplay()
    {
        if (m_currentEquipmentInterface == m_LanstarEquipmentDisplay) {
            m_currentEquipmentInterface.gameObject.SetActive(true);
            return; 
        }
        m_currentEquipmentInterface.gameObject.SetActive(false);
        m_currentEquipmentInterface = m_LanstarEquipmentDisplay;
        m_currentEquipmentInterface.gameObject.SetActive(true);
    }

    public void SigfridEquipmentDisplay()
    {
        if (m_currentEquipmentInterface == m_SigfridEquipmentDisplay) {
            m_currentEquipmentInterface.gameObject.SetActive(true);
            return;
        }
        m_currentEquipmentInterface.gameObject.SetActive(false);
        m_currentEquipmentInterface = m_SigfridEquipmentDisplay;
        m_currentEquipmentInterface.gameObject.SetActive(true);
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
    private void OnApplicationQuit()
    {
        m_inventory.Clear();
        m_GrodnarEquipmentInventory.Clear();
        m_LanstarEquipmentInventory.Clear();
        m_SigfridEquipmentInventory.Clear();
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
