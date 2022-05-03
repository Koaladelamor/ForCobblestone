using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestControl : MonoBehaviour
{

    public GameObject ChestOpen;
    public GameObject ChestClosed;
    public GameObject E_;
    public bool Chest;
   
    void Update()
    {
        if (Chest==true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {

                // GetItem();
                ChestClosed.SetActive(false);
                ChestOpen.SetActive(true);
                
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

    