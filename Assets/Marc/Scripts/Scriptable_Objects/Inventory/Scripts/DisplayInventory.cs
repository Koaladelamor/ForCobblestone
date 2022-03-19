using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DisplayInventory : MonoBehaviour
{
    public GameObject inventoryPrefab;

    public InventorySystem mInventory;

    public int X_START;
    public int Y_START;
    public int X_SPACING;
    public int Y_SPACING;
    public int COLUMN_NUMBER;

    Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        CreateDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDisplay();
    }

    public void UpdateDisplay() {
        for (int i = 0; i < mInventory.Container.Items.Count; i++)
        {
            InventorySlot slot = mInventory.Container.Items[i];

            if (itemsDisplayed.ContainsKey(slot))
            {
                itemsDisplayed[slot].GetComponentInChildren<TextMeshProUGUI>().text = slot.Amount.ToString();
            }
            else {
                GameObject obj = Instantiate(inventoryPrefab, Vector3.zero, transform.rotation, transform);
                obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite = mInventory.Database.GetItem[slot.Item.ID].iDisplay;
                obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
                obj.GetComponentInChildren<TextMeshProUGUI>().text = slot.Amount.ToString();
                itemsDisplayed.Add(slot, obj);
            }

        }
    }

    public void CreateDisplay() {
        for (int i = 0; i < mInventory.Container.Items.Count; i++) {
            InventorySlot slot = mInventory.Container.Items[i];

            GameObject obj = Instantiate(inventoryPrefab, Vector3.zero, transform.rotation, transform);
            obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite = mInventory.Database.GetItem[slot.Item.ID].iDisplay;
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = slot.Amount.ToString();
            
        }
    }

    public Vector3 GetPosition(int i) {
        return new Vector3(X_START + (X_SPACING * (i % COLUMN_NUMBER)), Y_START + (-Y_SPACING * (i/COLUMN_NUMBER)), 0f);
    }
}
