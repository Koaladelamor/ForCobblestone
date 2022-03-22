using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
   public void StartGame()
    {
        SceneManager.LoadScene("Escena_Mapa_Buena");
    }
    public void Exit()
    {
        Application.Quit();
    }
}
