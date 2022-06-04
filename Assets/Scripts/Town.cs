using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Town : MonoBehaviour
{

    [SerializeField] private GameObject m_canvasTown;

    private bool canInteract;

    // Start is called before the first frame update
    void Start()
    {
        canInteract = true;
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerMap") && canInteract)
        {
            m_canvasTown.SetActive(true);
            GameManager.Instance.DisablePartyMovement();
            collision.gameObject.GetComponent<PlayerController>().StopMovement();
            canInteract = false;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerMap"))
        {
            canInteract = true;
        }
    }
    public void CloseTownCanvas()
    {
        m_canvasTown.SetActive(false);
    }
}
