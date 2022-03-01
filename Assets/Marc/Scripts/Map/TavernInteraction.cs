using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TavernInteraction : MonoBehaviour
{

    private GameObject m_canvasHostal;
    private GameObject sleeping;

    // Start is called before the first frame update
    void Start()
    {
        sleeping = GameObject.FindGameObjectWithTag("Sleeping");
        sleeping.SetActive(false);
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

    public void Sleeping()
    {
        sleeping.SetActive(true);
    }

}
