using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapToCombatSound : MonoBehaviour
{

    public GameObject battleButton;

    public AudioSource buttonSounds;

    public AudioClip buttonSelected;
    public AudioClip buttonPressed;


    
    public void SelectedSound() {
        buttonSounds.clip = buttonSelected;
        buttonSounds.Play();
    }

    public void MouseUpSound() {
        buttonSounds.clip = buttonPressed;
        buttonSounds.Play();
    }

}
