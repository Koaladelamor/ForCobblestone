using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Shield", menuName = "Inventory System/Items/Shield")]
public class ShieldObj : ItemObject
{
    public void Awake()
    {
        iType = ItemType.SHIELD;
    }
}
