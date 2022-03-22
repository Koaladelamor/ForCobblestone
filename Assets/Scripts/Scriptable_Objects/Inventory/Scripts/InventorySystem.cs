using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;
using System.Runtime.Serialization;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventorySystem : ScriptableObject {
    public string savePath;
    public ItemDB Database;
    public Inventory Container;

    public bool AddItem(Item _item, int _amount) {
        if(EmptySlotCount <= 0) { return false; }
        InventorySlot slot = FindItemOnInventory(_item);
        if (!Database.GetItem[_item.ID].stackable || slot == null) {
            SetEmptySlot(_item, _amount);
            return true;
        }
        slot.AddAmount(_amount);
        return true;
    }
    public int EmptySlotCount 
    {
        get {
            int counter = 0;
            for (int i = 0; i < Container.Items.Length; i++)
            {
                if (Container.Items[i].Item.ID <= -1) { counter++; }
            }
            return counter;
        }
    }

    public InventorySlot FindItemOnInventory(Item _item) {
        for (int i = 0; i < Container.Items.Length; i++)
        {
            if (Container.Items[i].Item.ID == _item.ID) { return Container.Items[i]; }
        }
        return null;
    }
    public InventorySlot SetEmptySlot(Item _item, int _amount) {
        for (int i = 0; i < Container.Items.Length; i++)
        {
            if (Container.Items[i].Item.ID <= -1)
            {
                Container.Items[i].UpdateSlot(_item, _amount);
                return Container.Items[i];
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
        for (int i = 0; i < Container.Items.Length; i++)
        {
            if (Container.Items[i].Item == _item) {
                Container.Items[i].UpdateSlot(null, 0);
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
        for (int i = 0; i < Container.Items.Length; i++)
        {
            Container.Items[i].UpdateSlot(newContainer.Items[i].Item, newContainer.Items[i].Amount);
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
    public InventorySlot[] Items = new InventorySlot[30];
    public void Clear() {
        for (int i = 0; i < Items.Length; i++)
        {
            Items[i].UpdateSlot(new Item(), 0);
        }
    }
}


[System.Serializable]
public class InventorySlot
{
    public ItemType[] AllowedItems = new ItemType[0];
    public UserInterface parent;
    public Item Item;
    public int Amount;

    public ItemObject ItemObject
    { 
        get 
        {
            if (Item.ID >= 0) 
            {
                return parent.mInventory.Database.GetItem[Item.ID];
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
            if (_itemObject.iType == AllowedItems[i])
                return true;
        }
        return false;
    }

    public InventorySlot()
    {
        Item = null;
        Amount = 0;
    }

    public InventorySlot(Item _item, int _amount) {
        Item = _item;
        Amount = _amount;
    }
    public void UpdateSlot(Item _item, int _amount)
    {
        Item = _item;
        Amount = _amount;
    }

    public void AddAmount(int value) {
        Amount += value;
    }

    public void RemoveItem() {
        Item = new Item();
        Amount = 0;
    }

}
