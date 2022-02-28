using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PAWN_STATUS { IDLE, SEARCH, ATTACK }

public class Controlador_Peon : MonoBehaviour
{
    bool draggable;
    bool isDragged;

    private Vector3 mouseDragStartPos;
    private Vector3 objDragStartPos;

    Manager_Grid m_board;
    Vector3 m_position;
    Vector2 m_tilePosition;
    Vector3 m_previousPosition;

    Transform m_positionToGo;

    PAWN_STATE m_state;
    Vector3[] m_directions = { Vector3.right, Vector3.down, Vector3.left, Vector3.up };

    int current_hp;
    int max_hp;

    int damage;

    public bool m_isAlive;

    float timer = 0f;
    float waitTime = 0.3f;

    int m_maxSteps;
    int m_currentStep;

    public int m_turnOrder;
    public bool m_isMyTurn;

    bool readyToAttack;

    Controlador_Peon m_pawnToAttack;

    private GameObject combatManager;


    private void Start()
    {

        if (CompareTag("Player"))
        {
            damage = 5;
            max_hp = 15;
            draggable = true;
        }
        else if (CompareTag("Enemy"))
        {
            damage = 1;
            max_hp = 15;
            draggable = false;
        }
        current_hp = max_hp;

        readyToAttack = true;
        m_isAlive = true;
        m_isMyTurn = false;
        m_currentStep = 0;
        m_maxSteps = 4;
        m_state = PAWN_STATE.IDLE;
        m_position = transform.position;
        m_previousPosition = m_position;

        combatManager = GameObject.FindGameObjectWithTag("CombatManager");
    }
    private void Update()
    {
        timer += Time.deltaTime;

        ClosestPawn();

        bool enemyIsClose = EnemyIsClose();

        switch (m_state)
        {
            default:
                break;
            case PAWN_STATE.SEARCH:

                if (timer >= waitTime)
                {
                    Search();
                    timer = 0;
                }

                break;

            case PAWN_STATE.IDLE:
                if (m_isMyTurn && enemyIsClose && m_pawnToAttack.m_isAlive)
                {
                    m_state = PAWN_STATE.ATTACK;
                }

                else if (m_isMyTurn && !enemyIsClose)
                {
                    m_state = PAWN_STATE.SEARCH;
                }

                else if (m_isMyTurn && enemyIsClose && !m_pawnToAttack.m_isAlive)
                {
                    m_isMyTurn = false;
                    //combatManager.GetComponent<CombatManager>().turnDone = true;
                }
                break;

            case PAWN_STATE.ATTACK:
                if (readyToAttack)
                {
                    Invoke("Attack", 0.9f);
                    readyToAttack = false;
                }
                /*if (attackTimer >= attackWaitTime) {
                    Attack();
                    attackTimer = 0;
                }*/

                break;
        }


    }
    private void OnMouseDown()
    {
        if (draggable)
        {
            isDragged = true;
            objDragStartPos = this.transform.position;
        }
    }

    private void OnMouseDrag()
    {
        if (isDragged)
        {
            Vector3 screenCoordinate = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));

