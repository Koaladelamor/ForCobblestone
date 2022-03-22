using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisionRange : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("EnemyMap"))
        {
            other.gameObject.GetComponent<SpriteRenderer>().enabled = true;

            //enemyDetected = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("EnemyMap"))
        {
            //enemyDetected = false;
            other.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
