using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controlador_Player : MonoBehaviour
{
    private GameObject m_gameManager;
    private PlayerVisionRange _visionRange;
    public bool engaged;
    // Start is called before the first frame update
    void Start()
    {
        m_gameManager = GameObject.FindGameObjectWithTag("GameManager");
        _visionRange = GetComponentInChildren<PlayerVisionRange>();
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
            m_gameManager.GetComponent<GameManager>().enemyEngaged = true;

            //m_gameManager.GetComponent<GameManager>().enemyOnCombat = other.gameObject;


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
