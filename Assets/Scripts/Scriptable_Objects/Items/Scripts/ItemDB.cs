using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item DB", menuName = "Inventory System/Items/DB")]
public class ItemDB : ScriptableObject, ISerializationCallbackReceiver
{
    public ItemObject[] ItemObjects;

    [ContextMenu("Update IDs")]
    public void UpdateID() 
    {
        for (int i = 0; i < ItemObjects.Length; i++)
        {
            if (ItemObjects[i].data.ID != i)
            {
                ItemObjects[i].data.ID = i;
            }
        }
    }

    public int GetRandomID() {
        int ID = Random.Range(0, ItemObjects.Length);
        return ID;
    }

    public void OnAfterDeserialize()
    {
        UpdateID();
    }

    public void OnBeforeSerialize()
    {
        
    }
}
