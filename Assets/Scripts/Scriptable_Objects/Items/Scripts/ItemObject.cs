using UnityEngine;

public enum ItemType { WEAPON, SHIELD, HELMET, CHEST, BOOTS, CONSUMABLE, TRADE, LAST_NO_USE };

public abstract class ItemObject : ScriptableObject
{
    public bool Stackable;
    public Sprite Display;
    public ItemType Type;
    [TextArea(5, 10)] 
    public string Description;
    public int Value;
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
    public InventoryType Holder;
    public InventoryType PreviousHolder;

    public Item() {
        Name = "";
        ID = -1;
        Holder = InventoryType.LAST_NO_USE;
        PreviousHolder = InventoryType.LAST_NO_USE;
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
        Holder = InventoryType.LAST_NO_USE;
        PreviousHolder = InventoryType.LAST_NO_USE;
    }

}

[System.Serializable]
public class ItemBuff 
{
    public Attributes attribute;
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
