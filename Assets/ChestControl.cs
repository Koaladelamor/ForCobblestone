using System.Collections;
using System.Collections.Generic;
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
                GameManager.Instance.GenerateRandomChest((int)Random.Range(1, 16));
                ChestClosed.SetActive(false);
                ChestOpen.SetActive(true);
                GameManager.Instance.OpenChestInventory();
                GameManager.Instance.OpenMainInventory();
                itemsPicked = true;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerMap")
        {
            Chest = true;
            E_.SetActive(true);
         
        }
        else
        {
            Chest = false;
            E_.SetActive(false);
        }
    }

    public void GetItem()
    {
        //aqui va lo del inventario
    }
}

    