using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ambience_Sounds_Manager : MonoBehaviour
{

    private Audio_Manager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = gameObject.GetComponent<Audio_Manager>();

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
                    audioManager.PlayInstant(Audio_Manager.InstantAudios.OWL);
                    break;
                case 2:
                    audioManager.PlayInstant(Audio_Manager.InstantAudios.WOLFHOWL);
                    break;
                case 3:
                    audioManager.PlayInstant(Audio_Manager.InstantAudios.BIRD);
                    break;
                default:
                    break;

            }
        }
    
    }

}
