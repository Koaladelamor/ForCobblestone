using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { WEAPON, GEAR, CONSUMABLE, TRADE, LAST_NO_USE };

public enum Attribute { STRENGTH, STAMINA, AGILITY };
public abstract class ItemObject : ScriptableObject
{
    public int ID;
    public Sprite iDisplay;
    public ItemType iType;
    [TextArea(5, 10)] public string iDescription;
    public ItemBuff[] Buffs;
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
    public int ID;
    public ItemBuff[] Buffs;
    public Item(ItemObject _item) {
        Name = _item.name;
        ID = _item.ID;
        Buffs = new ItemBuff[_item.Buffs.Length];
        for (int i = 0; i < Buffs.Length; i++) {
            Buffs[i] = new ItemBuff(_item.Buffs[i].min, _item.Buffs[i].max)
            {
                attribute = _item.Buffs[i].attribute
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
