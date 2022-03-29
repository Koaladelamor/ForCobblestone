using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable", menuName = "Inventory System/Items/Consumable")]
public class ConsumableObj : ItemObject
{
    public void Awake()
    {
        Type = ItemType.CONSUMABLE;
    }
}
