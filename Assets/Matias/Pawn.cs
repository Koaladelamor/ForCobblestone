using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PAWN_STATE { IDLE, SEARCH, ATTACK }

public class Pawn : MonoBehaviour
{
    [SerializeField] Transform m_positionToGo;

    public bool m_isBeingDragged;
    Board m_board;
    Vector3 m_position;
    Vector2 m_tilePosition;
    Vector3 m_previousPosition;

    PAWN_STATE m_state;
    Vector3[] m_directions = { Vector3.right, Vector3.down, Vector3.left, Vector3.up };

    [SerializeField] bool m_isMyTurn;

    private void Start()
    {
        m_position = transform.position;
        m_previousPosition = m_position;
    }

    private void Update()
    {

        switch (m_state)
        {
            default:
                break;
            case PAWN_STATE.SEARCH:
                Search();
                break;
            case PAWN_STATE.IDLE:
                if (Input.GetKeyDown("space") && m_isMyTurn)
                {
                    m_state = PAWN_STATE.SEARCH;
                }
                break;
        }
    }

    private void OnMouseDrag()
    {
        Vector3 screenCoordinate = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));

        transform.position = new Vector3(screenCoordinate.x, screenCoordinate.y, transform.position.z);
    }

    private void OnMouseDown()
    {
        m_isBeingDragged = true;
    }

    private void OnMouseUp()
    {
        if (m_isBeingDragged) {

            m_isBeingDragged = false;
            Vector2 tilePosition = Board.Instance.ScreenToTilePosition(Input.mousePosition);

            if (tilePosition.x == Vector2.positiveInfinity.x) {
                transform.position = m_position;
                return;
            }

            if (Board.Instance.IsTileEmpty(tilePosition))
            {
                Board.Instance.TakePawnFromTile(m_tilePosition);
                m_tilePosition = tilePosition;
                Board.Instance.AssignPawnToTile(this.gameObject, tilePosition);
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
            m_state = PAWN_STATE.IDLE;
            return;
        }

        Vector3 closestDirection = Vector2.zero;
        float closestDistance = 100000;

        // check all 4 tiles and pick the closes one to the objective
        for (int i = 0; i < m_directions.Length; i++)
        {
            Vector2 positionToCheck = Board.Instance.ScreenToTilePosition(Camera.main.WorldToScreenPoint(transform.position + m_directions[i]));

            if (Board.Instance.IsTileEmpty(positionToCheck))
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
        Vector2 tileToMove = Board.Instance.ScreenToTilePosition(Camera.main.WorldToScreenPoint(positionToMove));
        // move into the designated tile

        m_previousPosition = transform.position;
        
        Board.Instance.TakePawnFromTile(m_tilePosition);
        m_tilePosition = tileToMove;
        Board.Instance.AssignPawnToTile(this.gameObject, tileToMove);
    }

    public void SetPosition(Vector3 p_position)
    {
        transform.position = p_position;
        m_position = p_position;
    }
}
