using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Trade Obj", menuName = "Inventory System/Items/Trade")]
public class TradeObj : ItemObject
{
    public void Awake()
    {
        Type = ItemType.TRADE;
    }
}
