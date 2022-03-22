using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Chest", menuName = "Inventory System/Items/Chest")]
public class ChestObj : ItemObject
{
    public void Awake()
    {
        iType = ItemType.CHEST;
    }

}
