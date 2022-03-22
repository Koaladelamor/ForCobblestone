using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*if (collision.gameObject.CompareTag("Square")) {
            if (GetComponent<Draggable>().isDragged == false) { 
                Vector3 squarePos = collision.gameObject.GetComponent<TileManager>().getTransform();
                this.transform.position = squarePos;
            }
        }*/
    }
}
