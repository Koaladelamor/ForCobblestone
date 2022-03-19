using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Weapon", menuName = "Inventory System/Items/Weapon")]
public class WeaponObj : ItemObject
{
    public void Awake()
    {
        iType = ItemType.WEAPON;
    }

}
