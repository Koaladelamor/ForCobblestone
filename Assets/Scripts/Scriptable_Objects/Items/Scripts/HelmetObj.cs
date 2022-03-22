using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Helmet", menuName = "Inventory System/Items/Helmet")]
public class HelmetObj : ItemObject
{
    public void Awake()
    {
        iType = ItemType.HELMET;
    }
}
