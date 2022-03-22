using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { WEAPON, SHIELD, HELMET, CHEST, BOOTS, CONSUMABLE, TRADE, LAST_NO_USE };

public enum Attribute { STRENGTH, STAMINA, AGILITY };
public abstract class ItemObject : ScriptableObject
{
    public bool stackable;
    public Sprite iDisplay;
    public ItemType iType;
    [TextArea(5, 10)] 
    public string iDescription;
    public Item data = new Item();
    public Item CreateItem()
    {
        Item newItem = new Item(this);
        return newItem;
    }
}



[System.Serializable]
public class Item 
{

    public string Name;
    public int ID = -1;
    public ItemBuff[] Buffs;

    public Item() {
        Name = "";
        ID = -1;
    }

    public Item(ItemObject _item) {
        Name = _item.name;
        ID = _item.data.ID;
        Buffs = new ItemBuff[_item.data.Buffs.Length];
        for (int i = 0; i < Buffs.Length; i++) {
            Buffs[i] = new ItemBuff(_item.data.Buffs[i].min, _item.data.Buffs[i].max)
            {
                attribute = _item.data.Buffs[i].attribute
            };

        }
    }

}

[System.Serializable]
public class ItemBuff 
{
    public Attribute attribute;
    public int value;
    public int min;
    public int max;
    public ItemBuff(int _min, int _max) {
        min = _min;
        max = _max;
        GenerateValue();
    }

    public void GenerateValue() {
        value = UnityEngine.Random.Range(min, max);
    }

}
