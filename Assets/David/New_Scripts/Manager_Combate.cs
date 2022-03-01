using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_Combate : MonoBehaviour
{
    static Manager_Combate m_instance = null;

    public GameObject[] m_players;
    public GameObject[] m_enemies;

    private GameObject m_gameManager;

    public GameObject m_enemyPrefab;

    Vector2 EnemySpawnTile1 = new Vector2(6, 2);
    Vector2 EnemySpawnTile2 = new Vector2(6, 3);
    Vector2 EnemySpawnTile3 = new Vector2(6, 4);

    int turn;

    public bool startCombat;


    bool turnChanged;

  

    // Start is called before the first frame update
    void Start()
    {
        m_enemies[0] = Instantiate(m_enemyPrefab, transform.position, Quaternion.identity);
        m_enemies[1] = Instantiate(m_enemyPrefab, transform.position, Quaternion.identity);
        m_enemies[2] = Instantiate(m_enemyPrefab, transform.position, Quaternion.identity);

        int enemies = m_enemies.Length;
        for (int i = 0; i < enemies; i++)
        {
            m_enemies[i].GetComponent<Controlador_Peon>().m_isAlive = true;
        }
        m_enemies[0].GetComponent<Controlador_Peon>().m_turnOrder = 2;
        m_enemies[1].GetComponent<Controlador_Peon>().m_turnOrder = 4;
        m_enemies[2].GetComponent<Controlador_Peon>().m_turnOrder = 6;


        Manager_Grid.Instance.AssignPawnToTile(m_enemies[0], EnemySpawnTile1);
        Manager_Grid.Instance.AssignPawnToTile(m_enemies[1], EnemySpawnTile2);
        Manager_Grid.Instance.AssignPawnToTile(m_enemies[2], EnemySpawnTile3);

        Manager_Grid.Instance.TakePawnFromTile(EnemySpawnTile1);
        Manager_Grid.Instance.TakePawnFromTile(EnemySpawnTile2);
        Manager_Grid.Instance.TakePawnFromTile(EnemySpawnTile3);



        turn = 0;

        m_gameManager = GameObject.FindGameObjectWithTag("GameManager");


      

    }


    void Update()
    {
        if (enemiesDefeated())
        {
            m_gameManager.GetComponent<Juego_Manager>().combatIsOver = true;

        }

        if (startCombat)
        {
            


            if (turn == 0)
            {
                if (!turnChanged)
                {
                    m_players[0].GetComponent<Controlador_Peon>().m_isMyTurn = true;
                    turnChanged = !turnChanged;
                }

                if (m_players[0].GetComponent<Controlador_Peon>().m_isMyTurn == false)
                {
                    turn++;
                    turnChanged = !turnChanged;
                }
            }

            else if (turn == 1)
            {
                if (!turnChanged)
                {
                    m_enemies[0].GetComponent<Controlador_Peon>().m_isMyTurn = true;
                    turnChanged = !turnChanged;
                }

                if (m_enemies[0].GetComponent<Controlador_Peon>().m_isMyTurn == false)
                {
                    turn++;
                    turnChanged = !turnChanged;
                }

            }

            else if (turn == 2)
            {
                if (!turnChanged)
                {
                    m_players[1].GetComponent<Controlador_Peon>().m_isMyTurn = true;
                    turnChanged = !turnChanged;
                }

                if (m_players[1].GetComponent<Controlador_Peon>().m_isMyTurn == false)
                {
                    turn++;
                    turnChanged = !turnChanged;
                }

            }

            else if (turn == 3)
            {
                if (!turnChanged)
                {
                    m_enemies[1].GetComponent<Controlador_Peon>().m_isMyTurn = true;
                    turnChanged = !turnChanged;
                }

                if (m_enemies[1].GetComponent<Controlador_Peon>().m_isMyTurn == false)
                {
                    turn++;
                    turnChanged = !turnChanged;
                }
            }

            else if (turn == 4)
            {
                if (!turnChanged)
                {
                    m_players[2].GetComponent<Controlador_Peon>().m_isMyTurn = true;
                    turnChanged = !turnChanged;
                }

                if (m_players[2].GetComponent<Controlador_Peon>().m_isMyTurn == false)
                {
                    turn++;
                    turnChanged = !turnChanged;
                }

            }

            else if (turn == 5)
            {
                if (!turnChanged)
                {
                    m_enemies[2].GetComponent<Controlador_Peon>().m_isMyTurn = true;
                    turnChanged = !turnChanged;
                }

                if (m_enemies[2].GetComponent<Controlador_Peon>().m_isMyTurn == false)
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
            if (!m_enemies[i].GetComponent<Controlador_Peon>().m_isAlive)
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
