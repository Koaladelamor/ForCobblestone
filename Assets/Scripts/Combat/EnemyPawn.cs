using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyPawn : PawnController
{
    
    public override void SpawnDamageText()
    {
        combatManager.GetComponent<DamagePopUp>().Create(m_pawnToAttack.transform.position, damage);
    }

    public override void EndAttackAnimation()
    {
        animator.SetBool("isAttacking", false);
    }
}
