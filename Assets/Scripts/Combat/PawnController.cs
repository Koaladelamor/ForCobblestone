using System.Collections.Generic;
using UnityEngine;

public enum PAWN_STATUS { IDLE, ATTACK, MOVE, RETURN, GET_PAWN }
public enum PAWN_TYPE { RANGED, MELEE, TANK }

public class PawnController : MonoBehaviour
{
    public enum CHARACTER { GRODNAR, LANSTAR, SIGFRID, SPIDER, WORM, LAST_NO_USE }
    public CHARACTER character;

    protected bool draggable;
    protected bool isDragged;
    protected Vector3 mouseDragStartPos;
    protected Vector3 objDragStartPos;

    protected Vector3 m_position;
    protected Vector2 m_tilePosition;
    protected Vector3 m_previousPosition;

    protected Transform m_positionToGo;
    protected Vector3 m_initialPosition;

    protected PAWN_STATUS m_state;
    public PAWN_TYPE m_type;

    protected int MAX_HP;
    protected int cur_hp;
    protected int min_damage;
    protected int max_damage;
    protected int damage;
    protected int agility;

    protected PawnController m_pawnToAttack;
    protected TileManager m_currentTile;
    protected CombatManager combatManager;
    protected TileManager tileToMove;

    protected GFXController gfxController;

    protected bool myTurn;

    protected bool diagonalChecked;
    protected bool straighMovement;
    protected bool positionReached;
    protected float speed;

    protected bool attackPerformed;
    protected bool attackEnded;
    protected float attackTimer;
    protected float attackCurrentTimer;

    protected bool alive;

    protected virtual void Awake()
    {
        alive = true;
        attackTimer = 1.5f;
        attackCurrentTimer = 0;
        attackPerformed = false;
        attackEnded = false;
        speed = 70f;
        diagonalChecked = false;
        straighMovement = true;
        positionReached = false;
        tileToMove = null;
        m_pawnToAttack = null;
        m_currentTile = null;
        myTurn = false;
        draggable = true;
        m_state = PAWN_STATUS.IDLE;
        m_tilePosition = new Vector2(-1, -1);
        m_position = transform.position;
        m_previousPosition = m_position;
        combatManager = GameObject.FindGameObjectWithTag("CombatManager").GetComponent<CombatManager>();
        gfxController = GetComponent<GFXController>();
        //OnHurtAnimEnd += OnHurtAnimDone;
    }

    protected virtual void Start()
    {
        switch (character)
        {
            case CHARACTER.GRODNAR:
                List<Stat> GrodnarStats = GameStats.Instance.GetGrodnarStats();
                for (int g = 0; g < GrodnarStats.Count; g++)
                {
                    if (GrodnarStats[g].attribute == Attributes.MIN_DAMAGE) {
                        min_damage = GrodnarStats[g].value;
                    }
                    else if (GrodnarStats[g].attribute == Attributes.MAX_DAMAGE)
                    {
                        max_damage = GrodnarStats[g].value;
                    }
                    else if (GrodnarStats[g].attribute == Attributes.MAX_HEALTH)
                    {
                        MAX_HP = GrodnarStats[g].value;
                    }
                    else if (GrodnarStats[g].attribute == Attributes.CURR_HEALTH)
                    {
                        cur_hp = GrodnarStats[g].value;
                    }
                    else if (GrodnarStats[g].attribute == Attributes.AGILITY)
                    {
                        agility = GrodnarStats[g].value;
                    }
                }
                break;

            case CHARACTER.LANSTAR:
                List<Stat> LanstarStats = GameStats.Instance.GetLanstarStats();
                for (int g = 0; g < LanstarStats.Count; g++)
                {
                    if (LanstarStats[g].attribute == Attributes.MIN_DAMAGE)
                    {
                        min_damage = LanstarStats[g].value;
                    }
                    else if (LanstarStats[g].attribute == Attributes.MAX_DAMAGE)
                    {
                        max_damage = LanstarStats[g].value;
                    }
                    else if (LanstarStats[g].attribute == Attributes.MAX_HEALTH)
                    {
                        MAX_HP = LanstarStats[g].value;
                    }
                    else if (LanstarStats[g].attribute == Attributes.CURR_HEALTH)
                    {
                        cur_hp = LanstarStats[g].value;
                    }
                    else if (LanstarStats[g].attribute == Attributes.AGILITY)
                    {
                        agility = LanstarStats[g].value;
                    }
                }
                break;

            case CHARACTER.SIGFRID:
                List<Stat> SigfridStats = GameStats.Instance.GetSigfridStats();
                for (int g = 0; g < SigfridStats.Count; g++)
                {
                    if (SigfridStats[g].attribute == Attributes.MIN_DAMAGE)
                    {
                        min_damage = SigfridStats[g].value;
                    }
                    else if (SigfridStats[g].attribute == Attributes.MAX_DAMAGE)
                    {
                        max_damage = SigfridStats[g].value;
                    }
                    else if (SigfridStats[g].attribute == Attributes.MAX_HEALTH)
                    {
                        MAX_HP = SigfridStats[g].value;
                    }
                    else if (SigfridStats[g].attribute == Attributes.CURR_HEALTH)
                    {
                        cur_hp = SigfridStats[g].value;
                    }
                    else if (SigfridStats[g].attribute == Attributes.AGILITY)
                    {
                        agility = SigfridStats[g].value;
                    }
                }
                break;

            default:
                break;
        }
    }

