using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public GameObject[] m_players_temp;
    private GameObject[] m_players;
    private GameObject[] m_enemies;

    public GameObject m_minotaurPrefab;
    public GameObject m_wolfPrefab;

    public GameObject m_canvasToMap;

    Vector2 EnemySpawnTile1 = new Vector2(3, 0);
    Vector2 EnemySpawnTile2 = new Vector2(3, 2);
    Vector2 EnemySpawnTile3 = new Vector2(3, 3);

    private int turn;
    public bool startCombat;
    private bool turnSet;

    private void Awake()
    {
        turnSet = false;
        m_enemies = new GameObject[3];
        m_players = new GameObject[m_players_temp.Length];
    }


    // Start is called before the first frame update
    void Start()
    {

        m_canvasToMap.SetActive(false);

        m_enemies[0] = Instantiate(m_minotaurPrefab, transform.position, transform.rotation);
        m_enemies[1] = Instantiate(m_minotaurPrefab, transform.position, transform.rotation);
        m_enemies[2] = Instantiate(m_minotaurPrefab, transform.position, transform.rotation);


        Invoke("AssignEnemies", 0.2f);

        turn = 0;
        CalculatePlayersTurn();
    }

    // Update is called once per frame
    void Update()
    {
        
        /*if (EnemiesDefeated())
        {
            Invoke("SetCanvasActive", 1f);
        }*/

        if (startCombat && !turnSet) {

            switch (turn)
            {
                default:
                    break;

                case 0:
                    m_players[0].GetComponent<PawnController>().SetTurnOn();
                    turnSet = true;
                    break;

                case 1:
                    m_enemies[0].GetComponent<EnemyPawn>().SetTurnOn();
                    turnSet = true;
                    break;

                case 2:
                    m_players[1].GetComponent<PawnController>().SetTurnOn();
                    turnSet = true;
                    break;

                case 3:
                    m_enemies[1].GetComponent<PawnController>().SetTurnOn();
                    turnSet = true;
                    break;

                case 4:
                    m_players[2].GetComponent<PawnController>().SetTurnOn();
                    turnSet = true;
                    break;

                case 5:
                    m_enemies[2].GetComponent<PawnController>().SetTurnOn();
                    turnSet = true;
                    break;

                case 6:
                    turn = 0;
                    break;
            }
        }
    }

    private void CalculatePlayersTurn() {

        int[] pawnsAgility = new int[m_players_temp.Length];

        for (int i = 0; i < m_players_temp.Length; i++)
        {
            pawnsAgility[i] = m_players_temp[i].GetComponent<PawnController>().GetAgility();
        }

        //1st Turn
        if (pawnsAgility[0] > pawnsAgility[1] && pawnsAgility[0] > pawnsAgility[1])
        {
            m_players[0] = m_players_temp[0];
        }
        else if (pawnsAgility[1] > pawnsAgility[0] && pawnsAgility[1] > pawnsAgility[2])
        {
            m_players[0] = m_players_temp[1];
        }
        else if (pawnsAgility[2] > pawnsAgility[0] && pawnsAgility[2] > pawnsAgility[1])
        {
            m_players[0] = m_players_temp[2];
        }

        //2nd Turn
        if (pawnsAgility[0] > pawnsAgility[1] && pawnsAgility[0] < pawnsAgility[2])
        {
            m_players[1] = m_players_temp[0];
        }
        else if (pawnsAgility[0] < pawnsAgility[1] && pawnsAgility[0] > pawnsAgility[2])
        {
            m_players[1] = m_players_temp[0];
        }
        else if (pawnsAgility[1] > pawnsAgility[0] && pawnsAgility[1] < pawnsAgility[2])
        {
            m_players[1] = m_players_temp[1];
        }
        else if (pawnsAgility[1] < pawnsAgility[0] && pawnsAgility[1] > pawnsAgility[2])
        {
            m_players[1] = m_players_temp[1];
        }
        else if (pawnsAgility[2] > pawnsAgility[0] && pawnsAgility[2] < pawnsAgility[1])
        {
            m_players[1] = m_players_temp[2];
        }
        else if (pawnsAgility[2] < pawnsAgility[0] && pawnsAgility[2] > pawnsAgility[1])
        {
            m_players[1] = m_players_temp[2];
        }


        //3rd Turn
        if (pawnsAgility[0] < pawnsAgility[1] && pawnsAgility[0] < pawnsAgility[2])
        {
            m_players[2] = m_players_temp[0];
        }
        else if (pawnsAgility[1] < pawnsAgility[0] && pawnsAgility[1] < pawnsAgility[2])
        {
            m_players[2] = m_players_temp[1];
        }
        else if (pawnsAgility[2] < pawnsAgility[0] && pawnsAgility[2] < pawnsAgility[1])
        {
            m_players[2] = m_players_temp[2];
        }

        for (int i = 0; i < m_players.Length; i++)
        {
            if (m_players[i] == null) {
                Debug.Log("ERROR on setting players turn");
            }
        }
    }

    bool EnemiesDefeated() {

        int enemiesDead = 0;
        int enemies = m_enemies.Length;
        for (int i = 0; i < enemies; i++) {
            if (!m_enemies[i].GetComponent<OldPawnController>().m_isAlive) {
                enemiesDead++;
            }
        }

        if (enemiesDead == enemies) {
            return true;
        }

        return false;
    }

    public void AssignEnemies() {
        GridManager.Instance.AssignPawnToTile(m_enemies[0], EnemySpawnTile1);
        GridManager.Instance.AssignPawnToTile(m_enemies[1], EnemySpawnTile2);
        GridManager.Instance.AssignPawnToTile(m_enemies[2], EnemySpawnTile3);
    }

    public void CombatIsOver() {
        GameManager.Instance.SetCombatIsOver(true);
    }

    public void SetCanvasActive() {
        m_canvasToMap.SetActive(true);
    }

    public void SetBoolCombatToTrue() {
        startCombat = true;
    }

    public void SetSpeedx2() {
        Time.timeScale = 2;
    }

    public void SetSpeedx1()
    {
        Time.timeScale = 1;
    }

    public void Pause()
    {
        Time.timeScale = 0;
    }

    public void NextTurn() {
        turn++;
        turnSet = false;
    }
}
