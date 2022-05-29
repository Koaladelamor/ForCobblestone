using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionRange : MonoBehaviour
{
    private bool playerDetected;
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
        if (other.gameObject.CompareTag("PlayerMap")) {
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

    public bool GetPlayerDetected() { return playerDetected; }
}
