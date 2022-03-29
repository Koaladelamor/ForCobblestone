using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyPawn : PawnController
{
    //bool draggable = false;
    protected override void Attack()
    {
        //attack
        if (m_isAlive && m_pawnToAttack.m_isAlive)
        {
            damage = Random.Range(0, 16);
            animator.SetBool("isAttacking", true);
            
            m_pawnToAttack.current_hp -= damage;

            if (m_pawnToAttack.current_hp < 1)
            {

                Invoke("killPawn", 0.4f);

                m_pawnToAttack.gameObject.GetComponent<PawnController>().m_isAlive = false;
                Vector2 pawnPosition = GridManager.Instance.ScreenToTilePosition(Camera.main.WorldToScreenPoint(m_pawnToAttack.transform.position));
                GridManager.Instance.TakePawnFromTile(pawnPosition);
                
            }

            m_state = PAWN_STATUS.IDLE;
            //m_isMyTurn = false;
            m_myTurnIsDone = true;

        }
        else
        {
            m_state = PAWN_STATUS.IDLE;
            //m_isMyTurn = false;
            m_myTurnIsDone = true;
        }

        readyToAttack = true;
    }

    protected override void ClosestPawn()
    {
        float distance;
        float closestDistance = 999999999;

        for (int i = 0; i < combatManager.GetComponent<CombatManager>().m_players.Length; i++)
        {
            if (combatManager.GetComponent<CombatManager>().m_players[i].GetComponent<PawnController>().m_isAlive)
            {
                distance = (transform.position - combatManager.GetComponent<CombatManager>().m_players[i].transform.position).magnitude;


                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    m_positionToGo = combatManager.GetComponent<CombatManager>().m_players[i].transform;
                }
            }
        }
    }

    public override bool EnemyIsClose()
    {
        for (int i = 0; i < m_directions.Length; i++)
        {
            Vector2 positionToCheck = GridManager.Instance.ScreenToTilePosition(Camera.main.WorldToScreenPoint(transform.position + m_directions[i]));

            for (int j = 0; j < combatManager.GetComponent<CombatManager>().m_players.Length; j++)
            {

                Vector2 playerPosition = combatManager.GetComponent<CombatManager>().m_players[j].transform.position;

                if (positionToCheck == GridManager.Instance.ScreenToTilePosition(Camera.main.WorldToScreenPoint(playerPosition)))
                {
                    if (combatManager.GetComponent<CombatManager>().m_players[j].GetComponent<PawnController>().m_isAlive)
                    {
                        m_pawnToAttack = combatManager.GetComponent<CombatManager>().m_players[j].GetComponent<PawnController>();
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public override void SpawnDamageText()
    {
        DamagePopUp damageText = combatManager.GetComponent<DamagePopUp>().Create(m_pawnToAttack.transform.position, damage);
    }
}
