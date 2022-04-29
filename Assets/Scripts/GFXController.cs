using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GFXController : MonoBehaviour
{
    public GameObject warrior, range, tank;

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
                range.SetActive(true); 
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

    }

    public void Hurt()
    {

    }

    public void Die()
    {

    }

    public void Move()
    {

    }
}
