using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{

    public AudioMixer Mixer;
    public Slider BMSlider;
    public Slider SFXSlider;

    public bool FXMuted = false;
    public bool MusicMuted = false;

    // Start is called before the first frame update
    void Start()
    {
        float value;

        Mixer.GetFloat("VolumeBM", out value);
        BMSlider.value = Mathf.Pow(10f, value/20f);

        Mixer.GetFloat("VolumeSFX", out value);
        SFXSlider.value = Mathf.Pow(10f, value / 20f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetVolumeBM(float volume) {
        Mixer.SetFloat("VolumeBM", Mathf.Log10(volume)*20);
    }

    public void SetVolumeSFX(float volume){
        Mixer.SetFloat("VolumeSFX", Mathf.Log10(volume) * 20);
    }
    /*
    public void MuteBM() {
        SetVolumeBM(0.001f);
        BMSlider.value = 0.001f;
    }

    public void MuteSFX() {
        SetVolumeSFX(0.001f);
        SFXSlider.value = 0.001f;
    }

    public void ToggleFX(bool mutedFx) {

        if (mutedFx = false)
        {
            MuteSFX();
            mutedFx = true;
        }

        if (mutedFx = true)
        {
            
        }
    }

    public void ToggleMusic(bool mutedBm) {
        if (mutedBm = false)
        {
            MuteBM();
            mutedBm = true;
        }

        if (mutedBm = true)
        {

        }
    }
    */
}
