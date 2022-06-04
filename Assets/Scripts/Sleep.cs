using UnityEngine;

public class Sleep : MonoBehaviour
{
    public void YawnSound() {
        AudioManager.Instance.PlayInstant(AudioManager.InstantAudios.YAWN);
        CanvasManager.Instance.SetYawnSoundBool(true);
    }
}
