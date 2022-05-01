using TMPro;
using UnityEngine;

public class GFXController : MonoBehaviour
{
    public GameObject warrior, archer, tank, spider;
    public Animator warriorAnim, archerAnim, tankAnim, spiderAnim;

    private bool attackDone;

    private PawnController pawnController;
    private CombatManager combatManager;
    // Start is called before the first frame update
    private void Awake()
    {
        attackDone = false;
        combatManager = GameObject.FindGameObjectWithTag("CombatManager").GetComponent<CombatManager>();
        pawnController = GetComponent<PawnController>();

        switch (pawnController.character)
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
            case PawnController.CHARACTER.SPIDER:
                
                break;
            case PawnController.CHARACTER.WORM:
                
                break;
            case PawnController.CHARACTER.LAST_NO_USE:
                break;
        }
    }
    private void Update()
    {
        if (attackDone) {
            PawnController pawnToAttack = GetComponent<PawnController>().GetCurrentTarget();
            if (pawnToAttack != null)
            {
                attackDone = false;
                SpawnDamageText();
                pawnToAttack.GetComponentInChildren<HealthBar>().HealthChangeEvent();
                pawnToAttack.GetGFXController().Hurt();
            }
            else Debug.Log("NO PAWN TO ATTACK DETECTED");
        }
    }



    public void Attack()
    {
        switch (pawnController.character)
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
            case PawnController.CHARACTER.SPIDER:
                spiderAnim.SetTrigger("Attack");
                break;
            case PawnController.CHARACTER.WORM:

                break;
            case PawnController.CHARACTER.LAST_NO_USE:
                break;
        }
    }

    public void Hurt()
    {
        switch (pawnController.character)
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
            case PawnController.CHARACTER.SPIDER:
                spiderAnim.SetTrigger("Hurt");
                break;
            case PawnController.CHARACTER.WORM:

                break;
            case PawnController.CHARACTER.LAST_NO_USE:
                break;
        }
    }

    public void Die()
    {
        switch (pawnController.character)
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
            case PawnController.CHARACTER.SPIDER:
                spiderAnim.SetBool("IsDead", true);
                break;
            case PawnController.CHARACTER.WORM:

                break;
            case PawnController.CHARACTER.LAST_NO_USE:
                break;
        }
    }

    public void Move()
    {
        switch (pawnController.character)
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
            case PawnController.CHARACTER.SPIDER:
                spiderAnim.SetBool("IsRunning", true);
                break;
            case PawnController.CHARACTER.WORM:

                break;
            case PawnController.CHARACTER.LAST_NO_USE:
                break;
        }
    }

    public void Idle()
    {
        switch (pawnController.character)
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
            case PawnController.CHARACTER.SPIDER:
                spiderAnim.SetBool("IsRunning", false);
                break;
            case PawnController.CHARACTER.WORM:

                break;
            case PawnController.CHARACTER.LAST_NO_USE:
                break;
        }
    }

    public void SpawnDamageText()
    {
        Vector3 targetPosition = GetComponent<PawnController>().GetCurrentTarget().transform.position;
        DamagePopUp damageText = combatManager.GetComponent<DamagePopUp>().Create(new Vector3(targetPosition.x, targetPosition.y + 10, 0), GetComponent<PawnController>().GetDamage());
        damageText.gameObject.GetComponent<TextMeshPro>().color = new Color(255, 50, 50);
    }

    public void AttackIsDone() { attackDone = true; }

}