    protected virtual void Update()
    {
        if (myTurn && character != CHARACTER.LANSTAR)
        {
            switch (m_state)
            {
                default:
                    break;

                case PAWN_STATUS.IDLE:
                    if (myTurn && alive)
                    {
                        m_state = PAWN_STATUS.GET_PAWN;
                    }
                    break;

                case PAWN_STATUS.ATTACK:
                    if (!attackPerformed)
                    {
                        gfxController.Idle();
                        gfxController.Attack();
                        attackPerformed = true;
                        damage = Random.Range(min_damage, max_damage + 1);
                        m_pawnToAttack.TakeDamage(damage);
                        //m_pawnToAttack.GetComponentInChildren<HealthBar>().HealthChangeEvent();
                    }
                    else
                    {
                        attackCurrentTimer += Time.deltaTime;
                        if (attackCurrentTimer >= attackTimer)
                        {
                            attackEnded = true;
                            attackCurrentTimer = 0;
                        }
                        if (attackEnded)
                        {
                            attackPerformed = false;
                            attackEnded = false;
                            m_state = PAWN_STATUS.RETURN;
                            gfxController.Move();
                        }
                    }
                    break;

                case PAWN_STATUS.RETURN:
                    if (transform.position.x <= m_initialPosition.x)
                    {
                        transform.position = m_initialPosition;
                        positionReached = true;
                        //Debug.Log("POSITION REACHED");
                        combatManager.NextTurn();
                        myTurn = false;
                        m_state = PAWN_STATUS.IDLE;
                        gfxController.Idle();
                        break;
                    }

                    if (transform.position.y == m_initialPosition.y)
                    {
                        straighMovement = true;
                    }
                    else straighMovement = false;

                    if (straighMovement)
                    {
                        transform.position += Vector3.left * speed * Time.deltaTime;
                    }
                    else
                    {
                        if (transform.position.y > m_initialPosition.y)
                        {
                            Vector3 movePos = Vector3.left + Vector3.down;
                            movePos.Normalize();
                            transform.position += movePos * speed * Time.deltaTime;
                        }
                        else
                        {
                            Vector3 movePos = Vector3.left + Vector3.up;
                            movePos.Normalize();
                            transform.position += movePos * speed * Time.deltaTime;
                        }
                    }
                    break;

                case PAWN_STATUS.MOVE:
                    if (m_positionToGo == null)
                    {
                        combatManager.NextTurn();
                        myTurn = false;
                        break;
                    }
                    if (transform.position.x >= m_positionToGo.position.x)
                    {
                        transform.position = m_positionToGo.position;
                        positionReached = true;
                        diagonalChecked = false;
                        //Debug.Log("POSITION REACHED");
                        m_state = PAWN_STATUS.ATTACK;
                        break;
                    }

                    if (transform.position.y != m_positionToGo.position.y && OnNextToLastTile(transform.position, m_positionToGo.position) && !diagonalChecked)
                    {
                        straighMovement = false;
                        diagonalChecked = true;
                    }
                    else if (!diagonalChecked)
                    {
                        straighMovement = true;
                    }

                    if (straighMovement)
                    {
                        transform.position += Vector3.right * speed * Time.deltaTime;
                    }
                    else
                    {
                        if (transform.position.y > m_positionToGo.position.y)
                        {
                            Vector3 movePos = Vector3.right + Vector3.down;
                            movePos.Normalize();
                            transform.position += movePos * speed * Time.deltaTime;
                        }
                        else
                        {
                            Vector3 movePos = Vector3.right + Vector3.up;
                            movePos.Normalize();
                            transform.position += movePos * speed * Time.deltaTime;
                        }
                    }
                    break;

                case PAWN_STATUS.GET_PAWN:
                    GetPawnToAttack();
                    if (m_positionToGo == null)
                    {
                        m_state = PAWN_STATUS.IDLE;
                        combatManager.NextTurn();
                        myTurn = false;
                        break;
                    }
                    m_initialPosition = transform.position;
                    m_state = PAWN_STATUS.MOVE;
                    gfxController.Move();

                    break;

            }
        }
        else if (myTurn && character == CHARACTER.LANSTAR) 
        {

            switch (m_state)
            {
                case PAWN_STATUS.IDLE:

                    if (myTurn)
                    {
                        m_state = PAWN_STATUS.GET_PAWN;
                    }
                    break;
                case PAWN_STATUS.ATTACK:
                    if (!attackPerformed)
                    {
                        gfxController.Idle();
                        gfxController.Attack();
                        attackPerformed = true;
                        damage = Random.Range(min_damage, max_damage + 1);
                        m_pawnToAttack.TakeDamage(damage);
                        //m_pawnToAttack.GetComponentInChildren<HealthBar>().HealthChangeEvent();
                    }
                    else
                    {
                        attackCurrentTimer += Time.deltaTime;
                        if (attackCurrentTimer >= attackTimer)
                        {
                            attackEnded = true;
                            attackCurrentTimer = 0;
                        }
                        if (attackEnded)
                        {
                            attackPerformed = false;
                            attackEnded = false;
                            /*m_state = PAWN_STATUS.RETURN;
                            gfxController.Move();*/
                            combatManager.NextTurn();
                            myTurn = false;
                            m_state = PAWN_STATUS.IDLE;
                        }
                    }
                    break;
                case PAWN_STATUS.MOVE:
                    break;
                case PAWN_STATUS.RETURN:
                    break;
                case PAWN_STATUS.GET_PAWN:
                    GetPawnToAttack();
                    if (m_positionToGo == null && m_pawnToAttack != null)
                    {
                        m_state = PAWN_STATUS.ATTACK;
                        break;
                    }
                    else if (m_positionToGo != null && m_pawnToAttack != null) {
                        m_initialPosition = transform.position;
                        m_state = PAWN_STATUS.MOVE;
                        gfxController.Move();
                        break;
                    }
                    break;
                default:
                    break;
            }
        }

    }

