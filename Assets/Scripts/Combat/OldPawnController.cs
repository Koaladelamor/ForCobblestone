using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class OldPawnController : MonoBehaviour
{
    public enum PAWN_STATUS { IDLE, SEARCH, ATTACK }

    protected bool draggable;
    protected bool isDragged;

    protected Vector3 mouseDragStartPos;
    protected Vector3 objDragStartPos;

    protected Vector3 m_position;
    protected Vector2 m_tilePosition;
    protected Vector3 m_previousPosition;

    protected Transform m_positionToGo;

    public PAWN_STATUS m_state;
    protected Vector3[] m_directions = { new Vector3(40f, 0, 0), new Vector3(0, -40f, 0), new Vector3(-40f, 0, 0), new Vector3(0, 40f, 0) };

    public int current_hp;
    protected int max_hp;
    protected int damage;

    public bool m_isAlive;

    protected float timer = 0f;
    protected float timeBetweenSteps = 0.3f;

    protected int m_maxSteps;
    protected int m_currentStep;

    public int m_turnOrder;
    public bool m_isMyTurn;
    public bool m_myTurnIsDone;

    protected bool readyToAttack;

    protected OldPawnController m_pawnToAttack;

    protected GameObject combatManager;

    public Animator animator;


    protected void Start()
    {

        if (CompareTag("Player"))
        {
            damage = 5;
            max_hp = 70;
            draggable = true;
        }
        else if (CompareTag("Enemy"))
        {
            damage = 1;
            max_hp = 20;
            draggable = false;
        }
        current_hp = max_hp;
        
        readyToAttack = true;
        m_isAlive = true;
        m_isMyTurn = false;
        m_currentStep = 0;
        m_maxSteps = 3;
        m_state = PAWN_STATUS.IDLE;
        m_position = transform.position;
        m_previousPosition = m_position;

        combatManager = GameObject.FindGameObjectWithTag("CombatManager");
        float tileSize = 40;
        m_directions[0] = new Vector3(tileSize, 0f, 0f);
        m_directions[1] = new Vector3(0, -tileSize, 0);
        m_directions[2] = new Vector3(-tileSize, 0, 0);
        m_directions[3] = new Vector3(0, tileSize, 0);

    }
    protected void Update()
    {
        timer += Time.deltaTime;

        //ClosestPawn();

        //bool enemyIsClose = EnemyIsClose();

        switch (m_state)
        {
            default:
                break;
            case PAWN_STATUS.SEARCH:

                if (timer >= timeBetweenSteps) { 
                    Search();
                    timer = 0;
                }

                break;

            /*case PAWN_STATUS.IDLE:
                if (m_isMyTurn && enemyIsClose)
                {
                    m_state = PAWN_STATUS.ATTACK;
                }

                else if (m_isMyTurn && !enemyIsClose)
                {
                    m_state = PAWN_STATUS.SEARCH;
                }

                break;
            */
            case PAWN_STATUS.ATTACK:
                if (readyToAttack)
                {
                    Invoke("Attack", 1.2f);
                    readyToAttack = false;
                }
                
                else m_state = PAWN_STATUS.IDLE;

                break;
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
                else {
                    transform.position = m_position;
                }
            }
            else {
                transform.position = m_position;
            }
        }
    }

    /*protected virtual void ClosestPawn() {

        float distance;
        float closestDistance = 999999999;

        for (int i = 0; i < combatManager.GetComponent<CombatManager>().m_enemies.Length; i++)
        {
            if (combatManager.GetComponent<CombatManager>().m_enemies[i].GetComponent<OldPawnController>().m_isAlive) {
                distance = (transform.position - combatManager.GetComponent<CombatManager>().m_enemies[i].transform.position).magnitude;


                if (distance < closestDistance) {

                    closestDistance = distance;
                    m_positionToGo = combatManager.GetComponent<CombatManager>().m_enemies[i].transform;
                }
            }
        }
    }*/

    protected virtual void Search()
    {

        if ((transform.position - m_positionToGo.transform.position).magnitude == 40)
        {
            m_currentStep = 0;
            /*if (EnemyIsClose())
            {
                m_state = PAWN_STATUS.ATTACK;
                return;
            }*/

            m_state = PAWN_STATUS.IDLE;
            //m_isMyTurn = false;
            m_myTurnIsDone = true;
            combatManager.GetComponent<CombatManager>().SetTurnDone(true);
            return;
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
            m_currentStep = 0;
            /*if (EnemyIsClose())
            {
                m_state = PAWN_STATUS.ATTACK;
                return;
            }
            m_state = PAWN_STATUS.IDLE;
            //m_isMyTurn = false;
            m_myTurnIsDone = true;
            combatManager.GetComponent<CombatManager>().SetTurnDone(true);*/
        }
    }

    protected virtual void Attack() {
        //Attack
        if (m_isAlive && m_pawnToAttack.m_isAlive)
        {
            //Damage & Anim
            animator.SetBool("playerAttack", true);
            damage = Random.Range(2, 10);
            m_pawnToAttack.current_hp -= damage;

            //Enemy Death
            if (m_pawnToAttack.current_hp < 1)
            {

                Invoke("KillPawn", 0.4f);

                m_pawnToAttack.gameObject.GetComponent<OldPawnController>().m_isAlive = false;
                Vector2 pawnPosition = GridManager.Instance.ScreenToTilePosition(Camera.main.WorldToScreenPoint(m_pawnToAttack.transform.position));
                GridManager.Instance.TakePawnFromTile(pawnPosition);

            }

            m_state = PAWN_STATUS.IDLE;
            //m_isMyTurn = false;
            m_myTurnIsDone = true;
            combatManager.GetComponent<CombatManager>().SetTurnDone(true);

        }
        else {
            m_state = PAWN_STATUS.IDLE;
            //m_isMyTurn = false;
            m_myTurnIsDone = true;
            combatManager.GetComponent<CombatManager>().SetTurnDone(true);
        }

        readyToAttack = true;
    }

    public void SetPosition(Vector3 p_position)
    {
        transform.position = p_position;
        m_position = p_position;
    }


    /*public virtual bool EnemyIsClose() {
        for (int i = 0; i < m_directions.Length; i++)
        {
            Vector2 positionToCheck = GridManager.Instance.ScreenToTilePosition(Camera.main.WorldToScreenPoint(transform.position + m_directions[i]));

            if (!GridManager.Instance.IsTileEmpty(positionToCheck)) {
                
                for (int j = 0; j < combatManager.GetComponent<CombatManager>().m_enemies.Length; j++)
                {
                    if (combatManager.GetComponent<CombatManager>().m_enemies[j].GetComponent<OldPawnController>().m_isAlive)
                    {
                        Vector2 enemyPosition = combatManager.GetComponent<CombatManager>().m_enemies[j].transform.position;

                        if (positionToCheck == GridManager.Instance.ScreenToTilePosition(Camera.main.WorldToScreenPoint(enemyPosition)))
                        {
                            m_pawnToAttack = combatManager.GetComponent<CombatManager>().m_enemies[j].GetComponent<OldPawnController>();
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }*/

    public virtual void EndAttackAnimation() {
        animator.SetBool("playerAttack", false);        
    }

    public virtual void SpawnDamageText() {
        DamagePopUp damageText = combatManager.GetComponent<DamagePopUp>().Create(m_pawnToAttack.transform.position, damage);
        damageText.gameObject.GetComponent<TextMeshPro>().color = new Color(0, 255, 0);
    }

    public void CheckIfEnemyIsAlive() {
        if (m_pawnToAttack.current_hp < 1)
        {
            m_pawnToAttack.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    public void KillPawn()
    {
        m_pawnToAttack.gameObject.GetComponent<SpriteRenderer>().enabled = false; 
    }

    public void SetTurn(bool turn) { m_isMyTurn = turn; }

    public void SetTurnOn() { 
        m_isMyTurn = true;
        m_myTurnIsDone = false;
    }

    public void SetTurnOff()
    {
        m_isMyTurn = false;
        m_myTurnIsDone = true;
    }

    public bool GetTurn() { return m_isMyTurn; }
    public bool GetTurnDone() { return m_myTurnIsDone; }


}
