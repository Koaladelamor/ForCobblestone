using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class Menu : MonoBehaviour
{

    public GameObject canvasSettings;
    public void loadNewGame() {
        SceneManager.LoadScene("MapScene", LoadSceneMode.Single);
    }

    public void exitPlayMode() {
        //EditorApplication.ExitPlaymode();
    }

    public void EnableCanvasSettings() { canvasSettings.SetActive(true); }

    public void DisableCanvasSettings() { canvasSettings.SetActive(false); }
}
