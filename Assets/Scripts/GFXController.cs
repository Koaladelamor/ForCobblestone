using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GFXController : MonoBehaviour
{
    public GameObject warrior, archer, tank;
    public Animator warriorAnim, archerAnim, tankAnim;

    private PawnController pc;

    // Start is called before the first frame update
    void Awake()
    {
        pc = GetComponent<PawnController>();

        switch (pc.character)
        {
            case PawnController.CHARACTER.GRODNAR:
                tank.SetActive(true);
                break;
            case PawnController.CHARACTER.LANSTAR:
                archer.SetActive(true); 
                break;
            case PawnController.CHARACTER.SIGFRID:
                warrior.SetActive(true);
                break;
            case PawnController.CHARACTER.LAST_NO_USE:
                break;
        }
    }

    public void Attack()
    {
        switch (pc.character)
        {
            case PawnController.CHARACTER.GRODNAR:
                tankAnim.SetTrigger("Attack");
                break;
            case PawnController.CHARACTER.LANSTAR:
                archerAnim.SetTrigger("Attack");
                break;
            case PawnController.CHARACTER.SIGFRID:
                warriorAnim.SetTrigger("Attack");
                break;
            case PawnController.CHARACTER.LAST_NO_USE:
                break;
        }
    }

    public void Hurt()
    {
        switch (pc.character)
        {
            case PawnController.CHARACTER.GRODNAR:
                tankAnim.SetTrigger("Hurt");
                break;
            case PawnController.CHARACTER.LANSTAR:
                archerAnim.SetTrigger("Hurt");
                break;
            case PawnController.CHARACTER.SIGFRID:
                warriorAnim.SetTrigger("Hurt");
                break;
            case PawnController.CHARACTER.LAST_NO_USE:
                break;
        }
    }

    public void Die()
    {
        switch (pc.character)
        {
            case PawnController.CHARACTER.GRODNAR:
                tankAnim.SetBool("IsDead", true);
                break;
            case PawnController.CHARACTER.LANSTAR:
                archerAnim.SetBool("IsDead", true);
                break;
            case PawnController.CHARACTER.SIGFRID:
                warriorAnim.SetBool("IsDead", true);
                break;
            case PawnController.CHARACTER.LAST_NO_USE:
                break;
        }
    }

    public void Move()
    {
        switch (pc.character)
        {
            case PawnController.CHARACTER.GRODNAR:
                tankAnim.SetBool("IsRunning", true);
                break;
            case PawnController.CHARACTER.LANSTAR:
                archerAnim.SetBool("IsRunning", true);
                break;
            case PawnController.CHARACTER.SIGFRID:
                warriorAnim.SetBool("IsRunning", true);
                break;
            case PawnController.CHARACTER.LAST_NO_USE:
                break;
        }
    }

    public void Idle()
    {
        switch (pc.character)
        {
            case PawnController.CHARACTER.GRODNAR:
                tankAnim.SetBool("IsRunning", false);
                break;
            case PawnController.CHARACTER.LANSTAR:
                archerAnim.SetBool("IsRunning", false);
                break;
            case PawnController.CHARACTER.SIGFRID:
                warriorAnim.SetBool("IsRunning", false);
                break;
            case PawnController.CHARACTER.LAST_NO_USE:
                break;
        }
    }
}
