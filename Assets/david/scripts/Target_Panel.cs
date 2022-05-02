using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target_Panel : MonoBehaviour
{
    private Animator anim;
    private manager man;
    void Start()
    {
        anim = GetComponent<Animator>();
        man = FindObjectOfType<manager>();
    }


    void Update()
    {
        if (man.panel_target == manager.PanelActions.MUESTRATE)
        {
            anim.SetBool("ON", true);
        }
        else
        {
            anim.SetBool("ON", false);
        }
    }
}
