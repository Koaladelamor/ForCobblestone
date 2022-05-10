using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapToCombat : MonoBehaviour
{
    private GameObject gameManager;
    // Start is called before the first frame update
    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        this.gameObject.SetActive(false);
    }

    /*private void Update()
    {
        if (gameManager.GetComponent<GameManager>().enemyEngaged == true) {
            this.gameObject.SetActive(true);

        }
    }*/

}
