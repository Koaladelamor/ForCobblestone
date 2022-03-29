using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Audio_Manager : MonoBehaviour
{

    [SerializeField] private AudioSource effectsSounds;
    [SerializeField] private AudioSource BackgroungMusic;

    [SerializeField] private AudioClip selectSound;
    [SerializeField] private AudioClip pressedSound;

    [SerializeField] private AudioClip enemyHurt;
    [SerializeField] private AudioClip playerHurt;

    [SerializeField] private AudioClip lifeSound;

    //AudioClip[] 

    //Hacer enum de audio sources

    public enum AudioEffects { NONE, SELECT, PRESSED, ENEMYHURT, PLAYERHURT, LIFESOUND };

    //*****************************************
    public void PlayEffect(AudioEffects effect) {

        effectsSounds.Stop();

        switch (effect)
        {
            case AudioEffects.NONE:
                break;
            case AudioEffects.SELECT:
                effectsSounds.clip = selectSound;
                break;
            case AudioEffects.PRESSED:
                effectsSounds.clip = pressedSound;
                break;
            case AudioEffects.ENEMYHURT:
                effectsSounds.clip = enemyHurt;
                break;
            case AudioEffects.PLAYERHURT:
                effectsSounds.clip = playerHurt;
                break;
            case AudioEffects.LIFESOUND:
                effectsSounds.clip = lifeSound;
                break;
            default:
                break;
        }

        effectsSounds.Play();

    }

    public void PlaySelect(BaseEventData data)
    {
        PlayEffect(AudioEffects.SELECT);
    }

    public void PlayPressed(BaseEventData data) 
    {
        PlayEffect(AudioEffects.PRESSED);
    }


    //public enum BackgroundSongs { NONE, MENUAUDIO, EXPLOREAUDIO, BATTLEAUDIO };

    ////public BackgroundSongs canciones;

    /*
    private void Update()
    {

    }

    private void OnMouseEnter()
    {
        SelectedSound();
    }

    private void OnMouseUp()
    {
        MouseUpSound();
    }

    public void SelectedSound()
    {

        buttonSounds.clip = selectSound;
        buttonSounds.Play();
    }

    public void MouseUpSound()
    {
        buttonSounds.clip = pressedSound;
        buttonSounds.Play();
    }

    void lifeRecoveredSound()
    {
        buttonSounds.clip = lifeSound;
        buttonSounds.Play();
    }
    */

    /*
    public void PlayTrack(AudioSource _audio) {
        _audio.Play();
    }
    */

    /*
    public void SetBackgroundSong(BackgroundSongs _song) {

        canciones = _song;
        BackgroungMusic.Play();


    }
    */

    /*
    private void chooseBackgroundSound() {
        
        switch (canciones)
        {
            case BackgroundSongs.NONE:

                break;
            case BackgroundSongs.MENUAUDIO:

                BackgroungMusic.clip = menuMusic;
                BackgroungMusic.Play();
                BackgroungMusic.
                break;
            case BackgroundSongs.EXPLOREAUDIO:
                break;
            case BackgroundSongs.BATTLEAUDIO:
                break;
            default:
                break;
        }
    }
    */
}