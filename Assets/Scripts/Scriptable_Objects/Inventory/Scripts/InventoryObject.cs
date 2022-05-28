using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;

public enum InventoryType { MAIN, GRODNAR, LANSTAR, SIGFRID, LOOT, TRADE, CHEST, LAST_NO_USE};

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject {
    public InventoryType type;
    public string savePath;
    public ItemDB Database;
    public Inventory Container;
    public InventorySlot[] GetSlots { get { return Container.Slots; } }

    public bool AddItem(Item _item, int _amount) {
        if(EmptySlotCount <= 0) { return false; }
        InventorySlot slot = FindItemOnInventory(_item);
        if (!Database.ItemObjects[_item.ID].Stackable || slot == null) {
            SetEmptySlot(_item, _amount);
            return true;
        }
        slot.AddAmount(_amount);
        return true;
    }

    public bool AddItem(Item _item, int _amount, InventoryType _invType)
    {
        if (EmptySlotCount <= 0) {
            Debug.Log("Inventory is full");
            return false;
        }
        InventorySlot slot = FindItemOnInventory(_item);
        _item.PreviousHolder = _invType;
        _item.Holder = _invType;
        if (!Database.ItemObjects[_item.ID].Stackable || slot == null)
        {
            SetEmptySlot(_item, _amount);
            return true;
        }
        slot.AddAmount(_amount);
        return true;
    }

    public Item GenerateRandomItem() {
        int randomID = Database.GetRandomID();
        Item item = new Item(Database.ItemObjects[randomID]);
        return item;
    }

    public int EmptySlotCount 
    {
        get {
            int counter = 0;
            for (int i = 0; i < GetSlots.Length; i++)
            {
                if (GetSlots[i].Item.ID <= -1) { counter++; }
            }
            return counter;
        }
    }

    public InventorySlot FindItemOnInventory(Item _item) {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].Item.ID == _item.ID) { return GetSlots[i]; }
        }
        return null;
    }
    public InventorySlot SetEmptySlot(Item _item, int _amount) {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].Item.ID <= -1)
            {
                GetSlots[i].UpdateSlot(_item, _amount);
                return GetSlots[i];
            }
        }
        //inventory is full
        return null;
    }

    public InventorySlot GetEmptySlot()
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].Item.ID <= -1)
            {
                return GetSlots[i];
            }
        }
        //inventory is full
        return null;
    }

    public void SwapItems(InventorySlot item1, InventorySlot item2) {

        if (item2.CanPlaceInSlot(item1.ItemObject) && item1.CanPlaceInSlot(item2.ItemObject)) {
            InventorySlot temp = new InventorySlot(item2.Item, item2.Amount);
            item2.UpdateSlot(item1.Item, item1.Amount);
            item1.UpdateSlot(temp.Item, temp.Amount);
        }

    }

    public void RemoveItem(Item _item) {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].Item == _item) {
                GetSlots[i].UpdateSlot(null, 0);
            }
        }
    }

    [ContextMenu("Save")]
    public void Save() {

        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, Container);
        stream.Close();
    }
    /*//JSON - better player edit
string saveData = JsonUtility.ToJson(this, true);
BinaryFormatter bf = new BinaryFormatter();
FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
bf.Serialize(file, saveData);
file.Close();*/


    [ContextMenu("Load")]
    public void Load() {

        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
        Inventory newContainer = (Inventory)formatter.Deserialize(stream);
        for (int i = 0; i < GetSlots.Length; i++)
        {
            GetSlots[i].UpdateSlot(newContainer.Slots[i].Item, newContainer.Slots[i].Amount);
        }
        stream.Close();
    }
    /*//JSON - better player edit
if (File.Exists(string.Concat(Application.persistentDataPath, savePath))) {
BinaryFormatter bf = new BinaryFormatter();
FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
file.Close();
}*/


    [ContextMenu("Clear")]
    public void Clear() 
    {
        Container.Clear();
    }

}

[System.Serializable]
public class Inventory 
{
    public InventorySlot[] Slots = new InventorySlot[30];
    public void Clear() {
        for (int i = 0; i < Slots.Length; i++)
        {
            Slots[i].RemoveItem();
        }
    }
}

public delegate void SlotUpdated(InventorySlot _slot);

[System.Serializable]
public class InventorySlot
{
    public ItemType[] AllowedItems = new ItemType[0];
    [System.NonSerialized]
    public UserInterface parent;
    [System.NonSerialized]
    public GameObject slotDisplay;
    [System.NonSerialized]
    public SlotUpdated OnAfterUpdate;
    [System.NonSerialized]
    public SlotUpdated OnBeforeUpdate;
    public Item Item;
    public int Amount;

    public ItemObject ItemObject
    { 
        get 
        {
            if (Item.ID >= 0) 
            {
                //if (parent == null) { return null; }
                return parent.mInventory.Database.ItemObjects[Item.ID];
            }
            return null;
        } 
    }

    public bool CanPlaceInSlot(ItemObject _itemObject)
    {
        if (AllowedItems.Length <= 0 || _itemObject == null || _itemObject.data.ID < 0)
            return true;
        for (int i = 0; i < AllowedItems.Length; i++)
        {
            if (_itemObject.Type == AllowedItems[i])
                return true;
        }
        return false;
    }

    public InventorySlot()
    {
        UpdateSlot(new Item(), 0);
    }

    public InventorySlot(Item _item, int _amount) {
        UpdateSlot(_item, _amount);

    }
    public void UpdateSlot(Item _item, int _amount)
    {
        if(OnBeforeUpdate != null) { OnBeforeUpdate.Invoke(this); }
        Item = _item;
        Amount = _amount;
        if (OnAfterUpdate != null) { OnAfterUpdate.Invoke(this); }
    }

    public void AddAmount(int value) {
        UpdateSlot(Item, Amount += value);
    }

    public void RemoveItem() {
        Item = new Item();
        Amount = 0;
    }

}
