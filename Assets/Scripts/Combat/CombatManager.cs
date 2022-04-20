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

    Vector2 EnemySpawnTile1 = new Vector2(3, 0);
    Vector2 EnemySpawnTile2 = new Vector2(3, 2);
    Vector2 EnemySpawnTile3 = new Vector2(3, 3);

    int turn;
    public bool startCombat;

    public bool turnDone = false;


    // Start is called before the first frame update
    void Start()
    {

        m_canvasToMap.SetActive(false);

        m_enemies[0] = Instantiate(m_minotaurPrefab, transform.position, transform.rotation);
        m_enemies[1] = Instantiate(m_minotaurPrefab, transform.position, transform.rotation);
        m_enemies[2] = Instantiate(m_minotaurPrefab, transform.position, transform.rotation);

        /*for (int i = 0; i < m_enemies.Length; i++)
        {
            m_enemies[i].GetComponent<PawnController>().m_isAlive = true;
            m_enemies[i].GetComponent<PawnController>().SetTurnOff();
        }*/
        /*m_enemies[0].GetComponent<PawnController>().m_turnOrder = 2;
        m_enemies[1].GetComponent<PawnController>().m_turnOrder = 4;
        m_enemies[2].GetComponent<PawnController>().m_turnOrder = 6;*/


        GridManager.Instance.AssignPawnToTile(m_enemies[0], EnemySpawnTile1);
        GridManager.Instance.AssignPawnToTile(m_enemies[1], EnemySpawnTile2);
        GridManager.Instance.AssignPawnToTile(m_enemies[2], EnemySpawnTile3);

        turn = 0;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (EnemiesDefeated())
        {
            Invoke("SetCanvasActive", 1f);
        }*/

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
                    if (turnDone)
                    {
                        m_players[0].GetComponent<PawnController>().SetTurn(false);
                        turn++;
                        turnDone = false;
                    }
                    break;

                case 1:
                    if (!m_enemies[0].GetComponent<PawnController>().GetTurn())
                    {
                        m_enemies[0].GetComponent<PawnController>().SetTurnOn();
                    }
                    if (turnDone)
                    {
                        m_enemies[0].GetComponent<PawnController>().SetTurn(false);
                        turn++;
                        turnDone = false;
                    }
                    break;

                case 2:
                    if (!m_players[1].GetComponent<PawnController>().GetTurn())
                    {
                        m_players[1].GetComponent<PawnController>().SetTurnOn();
                    }
                    if (turnDone)
                    {
                        m_players[1].GetComponent<PawnController>().SetTurn(false);
                        turn++;
                        turnDone = false;
                    }
                    break;

                case 3:
                    if (!m_enemies[1].GetComponent<PawnController>().GetTurn())
                    {
                        m_enemies[1].GetComponent<PawnController>().SetTurnOn();
                    }
                    if (turnDone)
                    {
                        m_enemies[1].GetComponent<PawnController>().SetTurn(false);
                        turn++;
                        turnDone = false;
                    }
                    break;

                case 4:
                    if (!m_players[2].GetComponent<PawnController>().GetTurn())
                    {
                        m_players[2].GetComponent<PawnController>().SetTurnOn();
                    }
                    if (turnDone)
                    {
                        m_players[2].GetComponent<PawnController>().SetTurn(false);
                        turn++;
                        turnDone = false;
                    }
                    break;

                case 5:
                    if (!m_enemies[2].GetComponent<PawnController>().GetTurn())
                    {
                        m_enemies[2].GetComponent<PawnController>().SetTurnOn();
                    }
                    if (turnDone)
                    {
                        m_enemies[2].GetComponent<PawnController>().SetTurn(false);
                        turn++;
                        turnDone = false;
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

    public void SetTurnDone(bool done) { turnDone = done; }
}
