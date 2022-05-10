using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

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
    public UserInterface m_CombatLootDisplay;
    public UserInterface m_ChestLootDisplay;
    public UserInterface m_TavernTradeDisplay;

    public StatsScreen m_statsScreen;

    public InventoryObject m_inventory;
    public InventoryObject m_GrodnarEquipmentInventory;
    public InventoryObject m_LanstarEquipmentInventory;
    public InventoryObject m_SigfridEquipmentInventory;
    public InventoryObject m_CombatLootInventory;
    public InventoryObject m_ChestInventory;
    public InventoryObject m_TavernTradeInventory;

    private bool inventoryOnScreen = false;

    public Button[] equipmentButtons;
    public Button confirmLootButton;
    public Button confirmChestButton;
    public Button confirmTradeButton;

    public GameObject m_spiderPrefab;

    private int currentEnemyID;
    //private GameObject enemyOnCombat;
    public EnemySpawner[] enemySpawners;

    public bool enemyEngaged;
    private bool combatIsOver;

    public GameObject m_canvasTavern;
    public GameObject m_canvasToCombat;
    public GameObject m_canvasPause;
    private bool gameIsPaused;

    public TextMeshProUGUI coinsAmount;
    public TextMeshProUGUI balanceAmount;
    private int tradeBalance;
    private bool addingItems;

    public GameObject levelUpWarning;

    public GameObject canvasMenu;
    public bool isMenuOnScreen;


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

        combatIsOver = false;
        enemyEngaged = false;
        currentEnemyID = -1;
        gameIsPaused = false;
        tradeBalance = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        if(this == null) { return; }

        /*for (int i = 0; i < enemySpawners.Length; i++)
        {
            enemySpawners[i].RespawnEnemy(m_spiderPrefab, enemySpawners[i].transform.position, i);
        }*/

        ClearInventories();
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

        for (int i = 0; i < m_inventory.GetSlots.Length; i++)
        {
            m_inventory.GetSlots[i].OnBeforeUpdate += OnBeforeSlotUpdate;
            m_inventory.GetSlots[i].OnAfterUpdate += OnAfterSlotUpdate;

        }

        for (int i = 0; i < m_TavernTradeInventory.GetSlots.Length; i++)
        {
            m_TavernTradeInventory.GetSlots[i].OnBeforeUpdate += OnBeforeSlotUpdate;
            m_TavernTradeInventory.GetSlots[i].OnAfterUpdate += OnAfterSlotUpdate;

        }

        for (int i = 0; i < m_CombatLootInventory.GetSlots.Length; i++)
        {
            m_CombatLootInventory.GetSlots[i].OnBeforeUpdate += OnBeforeSlotUpdate;
            m_CombatLootInventory.GetSlots[i].OnAfterUpdate += OnAfterSlotUpdate;

        }

        for (int i = 0; i < m_ChestInventory.GetSlots.Length; i++)
        {
            m_ChestInventory.GetSlots[i].OnBeforeUpdate += OnBeforeSlotUpdate;
            m_ChestInventory.GetSlots[i].OnAfterUpdate += OnAfterSlotUpdate;

        }

        Invoke("HideInventories", 0.1f);
        Invoke("InitTavernLoot", 0.12f);
        m_canvasPause.SetActive(false);
        DisableCombatCanvas();

        SceneManager.sceneLoaded += OnSceneLoaded;

        /*for (int i = 0; i < 7; i++)
        {
            m_ChestInventory.AddItem(m_ChestInventory.GenerateRandomItem(), 1, InventoryType.CHEST);
        }*/
        //GenerateRandomChest((int)Random.Range(1, 16));

    }

    void Update()
    {
        if (InputManager.Instance.PauseButtonPressed && !gameIsPaused)
        {
            PauseGame();

        }
        else if (InputManager.Instance.PauseButtonPressed && gameIsPaused)
        {
            NormalSpeed();
        }

        if (InputManager.Instance.NormalSpeedButtonPressed) {
            NormalSpeed();
        } 
        else if (InputManager.Instance.FastSpeedButtonPressed) {
            FastSpeed();
        }

        if (InputManager.Instance.MenuButtonPressed && isMenuOnScreen)
        {
            canvasMenu.SetActive(false);
            isMenuOnScreen = false;
        }
        else if (InputManager.Instance.MenuButtonPressed && !isMenuOnScreen)
        {
            canvasMenu.SetActive(true);
            isMenuOnScreen = true;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            GameStats.Instance.AddCoins(2000);
            UpdateCoinsAmount();
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            GameStats.Instance.AddXpToGrodnar(600f);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            GameStats.Instance.AddXpToLanstar(600f);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            GameStats.Instance.AddXpToSigfrid(600f);
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
            Audio_Manager.Instance.PlayInstant(Audio_Manager.InstantAudios.BAGCLOSE);
        }
        else if (InputManager.Instance.InventoryButtonPressed && !inventoryOnScreen)
        {
            ShowInventories();
            Audio_Manager.Instance.PlayInstant(Audio_Manager.InstantAudios.BAGOPEN);
        }

        if (enemyEngaged)
        {
            enemyEngaged = false;

            //Save stats

            //Enemy info
            Debug.Log(currentEnemyID);
            EnableCombatCanvas();
        }


        if (combatIsOver)
        {
            LoadMapScene();

            GenerateRandomLoot((int)Random.Range(1, 6));
            GameStats.Instance.AddCoins(500);
            GameStats.Instance.AddXpToGrodnar(800f);
            GameStats.Instance.AddXpToLanstar(800f);
            GameStats.Instance.AddXpToSigfrid(800f);

            for (int i = 0; i < enemySpawners.Length; i++)
            {
                if (enemySpawners[i].GetSpawnerID() == currentEnemyID)
                {
                    enemySpawners[i].GetEnemy().GetComponent<PatrolAI>().SetAlive(false);
                }
            }
            NormalSpeed();

            UpdateCoinsAmount();
            DisplayLoot();

            combatIsOver = false;
        }
    }
    public void OnBeforeSlotUpdate(InventorySlot _slot)
    {
        if (_slot.ItemObject == null) { return; }
        if (this == null) { return; }

        switch (_slot.parent.mInventory.type)
        {
            case InventoryType.MAIN:
                _slot.Item.PreviousHolder = _slot.parent.mInventory.type;
                print(string.Concat("Removed ", _slot.ItemObject, " on ", _slot.parent.mInventory.type));
                break;
            case InventoryType.GRODNAR:
                _slot.Item.PreviousHolder = _slot.parent.mInventory.type;
                print(string.Concat("Removed ", _slot.ItemObject, " on ", _slot.parent.mInventory.type));
                RemoveItemModifier(_slot.Item, GameStats.Instance.Grodnar._stats);
                m_statsScreen.DisplayGrodnarStats();
                break;
            case InventoryType.LANSTAR:
                _slot.Item.PreviousHolder = _slot.parent.mInventory.type;
                print(string.Concat("Removed ", _slot.ItemObject, " on ", _slot.parent.mInventory.type));
                RemoveItemModifier(_slot.Item, GameStats.Instance.Lanstar._stats);
                m_statsScreen.DisplayLanstarStats();
                break;
            case InventoryType.SIGFRID:
                _slot.Item.PreviousHolder = _slot.parent.mInventory.type;
                print(string.Concat("Removed ", _slot.ItemObject, " on ", _slot.parent.mInventory.type));
                RemoveItemModifier(_slot.Item, GameStats.Instance.Sigfrid._stats);
                m_statsScreen.DisplaySigfridStats();
                break;
            case InventoryType.TRADE:
                _slot.Item.PreviousHolder = _slot.parent.mInventory.type;
                print(string.Concat("Removed ", _slot.ItemObject, " on ", _slot.parent.mInventory.type));
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
        if (this == null) { 
            return; 
        }

        switch (_slot.parent.mInventory.type)
        {
            case InventoryType.MAIN:
                if (!addingItems && _slot.Item.PreviousHolder == InventoryType.TRADE)
                {
                    tradeBalance -= _slot.ItemObject.Value;
                    UpdateBalanceAmount();
                }
                print(string.Concat("Placed ", _slot.ItemObject, " on ", _slot.parent.mInventory.type));
                _slot.Item.Holder = InventoryType.MAIN;
                break;
            case InventoryType.GRODNAR:
                print(string.Concat("Placed ", _slot.ItemObject, " on ", _slot.parent.mInventory.type));
                AddItemModifier(_slot.Item, GameStats.Instance.Grodnar._stats);
                m_statsScreen.DisplayGrodnarStats();
                _slot.Item.Holder = InventoryType.GRODNAR;
                break;
            case InventoryType.LANSTAR:
                print(string.Concat("Placed ", _slot.ItemObject, " on ", _slot.parent.mInventory.type));
                AddItemModifier(_slot.Item, GameStats.Instance.Lanstar._stats);
                m_statsScreen.DisplayLanstarStats();
                _slot.Item.Holder = InventoryType.LANSTAR;
                break;
            case InventoryType.SIGFRID:
                print(string.Concat("Placed ", _slot.ItemObject, " on ", _slot.parent.mInventory.type));
                AddItemModifier(_slot.Item, GameStats.Instance.Sigfrid._stats);
                m_statsScreen.DisplaySigfridStats();
                _slot.Item.Holder = InventoryType.SIGFRID;
                break;
            case InventoryType.TRADE:
                if (!addingItems && _slot.Item.PreviousHolder == InventoryType.MAIN)
                {
                    tradeBalance += _slot.ItemObject.Value;
                    UpdateBalanceAmount();
                }
                print(string.Concat("Placed ", _slot.ItemObject, " on ", _slot.parent.mInventory.type));
                _slot.Item.Holder = InventoryType.TRADE;
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
        m_CombatLootDisplay.HideInventory();
        confirmLootButton.gameObject.SetActive(false);
        confirmChestButton.gameObject.SetActive(false);
        m_inventoryDisplay.HideInventory();
        m_statsScreen.DisableStatButtons();
        m_statsScreen.HideDisplay();
        m_ChestLootDisplay.HideInventory();
        
        inventoryOnScreen = false;

        if (GameStats.Instance.GetGrodnar()._attribute_points == 0 && GameStats.Instance.GetLanstar()._attribute_points == 0 && GameStats.Instance.GetSigfrid()._attribute_points == 0)
        {
            SetLvlUpWarning(false);
        }
    }

    public void ShowInventories() {
        ShowButtons();
        m_currentEquipmentInterface.gameObject.SetActive(true);

        if (m_currentEquipmentInterface.mInventory.type == InventoryType.GRODNAR && GameStats.Instance.GetGrodnar()._attribute_points > 0) { m_statsScreen.EnableStatButtons(); }

        else if (m_currentEquipmentInterface.mInventory.type == InventoryType.LANSTAR && GameStats.Instance.GetLanstar()._attribute_points > 0) { m_statsScreen.EnableStatButtons(); }
        else if (m_currentEquipmentInterface.mInventory.type == InventoryType.SIGFRID && GameStats.Instance.GetSigfrid()._attribute_points > 0) { m_statsScreen.EnableStatButtons(); }

        m_inventoryDisplay.ShowInventory();
        m_statsScreen.ShowDisplay();
        inventoryOnScreen = true;
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

    public void InitTavernLoot() {
        addingItems = true;
        for (int i = 0; i < 20; i++)
        {
            m_TavernTradeInventory.AddItem(m_TavernTradeInventory.GenerateRandomItem(), 1, InventoryType.TRADE);
        }
        addingItems = false;
        m_TavernTradeDisplay.HideInventory();
        confirmTradeButton.gameObject.SetActive(false);
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
        m_CombatLootInventory.Clear();
        m_TavernTradeInventory.Clear();
        m_ChestInventory.Clear();
    }

    private void ClearInventories() {
        m_inventory.Clear();
        m_GrodnarEquipmentInventory.Clear();
        m_LanstarEquipmentInventory.Clear();
        m_SigfridEquipmentInventory.Clear();
        m_CombatLootInventory.Clear();
        m_TavernTradeInventory.Clear();
        m_ChestInventory.Clear();
    }



    /*void EnemiesRespawner() {
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemyIsAlive[i])
            {
                RespawnEnemy(m_spiderPrefab, enemyRespawnPositions[i], i);
            }
        }
    }*/

    public void LoadCombatScene() {
        GameObject[] gameObjectsOnScene;
        gameObjectsOnScene = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (GameObject obj in gameObjectsOnScene)
        {
            obj.SetActive(false);
        }
        SceneManager.LoadSceneAsync("CombatScene", LoadSceneMode.Additive);
    }

    public void LoadMapScene()
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("CombatScene"));
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("MapScene"));
        GameObject[] gameObjectsOnScene;
        gameObjectsOnScene = SceneManager.GetSceneByName("MapScene").GetRootGameObjects();
        foreach (GameObject obj in gameObjectsOnScene)
        {
            obj.SetActive(true);
        }

        HideInventories();
        DisableCombatCanvas();
        GameObject canvasHostal = GameObject.FindGameObjectWithTag("CanvasHostal");
        canvasHostal.SetActive(false);
        GameObject canvasPause = GameObject.FindGameObjectWithTag("CanvasPause");
        canvasPause.SetActive(false);
        canvasMenu.SetActive(false);

    }

    public void SetActiveScene(string scene) {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene));
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        Debug.Log("OnSceneLoad: "+scene.name);
        SetActiveScene(scene.name);
    }

    public void DisableCombatCanvas() {
        m_canvasToCombat.SetActive(false);
    }

    public void EnableCombatCanvas()
    {
        m_canvasToCombat.SetActive(true);
        //Audio_Manager.Instance.PlayInstant(Audio_Manager.InstantAudios.BELLSOUND);
    }

    public UserInterface GetCurrentEquipmentInterface() { return m_currentEquipmentInterface; }

    public void PauseGame()
    {
        m_canvasPause.SetActive(true);
        gameIsPaused = true;
        Time.timeScale = 0;
    }

    public void NormalSpeed()
    {
        Time.timeScale = 1;
        m_canvasPause.SetActive(false);
        gameIsPaused = false;
    }

    public void FastSpeed()
    {
        Time.timeScale = 2;
        m_canvasPause.SetActive(false);
        gameIsPaused = false;
    }

    public void InventoryButton() {
        if (inventoryOnScreen) {
            HideInventories();
        }
        else {
            ShowInventories();
        }

    }

    public void UpdateCoinsAmount() {
        coinsAmount.text = GameStats.Instance.GetCoins().ToString();
    }

    public void UpdateBalanceAmount()
    {
        balanceAmount.text = tradeBalance.ToString();
    }

    public void GenerateRandomLoot(int itemsToAdd) {
        for (int i = 0; i < itemsToAdd; i++)
        {
            m_CombatLootInventory.AddItem(m_CombatLootInventory.GenerateRandomItem(), 1, InventoryType.LOOT);
        }
    }

    public void GenerateRandomChest(int itemsToAdd)
    {
        for (int i = 0; i < itemsToAdd; i++)
        {
            m_ChestInventory.AddItem(m_ChestInventory.GenerateRandomItem(), 1, InventoryType.CHEST);
        }
    }

    public void DisplayLoot() {
        m_CombatLootDisplay.ShowInventory();
        m_inventoryDisplay.ShowInventory();
        confirmLootButton.gameObject.SetActive(true);
    }

    public void ConfirmLoot() {
        m_CombatLootDisplay.HideInventory();
        m_inventoryDisplay.HideInventory();
        m_CombatLootInventory.Clear();
        confirmLootButton.gameObject.SetActive(false);
    }

    public void ConfirmChestLoot()
    {
        m_ChestLootDisplay.HideInventory();
        m_inventoryDisplay.HideInventory();
        m_ChestInventory.Clear();
        confirmChestButton.gameObject.SetActive(false);
    }

    public void ConfirmTrade() {
        GameStats.Instance.AddCoins(tradeBalance);
        tradeBalance = 0;
        UpdateBalanceAmount();
        UpdateCoinsAmount();
        m_inventoryDisplay.HideInventory();
        m_TavernTradeDisplay.HideInventory();
        confirmTradeButton.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void TradingModeON() {
        tradeBalance = 0;
        m_inventoryDisplay.ShowInventory();
        m_TavernTradeDisplay.ShowInventory();
        confirmTradeButton.gameObject.SetActive(true);
        m_canvasTavern.SetActive(false);
    }
    private void OnDestroy()
    {
        foreach (EnemySpawner spawner in enemySpawners)
        {
            if (spawner != null)
            {
                Destroy(spawner.GetEnemy());
                Destroy(spawner.gameObject);
            }
        }
    }
    public void TradingModeOFF()
    {
        m_inventoryDisplay.HideInventory();
        m_TavernTradeDisplay.HideInventory();
        confirmTradeButton.gameObject.SetActive(false);
        m_canvasTavern.SetActive(true);
        Time.timeScale = 1;
    }

    public bool GetCombatIsOver() { return combatIsOver; }

    public void SetCombatIsOver(bool isCombatOver) { combatIsOver = isCombatOver; }

    //public void SetEnemyOnCombat(GameObject enemy) { enemyOnCombat = enemy; }

    //public GameObject GetEnemyOnCombat() { return enemyOnCombat; }

    public void SetAddingItemsBool(bool _addingItems) { addingItems = _addingItems; }

    public void SetLvlUpWarning(bool lvlUp) { levelUpWarning.SetActive(lvlUp); }

    public void SetCurrentEnemyID(int ID) { currentEnemyID = ID; }

    public GameObject GetSpiderPrefab() { return m_spiderPrefab; }

    public void OpenChestInventory() {
        m_ChestLootDisplay.ShowInventory();
        confirmChestButton.gameObject.SetActive(true);
    }

    public void CloseChestInventory()
    {
        m_ChestLootDisplay.HideInventory();
    }

    public void OpenMainInventory() {
        m_inventoryDisplay.ShowInventory();
    }

    public void CloseMainInventory()
    {
        m_inventoryDisplay.HideInventory();
    }


}
