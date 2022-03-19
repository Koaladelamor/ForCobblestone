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

    public void AddItem(Item _item, int _amount) {

        if (_item.Buffs.Length > 0) {
            Container.Items.Add(new InventorySlot(_item.ID, _item, _amount));
            return;
        }

        for (int i = 0; i < Container.Items.Count; i++) {
            if (Container.Items[i].Item.ID == _item.ID) {
                Container.Items[i].AddAmount(_amount);
                return;
            }
        }
        Container.Items.Add(new InventorySlot(_item.ID, _item, _amount));
    }

    [ContextMenu("Save")]
    public void Save() {

        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, Container);
        stream.Close();

        /*//JSON - better player edit
    string saveData = JsonUtility.ToJson(this, true);
    BinaryFormatter bf = new BinaryFormatter();
    FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
    bf.Serialize(file, saveData);
    file.Close();*/
    }

    [ContextMenu("Load")]
    public void Load() {

        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
        Container = (Inventory)formatter.Deserialize(stream);
        stream.Close();

        /*//JSON - better player edit
    if (File.Exists(string.Concat(Application.persistentDataPath, savePath))) {
    BinaryFormatter bf = new BinaryFormatter();
    FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
    JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
    file.Close();
    }*/
    }

    [ContextMenu("Clear")]
    public void Clear() {
        Container = new Inventory();
    }
}

[System.Serializable]
public class Inventory 
{
    public List<InventorySlot> Items = new List<InventorySlot>();
}


[System.Serializable]
public class InventorySlot
{
    public int ID;
    public Item Item;
    public int Amount;
    public InventorySlot(int _ID, Item _item, int _amount) {
        ID = _ID;
        Item = _item;
        Amount = _amount;
    }

    public void AddAmount(int value) {
        Amount += value;
    }
}
