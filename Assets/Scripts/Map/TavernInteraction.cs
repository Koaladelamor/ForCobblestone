using UnityEngine;

public class TavernInteraction : MonoBehaviour
{

    [SerializeField] GameObject m_canvasHostal;

    private bool canInteract;

    // Start is called before the first frame update
    void Start()
    {
        canInteract = true;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerMap") && canInteract)
        {
            m_canvasHostal.SetActive(true);
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

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerMap") && canInteract)
        {
            m_canvasHostal.SetActive(true);
            Time.timeScale = 0;
            canInteract = false;

        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerMap"))
        {
            canInteract = true;
        }
    }*/

    public void CloseTavernCanvas() {
        m_canvasHostal.SetActive(false);
        Time.timeScale = 1;
        InventoryManager.Instance.inventoryBlackScreen.SetActive(false);
    }

}
