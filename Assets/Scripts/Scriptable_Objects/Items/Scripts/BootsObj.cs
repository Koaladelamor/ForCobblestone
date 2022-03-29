using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Boots", menuName = "Inventory System/Items/Boots")]
public class BootsObj : ItemObject
{
    public void Awake()
    {
        Type = ItemType.BOOTS;
    }
}
