using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyPawn : PawnController
{
    public enum EnemyType { MINOTAUR, SPIDER }
    public EnemyType enemyType;

    protected override void Awake() {
        attackTimer = 1.5f;
        attackCurrentTimer = 0;
        attackPerformed = false;
        attackEnded = false;
        speed = 50f;
        diagonalChecked = false;
        straighMovement = true;
        positionReached = false;
        tileToMove = null;
        m_pawnToAttack = null;
        m_currentTile = null;
        myTurn = false;
        draggable = false;
        m_state = PAWN_STATUS.IDLE;
        m_position = transform.position;
        m_previousPosition = m_position;
        combatManager = GameObject.FindGameObjectWithTag("CombatManager").GetComponent<CombatManager>();
    }

    protected override void Start()
    {
        switch (enemyType)
        {
            case EnemyType.MINOTAUR:
                List<Stat> MinotaurStats = GameStats.Instance.GetMinotaurStats();
                for (int g = 0; g < MinotaurStats.Count; g++)
                {
                    if (MinotaurStats[g].attribute == Attributes.MIN_DAMAGE)
                    {
                        min_damage = MinotaurStats[g].value;
                    }
                    else if (MinotaurStats[g].attribute == Attributes.MAX_DAMAGE)
                    {
                        max_damage = MinotaurStats[g].value;
                    }
                    else if (MinotaurStats[g].attribute == Attributes.MAX_HEALTH)
                    {
                        MAX_HP = MinotaurStats[g].value;
                    }
                    else if (MinotaurStats[g].attribute == Attributes.CURR_HEALTH)
                    {
                        cur_hp = MinotaurStats[g].value;
                    }
                    else if (MinotaurStats[g].attribute == Attributes.AGILITY)
                    {
                        agility = MinotaurStats[g].value;
                    }
                }
                break;
            case EnemyType.SPIDER:
                break;
            default:
                break;
        }
    }

    protected override void Update()
    {
        if (myTurn)
        {
            switch (m_state)
            {
                default:
                    break;

                case PAWN_STATUS.IDLE:
                    if (myTurn)
                    {
                        m_state = PAWN_STATUS.GET_PAWN;
                    }
                    break;

                case PAWN_STATUS.ATTACK:
                    if (!attackPerformed)
                    {
                        attackPerformed = true;
                        damage = Random.Range(min_damage, max_damage + 1);
                        m_pawnToAttack.TakeDamage(damage);
                        animator.SetBool("isAttacking", true);
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
                        }
                    }
                    break;


                case PAWN_STATUS.RETURN:
                    if (transform.position.x >= m_initialPosition.x)
                    {
                        transform.position = m_initialPosition;
                        positionReached = true;
                        Debug.Log("POSITION REACHED");
                        combatManager.NextTurn();
                        myTurn = false;
                        m_state = PAWN_STATUS.IDLE;
                        break;
                    }

                    if (transform.position.y == m_initialPosition.y)
                    {
                        straighMovement = true;
                    }
                    else straighMovement = false;

                    if (straighMovement)
                    {
                        transform.position += Vector3.right * speed * Time.deltaTime;
                    }
                    else
                    {
                        if (transform.position.y > m_initialPosition.y)
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

                case PAWN_STATUS.MOVE:
                    if (transform.position.x <= m_positionToGo.position.x)
                    {
                        transform.position = m_positionToGo.position;
                        positionReached = true;
                        diagonalChecked = false;
                        Debug.Log("POSITION REACHED");
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
                        transform.position += Vector3.left * speed * Time.deltaTime;
                    }
                    else
                    {
                        if (transform.position.y > m_positionToGo.position.y)
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

                case PAWN_STATUS.GET_PAWN:
                    GetPawnToAttack();
                    m_initialPosition = transform.position;
                    m_state = PAWN_STATUS.MOVE;
                    break;

            }
        }

    }

    protected override bool OnNextToLastTile(Vector3 currentPosition, Vector3 positionToGo)
    {
        Vector3 nextToLastTileIndex = GridManager.Instance.ScreenToTilePosition(Camera.main.WorldToScreenPoint(positionToGo));
        nextToLastTileIndex += Vector3.right;
        Vector3 nextToLastPosition = GridManager.Instance.GetTile(nextToLastTileIndex).transform.position;

        if (currentPosition.x >= nextToLastPosition.x)
        {
            return true;
        }
        return false;
    }

    protected override void GetPawnToAttack()
    {

        Vector2 currentTilePosition = GridManager.Instance.ScreenToTilePosition(Camera.main.WorldToScreenPoint(GetCurrentTile().transform.position));

        switch (m_type)
        {
            case PAWN_TYPE.RANGED:
                break;
            case PAWN_TYPE.MELEE:
                if (!GridManager.Instance.IsTileEmpty(currentTilePosition + new Vector2(-2, 0)))
                {
                    TileManager enemyTilePosition = GridManager.Instance.GetTile(currentTilePosition + new Vector2(-2, 0));
                    m_pawnToAttack = enemyTilePosition.GetPawn();
                    m_positionToGo = GridManager.Instance.GetTile(currentTilePosition + new Vector2(-1, 0)).transform;
                }
                else if (!GridManager.Instance.IsTileEmpty(currentTilePosition + new Vector2(-3, 0)))
                {
                    TileManager enemyTilePosition = GridManager.Instance.GetTile(currentTilePosition + new Vector2(-3, 0));
                    m_pawnToAttack = enemyTilePosition.GetPawn();
                    m_positionToGo = GridManager.Instance.GetTile(currentTilePosition + new Vector2(-2, 0)).transform;
                }
                else if (!GridManager.Instance.IsTileEmpty(currentTilePosition + new Vector2(-2, 1)) && !GridManager.Instance.IsTileEmpty(currentTilePosition + new Vector2(-2, -1)))
                {
                    //RANDOM PICK
                    int randomInt = Random.Range(0, 2);
                    if (randomInt == 0)
                    {
                        TileManager enemyTilePosition = GridManager.Instance.GetTile(currentTilePosition + new Vector2(-2, 1));
                        m_pawnToAttack = enemyTilePosition.GetPawn();
                        m_positionToGo = GridManager.Instance.GetTile(currentTilePosition + new Vector2(-1, 1)).transform;
                    }
                    else if (randomInt == 1)
                    {
                        TileManager enemyTilePosition = GridManager.Instance.GetTile(currentTilePosition + new Vector2(-2, -1));
                        m_pawnToAttack = enemyTilePosition.GetPawn();
                        m_positionToGo = GridManager.Instance.GetTile(currentTilePosition + new Vector2(-1, -1)).transform;
                    }
                }
                else if (!GridManager.Instance.IsTileEmpty(currentTilePosition + new Vector2(-2, 1)))
                {
                    TileManager enemyTilePosition = GridManager.Instance.GetTile(currentTilePosition + new Vector2(-2, 1));
                    m_pawnToAttack = enemyTilePosition.GetPawn();
                    m_positionToGo = GridManager.Instance.GetTile(currentTilePosition + new Vector2(-1, 1)).transform;
                }
                else if (!GridManager.Instance.IsTileEmpty(currentTilePosition + new Vector2(-2, -1)))
                {
                    TileManager enemyTilePosition = GridManager.Instance.GetTile(currentTilePosition + new Vector2(-2, -1));
                    m_pawnToAttack = enemyTilePosition.GetPawn();
                    m_positionToGo = GridManager.Instance.GetTile(currentTilePosition + new Vector2(-1, -1)).transform;
                }
                else if (!GridManager.Instance.IsTileEmpty(currentTilePosition + new Vector2(-3, 1)) && !GridManager.Instance.IsTileEmpty(currentTilePosition + new Vector2(-3, -1)))
                {
                    //RANDOM PICK
                    int randomInt = Random.Range(0, 2);
                    if (randomInt == 0)
                    {
                        TileManager enemyTilePosition = GridManager.Instance.GetTile(currentTilePosition + new Vector2(-3, 1));
                        m_pawnToAttack = enemyTilePosition.GetPawn();
                        m_positionToGo = GridManager.Instance.GetTile(currentTilePosition + new Vector2(-2, 1)).transform;
                    }
                    else if (randomInt == 1)
                    {
                        TileManager enemyTilePosition = GridManager.Instance.GetTile(currentTilePosition + new Vector2(-3, -1));
                        m_pawnToAttack = enemyTilePosition.GetPawn();
                        m_positionToGo = GridManager.Instance.GetTile(currentTilePosition + new Vector2(-2, -1)).transform;
                    }
                }
                else if (!GridManager.Instance.IsTileEmpty(currentTilePosition + new Vector2(-3, 1)))
                {
                    TileManager enemyTilePosition = GridManager.Instance.GetTile(currentTilePosition + new Vector2(-3, 1));
                    m_pawnToAttack = enemyTilePosition.GetPawn();
                    m_positionToGo = GridManager.Instance.GetTile(currentTilePosition + new Vector2(-2, 1)).transform;
                }
                else if (!GridManager.Instance.IsTileEmpty(currentTilePosition + new Vector2(-3, -1)))
                {
                    TileManager enemyTilePosition = GridManager.Instance.GetTile(currentTilePosition + new Vector2(-3, -1));
                    m_pawnToAttack = enemyTilePosition.GetPawn();
                    m_positionToGo = GridManager.Instance.GetTile(currentTilePosition + new Vector2(-2, -1)).transform;
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
            case PAWN_TYPE.TANK:
                if (!GridManager.Instance.IsTileEmpty(currentTilePosition + new Vector2(-2, 0)))
                {
                    TileManager enemyTilePosition = GridManager.Instance.GetTile(currentTilePosition + new Vector2(-2, 0));
                    m_pawnToAttack = enemyTilePosition.GetPawn();
                    m_positionToGo = GridManager.Instance.GetTile(currentTilePosition + new Vector2(-1, 0)).transform;
                }
                else if (!GridManager.Instance.IsTileEmpty(currentTilePosition + new Vector2(-2, 1)) && !GridManager.Instance.IsTileEmpty(currentTilePosition + new Vector2(-2, -1)))
                {
                    //RANDOM PICK
                    int randomInt = Random.Range(0, 2);
                    if (randomInt == 0)
                    {
                        TileManager enemyTilePosition = GridManager.Instance.GetTile(currentTilePosition + new Vector2(-2, 1));
                        m_pawnToAttack = enemyTilePosition.GetPawn();
                        m_positionToGo = GridManager.Instance.GetTile(currentTilePosition + new Vector2(-1, 1)).transform;
                    }
                    else if (randomInt == 1)
                    {
                        TileManager enemyTilePosition = GridManager.Instance.GetTile(currentTilePosition + new Vector2(-2, -1));
                        m_pawnToAttack = enemyTilePosition.GetPawn();
                        m_positionToGo = GridManager.Instance.GetTile(currentTilePosition + new Vector2(-1, -1)).transform;
                    }
                }
                else if (!GridManager.Instance.IsTileEmpty(currentTilePosition + new Vector2(-2, 1)))
                {
                    TileManager enemyTilePosition = GridManager.Instance.GetTile(currentTilePosition + new Vector2(-2, 1));
                    m_pawnToAttack = enemyTilePosition.GetPawn();
                    m_positionToGo = GridManager.Instance.GetTile(currentTilePosition + new Vector2(-1, 1)).transform;
                }
                else if (!GridManager.Instance.IsTileEmpty(currentTilePosition + new Vector2(-2, -1)))
                {
                    TileManager enemyTilePosition = GridManager.Instance.GetTile(currentTilePosition + new Vector2(-2, -1));
                    m_pawnToAttack = enemyTilePosition.GetPawn();
                    m_positionToGo = GridManager.Instance.GetTile(currentTilePosition + new Vector2(-1, -1)).transform;
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
            default:
                break;
        }
    }

    public override void SpawnDamageText()
    {
        combatManager.GetComponent<DamagePopUp>().Create(m_pawnToAttack.transform.position, damage);
    }

    public override void EndAttackAnimation()
    {
        animator.SetBool("isAttacking", false);
    }
}
