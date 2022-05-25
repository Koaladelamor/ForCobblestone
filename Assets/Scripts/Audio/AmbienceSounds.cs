using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceSounds : MonoBehaviour
{

    private AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = gameObject.GetComponent<AudioManager>();

        StartCoroutine(PlayBackgroundSounds());
    }

    IEnumerator PlayBackgroundSounds() {

        while (true)
        {
            yield return new WaitForSeconds(Random.Range(12f, 30f));

            int random = Random.Range(1, 4);

            switch (random)
            {
                case 1:
                    audioManager.PlayInstant(AudioManager.InstantAudios.OWL);
                    break;
                case 2:
                    audioManager.PlayInstant(AudioManager.InstantAudios.WOLFHOWL);
                    break;
                case 3:
                    audioManager.PlayInstant(AudioManager.InstantAudios.BIRD);
                    break;
                default:
                    break;

            }
        }
    
    }

}
