using UnityEngine;

public class ChestControl : MonoBehaviour
{

    public GameObject ChestOpen;
    public GameObject ChestClosed;
    public GameObject E_;
    public bool Chest;
    private bool itemsPicked;

    private void Start()
    {
        itemsPicked = false;
    }

    void Update()
    {
        if (Chest==true)
        {
            if (Input.GetKeyDown(KeyCode.E) && !itemsPicked)
            {
                InventoryManager.Instance.m_ChestInventory.Clear();
                InventoryManager.Instance.GenerateRandomChest((int)Random.Range(1, 14));
                ChestClosed.SetActive(false);
                ChestOpen.SetActive(true);
                InventoryManager.Instance.OpenChestInventory();
                InventoryManager.Instance.OpenMainInventory();
                InventoryManager.Instance.inventoryBlackScreen.SetActive(true);
                GameManager.Instance.DisablePartyMovement();
                itemsPicked = true;
                E_.SetActive(false);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerMap" && !itemsPicked)
        {
            Chest = true;
            E_.SetActive(true);
         
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "PlayerMap")
        {
            Chest = false;
            E_.SetActive(false);
        }
    }

}

    