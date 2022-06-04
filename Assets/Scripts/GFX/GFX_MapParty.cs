using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GFX_MapParty : MonoBehaviour
{

    public GameObject warrior, archer, tank;
    public Animator warriorAnim, archerAnim, tankAnim;

    public void MapMove()
    {
        tankAnim.SetBool("IsRunning", true);
        archerAnim.SetBool("IsRunning", true);
        warriorAnim.SetBool("IsRunning", true);
    }

    public void MapIdle()
    {
        tankAnim.SetBool("IsRunning", false);
        archerAnim.SetBool("IsRunning", false);
        warriorAnim.SetBool("IsRunning", false);
    }

}
