using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public GameObject[] m_players;
    public GameObject[] m_enemies;

    private GameObject m_gameManager;

    public GameObject m_minotaurPrefab;
    public GameObject m_wolfPrefab;

    private GameObject m_canvasToMap;

    Vector2 EnemySpawnTile1 = new Vector2(7, 1);
    Vector2 EnemySpawnTile2 = new Vector2(7, 3);
    Vector2 EnemySpawnTile3 = new Vector2(7, 5);

    int turn;
    public bool startCombat;
    bool turnChanged;

    // Start is called before the first frame update
    void Start()
    {
        m_gameManager = GameObject.FindGameObjectWithTag("GameManager");

        m_canvasToMap = GameObject.FindGameObjectWithTag("CanvasToMap");
        m_canvasToMap.SetActive(false);

        m_enemies[0] = Instantiate(m_minotaurPrefab, transform.position, Quaternion.identity);
        m_enemies[1] = Instantiate(m_minotaurPrefab, transform.position, Quaternion.identity);
        m_enemies[2] = Instantiate(m_minotaurPrefab, transform.position, Quaternion.identity);

        /*if (m_gameManager.GetComponent<GameManager>().enemyOnCombatType == EnemyType.MINOTAUR) {
            m_enemies[0] = Instantiate(m_minotaurPrefab, transform.position, Quaternion.identity);
            m_enemies[1] = Instantiate(m_minotaurPrefab, transform.position, Quaternion.identity);
            m_enemies[2] = Instantiate(m_minotaurPrefab, transform.position, Quaternion.identity);
        } 
        
        else if (m_gameManager.GetComponent<GameManager>().enemyOnCombatType == EnemyType.WOLF) {
            m_enemies[0] = Instantiate(m_wolfPrefab, transform.position, Quaternion.identity);
            m_enemies[1] = Instantiate(m_wolfPrefab, transform.position, Quaternion.identity);
            m_enemies[2] = Instantiate(m_wolfPrefab, transform.position, Quaternion.identity);
        }*/


        int enemies = m_enemies.Length;
        for (int i = 0; i < enemies; i++)
        {
            m_enemies[i].GetComponent<PawnController>().m_isAlive = true;
        }
        m_enemies[0].GetComponent<PawnController>().m_turnOrder = 2;
        m_enemies[1].GetComponent<PawnController>().m_turnOrder = 4;
        m_enemies[2].GetComponent<PawnController>().m_turnOrder = 6;


        GridManager.Instance.AssignPawnToTile(m_enemies[0], EnemySpawnTile1);
        GridManager.Instance.AssignPawnToTile(m_enemies[1], EnemySpawnTile2);
        GridManager.Instance.AssignPawnToTile(m_enemies[2], EnemySpawnTile3);

        GridManager.Instance.TakePawnFromTile(EnemySpawnTile1);
        GridManager.Instance.TakePawnFromTile(EnemySpawnTile2);
        GridManager.Instance.TakePawnFromTile(EnemySpawnTile3);

        turn = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemiesDefeated()) {
            Invoke("setCanvasActive", 1f);
        }

            if (startCombat)
        {
            /*if (turnDone)
            {
                i++;
                turn++;
                turnDone = !turnDone;
                if (turn > 5) { turn = 0; }
            }
            else {
                if (i % 2 == 0)
                {
                    m_players[i / 2].GetComponent<PawnController>().m_isMyTurn = true;
                }
                else
                {
                    m_enemies[i / 2].GetComponent<PawnController>().m_isMyTurn = true;
                }

            }*/

            
            if (turn == 0)
            {
                if (!turnChanged)
                {
                    m_players[0].GetComponent<PawnController>().m_isMyTurn = true;
                    turnChanged = !turnChanged;
                }

                if (m_players[0].GetComponent<PawnController>().m_isMyTurn == false) {
                    turn++;
                    turnChanged = !turnChanged;
                }
            }

            else if (turn == 1)
            {
                if (!turnChanged)
                {
                    m_enemies[0].GetComponent<PawnController>().m_isMyTurn = true;
                    turnChanged = !turnChanged;
                }

                if (m_enemies[0].GetComponent<PawnController>().m_isMyTurn == false)
                {
                    turn++;
                    turnChanged = !turnChanged;
                }

            }

            else if (turn == 2)
            {
                if (!turnChanged)
                {
                    m_players[1].GetComponent<PawnController>().m_isMyTurn = true;
                    turnChanged = !turnChanged;
                }

                if (m_players[1].GetComponent<PawnController>().m_isMyTurn == false)
                {
                    turn++;
                    turnChanged = !turnChanged;
                }

            }

            else if (turn == 3)
            {
                if (!turnChanged)
                {
                    m_enemies[1].GetComponent<PawnController>().m_isMyTurn = true;
                    turnChanged = !turnChanged;
                }

                if (m_enemies[1].GetComponent<PawnController>().m_isMyTurn == false)
                {
                    turn++;
                    turnChanged = !turnChanged;
                }
            }

            else if (turn == 4)
            {
                if (!turnChanged)
                {
                    m_players[2].GetComponent<PawnController>().m_isMyTurn = true;
                    turnChanged = !turnChanged;
                }

                if (m_players[2].GetComponent<PawnController>().m_isMyTurn == false)
                {
                    turn++;
                    turnChanged = !turnChanged;
                }

            }

            else if (turn == 5)
            {
                if (!turnChanged)
                {
                    m_enemies[2].GetComponent<PawnController>().m_isMyTurn = true;
                    turnChanged = !turnChanged;
                }

                if (m_enemies[2].GetComponent<PawnController>().m_isMyTurn == false)
                {
                    turn++;
                    turnChanged = !turnChanged;
                }

            }

            else if (turn > 5) { turn = 0; }
            
        }




    }

    bool enemiesDefeated() {

        int enemiesDead = 0;
        int enemies = m_enemies.Length;
        for (int i = 0; i < enemies; i++) {
            if (!m_enemies[i].GetComponent<PawnController>().m_isAlive) {
                enemiesDead++;
            }
        }

        if (enemiesDead == enemies) {
            return true;
        }

        return false;
    }

    public void combatIsOver() {
        m_gameManager.GetComponent<GameManager>().combatIsOver = true;
    }

    public void setCanvasActive() {
        m_canvasToMap.SetActive(true);
    }

    public void setBoolCombatToTrue() {
        startCombat = true;
    }
}
