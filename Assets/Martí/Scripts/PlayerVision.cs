using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVision : MonoBehaviour
{
    public bool enemyDetected;

    private GameObject[] enemy;
    // Start is called before the first frame update
    void Start()
    {
        /*
        enemyDetected = false;

        enemy = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemy.Length; i++)
        {
            enemy[i].GetComponent<SpriteRenderer>().enabled = false;
        }
        */
    }

    // Update is called once per frame
    void Update()
    {
        //transform = GetComponentInParent<Transform>();
        
        
        /*
        if (enemyDetected)
        {
            for (int i = 0; i < enemy.Length; i++)
            {
                enemy[i].GetComponent<SpriteRenderer>().enabled = true;
            }
        }
        */
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            
            //enemyDetected = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            //enemyDetected = false;
            other.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
