using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public GameObject[] m_players;
    public GameObject[] m_enemies;

    public GameObject m_minotaurPrefab;
    public GameObject m_wolfPrefab;

    public GameObject m_canvasToMap;

    Vector2 EnemySpawnTile1 = new Vector2(7, 1);
    Vector2 EnemySpawnTile2 = new Vector2(7, 3);
    Vector2 EnemySpawnTile3 = new Vector2(7, 4);

    int turn;
    public bool startCombat;

    // Start is called before the first frame update
    void Start()
    {

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


        for (int i = 0; i < m_enemies.Length; i++)
        {
            m_enemies[i].GetComponent<PawnController>().m_isAlive = true;
            m_enemies[i].GetComponent<PawnController>().SetTurnOff();
        }
        m_enemies[0].GetComponent<PawnController>().m_turnOrder = 2;
        m_enemies[1].GetComponent<PawnController>().m_turnOrder = 4;
        m_enemies[2].GetComponent<PawnController>().m_turnOrder = 6;


        GridManager.Instance.AssignPawnToTile(m_enemies[0], EnemySpawnTile1);
        GridManager.Instance.AssignPawnToTile(m_enemies[1], EnemySpawnTile2);
        GridManager.Instance.AssignPawnToTile(m_enemies[2], EnemySpawnTile3);

        turn = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (EnemiesDefeated())
        {
            Invoke("SetCanvasActive", 1f);
        }

        if (startCombat) {

            switch (turn)
            {
                default:
                    break;

                case 0:
                    if (!m_players[0].GetComponent<PawnController>().GetTurn())
                    {
                        m_players[0].GetComponent<PawnController>().SetTurnOn();
                    }
                    if (m_players[0].GetComponent<PawnController>().GetTurnDone())
                    {
                        m_players[0].GetComponent<PawnController>().SetTurn(false);
                        turn++;
                    }
                    break;

                case 1:
                    if (!m_enemies[0].GetComponent<PawnController>().GetTurn())
                    {
                        m_enemies[0].GetComponent<PawnController>().SetTurnOn();
                    }
                    if (m_enemies[0].GetComponent<PawnController>().GetTurnDone())
                    {
                        m_enemies[0].GetComponent<PawnController>().SetTurn(false);
                        turn++;
                    }
                    break;

                case 2:
                    if (!m_players[1].GetComponent<PawnController>().GetTurn())
                    {
                        m_players[1].GetComponent<PawnController>().SetTurnOn();
                    }
                    if (m_players[1].GetComponent<PawnController>().GetTurnDone())
                    {
                        m_players[1].GetComponent<PawnController>().SetTurn(false);
                        turn++;
                    }
                    break;

                case 3:
                    if (!m_enemies[1].GetComponent<PawnController>().GetTurn())
                    {
                        m_enemies[1].GetComponent<PawnController>().SetTurnOn();
                    }
                    if (m_enemies[1].GetComponent<PawnController>().GetTurnDone())
                    {
                        m_enemies[1].GetComponent<PawnController>().SetTurn(false);
                        turn++;
                    }
                    break;

                case 4:
                    if (!m_players[2].GetComponent<PawnController>().GetTurn())
                    {
                        m_players[2].GetComponent<PawnController>().SetTurnOn();
                    }
                    if (m_players[2].GetComponent<PawnController>().GetTurnDone())
                    {
                        m_players[2].GetComponent<PawnController>().SetTurn(false);
                        turn++;
                    }
                    break;

                case 5:
                    if (!m_enemies[2].GetComponent<PawnController>().GetTurn())
                    {
                        m_enemies[2].GetComponent<PawnController>().SetTurnOn();
                    }
                    if (m_enemies[2].GetComponent<PawnController>().GetTurnDone())
                    {
                        m_enemies[2].GetComponent<PawnController>().SetTurn(false);
                        turn++;
                    }
                    break;

                case 6:
                    turn = 0;
                    break;
            }
        }
    }

    bool EnemiesDefeated() {

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
}
