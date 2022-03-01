using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marti_Player_Controller : MonoBehaviour
{
    private GameObject m_gameManager;
    private Marti_Player_Vision_Range _visionRange;
    public bool engaged;
    // Start is called before the first frame update
    void Start()
    {
        m_gameManager = GameObject.FindGameObjectWithTag("GameManager");
        _visionRange = GetComponentInChildren<Marti_Player_Vision_Range>();
        engaged = false;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("entra");
            engaged = true;
            m_gameManager.GetComponent<Marti_Game_Manager>().enemyEngaged = true;

            m_gameManager.GetComponent<Marti_Game_Manager>().enemyOnCombat = other.gameObject;


        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            engaged = false;

        }
    }
}
