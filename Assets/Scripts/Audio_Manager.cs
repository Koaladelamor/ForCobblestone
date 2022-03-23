using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_Manager : MonoBehaviour
{

    static Audio_Manager mAudio;

    static public Audio_Manager Instance
    {
        get { return mAudio; }
        private set { }
    }

    public GameObject anyButton;

    public AudioSource buttonSounds;
    public AudioSource BackgroungMusic;

    public AudioClip menuMusic;

    public AudioClip selectSound;
    public AudioClip pressedSound;

    public AudioClip enemyHurt;
    public AudioClip playerHurt;

    public AudioClip lifeSound;

    //AudioClip[] 

    //Hacer enum de audio sources

    public enum BackgroundSongs { NONE, MENUAUDIO, EXPLOREAUDIO, BATTLEAUDIO };

    public BackgroundSongs canciones;

    private void Start()
    {
        if (mAudio == null)
        {
            mAudio = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

        BackgroungMusic.clip = menuMusic;
        BackgroungMusic.Play();
        //BackgroungMusic.
    }

    private void Update()
    {
        switch (canciones)
        {
            case BackgroundSongs.NONE:

                break;
            case BackgroundSongs.MENUAUDIO:

                BackgroungMusic.clip = menuMusic;
                BackgroungMusic.Play();
                //BackgroungMusic.
                break;
            case BackgroundSongs.EXPLOREAUDIO:
                break;
            case BackgroundSongs.BATTLEAUDIO:
                break;
            default:
                break;
        }

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

    public void PlayTrack(AudioSource _audio) {
        _audio.Play();
    }

    public void SetBackgroundSong(BackgroundSongs _song) {

        canciones = _song;
        BackgroungMusic.Play();


    }

}