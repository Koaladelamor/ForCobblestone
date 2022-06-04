using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Attributes { CURR_HEALTH, MAX_HEALTH, MIN_DAMAGE, MAX_DAMAGE, STRENGHT, STAMINA, AGILITY, LAST_NO_USE }
public class Stat
{
    public Attributes attribute;
    public int baseValue;
    public int mods;
    public int value;

    public Stat(Attributes _attribute, int _baseValue, int _mods) {
        attribute = _attribute;
        baseValue = _baseValue;
        mods = _mods;
        value = _baseValue + _mods;
    }

    public Stat(Attributes _attribute, int _baseValue)
    {
        attribute = _attribute;
        baseValue = _baseValue;
        mods = 0;
        value = _baseValue;
    }

    public Stat AddStatMods(Stat _stat, int _mods) {
        _stat.mods = _mods;
        _stat.value = _stat.baseValue + _stat.mods;
        return _stat;
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
