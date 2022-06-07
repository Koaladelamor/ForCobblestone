using UnityEngine;

public class EndGameAnim : MonoBehaviour
{

    public GameObject endGame;

    public void StartCrowdAmbience() {
        AudioManager.Instance.StopMusic();
        AudioManager.Instance.ChangeBackgroundMusic(AudioManager.Instance.crowdAmbience);
        AudioManager.Instance.PlayMusic();
    }

    public void StartVictoryTheme()
    {
        AudioManager.Instance.StopMusic();
        AudioManager.Instance.ChangeBackgroundMusic(AudioManager.Instance.endGameVictory);
        AudioManager.Instance.PlayMusic();
    }

    public void StopMusic() { AudioManager.Instance.StopMusic(); }

    public void EnableEndGamePanel() {
        endGame.SetActive(true);
    }

}
