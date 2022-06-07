using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{

    static CanvasManager mInstance;

    static public CanvasManager Instance
    {
        get { return mInstance; }
        private set { }
    }

    public GameObject m_canvasTavern;
    public GameObject m_canvasTown;
    public GameObject m_canvasToCombat;
    public GameObject m_canvasPause;
    public GameObject m_canvasMenu;
    public GameObject m_canvasCampfire;
    public GameObject blackScreen;

    private bool isMenuOnScreen;
    private bool gameIsPaused;

    public GameObject sleepAnimation;

    private bool yawnSoundStarted;
    private float sleepTimer;
    private float sleepMaxTimer;

    public GameObject wellRested;
    private bool wellRestedTextShown;


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

        gameIsPaused = false;
        isMenuOnScreen = false;
        yawnSoundStarted = false;
        sleepTimer = 0;
        sleepMaxTimer = 2.5f;
        wellRestedTextShown = false;
    }

    private void Update()
    {
        if (InputManager.Instance.PauseButtonPressed && !gameIsPaused)
        {
            PauseGame();
        }
        else if (InputManager.Instance.PauseButtonPressed && gameIsPaused)
        {
            NormalSpeed();
        }

        if (InputManager.Instance.NormalSpeedButtonPressed)
        {
            NormalSpeed();
        }
        else if (InputManager.Instance.FastSpeedButtonPressed)
        {
            FastSpeed();
        }

        if (InputManager.Instance.MenuButtonPressed && isMenuOnScreen)
        {
            m_canvasMenu.SetActive(false);
            isMenuOnScreen = false;
            GameManager.Instance.EnablePartyMovement();
            Time.timeScale = 1;
        }
        else if (InputManager.Instance.MenuButtonPressed && !isMenuOnScreen)
        {
            m_canvasMenu.SetActive(true);
            isMenuOnScreen = true;
            AudioManager.Instance.partyFX.Stop();
            GameManager.Instance.StopMovement();
            GameManager.Instance.DisablePartyMovement();
            Time.timeScale = 0;
        }

        if (yawnSoundStarted) {
            sleepTimer += Time.deltaTime;
            if (sleepTimer >= sleepMaxTimer) {
                sleepTimer = 0;
                yawnSoundStarted = false;
                sleepAnimation.SetActive(false);
                AudioManager.Instance.PlayMusic();
            }
        }

        if (wellRestedTextShown) {
            sleepTimer += Time.deltaTime;
            if (sleepTimer >= sleepMaxTimer)
            {
                sleepTimer = 0;
                wellRestedTextShown = false;
                wellRested.SetActive(false);
            }
        }
    }

    public void PauseGame()
    {
        AudioManager.Instance.partyFX.Stop();
        GameManager.Instance.StopMovement();
        GameManager.Instance.DisablePartyMovement();
        m_canvasPause.SetActive(true);
        gameIsPaused = true;
        Time.timeScale = 0;
    }

    public void NormalSpeed()
    {
        GameManager.Instance.EnablePartyMovement();
        Time.timeScale = 1;
        m_canvasPause.SetActive(false);
        gameIsPaused = false;
    }

    public void FastSpeed()
    {
        GameManager.Instance.EnablePartyMovement();
        Time.timeScale = 2;
        m_canvasPause.SetActive(false);
        gameIsPaused = false;
    }

    public void SettingsButton()
    {
        if (isMenuOnScreen)
        {
            m_canvasMenu.SetActive(false);
            isMenuOnScreen = false;
            GameManager.Instance.EnablePartyMovement();
        }
        else
        {
            m_canvasMenu.SetActive(true);
            isMenuOnScreen = true;
            GameManager.Instance.StopMovement();
            GameManager.Instance.DisablePartyMovement();
        }

    }

    public void DisableCombatCanvas() { m_canvasToCombat.SetActive(false); }

    public void EnableCombatCanvas() { m_canvasToCombat.SetActive(true); }

    public void ContinueButton() {
        m_canvasMenu.SetActive(false);
        isMenuOnScreen = false;
        GameManager.Instance.EnablePartyMovement();
    }

    public void SleepButton() {
        List<Stat> grodnarStats = GameStats.Instance.GetGrodnarStats();
        List<Stat> lanstarStats = GameStats.Instance.GetLanstarStats();
        List<Stat> sigfridStats = GameStats.Instance.GetSigfridStats();
        int playersFullHealth = 0;
        for (int i = 0; i < grodnarStats.Count; i++)
        {
            if (grodnarStats[i].attribute == Attributes.CURR_HEALTH)
            {
                if (grodnarStats[i].value == GameStats.Instance.GetMaxHP(grodnarStats))
                {
                    playersFullHealth++;
                }
            }
        }
        for (int i = 0; i < lanstarStats.Count; i++)
        {
            if (lanstarStats[i].attribute == Attributes.CURR_HEALTH)
            {
                if (lanstarStats[i].value == GameStats.Instance.GetMaxHP(lanstarStats))
                {
                    playersFullHealth++;
                }
            }
        }
        for (int i = 0; i < sigfridStats.Count; i++)
        {
            if (sigfridStats[i].attribute == Attributes.CURR_HEALTH)
            {
                if (sigfridStats[i].value == GameStats.Instance.GetMaxHP(sigfridStats))
                {
                    playersFullHealth++;
                }
            }
        }
        if (playersFullHealth == 3) {
            wellRested.SetActive(true);
            wellRestedTextShown = true;
            return;
        }

        AudioManager.Instance.StopMusic();
        AudioManager.Instance.PlayInstant(AudioManager.InstantAudios.SNORE1);
        sleepAnimation.SetActive(true);
    }

    public void SetYawnSoundBool(bool yawn) { yawnSoundStarted = yawn; }

    public void CloseCampfireCanvas() {
        m_canvasCampfire.SetActive(false);
        GameManager.Instance.EnablePartyMovement();
    }
}
