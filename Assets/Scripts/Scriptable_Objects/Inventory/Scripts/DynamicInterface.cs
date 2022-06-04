using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DynamicInterface : UserInterface
{
    public GameObject inventoryPrefab;

    public int X_START;
    public int Y_START;
    public int X_SPACING;
    public int Y_SPACING;
    public int COLUMN_NUMBER;


    public override void CreateSlots()
    {

        slotsOnInterface = new Dictionary<GameObject, InventorySlot>();

        for (int i = 0; i < mInventory.GetSlots.Length; i++)
        {
            GameObject obj = Instantiate(inventoryPrefab, Vector3.zero, transform.rotation, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);

            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(); });
            if (mInventory.type == InventoryType.MAIN) {
                AddEvent(obj, EventTriggerType.PointerClick, delegate { OnClick(obj); });
            }

            mInventory.GetSlots[i].slotDisplay = obj;
            slotsOnInterface.Add(obj, mInventory.GetSlots[i]);
        }
    }

    private Vector3 GetPosition(int i)
    {
        return new Vector3(X_START + (X_SPACING * (i % COLUMN_NUMBER)), Y_START + (-Y_SPACING * (i / COLUMN_NUMBER)), 0f);
    }

}