            transform.position = new Vector3(screenCoordinate.x, screenCoordinate.y, transform.position.z);
        }
    }

    private void OnMouseUp()
    {
        if (isDragged)
        {

            isDragged = false;
            Vector2 tilePosition = Manager_Grid.Instance.ScreenToTilePosition(Input.mousePosition);

            if (tilePosition.x == Vector2.positiveInfinity.x)
            {
                transform.position = m_position;
                return;
            }

            if (Manager_Grid.Instance.IsTileEmpty(tilePosition))
            {
                Manager_Grid.Instance.TakePawnFromTile(m_tilePosition);
                m_tilePosition = tilePosition;
                Manager_Grid.Instance.AssignPawnToTile(this.gameObject, tilePosition);
            }
            else
            {
                transform.position = m_position;
            }
        }
    }

    void ClosestPawn()
    {


        if (CompareTag("Player"))
        {
            float distance;
            float closestDistance = 999999999;

            for (int i = 0; i < combatManager.GetComponent<Manager_Combate>().m_enemies.Length; i++)
            {
                distance = (transform.position - combatManager.GetComponent<Manager_Combate>().m_enemies[i].transform.position).magnitude;


                if (distance < closestDistance)
                {

                    closestDistance = distance;
                    m_positionToGo = combatManager.GetComponent<Manager_Combate>().m_enemies[i].transform;
                }
            }

        }

        else if (CompareTag("Enemy"))
        {
            float distance;
            float closestDistance = 999999999;

            for (int i = 0; i < combatManager.GetComponent<Manager_Combate>().m_players.Length; i++)
            {
                distance = (transform.position - combatManager.GetComponent<Manager_Combate>().m_players[i].transform.position).magnitude;


                if (distance < closestDistance)
                {

                    closestDistance = distance;
                    m_positionToGo = combatManager.GetComponent<Manager_Combate>().m_players[i].transform;
                }
            }

        }
    }

    void Search()
    {

        if ((transform.position - m_positionToGo.transform.position).magnitude == 1)
        {
            m_state = PAWN_STATE.IDLE;
            m_isMyTurn = false;
         
            return;

           
        }

        Vector3 closestDirection = Vector2.zero;
        float closestDistance = 100000;

     
        for (int i = 0; i < m_directions.Length; i++)
        {
            Vector2 positionToCheck = Manager_Grid.Instance.ScreenToTilePosition(Camera.main.WorldToScreenPoint(transform.position + m_directions[i]));

            if (Manager_Grid.Instance.IsTileEmpty(positionToCheck))
            {
                float distance = (transform.position + m_directions[i] - m_positionToGo.position).magnitude;
                if (distance < closestDistance && (transform.position + m_directions[i]) != m_previousPosition)
                {
                    closestDistance = distance;
                    closestDirection = m_directions[i];
                }
            }
        }

        Vector2 positionToMove = transform.position + closestDirection;
        Vector2 tileToMove = Manager_Grid.Instance.ScreenToTilePosition(Camera.main.WorldToScreenPoint(positionToMove));

        m_previousPosition = transform.position;

        Manager_Grid.Instance.TakePawnFromTile(m_tilePosition);
        m_tilePosition = tileToMove;
        Manager_Grid.Instance.AssignPawnToTile(this.gameObject, tileToMove);

        m_currentStep++;

        if (m_currentStep >= m_maxSteps)
        {
            m_state = PAWN_STATE.IDLE;
            m_isMyTurn = false;
         
            m_currentStep = 0;
        }
    }

    void Attack()
    {
        //attack
        if (m_isAlive && m_pawnToAttack.m_isAlive)
        {
            m_pawnToAttack.current_hp -= damage;

            combatManager.GetComponent<DamagePopUp>().Create(m_pawnToAttack.transform.position, damage);

            

            if (m_pawnToAttack.current_hp < 1)
            {
                m_pawnToAttack.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                m_pawnToAttack.gameObject.GetComponent<Controlador_Peon>().m_isAlive = false;


            }

            m_state = PAWN_STATE.IDLE;
            m_isMyTurn = false;

        }
        else
        {
            m_state = PAWN_STATE.IDLE;
            m_isMyTurn = false;
        }

        readyToAttack = true;
    }

    public void SetPosition(Vector3 p_position)
    {
        transform.position = p_position;
        m_position = p_position;
    }


    public bool EnemyIsClose()
    {
        for (int i = 0; i < m_directions.Length; i++)
        {
            Vector2 positionToCheck = Manager_Grid.Instance.ScreenToTilePosition(Camera.main.WorldToScreenPoint(transform.position + m_directions[i]));

            if (!Manager_Grid.Instance.IsTileEmpty(positionToCheck))
            {
                if (CompareTag("Player"))
                {
                    for (int j = 0; j < combatManager.GetComponent<Manager_Combate>().m_enemies.Length; j++)
                    {
                        if (combatManager.GetComponent<Manager_Combate>().m_enemies[j].GetComponent<Controlador_Peon>().m_isAlive)
                        {
                            Vector2 enemyPosition = combatManager.GetComponent<Manager_Combate>().m_enemies[j].transform.position;

                            if (positionToCheck == Manager_Grid.Instance.ScreenToTilePosition(Camera.main.WorldToScreenPoint(enemyPosition)))
                            {
                                if (combatManager.GetComponent<Manager_Combate>().m_enemies[j].GetComponent<Controlador_Peon>().m_isAlive)
                                {
                                    m_pawnToAttack = combatManager.GetComponent<Manager_Combate>().m_enemies[j].GetComponent<Controlador_Peon>();
                                    //Debug.Log("Enemy Found");
                                    return true;
                                }
                                else return false;

                            }
                        }
                    }
                }

                else if (CompareTag("Enemy"))
                {
                    for (int j = 0; j < combatManager.GetComponent<Manager_Combate>().m_players.Length; j++)
                    {

                        Vector2 playerPosition = combatManager.GetComponent<Manager_Combate>().m_players[j].transform.position;

                        if (positionToCheck == Manager_Grid.Instance.ScreenToTilePosition(Camera.main.WorldToScreenPoint(playerPosition)))
                        {
                            if (combatManager.GetComponent<Manager_Combate>().m_players[j].GetComponent<Controlador_Peon>().m_isAlive)
                            {
                                m_pawnToAttack = combatManager.GetComponent<Manager_Combate>().m_players[j].GetComponent<Controlador_Peon>();
                                //Debug.Log("Player Found");
                                return true;
                            }
                            else return false;
                        }


                    }
                }

               
            }
        }
        return false;
    }

}
