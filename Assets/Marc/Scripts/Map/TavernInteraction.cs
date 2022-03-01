using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TavernInteraction : MonoBehaviour
{

    private GameObject m_canvasHostal;

    // Start is called before the first frame update
    void Start()
    {
        m_canvasHostal = GameObject.FindGameObjectWithTag("CanvasHostal");
        m_canvasHostal.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerMap"))
        {
            m_canvasHostal.SetActive(true);

        }

    }

    public void closeTavernCanvas() {
        m_canvasHostal.SetActive(false);

    }


}
