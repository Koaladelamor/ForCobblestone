using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject[] itemTest;
    public InventorySystem m_inventory;
    private GameObject m_gameManager;
    private GameObject m_pointToGo;
    public bool engaged;

    Vector2 previousPosition;
    Vector2 currentPosition;

    private Animator[] anims;
    private SpriteRenderer[] sprites;

    // Start is called before the first frame update
    void Start()
    {
        m_gameManager = GameObject.FindGameObjectWithTag("GameManager");
        engaged = false;

        m_pointToGo = GameObject.FindGameObjectWithTag("PointToGo");

        anims = GetComponentsInChildren<Animator>();
        sprites = GetComponentsInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) {
            m_inventory.Save();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            m_inventory.Load();
        }

        previousPosition = currentPosition;
        currentPosition = transform.position;
        FlipSprites();

        if (Input.GetKeyDown(KeyCode.E)) {
            //ItemToPick item = itemTest[0].GetComponent<ItemToPick>();
            foreach (GameObject _item in itemTest){
                if (_item)
                {
                    ItemToPick item = _item.GetComponent<ItemToPick>(); 
                    m_inventory.AddItem(new Item(item.item), 1);
                    Destroy(item.gameObject);
                }
            }

        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("EnemyMap")) {
            //Debug.Log("entra");
            engaged = true;
            m_gameManager.GetComponent<Game_Manager>().enemyEngaged = true;

            m_gameManager.GetComponent<Game_Manager>().enemyOnCombat = other.gameObject;

            other.gameObject.GetComponent<Collider2D>().enabled = false;
            m_pointToGo.transform.position = transform.position;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("EnemyMap"))
        {
            engaged = false;

        }
    }

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Item")) {

        }
    }*/

    private void OnApplicationQuit()
    {
        m_inventory.Container.Items.Clear();
    }

    public bool PartyIsMoving()
    {

        if (currentPosition != previousPosition)
        {
            return true;
        }

        return false;
    }

    public void SetWalkingAnimation()
    {
        if (PartyIsMoving())
        {
            for (int i = 0; i < anims.Length; i++)
            {
                anims[i].SetBool("isWalking", true);
            }
            return;
        }
        else {
            for (int i = 0; i < anims.Length; i++)
            {
                anims[i].SetBool("isWalking", false);
            }
        }
    }

    public void FlipSprites() {
        if (currentPosition.x > previousPosition.x) {
            for (int i = 0; i < sprites.Length; i++) {
                sprites[i].flipX = false;
            }
        }
        else if(currentPosition.x < previousPosition.x) {
            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i].flipX = true;
            }
        }
    }




}
