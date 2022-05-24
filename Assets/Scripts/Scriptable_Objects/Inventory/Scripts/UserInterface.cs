using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;

public abstract class UserInterface : MonoBehaviour
{
    public InventoryObject mInventory;
    public Dictionary<GameObject, InventorySlot> slotsOnInterface = new Dictionary<GameObject, InventorySlot>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < mInventory.GetSlots.Length; i++)
        {
            mInventory.GetSlots[i].parent = this;
            mInventory.GetSlots[i].OnAfterUpdate += OnSlotUpdate;
        }
        CreateSlots();
        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(gameObject); });
        AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(); });
    }

    private void OnSlotUpdate(InventorySlot _slot)
    {
        if (_slot.slotDisplay == null) {
            Debug.LogError("ERROR slotDisplay is null");
            return;
        }
        if (_slot.Item.ID >= 0)
        {
            _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().sprite = _slot.ItemObject.Display;
            _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
            _slot.slotDisplay.transform.GetComponentInChildren<TextMeshProUGUI>().text = _slot.Amount == 1 ? "" : _slot.Amount.ToString();
        }
        else
        {
            _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
            _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
            _slot.slotDisplay.transform.GetComponentInChildren<TextMeshProUGUI>().text = "";
        }
    }

    public void ShowInventory()
    {
        //GetComponentInChildren<Text>().enabled = true;
        GetComponent<Image>().enabled = true;
        Image[] imgs = GetComponentsInChildren<Image>();
        foreach (Image img in imgs)
        {
            img.GetComponent<Image>().enabled = true;
        }

        TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();
        if (texts == null) { return; }
        foreach (TextMeshProUGUI text in texts)
        {
            text.GetComponent<TextMeshProUGUI>().enabled = true;
        }
    }
    public void HideInventory() 
    {
        //GetComponentInChildren<Text>().enabled = false;
        GetComponent<Image>().enabled = false;
        for (int i = 0; i < mInventory.GetSlots.Length; i++)
        {
            Image[] imgs = GetComponentsInChildren<Image>();
            foreach (Image img in imgs)
            {
                img.GetComponent<Image>().enabled = false;
            }
        }

        TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();
        if (texts == null) { return; }
        foreach (TextMeshProUGUI text in texts)
        {
            text.GetComponent<TextMeshProUGUI>().enabled = false;
        }
    }

    // Update is called once per frame
    /*void Update()
    {
        UpdateSlots();
    }*/

    public abstract void CreateSlots();

    public void UpdateSlots()
    {
        foreach (KeyValuePair<GameObject, InventorySlot> _slot in slotsOnInterface)
        {
            if (_slot.Value.Item.ID >= 0)
            {
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = _slot.Value.ItemObject.Display;
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                _slot.Key.transform.GetComponentInChildren<TextMeshProUGUI>().text = _slot.Value.Amount == 1 ? "" : _slot.Value.Amount.ToString();
            }
            else
            {
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
                _slot.Key.transform.GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
        }
    }

    public void OnEnter(GameObject obj)
    {
        MouseData.slotHoveredOver = obj;
        if (slotsOnInterface[obj].Item.Buffs == null) { return; }
        InventoryManager.Instance.itemInfo.SetActive(true);
        InventoryManager.Instance.SetNameInfo(slotsOnInterface[obj].Item.Name);
        InventoryManager.Instance.SetAttributesInfo(slotsOnInterface[obj].Item.Buffs);
    }
    public void OnExit(GameObject obj)
    {
        MouseData.slotHoveredOver = null;
        InventoryManager.Instance.itemInfo.SetActive(false);
        InventoryManager.Instance.ClearAttributesInfo();
        InventoryManager.Instance.ClearNameInfo();
    }
    public void OnDragStart(GameObject obj)
    {
        MouseData.tempItemBeingDragged = CreateTempItem(obj);
    }
    public GameObject CreateTempItem(GameObject obj) {
        GameObject tempItem = null;

        if (slotsOnInterface[obj].Item.ID >= 0) {
            tempItem = new GameObject();
            RectTransform rt = tempItem.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(50, 50);
            tempItem.transform.SetParent(transform.parent);
            Image img = tempItem.AddComponent<Image>();
            img.sprite = slotsOnInterface[obj].ItemObject.Display;
            img.raycastTarget = false;
        }
        return tempItem;
    }
    public void OnDragEnd(GameObject obj)
    {
        Destroy(MouseData.tempItemBeingDragged);

        if (MouseData.interfaceMouseIsOver == null) {
            InventoryManager.Instance.SetSlotSelected(slotsOnInterface[obj]);
            InventoryManager.Instance.ShowDeleteItemScreen();
            /*slotsOnInterface[obj].RemoveItem();
            slotsOnInterface[obj].UpdateSlot(slotsOnInterface[obj].Item, 0);*/
            return;
        }
        if (MouseData.slotHoveredOver) {
            InventorySlot mouseHoverSlotData = MouseData.interfaceMouseIsOver.slotsOnInterface[MouseData.slotHoveredOver];
            mInventory.SwapItems(mouseHoverSlotData, slotsOnInterface[obj]);
        }
    }
    public void OnDrag()
    {
        if (MouseData.tempItemBeingDragged != null)
        {
            MouseData.tempItemBeingDragged.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }

    protected void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void OnEnterInterface(GameObject obj)
    {
        MouseData.interfaceMouseIsOver = obj.GetComponent<UserInterface>();
    }

    public void OnExitInterface()
    {
        MouseData.interfaceMouseIsOver = null;
    }

}

public static class MouseData
{
    public static UserInterface interfaceMouseIsOver;
    public static GameObject tempItemBeingDragged;
    public static GameObject slotHoveredOver;

}