    protected virtual bool OnNextToLastTile(Vector3 currentPosition, Vector3 positionToGo) {
        Vector3 nextToLastTileIndex = GridManager.Instance.ScreenToTilePosition(Camera.main.WorldToScreenPoint(positionToGo));
        nextToLastTileIndex += Vector3.left;
        Vector3 nextToLastPosition = GridManager.Instance.GetTile(nextToLastTileIndex).transform.position;

        if (currentPosition.x >= nextToLastPosition.x) {
            return true;
        }
        return false;
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

            GridManager.Instance.StartingTiles_LightsOn();
        }
    }

    private void OnMouseUp()
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
                    if (m_tilePosition != new Vector2(-1, -1))
                    {
                        GridManager.Instance.TakePawnFromTile(m_tilePosition);
                    }
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

    protected virtual void GetPawnToAttack() {

        if (!alive)
        {
            myTurn = false;
            combatManager.NextTurn();
            Debug.Log("PAWN IS DEAD");
            return;
        }

        Vector2 currentTilePosition = GridManager.Instance.ScreenToTilePosition(Camera.main.WorldToScreenPoint(GetCurrentTile().transform.position));

        switch (m_type)
        {
            case PAWN_TYPE.RANGED:
                if (!GridManager.Instance.IsTileEmpty(currentTilePosition + new Vector2(2, 0)) && !GridManager.Instance.OutOfGrid(currentTilePosition + new Vector2(2, 0)))
                {
                    TileManager enemyTilePosition = GridManager.Instance.GetTile(currentTilePosition + new Vector2(2, 0));
                    m_pawnToAttack = enemyTilePosition.GetPawn();
                    m_positionToGo = null;
                }
                else if (!GridManager.Instance.IsTileEmpty(currentTilePosition + new Vector2(3, 0)) && !GridManager.Instance.OutOfGrid(currentTilePosition + new Vector2(3, 0)))
                {
                    TileManager enemyTilePosition = GridManager.Instance.GetTile(currentTilePosition + new Vector2(3, 0));
                    m_pawnToAttack = enemyTilePosition.GetPawn();
                    m_positionToGo = null;
                }
                else if (!GridManager.Instance.IsTileEmpty(currentTilePosition + new Vector2(2, 1)) && !GridManager.Instance.OutOfGrid(currentTilePosition + new Vector2(2, 1)) && !GridManager.Instance.IsTileEmpty(currentTilePosition + new Vector2(2, -1)) && !GridManager.Instance.OutOfGrid(currentTilePosition + new Vector2(2, -1)))
                {
                    //RANDOM PICK
                    int randomInt = Random.Range(0, 2);
                    if (randomInt == 0)
                    {
                        TileManager enemyTilePosition = GridManager.Instance.GetTile(currentTilePosition + new Vector2(2, 1));
                        m_pawnToAttack = enemyTilePosition.GetPawn();
                        m_positionToGo = GridManager.Instance.GetTile(currentTilePosition + new Vector2(0, 1)).transform;
                    }
                    else if (randomInt == 1)
                    {
                        TileManager enemyTilePosition = GridManager.Instance.GetTile(currentTilePosition + new Vector2(2, -1));
                        m_pawnToAttack = enemyTilePosition.GetPawn();
                        m_positionToGo = GridManager.Instance.GetTile(currentTilePosition + new Vector2(0, -1)).transform;
                    }
                }
                else if (!GridManager.Instance.IsTileEmpty(currentTilePosition + new Vector2(2, 1)) && !GridManager.Instance.OutOfGrid(currentTilePosition + new Vector2(2, 1)))
                {
                    TileManager enemyTilePosition = GridManager.Instance.GetTile(currentTilePosition + new Vector2(2, 1));
                    m_pawnToAttack = enemyTilePosition.GetPawn();
                    m_positionToGo = GridManager.Instance.GetTile(currentTilePosition + new Vector2(0, 1)).transform;
                }
                else if (!GridManager.Instance.IsTileEmpty(currentTilePosition + new Vector2(2, -1)) && !GridManager.Instance.OutOfGrid(currentTilePosition + new Vector2(2, -1)))
                {
                    TileManager enemyTilePosition = GridManager.Instance.GetTile(currentTilePosition + new Vector2(2, -1));
                    m_pawnToAttack = enemyTilePosition.GetPawn();
                    m_positionToGo = GridManager.Instance.GetTile(currentTilePosition + new Vector2(0, -1)).transform;
                }
                else if (!GridManager.Instance.IsTileEmpty(currentTilePosition + new Vector2(3, 1)) && !GridManager.Instance.OutOfGrid(currentTilePosition + new Vector2(3, 1)) && !GridManager.Instance.IsTileEmpty(currentTilePosition + new Vector2(3, -1)) && !GridManager.Instance.OutOfGrid(currentTilePosition + new Vector2(3, -1)))
                {
                    //RANDOM PICK
                    int randomInt = Random.Range(0, 2);
                    if (randomInt == 0)
                    {
                        TileManager enemyTilePosition = GridManager.Instance.GetTile(currentTilePosition + new Vector2(3, 1));
                        m_pawnToAttack = enemyTilePosition.GetPawn();
                        m_positionToGo = GridManager.Instance.GetTile(currentTilePosition + new Vector2(0, 1)).transform;
                    }
                    else if (randomInt == 1)
                    {
                        TileManager enemyTilePosition = GridManager.Instance.GetTile(currentTilePosition + new Vector2(3, -1));
                        m_pawnToAttack = enemyTilePosition.GetPawn();
                        m_positionToGo = GridManager.Instance.GetTile(currentTilePosition + new Vector2(0, -1)).transform;
                    }
                }
                else if (!GridManager.Instance.IsTileEmpty(currentTilePosition + new Vector2(3, 1)) && !GridManager.Instance.OutOfGrid(currentTilePosition + new Vector2(3, 1)))
                {
                    TileManager enemyTilePosition = GridManager.Instance.GetTile(currentTilePosition + new Vector2(3, 1));
                    m_pawnToAttack = enemyTilePosition.GetPawn();
                    m_positionToGo = GridManager.Instance.GetTile(currentTilePosition + new Vector2(0, 1)).transform;
                }
                else if (!GridManager.Instance.IsTileEmpty(currentTilePosition + new Vector2(3, -1)) && !GridManager.Instance.OutOfGrid(currentTilePosition + new Vector2(2, -1)))
                {
                    TileManager enemyTilePosition = GridManager.Instance.GetTile(currentTilePosition + new Vector2(3, -1));
                    m_pawnToAttack = enemyTilePosition.GetPawn();
                    m_positionToGo = GridManager.Instance.GetTile(currentTilePosition + new Vector2(0, -1)).transform;
                }
                else if (!GridManager.Instance.IsTileEmpty(currentTilePosition + new Vector2(2, 2)) && !GridManager.Instance.OutOfGrid(currentTilePosition + new Vector2(2, 2)))
                {
                    TileManager enemyTilePosition = GridManager.Instance.GetTile(currentTilePosition + new Vector2(2, 2));
                    m_pawnToAttack = enemyTilePosition.GetPawn();
                    m_positionToGo = GridManager.Instance.GetTile(currentTilePosition + new Vector2(0, 2)).transform;
                }
                else if (!GridManager.Instance.IsTileEmpty(currentTilePosition + new Vector2(2, -2)) && !GridManager.Instance.OutOfGrid(currentTilePosition + new Vector2(2, -2)))
                {
                    TileManager enemyTilePosition = GridManager.Instance.GetTile(currentTilePosition + new Vector2(2, -2));
                    m_pawnToAttack = enemyTilePosition.GetPawn();
                    m_positionToGo = GridManager.Instance.GetTile(currentTilePosition + new Vector2(0, -2)).transform;
                }
                else if (!GridManager.Instance.IsTileEmpty(currentTilePosition + new Vector2(3, 2)) && !GridManager.Instance.OutOfGrid(currentTilePosition + new Vector2(3, 2)))
                {
                    TileManager enemyTilePosition = GridManager.Instance.GetTile(currentTilePosition + new Vector2(3, 2));
                    m_pawnToAttack = enemyTilePosition.GetPawn();
                    m_positionToGo = GridManager.Instance.GetTile(currentTilePosition + new Vector2(0, 2)).transform;
                }
                else if (!GridManager.Instance.IsTileEmpty(currentTilePosition + new Vector2(3, -2)) && !GridManager.Instance.OutOfGrid(currentTilePosition + new Vector2(3, -2)))
                {
                    TileManager enemyTilePosition = GridManager.Instance.GetTile(currentTilePosition + new Vector2(3, -2));
                    m_pawnToAttack = enemyTilePosition.GetPawn();
                    m_positionToGo = GridManager.Instance.GetTile(currentTilePosition + new Vector2(0, -2)).transform;
                }
                else if (!GridManager.Instance.IsTileEmpty(currentTilePosition + new Vector2(2, 3)) && !GridManager.Instance.OutOfGrid(currentTilePosition + new Vector2(2, 3)))
                {
                    TileManager enemyTilePosition = GridManager.Instance.GetTile(currentTilePosition + new Vector2(2, 3));
                    m_pawnToAttack = enemyTilePosition.GetPawn();
                    m_positionToGo = GridManager.Instance.GetTile(currentTilePosition + new Vector2(0, 3)).transform;
                }
                else if (!GridManager.Instance.IsTileEmpty(currentTilePosition + new Vector2(2, -3)) && !GridManager.Instance.OutOfGrid(currentTilePosition + new Vector2(2, -3)))
                {
                    TileManager enemyTilePosition = GridManager.Instance.GetTile(currentTilePosition + new Vector2(2, -3));
                    m_pawnToAttack = enemyTilePosition.GetPawn();
                    m_positionToGo = GridManager.Instance.GetTile(currentTilePosition + new Vector2(0, -3)).transform;
                }
                else if (!GridManager.Instance.IsTileEmpty(currentTilePosition + new Vector2(3, 3)) && !GridManager.Instance.OutOfGrid(currentTilePosition + new Vector2(3, 3)))
                {
                    TileManager enemyTilePosition = GridManager.Instance.GetTile(currentTilePosition + new Vector2(3, 3));
                    m_pawnToAttack = enemyTilePosition.GetPawn();
                    m_positionToGo = GridManager.Instance.GetTile(currentTilePosition + new Vector2(0, 3)).transform;
                }
                else if (!GridManager.Instance.IsTileEmpty(currentTilePosition + new Vector2(3, -3)) && !GridManager.Instance.OutOfGrid(currentTilePosition + new Vector2(3, -3)))
                {
                    TileManager enemyTilePosition = GridManager.Instance.GetTile(currentTilePosition + new Vector2(3, -3));
                    m_pawnToAttack = enemyTilePosition.GetPawn();
                    m_positionToGo = GridManager.Instance.GetTile(currentTilePosition + new Vector2(0, -3)).transform;
                }
                else
                {
                    m_pawnToAttack = null;
                    m_positionToGo = null;
                    combatManager.NextTurn();
                    myTurn = false;
                    m_state = PAWN_STATUS.IDLE;
                    Debug.Log("NO ENEMY TO ATTACK");
                }
                break;
            case PAWN_TYPE.MELEE:
                if (!GridManager.Instance.IsTileEmpty(currentTilePosition + new Vector2(2, 0)) && !GridManager.Instance.OutOfGrid(currentTilePosition + new Vector2(2, 0)))
                {
                    TileManager enemyTilePosition = GridManager.Instance.GetTile(currentTilePosition + new Vector2(2, 0));
                    m_pawnToAttack = enemyTilePosition.GetPawn();
                    m_positionToGo = GridManager.Instance.GetTile(currentTilePosition + new Vector2(1, 0)).transform;
                }
                else if (!GridManager.Instance.IsTileEmpty(currentTilePosition + new Vector2(3, 0)) && !GridManager.Instance.OutOfGrid(currentTilePosition + new Vector2(3, 0)))
                {
                    TileManager enemyTilePosition = GridManager.Instance.GetTile(currentTilePosition + new Vector2(3, 0));
                    m_pawnToAttack = enemyTilePosition.GetPawn();
                    m_positionToGo = GridManager.Instance.GetTile(currentTilePosition + new Vector2(2, 0)).transform;
                }
                else if (!GridManager.Instance.IsTileEmpty(currentTilePosition + new Vector2(2, 1)) && !GridManager.Instance.OutOfGrid(currentTilePosition + new Vector2(2, 1)) && !GridManager.Instance.IsTileEmpty(currentTilePosition + new Vector2(2, -1)) && !GridManager.Instance.OutOfGrid(currentTilePosition + new Vector2(2, -1)))
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
                else if (!GridManager.Instance.IsTileEmpty(currentTilePosition + new Vector2(2, 1)) && !GridManager.Instance.OutOfGrid(currentTilePosition + new Vector2(2, 1)))
                {
                    TileManager enemyTilePosition = GridManager.Instance.GetTile(currentTilePosition + new Vector2(2, 1));
                    m_pawnToAttack = enemyTilePosition.GetPawn();
                    m_positionToGo = GridManager.Instance.GetTile(currentTilePosition + new Vector2(1, 1)).transform;
                }
                else if (!GridManager.Instance.IsTileEmpty(currentTilePosition + new Vector2(2, -1)) && !GridManager.Instance.OutOfGrid(currentTilePosition + new Vector2(2, -1)))
                {
                    TileManager enemyTilePosition = GridManager.Instance.GetTile(currentTilePosition + new Vector2(2, -1));
                    m_pawnToAttack = enemyTilePosition.GetPawn();
                    m_positionToGo = GridManager.Instance.GetTile(currentTilePosition + new Vector2(1, -1)).transform;
                }
                else if (!GridManager.Instance.IsTileEmpty(currentTilePosition + new Vector2(3, 1)) && !GridManager.Instance.OutOfGrid(currentTilePosition + new Vector2(3, 1)) && !GridManager.Instance.IsTileEmpty(currentTilePosition + new Vector2(3, -1)) && !GridManager.Instance.OutOfGrid(currentTilePosition + new Vector2(3, -1)))
                {
                    //RANDOM PICK
                    int randomInt = Random.Range(0, 2);
                    if (randomInt == 0)
                    {
                        TileManager enemyTilePosition = GridManager.Instance.GetTile(currentTilePosition + new Vector2(3, 1));
                        m_pawnToAttack = enemyTilePosition.GetPawn();
                        m_positionToGo = GridManager.Instance.GetTile(currentTilePosition + new Vector2(2, 1)).transform;
                    }
                    else if (randomInt == 1)
                    {
                        TileManager enemyTilePosition = GridManager.Instance.GetTile(currentTilePosition + new Vector2(3, -1));
                        m_pawnToAttack = enemyTilePosition.GetPawn();
                        m_positionToGo = GridManager.Instance.GetTile(currentTilePosition + new Vector2(2, -1)).transform;
                    }
                }
                else if (!GridManager.Instance.IsTileEmpty(currentTilePosition + new Vector2(3, 1)) && !GridManager.Instance.OutOfGrid(currentTilePosition + new Vector2(3, 1)))
                {
                    TileManager enemyTilePosition = GridManager.Instance.GetTile(currentTilePosition + new Vector2(3, 1));
                    m_pawnToAttack = enemyTilePosition.GetPawn();
                    m_positionToGo = GridManager.Instance.GetTile(currentTilePosition + new Vector2(2, 1)).transform;
                }
                else if (!GridManager.Instance.IsTileEmpty(currentTilePosition + new Vector2(3, -1)) && !GridManager.Instance.OutOfGrid(currentTilePosition + new Vector2(3, -1)))
                {
                    TileManager enemyTilePosition = GridManager.Instance.GetTile(currentTilePosition + new Vector2(3, -1));
                    m_pawnToAttack = enemyTilePosition.GetPawn();
                    m_positionToGo = GridManager.Instance.GetTile(currentTilePosition + new Vector2(2, -1)).transform;
                }
                else {
                    m_pawnToAttack = null;
                    m_positionToGo = null;
                    combatManager.NextTurn();
                    myTurn = false;
                    m_state = PAWN_STATUS.IDLE;
                    Debug.Log("NO ENEMY TO ATTACK");
                }
                break;
            case PAWN_TYPE.TANK:
                if (!GridManager.Instance.IsTileEmpty(currentTilePosition + new Vector2(2, 0)) && !GridManager.Instance.OutOfGrid(currentTilePosition + new Vector2(2, 0)))
                {
                    TileManager enemyTilePosition = GridManager.Instance.GetTile(currentTilePosition + new Vector2(2, 0));
                    m_pawnToAttack = enemyTilePosition.GetPawn();
                    m_positionToGo = GridManager.Instance.GetTile(currentTilePosition + new Vector2(1, 0)).transform;
                }
                else if (!GridManager.Instance.IsTileEmpty(currentTilePosition + new Vector2(2, 1)) && !GridManager.Instance.OutOfGrid(currentTilePosition + new Vector2(2, 1)) && !GridManager.Instance.IsTileEmpty(currentTilePosition + new Vector2(2, -1)) && !GridManager.Instance.OutOfGrid(currentTilePosition + new Vector2(2, -1)))
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
                else if (!GridManager.Instance.IsTileEmpty(currentTilePosition + new Vector2(2, 1)) && !GridManager.Instance.OutOfGrid(currentTilePosition + new Vector2(2, 1)))
                {
                    TileManager enemyTilePosition = GridManager.Instance.GetTile(currentTilePosition + new Vector2(2, 1));
                    m_pawnToAttack = enemyTilePosition.GetPawn();
                    m_positionToGo = GridManager.Instance.GetTile(currentTilePosition + new Vector2(1, 1)).transform;
                }
                else if (!GridManager.Instance.IsTileEmpty(currentTilePosition + new Vector2(2, -1)) && !GridManager.Instance.OutOfGrid(currentTilePosition + new Vector2(2, -1)))
                {
                    Debug.Log(GridManager.Instance.IsTileEmpty(currentTilePosition + new Vector2(2, -1)));
                    TileManager enemyTilePosition = GridManager.Instance.GetTile(currentTilePosition + new Vector2(2, -1));
                    m_pawnToAttack = enemyTilePosition.GetPawn();
                    m_positionToGo = GridManager.Instance.GetTile(currentTilePosition + new Vector2(1, -1)).transform;
                }
                else {
                    m_pawnToAttack = null;
                    m_positionToGo = null;
                    combatManager.NextTurn();
                    myTurn = false;
                    m_state = PAWN_STATUS.IDLE;
                    Debug.Log("NO ENEMY TO ATTACK");
                }
                break;
            default:
                break;
        }
    }

    public void TakeDamage(int damage) {
        cur_hp -= damage;
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



    public void KillPawn()
    {
        m_pawnToAttack.gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void SetTurn(bool turn) { myTurn = turn; }

    public void SetTurnOn()
    {
        myTurn = true;
    }

    public void SetTurnOff()
    {
        myTurn = false;
    }

    public bool GetTurn() { return myTurn; }

    public int GetMaxHP() { return MAX_HP; }

    public int GetCurrentHP() { return cur_hp; }

    public int GetAgility() { return agility; }

    public PawnController GetCurrentTarget() { return m_pawnToAttack; }

    public int GetDamage() { return damage; }

    public GFXController GetGFXController() { return gfxController; }

    public CHARACTER GetCharacterType() { return character; }

    public void HurtAnimDone() { 
        if (cur_hp < 1) {
            alive = false;
            //DieSound
            if (m_state == PAWN_STATUS.MOVE || m_state == PAWN_STATUS.GET_PAWN)
            {
                m_state = PAWN_STATUS.IDLE;
            }
            GetCurrentTile().TakePawn();
            GetComponentInChildren<HealthBar>().transform.parent.gameObject.SetActive(false);
            gfxController.Die();
            if (myTurn) {
                combatManager.NextTurn();
                myTurn = false;
            }
        }
    }

    public bool IsAlive() { return alive; }
}
