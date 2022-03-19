using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item DB", menuName = "Inventory System/Items/DB")]
public class ItemDB : ScriptableObject, ISerializationCallbackReceiver
{
    public ItemObject[] Items;

    public Dictionary<int, ItemObject> GetItem = new Dictionary<int, ItemObject>();

    public void OnAfterDeserialize()
    {
        for (int i = 0; i < Items.Length; i++) {
            Items[i].ID = i;
            GetItem.Add(i, Items[i]);
        }
    }

    public void OnBeforeSerialize()
    {
        GetItem = new Dictionary<int, ItemObject>();
    }
}
