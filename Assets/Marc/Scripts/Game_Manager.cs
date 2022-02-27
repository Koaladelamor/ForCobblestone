using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_Manager : MonoBehaviour
{
    static Game_Manager m_gameManager = null;

    private GameObject m_combatManager;

    public GameObject m_playerPrefab;
    private GameObject m_player;
    private GameObject m_pointToGo;

    Vector3 playerPosition;

    public bool enemyEngaged;
    public bool combatIsOver;

    // Start is called before the first frame update
    void Start()
    {
        //Singleton
        if (m_gameManager == null) { 
            m_gameManager = this;
            DontDestroyOnLoad(this);
        }
        else { Destroy(this.gameObject); }

        m_player = GameObject.FindGameObjectWithTag("PlayerMap");
        enemyEngaged = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (enemyEngaged) {

            //Save stats
            playerPosition = m_player.transform.position;

            m_player = null;
            enemyEngaged = false;

            //Load scene
            SceneManager.LoadScene("CombatScene");

        }
        

        if (combatIsOver) {
            SceneManager.LoadScene("MapScene");

            //RespawnPlayer();
            Invoke("SetupPlayer", 1f);

            combatIsOver = false;
        }
    }

    void RespawnPlayer() {
        GameObject playerInstantiate = Instantiate(m_playerPrefab, playerPosition, Quaternion.identity);
        m_player = playerInstantiate;
    }

    void SetupPlayer() {
        m_player = GameObject.FindGameObjectWithTag("PlayerMap");
        m_pointToGo = GameObject.FindGameObjectWithTag("PointToGo");
        m_player.transform.position = playerPosition;
        m_pointToGo.transform.position = playerPosition;
    }
}
