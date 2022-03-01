using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marti_Vision_Range : MonoBehaviour
{
    public bool playerDetected;
    // Start is called before the first frame update
    void Start()
    {
        playerDetected = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerMap"))
        {
            playerDetected = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerMap"))
        {
            playerDetected = false;
        }
    }
}
