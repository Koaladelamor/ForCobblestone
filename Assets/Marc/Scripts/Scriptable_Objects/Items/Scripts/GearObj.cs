using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Gear", menuName = "Inventory System/Items/Gear")]
public class GearObj : ItemObject
{
    public void Awake()
    {
        iType = ItemType.GEAR;
    }

}
