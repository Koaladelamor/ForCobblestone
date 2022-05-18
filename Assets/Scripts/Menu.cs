using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class Menu : MonoBehaviour
{
    public GameObject canvasMenu;
    public GameObject canvasSettings;
    public GameObject canvasControls;

    private void Awake()
    {
        canvasSettings.SetActive(false);
    }

    public void loadNewGame() {
        SceneManager.LoadScene("MapScene", LoadSceneMode.Single);
    }

    public void exitPlayMode() {
        Debug.Log("Exit game");
        Application.Quit();
    }

    public void EnableCanvasMenu() { canvasMenu.SetActive(true); }

    public void DisableCanvasMenu() { canvasMenu.SetActive(false); }

    public void EnableCanvasSettings() { canvasSettings.SetActive(true); }

    public void DisableCanvasSettings() { canvasSettings.SetActive(false); }

    public void ToCanvasSettings() {
        DisableCanvasMenu();
        EnableCanvasSettings();
    }

    public void ToCanvasMenu() {
        DisableCanvasSettings();
        EnableCanvasMenu();
    }

    public void showControls()
    {
        canvasControls.SetActive(true);
    }

    public void exitControls()
    {
        canvasControls.SetActive(false);
    }
}
