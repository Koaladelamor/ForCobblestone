using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    private GameObject m_canvasPause;
    bool gameIsPaused;
    // Start is called before the first frame update
    void Start()
    {
        m_canvasPause = GameObject.FindGameObjectWithTag("CanvasPause");
        m_canvasPause.SetActive(false);
        gameIsPaused = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !gameIsPaused)
        {
            Time.timeScale = 0;
            m_canvasPause.SetActive(true);
            gameIsPaused = true;
        } else if (Input.GetKeyDown(KeyCode.Space) && gameIsPaused)
        {
            Time.timeScale = 1;
            m_canvasPause.SetActive(false);
            gameIsPaused = false;
        }
    }
}
