using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PAWN_STATUS { IDLE, SEARCH, ATTACK }

public class PawnController : MonoBehaviour
{
    public bool draggable;
    bool isDragged;

    private Vector3 mouseDragStartPos;
    private Vector3 objDragStartPos;

    GridManager m_board;
    Vector3 m_position;
    Vector2 m_tilePosition;
    Vector3 m_previousPosition;

    [SerializeField] Transform m_positionToGo;

    PAWN_STATE m_state;
    Vector3[] m_directions = { Vector3.right, Vector3.down, Vector3.left, Vector3.up };

    int current_hp;
    int max_hp;

    int damage;

    float timer = 0f;
    private float waitTime = 0.3f;

    int m_maxSteps;
    int m_currentStep;

    public int m_turnOrder;
    public bool m_isMyTurn;

    PawnController m_pawnToAttack;

    private GameObject combatManager;

    private void Start()
    {
        if (this.tag == "Player")
        {
            damage = 5;
            max_hp = 15;
        }
        else if (this.tag == "Enemy")
        {
            damage = 1;
            max_hp = 15;
        }
        current_hp = max_hp;

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

        switch (m_state)
        {
            default:
                break;
            case PAWN_STATE.SEARCH:

                if (timer >= waitTime) { 
                    Search();
                    timer = 0;
                }

                break;

            case PAWN_STATE.IDLE:
                if (/*Input.GetKeyDown(KeyCode.Space) &&*/ m_isMyTurn && !EnemyClose())
                {
                    m_state = PAWN_STATE.SEARCH;
                }
                if (m_isMyTurn && EnemyClose() ) {
                    m_state = PAWN_STATE.ATTACK;
                }
                break;

            case PAWN_STATE.ATTACK:

                //attack
                m_pawnToAttack.current_hp -= damage;
                if (m_pawnToAttack.current_hp <= 0)
                {
                    m_pawnToAttack.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                }
                Debug.Log(m_pawnToAttack.current_hp);

                if (!EnemyClose() && m_currentStep < m_maxSteps)
                {
                    m_state = PAWN_STATE.SEARCH;
                }

                m_isMyTurn = false;
                combatManager.GetComponent<CombatManager>().turnDone = true;
                m_state = PAWN_STATE.IDLE;


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
            Vector2 tilePosition = GridManager.Instance.ScreenToTilePosition(Input.mousePosition);

            if (tilePosition.x == Vector2.positiveInfinity.x)
            {
                transform.position = m_position;
                return;
            }

            if (GridManager.Instance.IsTileEmpty(tilePosition))
            {
                GridManager.Instance.TakePawnFromTile(m_tilePosition);
                m_tilePosition = tilePosition;
                GridManager.Instance.AssignPawnToTile(this.gameObject, tilePosition);
            }
            else
            {
                transform.position = m_position;
            }
        }
    }

    void Search()
    {
        if ((transform.position - m_positionToGo.transform.position).magnitude == 1)
        {
            if (EnemyClose()) { m_state = PAWN_STATE.ATTACK; }
            else {
                m_state = PAWN_STATE.IDLE;
                m_isMyTurn = false;
                combatManager.GetComponent<CombatManager>().turnDone = true;
                return;
            }
        }

        Vector3 closestDirection = Vector2.zero;
        float closestDistance = 100000;

        // check all 4 tiles and pick the closes one to the objective
        for (int i = 0; i < m_directions.Length; i++)
        {
            Vector2 positionToCheck = GridManager.Instance.ScreenToTilePosition(Camera.main.WorldToScreenPoint(transform.position + m_directions[i]));

            if (GridManager.Instance.IsTileEmpty(positionToCheck))
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
        Vector2 tileToMove = GridManager.Instance.ScreenToTilePosition(Camera.main.WorldToScreenPoint(positionToMove));

        // move into the designated tile
        m_previousPosition = transform.position;

        GridManager.Instance.TakePawnFromTile(m_tilePosition);
        m_tilePosition = tileToMove;
        GridManager.Instance.AssignPawnToTile(this.gameObject, tileToMove);

        m_currentStep++;

        if (m_currentStep >= m_maxSteps)
        {
            m_state = PAWN_STATE.IDLE;
            m_isMyTurn = false;
            m_currentStep = 0;
            combatManager.GetComponent<CombatManager>().turnDone = true;
        }
    }

    public void SetPosition(Vector3 p_position)
    {
        transform.position = p_position;
        m_position = p_position;
    }


    public bool EnemyClose() {
        for (int i = 0; i < m_directions.Length; i++)
        {
            Vector2 positionToCheck = transform.position + m_directions[i];

            if (GridManager.Instance.IsTileEmpty(positionToCheck))
            {
                return false;
            }
            else {
                if (this.tag == "Player") {
                    for (int j = 0; j < combatManager.GetComponent<CombatManager>().m_enemies.Length; j++) {
                        if (combatManager.GetComponent<CombatManager>().m_enemies[j].transform.position.x == positionToCheck.x && combatManager.GetComponent<CombatManager>().m_enemies[j].transform.position.y == positionToCheck.y) {
                            m_pawnToAttack = combatManager.GetComponent<CombatManager>().m_enemies[j].GetComponent<PawnController>();
                            //Debug.Log("Enemy Found");
                            return true;

                        }
                    }
                }

                else if (this.tag == "Enemy") {
                    for (int j = 0; j < combatManager.GetComponent<CombatManager>().m_players.Length; j++)
                    {
                        if (combatManager.GetComponent<CombatManager>().m_players[j].transform.position.x == positionToCheck.x && combatManager.GetComponent<CombatManager>().m_players[j].transform.position.y == positionToCheck.y)
                        {
                            m_pawnToAttack = combatManager.GetComponent<CombatManager>().m_players[j].GetComponent<PawnController>();
                            //Debug.Log("Player Found");
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }

}
