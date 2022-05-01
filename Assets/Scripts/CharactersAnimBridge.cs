using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersAnimBridge : MonoBehaviour
{

    public GFXController gfxController;
    public void AttackIsDone() {
        gfxController.AttackIsDone();
    }

    public void HurtIsDone()
    {
        gfxController.GetComponent<PawnController>().HurtAnimDone();
    }
}
