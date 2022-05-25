using UnityEngine;

public class PlayerVisionRange : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("EnemyMap"))
        {
            AudioManager.Instance.PlayInstant(AudioManager.InstantAudios.BELLSOUND);
            //other.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            SpriteRenderer[] sprites = other.gameObject.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer sprite in sprites)
            {
                sprite.enabled = true;
            }
            other.gameObject.GetComponentInChildren<Canvas>().enabled = true;
        }
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("EnemyMap"))
        {
            //other.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            SpriteRenderer[] sprites = other.gameObject.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer sprite in sprites)
            {
                sprite.enabled = false;
            }
            other.gameObject.GetComponentInChildren<Canvas>().enabled = false;
        }
        
    }
}
