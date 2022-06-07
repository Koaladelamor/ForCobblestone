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

    public void SpawnArrow() {
        gfxController.GetComponent<PawnController>().SpawnArrow();
    }

    public void ShootArrow()
    {
        gfxController.GetComponent<PawnController>().ShootArrow();
    }

    public void ShootArrowSound()
    {
        AudioManager.Instance.PlayInstant(AudioManager.InstantAudios.ARROWSHOOT);
    }

    public void HurtStartSound() {
        PawnController pawnController = gfxController.GetComponent<PawnController>();

        switch (pawnController.GetCharacterType())
        {
            case PawnController.CHARACTER.GRODNAR:
                break;
            case PawnController.CHARACTER.LANSTAR:
                break;
            case PawnController.CHARACTER.SIGFRID:
                break;
            case PawnController.CHARACTER.SPIDER:
                break;
            case PawnController.CHARACTER.WORM:
                break;
            case PawnController.CHARACTER.LAST_NO_USE:
                break;
            default:
                break;
        }
    }
}
