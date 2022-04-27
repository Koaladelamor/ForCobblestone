using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimBridge : MonoBehaviour
{

    public PawnController pawnc;
    
    public void ShowDamage()
    {
        pawnc.SpawnDamageText();
    }

    public void EndAnimation()
    {
        pawnc.EndAttackAnimation();
    }
}
