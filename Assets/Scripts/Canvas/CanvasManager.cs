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
    public GameObject m_canvasToCombat;
    public GameObject m_canvasPause;
    public GameObject canvasMenu;

    private bool isMenuOnScreen;
    private bool gameIsPaused;


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
            canvasMenu.SetActive(false);
            isMenuOnScreen = false;
            GameManager.Instance.EnablePartyMovement();
        }
        else if (InputManager.Instance.MenuButtonPressed && !isMenuOnScreen)
        {
            canvasMenu.SetActive(true);
            isMenuOnScreen = true;
            GameManager.Instance.StopMovement();
            GameManager.Instance.DisablePartyMovement();
        }
    }

    public void PauseGame()
    {
        m_canvasPause.SetActive(true);
        gameIsPaused = true;
        Time.timeScale = 0;
    }

    public void NormalSpeed()
    {
        Time.timeScale = 1;
        m_canvasPause.SetActive(false);
        gameIsPaused = false;
    }

    public void FastSpeed()
    {
        Time.timeScale = 2;
        m_canvasPause.SetActive(false);
        gameIsPaused = false;
    }

    public void SettingsButton()
    {
        if (isMenuOnScreen)
        {
            canvasMenu.SetActive(false);
            isMenuOnScreen = false;
            GameManager.Instance.EnablePartyMovement();
        }
        else
        {
            canvasMenu.SetActive(true);
            isMenuOnScreen = true;
            GameManager.Instance.StopMovement();
            GameManager.Instance.DisablePartyMovement();
        }

    }

    public void DisableCombatCanvas() { m_canvasToCombat.SetActive(false); }

    public void EnableCombatCanvas() { m_canvasToCombat.SetActive(true); }

}
