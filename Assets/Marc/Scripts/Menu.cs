using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class Menu : MonoBehaviour
{
    public void loadNewGame() {
        SceneManager.LoadScene("MapScene", LoadSceneMode.Single);
    }

    public void exitPlayMode() {
        //EditorApplication.ExitPlaymode();
    }
}
