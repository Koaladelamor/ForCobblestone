using UnityEngine;

public class PartyTrigger : MonoBehaviour
{

    private bool canInteract;

    private void Start()
    {
        canInteract = true;    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Gate"))
        {
            collision.gameObject.GetComponent<Animator>().enabled = true;
        }

        if (collision.gameObject.CompareTag("BossTrigger"))
        {
            GameManager.Instance.LoadBossScene();
            AudioManager.Instance.partyFX.Stop();
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Campfire") && canInteract)
        {
            canInteract = false;
            GameManager.Instance.StopMovement();
            GameManager.Instance.DisablePartyMovement();
            AudioManager.Instance.partyFX.Stop();
            CanvasManager.Instance.m_canvasCampfire.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Campfire"))
        {
            canInteract = true;
        }
    }
}
