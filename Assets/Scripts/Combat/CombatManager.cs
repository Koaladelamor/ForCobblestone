using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public GameObject[] m_players;
    private GameObject[] m_enemies;

    public GameObject m_minotaurPrefab;
    public GameObject m_wolfPrefab;

    public GameObject m_canvasToMap;

    Vector2 EnemySpawnTile1 = new Vector2(3, 0);
    Vector2 EnemySpawnTile2 = new Vector2(3, 2);
    Vector2 EnemySpawnTile3 = new Vector2(3, 3);

    int turn;
    public bool startCombat;
    private bool turnSet;
    public bool turnDone;

    private void Awake()
    {
        turnSet = false;
        m_enemies = new GameObject[3];
    }


    // Start is called before the first frame update
    void Start()
    {

        m_canvasToMap.SetActive(false);

        m_enemies[0] = Instantiate(m_minotaurPrefab, transform.position, transform.rotation);
        m_enemies[1] = Instantiate(m_minotaurPrefab, transform.position, transform.rotation);
        m_enemies[2] = Instantiate(m_minotaurPrefab, transform.position, transform.rotation);


        Invoke("AssignEnemies", 0.2f);




        /*for (int i = 0; i < m_enemies.Length; i++)
{
    m_enemies[i].GetComponent<PawnController>().m_isAlive = true;
    m_enemies[i].GetComponent<PawnController>().SetTurnOff();
}*/
        /*m_enemies[0].GetComponent<PawnController>().m_turnOrder = 2;
        m_enemies[1].GetComponent<PawnController>().m_turnOrder = 4;
        m_enemies[2].GetComponent<PawnController>().m_turnOrder = 6;*/
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
                    if (!turnSet)
                    {
                        m_players[0].GetComponent<PawnController>().SetTurnOn();
                        turnSet = true;
                    }
                    break;

                case 1:
                    if (!turnSet)
                    {
                        m_enemies[0].GetComponent<EnemyPawn>().SetTurnOn();
                        turnSet = true;
                    }
                    break;

                case 2:
                    if (!turnSet)
                    {
                        m_players[1].GetComponent<PawnController>().SetTurnOn();
                        turnSet = true;
                    }
                    break;

                case 3:
                    if (!turnSet)
                    {
                        m_enemies[1].GetComponent<PawnController>().SetTurnOn();
                        turnSet = true;
                    }
                    break;

                case 4:
                    if (!turnSet)
                    {
                        m_players[2].GetComponent<PawnController>().SetTurnOn();
                        turnSet = true;
                    }
                    break;

                case 5:
                    if (!turnSet)
                    {
                        m_enemies[2].GetComponent<PawnController>().SetTurnOn();
                        turnSet = true;
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

    public void SetTurnDone(bool done) { turnDone = done; }

    public void NextTurn() {
        turn++;
        turnSet = false;
    }
}
