using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum EnemyType  { MINOTAUR, WOLF, WORM }
public class GameManager : MonoBehaviour
{
    static GameManager mInstance;

    static public GameManager Instance
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

    public GameObject m_minotaurPrefab;
    public GameObject m_wolfPrefab;

    private GameObject m_player;
    private GameObject m_pointToGo;

    public GameObject m_canvasToCombat;

    //Vector3 playerPosition;

    public GameObject enemyOnCombat;
    public EnemyType enemyOnCombatType;

    private Vector3[] enemyRespawnPositions = new Vector3[2];
    public int enemyIndex;
    public int totalEnemies;
    public bool[] enemyIsAlive;
    private GameObject[] enemies = new GameObject[2];

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
        enemyRespawnPositions[0] = new Vector3(-237f, 50f, 0f);
        enemyRespawnPositions[1] = new Vector3(370f, -100f, 0f);
        
        m_canvasToCombat.SetActive(false);

        m_player = GameObject.FindGameObjectWithTag("PlayerMap");
        enemyEngaged = false;

        enemies[0] = RespawnEnemy(m_minotaurPrefab, enemyRespawnPositions[0], 0);
        enemies[1] = RespawnEnemy(m_minotaurPrefab, enemyRespawnPositions[1], 1);

        m_currentEquipmentInterface = m_GrodnarEquipmentDisplay;



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
        m_statsScreen.DisableStatButtons();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            GameStats.Instance.AddXpToGrodnar(600f);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            GameStats.Instance.AddXpToLanstar(600f);
        }
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

            if (m_currentEquipmentInterface.mInventory.type == InventoryType.GRODNAR && GameStats.Instance.GetGrodnar()._attribute_points > 0) { m_statsScreen.EnableStatButtons(); }

            else if (m_currentEquipmentInterface.mInventory.type == InventoryType.LANSTAR && GameStats.Instance.GetLanstar()._attribute_points > 0) { m_statsScreen.EnableStatButtons(); }
            else if (m_currentEquipmentInterface.mInventory.type == InventoryType.SIGFRID && GameStats.Instance.GetSigfrid()._attribute_points > 0) { m_statsScreen.EnableStatButtons(); }

            m_inventoryDisplay.ShowInventory();
            m_statsScreen.ShowDisplay();
            inventoryOnScreen = true;
        }

        if (enemyEngaged)
        {
            enemyEngaged = false;

            //Save stats

            //Enemy info
            enemyIndex = enemyOnCombat.GetComponent<PatrolAI>().enemyID;
            totalEnemies = enemyOnCombat.GetComponent<PatrolAI>().totalEnemies;
            enemyOnCombatType = enemyOnCombat.GetComponent<PatrolAI>().enemyType;

            m_canvasToCombat.SetActive(true);
        }


        if (combatIsOver)
        {
            combatIsOver = false;
            LoadMapScene();
            enemyIsAlive[enemyIndex] = false;
            enemies[enemyIndex].SetActive(false);

        }
    }
    public void OnBeforeSlotUpdate(InventorySlot _slot)
    {
        if (_slot.ItemObject == null) { return; }

        switch (_slot.parent.mInventory.type)
        {
            case InventoryType.MAIN:
                break;
            case InventoryType.GRODNAR:
                print(string.Concat("Removed ", _slot.ItemObject, " on ", _slot.parent.mInventory.type));
                RemoveItemModifier(_slot.Item, GameStats.Instance.Grodnar._stats);
                m_statsScreen.DisplayGrodnarStats();
                break;
            case InventoryType.LANSTAR:
                print(string.Concat("Removed ", _slot.ItemObject, " on ", _slot.parent.mInventory.type));
                RemoveItemModifier(_slot.Item, GameStats.Instance.Lanstar._stats);
                m_statsScreen.DisplayLanstarStats();
                break;
            case InventoryType.SIGFRID:
                print(string.Concat("Removed ", _slot.ItemObject, " on ", _slot.parent.mInventory.type));
                RemoveItemModifier(_slot.Item, GameStats.Instance.Sigfrid._stats);
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
                AddItemModifier(_slot.Item, GameStats.Instance.Grodnar._stats);
                m_statsScreen.DisplayGrodnarStats();
                break;
            case InventoryType.LANSTAR:
                print(string.Concat("Placed ", _slot.ItemObject, " on ", _slot.parent.mInventory.type));
                AddItemModifier(_slot.Item, GameStats.Instance.Lanstar._stats);
                m_statsScreen.DisplayLanstarStats();
                break;
            case InventoryType.SIGFRID:
                print(string.Concat("Placed ", _slot.ItemObject, " on ", _slot.parent.mInventory.type));
                AddItemModifier(_slot.Item, GameStats.Instance.Sigfrid._stats);
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
        if (_item.Buffs.Length == 0) { return; }
        for (int j = 0; j < _item.Buffs.Length; j++)
        {
            for (int i = 0; i < _stat.Count; i++)
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
        if (_item.Buffs.Length == 0) { return; }
        for (int j = 0; j < _item.Buffs.Length; j++)
        {
            for (int i = 0; i < _stat.Count; i++)
            {
                if (_stat[i].attribute == _item.Buffs[j].attribute)
                {
                    _stat[i].value -= _item.Buffs[j].value;
                    AttributeModified(_stat[i]);
                }
            }
        }
    }

    public void HideInventories() {
        HideButtons();
        m_GrodnarEquipmentDisplay.gameObject.SetActive(false);
        m_LanstarEquipmentDisplay.gameObject.SetActive(false);
        m_SigfridEquipmentDisplay.gameObject.SetActive(false);
        m_inventoryDisplay.HideInventory();
        m_statsScreen.DisableStatButtons();
        m_statsScreen.HideDisplay();
        inventoryOnScreen = false;
    }

    public void GrodnarEquipmentDisplay() {
        if (m_currentEquipmentInterface == m_GrodnarEquipmentDisplay) {
            m_currentEquipmentInterface.gameObject.SetActive(true);
            if (GameStats.Instance.GetGrodnar()._attribute_points > 0) { m_statsScreen.EnableStatButtons(); }
            else m_statsScreen.DisableStatButtons();
            return;
        }
        m_currentEquipmentInterface.gameObject.SetActive(false);
        m_currentEquipmentInterface = m_GrodnarEquipmentDisplay;
        m_currentEquipmentInterface.gameObject.SetActive(true);
        if (GameStats.Instance.GetGrodnar()._attribute_points > 0) { m_statsScreen.EnableStatButtons(); }
        else m_statsScreen.DisableStatButtons();
    }

    public void LanstarEquipmentDisplay()
    {
        if (m_currentEquipmentInterface == m_LanstarEquipmentDisplay) {
            m_currentEquipmentInterface.gameObject.SetActive(true);
            if (GameStats.Instance.GetLanstar()._attribute_points > 0) { m_statsScreen.EnableStatButtons(); }
            else m_statsScreen.DisableStatButtons();
            return;
        }
        m_currentEquipmentInterface.gameObject.SetActive(false);
        m_currentEquipmentInterface = m_LanstarEquipmentDisplay;
        m_currentEquipmentInterface.gameObject.SetActive(true);
        if (GameStats.Instance.GetLanstar()._attribute_points > 0) { m_statsScreen.EnableStatButtons(); }
        else m_statsScreen.DisableStatButtons();
    }

    public void SigfridEquipmentDisplay()
    {
        if (m_currentEquipmentInterface == m_SigfridEquipmentDisplay) {
            m_currentEquipmentInterface.gameObject.SetActive(true);
            if (GameStats.Instance.GetSigfrid()._attribute_points > 0) { m_statsScreen.EnableStatButtons(); }
            else m_statsScreen.DisableStatButtons();
            return;
        }
        m_currentEquipmentInterface.gameObject.SetActive(false);
        m_currentEquipmentInterface = m_SigfridEquipmentDisplay;
        m_currentEquipmentInterface.gameObject.SetActive(true);
        if (GameStats.Instance.GetSigfrid()._attribute_points > 0) { m_statsScreen.EnableStatButtons(); }
        else m_statsScreen.DisableStatButtons();
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

    GameObject RespawnEnemy(GameObject type, Vector3 enemySpawnPos, int ID) {
        
        GameObject enemy = Instantiate(type, enemySpawnPos, transform.rotation);
        PatrolAI AI = enemy.GetComponent<PatrolAI>();
        

        AI.patrolPoints[0] = AI.InstantiatePatrolPoint(50f, 50f);
        AI.patrolPoints[1] = AI.InstantiatePatrolPoint(-50f, 50f);
        AI.patrolPoints[2] = AI.InstantiatePatrolPoint(-50f, -50f);
        AI.patrolPoints[3] = AI.InstantiatePatrolPoint(50f, -50f);
        


        enemy.GetComponent<PatrolAI>().enemyID = ID;

        enemy.name = "Enemy " + ID.ToString();



        return enemy;
    }

    void EnemiesRespawner() {
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemyIsAlive[i])
            {
                RespawnEnemy(m_minotaurPrefab, enemyRespawnPositions[i], i);
            }
        }
    }

    public void LoadCombatScene() {
        SceneManager.LoadSceneAsync("CombatScene", LoadSceneMode.Additive);
        //SetActiveScene("CombatScene");
    }

    public void LoadMapScene()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("MapScene"));

        //HideInventories();
        /*GameObject canvasHostal = GameObject.FindGameObjectWithTag("CanvasHostal");
        canvasHostal.SetActive(false);
        GameObject canvasPause = GameObject.FindGameObjectWithTag("CanvasPause");
        canvasPause.SetActive(false);*/
    }

    public void SetActiveScene(string scene) {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene));
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        Debug.Log("OnSceneLoad: "+scene.name);
        Debug.Log(mode);
        SetActiveScene(scene.name);
        DisableCanvas();

    }

    public void DisableCanvas() {
        m_canvasToCombat = GameObject.FindGameObjectWithTag("CanvasToBattle");
        m_canvasToCombat.SetActive(false);
    }

    public UserInterface GetCurrentEquipmentInterface() { return m_currentEquipmentInterface; }

}
