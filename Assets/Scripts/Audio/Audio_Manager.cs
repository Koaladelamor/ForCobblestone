using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Audio_Manager : MonoBehaviour
{

    static Audio_Manager mInstance;

    static public Audio_Manager Instance
    {
        get { return mInstance; }
        private set { }
    }

    [SerializeField] private AudioSource effectsSounds;
    [SerializeField] private AudioSource BackgroungMusic;

    [SerializeField] private AudioClip selectSound;
    [SerializeField] private AudioClip pressedSound;
    [SerializeField] private AudioClip bagCloseSound;
    [SerializeField] private AudioClip bagOpenSound;
    [SerializeField] private AudioClip arrowHitFlesh;
    [SerializeField] private AudioClip beOnYourGuard;
    [SerializeField] private AudioClip birdSound;
    [SerializeField] private AudioClip chestOpeningSound;
    [SerializeField] private AudioClip coinDroppedSound;
    [SerializeField] private AudioClip dramaticBirdsSound;
    [SerializeField] private AudioClip owlSound;
    [SerializeField] private AudioClip wolfHowlSound;
    [SerializeField] private AudioClip swordHitFleshSound;
    [SerializeField] private AudioClip swordHitObjectSound;
    [SerializeField] private AudioClip swordHitMetalSound;
    [SerializeField] private AudioClip GrodnarDeathSound;
    [SerializeField] private AudioClip SigfridDeathSound;
    [SerializeField] private AudioClip LanstarDeathSound;
    [SerializeField] private AudioClip BellSound;



    [SerializeField] private AudioClip enemyHurt;
    [SerializeField] private AudioClip playerHurt;

    [SerializeField] private AudioClip lifeSound;

    //AudioClip[] 

    //Hacer enum de audio sources

    private void Awake()
    {
        //Singleton
        if (mInstance == null)
        {
            mInstance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

    }

    public enum AudioEffects { NONE, SELECT, PRESSED, LIFESOUND };

    public enum InstantAudios { NONE, ENEMYHURT, PLAYERHURT, BAGCLOSE, BAGOPEN, ARROWFLESH, ONGUARD, BIRD, CHESTOPEN, COIN, DRAMATICBIRD, OWL, WOLFHOWL, SWORDFLESH,SWORDOBJECT, SWORDMETAL, GRODNARDEATH, SIGFRIDDEATH, LANSTARDEATH, BELLSOUND };

    //*****************************************
    public void PlayOnce(AudioEffects effect) {

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
            case AudioEffects.LIFESOUND:
                effectsSounds.clip = bagCloseSound;
                break;
            default:
                break;
        }

        effectsSounds.Play();

    }

    public void PlayInstant(InstantAudios effect) 
    {

        switch (effect)
        {
            case InstantAudios.NONE:
                break;
            case InstantAudios.ENEMYHURT:
                effectsSounds.PlayOneShot(enemyHurt);
                break;
            case InstantAudios.PLAYERHURT:
                effectsSounds.PlayOneShot(playerHurt);
                break;
            case InstantAudios.BAGCLOSE:
                effectsSounds.PlayOneShot(bagCloseSound);
                break;
            case InstantAudios.BAGOPEN:
                effectsSounds.PlayOneShot(bagOpenSound);
                break;
            case InstantAudios.ARROWFLESH:
                effectsSounds.PlayOneShot(arrowHitFlesh);
                break;
            case InstantAudios.ONGUARD:
                effectsSounds.PlayOneShot(beOnYourGuard);
                break;
            case InstantAudios.BIRD:
                effectsSounds.PlayOneShot(birdSound);
                break;
            case InstantAudios.CHESTOPEN:
                effectsSounds.PlayOneShot(chestOpeningSound);
                break;
            case InstantAudios.COIN:
                effectsSounds.PlayOneShot(coinDroppedSound);
                break;
            case InstantAudios.DRAMATICBIRD:
                effectsSounds.PlayOneShot(dramaticBirdsSound);
                break;
            case InstantAudios.OWL:
                effectsSounds.PlayOneShot(owlSound);
                break;
            case InstantAudios.WOLFHOWL:
                effectsSounds.PlayOneShot(wolfHowlSound);
                break;
            case InstantAudios.SWORDFLESH:
                effectsSounds.PlayOneShot(swordHitFleshSound);
                break;
            case InstantAudios.SWORDOBJECT:
                effectsSounds.PlayOneShot(swordHitObjectSound);
                break;
            case InstantAudios.SWORDMETAL:
                effectsSounds.PlayOneShot(swordHitMetalSound);
                break;
            case InstantAudios.GRODNARDEATH:
                effectsSounds.PlayOneShot(GrodnarDeathSound);
                break;
            case InstantAudios.SIGFRIDDEATH:
                effectsSounds.PlayOneShot(SigfridDeathSound);
                break;
            case InstantAudios.LANSTARDEATH:
                effectsSounds.PlayOneShot(LanstarDeathSound);
                break;
            case InstantAudios.BELLSOUND:
                effectsSounds.PlayOneShot(BellSound);
                break;
            default:
                break;
        }

    }

    public void PlaySelect(BaseEventData data)
    {
        PlayOnce(AudioEffects.SELECT);
    }

    public void PlayPressed(BaseEventData data) 
    {
        PlayOnce(AudioEffects.PRESSED);
    }

    public void PlayDeathSound(PawnController.CHARACTER ch) {
        //Debug.Log(ch.ToString());
        switch (ch)
        {
            case PawnController.CHARACTER.GRODNAR:
                PlayInstant(InstantAudios.GRODNARDEATH);
                break;
            case PawnController.CHARACTER.LANSTAR:
                PlayInstant(InstantAudios.LANSTARDEATH);
                break;
            case PawnController.CHARACTER.SIGFRID:
                PlayInstant(InstantAudios.SIGFRIDDEATH);
                break;
            case PawnController.CHARACTER.SPIDER:
                break;
            case PawnController.CHARACTER.WORM:
                break;
            case PawnController.CHARACTER.LAST_NO_USE:
                break;
            default:
                break;
        }
    }

    public void PlayAttackSound(PawnController.CHARACTER ch) {
        //Debug.Log(ch.ToString());
        switch (ch)
        {
            case PawnController.CHARACTER.GRODNAR:

            case PawnController.CHARACTER.SIGFRID:
                PlayInstant(InstantAudios.SWORDFLESH);
                break;
            case PawnController.CHARACTER.LANSTAR:

            case PawnController.CHARACTER.SPIDER:
                
            case PawnController.CHARACTER.WORM:
                PlayInstant(InstantAudios.ARROWFLESH);
                break;
            case PawnController.CHARACTER.LAST_NO_USE:
                break;
            default:
                break;
        }
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