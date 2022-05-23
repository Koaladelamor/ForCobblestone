using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{

    static InventoryManager mInstance;

    static public InventoryManager Instance
    {
        get { return mInstance; }
        private set { }
    }

    public UserInterface m_inventoryDisplay;
    public UserInterface m_GrodnarEquipmentDisplay;
    public UserInterface m_LanstarEquipmentDisplay;
    public UserInterface m_SigfridEquipmentDisplay;
    private UserInterface m_currentEquipmentInterface;
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

    public Button[] equipmentButtons;
    public Button confirmLootButton;
    public Button confirmChestButton;
    public Button confirmTradeButton;

    public GameObject inventoryBlackScreen;

    private bool inventoryOnScreen;

    private InventorySlot slotSelected;
    public GameObject deleteItemScreen;

    public TextMeshProUGUI coinsAmount;
    public TextMeshProUGUI balanceAmount;
    public TextMeshProUGUI payText;
    public TextMeshProUGUI receiveText;
    private int tradeBalance;
    private bool addingItems;

    public GameObject levelUpWarning;

    public GameObject itemInfo;
    public Text nameInfo;
    public Text[] attributesInfo;

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

        inventoryOnScreen = false;
        slotSelected = null;
        tradeBalance = 0;
    }

    private void Start()
    {

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

        HideInventories();
        InitTavernLoot();

    }

    private void Update()
    {
        if (InputManager.Instance.InventoryButtonPressed && inventoryOnScreen)
        {
            HideInventories();
            Audio_Manager.Instance.PlayInstant(Audio_Manager.InstantAudios.BAGCLOSE);
            GameManager.Instance.EnablePartyMovement();
        }
        else if (InputManager.Instance.InventoryButtonPressed && !inventoryOnScreen)
        {
            ShowInventories();
            Audio_Manager.Instance.PlayInstant(Audio_Manager.InstantAudios.BAGOPEN);
            GameManager.Instance.StopMovement();
            GameManager.Instance.DisablePartyMovement();
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
        if (this == null)
        {
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

    public void AttributeModified(Stat _stat)
    {
        Debug.Log(string.Concat(_stat.attribute, " was updated. Value is ", _stat.value));
    }

    public void ShowStats(List<Stat> playerStats)
    {
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

    private void ClearInventories()
    {
        m_inventory.Clear();
        m_GrodnarEquipmentInventory.Clear();
        m_LanstarEquipmentInventory.Clear();
        m_SigfridEquipmentInventory.Clear();
        m_CombatLootInventory.Clear();
        m_TavernTradeInventory.Clear();
        m_ChestInventory.Clear();
    }

    public void HideInventories()
    {
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

        inventoryBlackScreen.SetActive(false);
    }

    public void ShowInventories()
    {
        ShowButtons();
        m_currentEquipmentInterface.gameObject.SetActive(true);

        if (m_currentEquipmentInterface.mInventory.type == InventoryType.GRODNAR && GameStats.Instance.GetGrodnar()._attribute_points > 0) { m_statsScreen.EnableStatButtons(); }

        else if (m_currentEquipmentInterface.mInventory.type == InventoryType.LANSTAR && GameStats.Instance.GetLanstar()._attribute_points > 0) { m_statsScreen.EnableStatButtons(); }
        else if (m_currentEquipmentInterface.mInventory.type == InventoryType.SIGFRID && GameStats.Instance.GetSigfrid()._attribute_points > 0) { m_statsScreen.EnableStatButtons(); }

        m_inventoryDisplay.ShowInventory();
        m_statsScreen.ShowDisplay();
        inventoryOnScreen = true;

        inventoryBlackScreen.SetActive(true);
    }

    public void GrodnarEquipmentDisplay()
    {
        if (m_currentEquipmentInterface == m_GrodnarEquipmentDisplay)
        {
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
        if (m_currentEquipmentInterface == m_LanstarEquipmentDisplay)
        {
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
        if (m_currentEquipmentInterface == m_SigfridEquipmentDisplay)
        {
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

    public void InitTavernLoot()
    {
        addingItems = true;
        for (int i = 0; i < 20; i++)
        {
            m_TavernTradeInventory.AddItem(m_TavernTradeInventory.GenerateRandomItem(), 1, InventoryType.TRADE);
        }
        addingItems = false;
        m_TavernTradeDisplay.HideInventory();
        confirmTradeButton.gameObject.SetActive(false);
    }

    public void HideButtons()
    {
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

    public void UpdateCoinsAmount()
    {
        coinsAmount.text = GameStats.Instance.GetCoins().ToString();
    }

    public void UpdateBalanceAmount()
    {
        if (tradeBalance < 0)
        {
            payText.enabled = true;
            receiveText.enabled = false;
            int balance = tradeBalance * -1;
            balanceAmount.text = balance.ToString() + " coins";
        }
        else if (tradeBalance > 0)
        {
            payText.enabled = false;
            receiveText.enabled = true;
            balanceAmount.text = tradeBalance.ToString() + " coins";
        }

    }

    public void GenerateRandomLoot(int itemsToAdd)
    {
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

    public void DisplayLoot()
    {
        m_CombatLootDisplay.ShowInventory();
        m_inventoryDisplay.ShowInventory();
        confirmLootButton.gameObject.SetActive(true);
        inventoryBlackScreen.SetActive(true);
        GameManager.Instance.pointToGo.SetMovement(false);
    }

    public void ConfirmLoot()
    {
        m_CombatLootDisplay.HideInventory();
        m_inventoryDisplay.HideInventory();
        m_CombatLootInventory.Clear();
        confirmLootButton.gameObject.SetActive(false);
        inventoryBlackScreen.SetActive(false);
        GameManager.Instance.pointToGo.SetMovement(true);
    }

    public void ConfirmChestLoot()
    {
        m_ChestLootDisplay.HideInventory();
        m_inventoryDisplay.HideInventory();
        m_ChestInventory.Clear();
        confirmChestButton.gameObject.SetActive(false);
        inventoryBlackScreen.SetActive(false);
        GameManager.Instance.pointToGo.SetMovement(true);
    }

    public void ConfirmTrade()
    {
        GameStats.Instance.AddCoins(tradeBalance);
        tradeBalance = 0;
        UpdateBalanceAmount();
        UpdateCoinsAmount();
        m_inventoryDisplay.HideInventory();
        m_TavernTradeDisplay.HideInventory();
        confirmTradeButton.gameObject.SetActive(false);
        CanvasManager.Instance.m_canvasTavern.SetActive(true);
    }

    public void TradingModeOFF()
    {
        m_inventoryDisplay.HideInventory();
        m_TavernTradeDisplay.HideInventory();
        confirmTradeButton.gameObject.SetActive(false);
        inventoryBlackScreen.SetActive(false);
    }

    public void TradingModeON()
    {
        tradeBalance = 0;
        m_inventoryDisplay.ShowInventory();
        m_TavernTradeDisplay.ShowInventory();
        confirmTradeButton.gameObject.SetActive(true);
        CanvasManager.Instance.m_canvasTavern.SetActive(false);
        inventoryBlackScreen.SetActive(true);
        payText.enabled = false;
    }

    public void InventoryButton()
    {
        if (inventoryOnScreen)
        {
            HideInventories();
            inventoryOnScreen = false;
            GameManager.Instance.EnablePartyMovement();
        }
        else
        {
            ShowInventories();
            inventoryOnScreen = true;
            GameManager.Instance.StopMovement();
            GameManager.Instance.DisablePartyMovement();
        }

    }

    public void OpenChestInventory()
    {
        m_ChestLootDisplay.ShowInventory();
        confirmChestButton.gameObject.SetActive(true);
    }



    public UserInterface GetCurrentEquipmentInterface() { return m_currentEquipmentInterface; }

    public void CloseChestInventory() { m_ChestLootDisplay.HideInventory(); }

    public void OpenMainInventory() { m_inventoryDisplay.ShowInventory(); }

    public void CloseMainInventory() { m_inventoryDisplay.HideInventory(); }

    public void SetAddingItemsBool(bool _addingItems) { addingItems = _addingItems; }

    public void SetSlotSelected(InventorySlot _slot) { slotSelected = _slot; }

    public void RemoveItem()
    {
        slotSelected.RemoveItem();
        slotSelected.UpdateSlot(slotSelected.Item, 0);
        deleteItemScreen.SetActive(false);
    }

    public void ShowDeleteItemScreen() { deleteItemScreen.SetActive(true); }
    public void HideDeleteItemScreen() { deleteItemScreen.SetActive(false); }

    public void SetLvlUpWarning(bool lvlUp) { levelUpWarning.SetActive(lvlUp); }

    public void SetNameInfo(string name) { nameInfo.text = name; }

    public void ClearNameInfo() { nameInfo.text = " "; }

    public void SetAttributesInfo(ItemBuff[] buffs) {
        for (int i = 0; i < buffs.Length; i++)
        {
            attributesInfo[i].text = buffs[i].attribute.ToString() + " " + buffs[i].value.ToString();
        }
    }

    public void ClearAttributesInfo() {
        for (int i = 0; i < attributesInfo.Length; i++)
        {
            attributesInfo[i].text = " ";
        }
    }

}
