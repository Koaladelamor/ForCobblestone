using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marti_Combat_Manager : MonoBehaviour
{
    static Marti_Combat_Manager m_instance = null;

    public GameObject[] m_players;
    public GameObject[] m_enemies;

    private GameObject m_gameManager;

    public GameObject m_enemyPrefab;

    Vector2 EnemySpawnTile1 = new Vector2(6, 2);
    Vector2 EnemySpawnTile2 = new Vector2(6, 3);
    Vector2 EnemySpawnTile3 = new Vector2(6, 4);

    int turn;

    public bool startCombat;

    //public bool turnDone;
    bool turnChanged;

    //int i = 0;

    // Start is called before the first frame update
    void Start()
    {
        m_enemies[0] = Instantiate(m_enemyPrefab, transform.position, Quaternion.identity);
        m_enemies[1] = Instantiate(m_enemyPrefab, transform.position, Quaternion.identity);
        m_enemies[2] = Instantiate(m_enemyPrefab, transform.position, Quaternion.identity);

        int enemies = m_enemies.Length;
        for (int i = 0; i < enemies; i++)
        {
            m_enemies[i].GetComponent<Marti_Pawn_Controller>().m_isAlive = true;
        }
        m_enemies[0].GetComponent<Marti_Pawn_Controller>().m_turnOrder = 2;
        m_enemies[1].GetComponent<Marti_Pawn_Controller>().m_turnOrder = 4;
        m_enemies[2].GetComponent<Marti_Pawn_Controller>().m_turnOrder = 6;


        Marti_Grid_Manager.Instance.AssignPawnToTile(m_enemies[0], EnemySpawnTile1);
        Marti_Grid_Manager.Instance.AssignPawnToTile(m_enemies[1], EnemySpawnTile2);
        Marti_Grid_Manager.Instance.AssignPawnToTile(m_enemies[2], EnemySpawnTile3);

        Marti_Grid_Manager.Instance.TakePawnFromTile(EnemySpawnTile1);
        Marti_Grid_Manager.Instance.TakePawnFromTile(EnemySpawnTile2);
        Marti_Grid_Manager.Instance.TakePawnFromTile(EnemySpawnTile3);



        //Singleton
        /*if (m_instance == null) { 
            m_instance = this;
            DontDestroyOnLoad(this);
        }
        else { Destroy(this.gameObject); }*/

        //turnDone = false;
        turn = 0;

        m_gameManager = GameObject.FindGameObjectWithTag("GameManager");


        /*players[0] = GameObject.Find("AI_Player");
        players[1] = GameObject.Find("AI_Player2");
        players[2] = GameObject.Find("AI_Player3");

        enemies[0] = GameObject.Find("AI_Enemy");
        enemies[1] = GameObject.Find("AI_Enemy2");
        enemies[2] = GameObject.Find("AI_Enemy3");*/

    }

    // Update is called once per frame
    void Update()
    {
        if (enemiesDefeated())
        {
            m_gameManager.GetComponent<Marti_Game_Manager>().combatIsOver = true;

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
                    m_players[0].GetComponent<Marti_Pawn_Controller>().m_isMyTurn = true;
                    turnChanged = !turnChanged;
                }

                if (m_players[0].GetComponent<Marti_Pawn_Controller>().m_isMyTurn == false)
                {
                    turn++;
                    turnChanged = !turnChanged;
                }
            }

            else if (turn == 1)
            {
                if (!turnChanged)
                {
                    m_enemies[0].GetComponent<Marti_Pawn_Controller>().m_isMyTurn = true;
                    turnChanged = !turnChanged;
                }

                if (m_enemies[0].GetComponent<Marti_Pawn_Controller>().m_isMyTurn == false)
                {
                    turn++;
                    turnChanged = !turnChanged;
                }

            }

            else if (turn == 2)
            {
                if (!turnChanged)
                {
                    m_players[1].GetComponent<Marti_Pawn_Controller>().m_isMyTurn = true;
                    turnChanged = !turnChanged;
                }

                if (m_players[1].GetComponent<Marti_Pawn_Controller>().m_isMyTurn == false)
                {
                    turn++;
                    turnChanged = !turnChanged;
                }

            }

            else if (turn == 3)
            {
                if (!turnChanged)
                {
                    m_enemies[1].GetComponent<Marti_Pawn_Controller>().m_isMyTurn = true;
                    turnChanged = !turnChanged;
                }

                if (m_enemies[1].GetComponent<Marti_Pawn_Controller>().m_isMyTurn == false)
                {
                    turn++;
                    turnChanged = !turnChanged;
                }
            }

            else if (turn == 4)
            {
                if (!turnChanged)
                {
                    m_players[2].GetComponent<Marti_Pawn_Controller>().m_isMyTurn = true;
                    turnChanged = !turnChanged;
                }

                if (m_players[2].GetComponent<Marti_Pawn_Controller>().m_isMyTurn == false)
                {
                    turn++;
                    turnChanged = !turnChanged;
                }

            }

            else if (turn == 5)
            {
                if (!turnChanged)
                {
                    m_enemies[2].GetComponent<Marti_Pawn_Controller>().m_isMyTurn = true;
                    turnChanged = !turnChanged;
                }

                if (m_enemies[2].GetComponent<Marti_Pawn_Controller>().m_isMyTurn == false)
                {
                    turn++;
                    turnChanged = !turnChanged;
                }

            }

            else if (turn > 5) { turn = 0; }

        }




    }

    bool enemiesDefeated()
    {

        int enemiesDead = 0;
        int enemies = m_enemies.Length;
        for (int i = 0; i < enemies; i++)
        {
            if (!m_enemies[i].GetComponent<Marti_Pawn_Controller>().m_isAlive)
            {
                enemiesDead++;
            }
        }

        if (enemiesDead == enemies)
        {
            return true;
        }

        return false;
    }
}
