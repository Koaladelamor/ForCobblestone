using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum PAWN_STATUS { IDLE, ATTACK, MOVE, GET_PAWN }
public enum PAWN_TYPE { RANGED, MELEE, TANK }

public class PawnController : MonoBehaviour
{
    

    protected bool draggable;
    protected bool isDragged;
    protected Vector3 mouseDragStartPos;
    protected Vector3 objDragStartPos;

    protected Vector3 m_position;
    protected Vector2 m_tilePosition;
    protected Vector3 m_previousPosition;

    protected Transform m_positionToGo;

    PAWN_STATUS m_state;
    public PAWN_TYPE m_type;

    protected float timer = 0f;

    protected int damage;

    protected PawnController m_pawnToAttack;
    protected TileManager m_currentTile;
    protected GameObject combatManager;

    public Animator animator;

    protected bool myTurn;
    protected bool myTurnIsDone;

    protected void Start()
    {
        m_pawnToAttack = null;
        m_currentTile = null;
        myTurn = false;
        draggable = true;
        m_state = PAWN_STATUS.IDLE;
        m_position = transform.position;
        m_previousPosition = m_position;
        combatManager = GameObject.FindGameObjectWithTag("CombatManager");
    }
    protected void Update()
    {
        if (myTurn)
        {
            timer += Time.deltaTime;

            switch (m_state)
            {
                default:
                    break;

                case PAWN_STATUS.IDLE:
                    if (myTurn) {
                        m_state = PAWN_STATUS.GET_PAWN;
                    }
                    break;

                case PAWN_STATUS.ATTACK:

                    break;

                case PAWN_STATUS.MOVE:
                    transform.position = m_positionToGo.position;
                    break;

                case PAWN_STATUS.GET_PAWN:
                    GetPawnToAttack();
                    m_state = PAWN_STATUS.MOVE;
                    break;

            }
        }

    }
    protected void OnMouseDown()
    {
        if (draggable)
        {
            isDragged = true;
            objDragStartPos = this.transform.position;
        }
    }

    protected void OnMouseDrag()
    {
        if (isDragged)
        {
            Vector3 screenCoordinate = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));

            transform.position = new Vector3(screenCoordinate.x, screenCoordinate.y, transform.position.z);

            GridManager.Instance.StartingTiles_LightsOn();
        }
    }

    protected void OnMouseUp()
    {
        if (isDragged)
        {

            GridManager.Instance.StartingTiles_LightsOff();
            isDragged = false;
            Vector2 tilePosition = GridManager.Instance.ScreenToTilePosition(Input.mousePosition);

            if (tilePosition.x == Vector2.positiveInfinity.x)
            {
                transform.position = m_position;
                return;
            }

            TileManager currentTile = GridManager.Instance.GetTile(tilePosition);
            if (currentTile.GetComponent<TileManager>().playerDraggableOnTile)
            {
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
            else
            {
                transform.position = m_position;
            }
        }
    }

    public void GetPawnToAttack() {
        switch (m_type)
        {
            case PAWN_TYPE.RANGED:
                break;
            case PAWN_TYPE.MELEE:
                Vector2 currentTilePosition = GridManager.Instance.ScreenToTilePosition(Camera.main.WorldToScreenPoint(GetCurrentTile().transform.position));
                if (!GridManager.Instance.IsTileEmpty(currentTilePosition + new Vector2(2, 0)))
                {
                    TileManager enemyTilePosition = GridManager.Instance.GetTile(currentTilePosition + new Vector2(2, 0));
                    m_pawnToAttack = enemyTilePosition.GetPawn();
                    m_positionToGo = GridManager.Instance.GetTile(currentTilePosition + new Vector2(1, 0)).transform;
                }
                else if (!GridManager.Instance.IsTileEmpty(currentTilePosition + new Vector2(3, 0)))
                {
                    TileManager enemyTilePosition = GridManager.Instance.GetTile(currentTilePosition + new Vector2(3, 0));
                    m_pawnToAttack = enemyTilePosition.GetPawn();
                    m_positionToGo = GridManager.Instance.GetTile(currentTilePosition + new Vector2(2, 0)).transform;
                }
                else if (!GridManager.Instance.IsTileEmpty(currentTilePosition + new Vector2(2, 1)) && !GridManager.Instance.IsTileEmpty(currentTilePosition + new Vector2(2, -1)))
                {
                    //RANDOM PICK
                    int randomInt = Random.Range(0, 2);
                    if (randomInt == 0)
                    {
                        TileManager enemyTilePosition = GridManager.Instance.GetTile(currentTilePosition + new Vector2(2, 1));
                        m_pawnToAttack = enemyTilePosition.GetPawn();
                        m_positionToGo = GridManager.Instance.GetTile(currentTilePosition + new Vector2(1, 1)).transform;
                    }
                    else if (randomInt == 1)
                    {
                        TileManager enemyTilePosition = GridManager.Instance.GetTile(currentTilePosition + new Vector2(2, -1));
                        m_pawnToAttack = enemyTilePosition.GetPawn();
                        m_positionToGo = GridManager.Instance.GetTile(currentTilePosition + new Vector2(1, -1)).transform;
                    }
                }
                else if (!GridManager.Instance.IsTileEmpty(currentTilePosition + new Vector2(2, 1)))
                {
                    TileManager enemyTilePosition = GridManager.Instance.GetTile(currentTilePosition + new Vector2(2, 1));
                    m_pawnToAttack = enemyTilePosition.GetPawn();
                    m_positionToGo = GridManager.Instance.GetTile(currentTilePosition + new Vector2(1, 1)).transform;
                }
                else if (!GridManager.Instance.IsTileEmpty(currentTilePosition + new Vector2(2, -1)))
                {
                    TileManager enemyTilePosition = GridManager.Instance.GetTile(currentTilePosition + new Vector2(2, -1));
                    m_pawnToAttack = enemyTilePosition.GetPawn();
                    m_positionToGo = GridManager.Instance.GetTile(currentTilePosition + new Vector2(1, -1)).transform;
                }
                else {
                    m_pawnToAttack = null;
                }
                break;
            case PAWN_TYPE.TANK:
                break;
            default:
                break;
        }
    }

    public void SetCurrentTile(TileManager tile) {
        m_currentTile = tile;
    }

    public TileManager GetCurrentTile() {
        return m_currentTile;
    }

    public void SetPosition(Vector3 p_position)
    {
        transform.position = p_position;
        m_position = p_position;
    }

    public virtual void SpawnDamageText()
    {
        DamagePopUp damageText = combatManager.GetComponent<DamagePopUp>().Create(m_pawnToAttack.transform.position, damage);
        damageText.gameObject.GetComponent<TextMeshPro>().color = new Color(0, 255, 0);
    }

    public void KillPawn()
    {
        m_pawnToAttack.gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void SetTurn(bool turn) { myTurn = turn; }

    public void SetTurnOn()
    {
        myTurn = true;
        myTurnIsDone = false;
    }

    public void SetTurnOff()
    {
        myTurn = false;
        myTurnIsDone = true;
    }

    public bool GetTurn() { return myTurn; }
    public bool GetTurnDone() { return myTurnIsDone; }

}
