using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombateManager : MonoBehaviour
{
    static CombateManager m_instance = null;

    public GameObject[] m_players;
    public GameObject[] m_enemies;

    int turn;
    public bool startCombat;
    //public bool turnDone;
    bool turnChanged;
    //int i = 0;

    // Start is called before the first frame update
    void Start()
    {
        //Singleton
        if (m_instance == null) { m_instance = this; }
        else { Destroy(this.gameObject); }

        //turnDone = false;
        turn = 0;

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

                if (m_players[0].GetComponent<PawnController>().m_isMyTurn == false)
                {
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
}
