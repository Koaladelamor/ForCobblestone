using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Attributes { HEALTH, MIN_DAMAGE, MAX_DAMAGE, STRENGHT, STAMINA, AGILITY, INTELLIGENCE, LAST_NO_USE }
public class Stat
{
    public Attributes attribute;
    public int baseValue;
    public int value;

    public Stat(Attributes _attribute, int _baseValue) {
        attribute = _attribute;
        baseValue = _baseValue;
        value = _baseValue;
    }

    public Stat(Attributes _attribute, int _baseValue, int _value)
    {
        attribute = _attribute;
        baseValue = _baseValue;
        value = _value;
    }


    public int CalculateFinalValue(InventoryObject _inventory, Stat _stat) 
    {
        int finalValue = _stat.baseValue;

        for (int i = 0; i < _inventory.GetSlots.Length; i++)
        {
            if (_inventory.GetSlots[i].Item.ID >= 0) 
            {
                for (int k = 0; k < _inventory.GetSlots[i].Item.Buffs.Length; k++)
                {
                    if (_inventory.GetSlots[i].Item.Buffs[k].attribute == _stat.attribute)
                    {
                        finalValue += _inventory.GetSlots[i].Item.Buffs[k].value;
                    }
                }
            }
        }
        
        return finalValue;
    }

}
