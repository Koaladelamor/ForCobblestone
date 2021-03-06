using UnityEngine;

public class TileManager : MonoBehaviour
{
    [SerializeField] GameObject _highlight;
    public bool playerDraggableOnTile;
    private bool isEmpty;
    private PawnController pawnOnTile;

    private void Start()
    {
        pawnOnTile = null;
        isEmpty = true;
    }


    public Vector2 GetTilePosition()
    {
        Vector2 tilePos = new Vector2();
        tilePos.x = this.transform.position.x;
        tilePos.y = this.transform.position.y;
        return tilePos;
    }

    public bool AddPawn(GameObject p_pawn)
    {
        if ((p_pawn.CompareTag("Player") || p_pawn.CompareTag("Enemy")) && isEmpty)
        {
            p_pawn.GetComponent<PawnController>().SetPosition(new Vector3(transform.position.x, transform.position.y, transform.position.z - 1));
            p_pawn.GetComponent<PawnController>().SetCurrentTile(this);
            pawnOnTile = p_pawn.GetComponent<PawnController>();
            isEmpty = false;
            return true;
        }
        Debug.Log("ERROR ADDING PAWN TO TILE");
        return false;
    }

    public bool AddBoss(GameObject p_pawn)
    {
        if (p_pawn.CompareTag("Enemy") && isEmpty)
        {
            p_pawn.GetComponent<PawnController>().SetPosition(new Vector3(transform.position.x + 50, transform.position.y, transform.position.z - 1));
            p_pawn.GetComponent<PawnController>().SetCurrentTile(this);
            pawnOnTile = p_pawn.GetComponent<PawnController>();
            isEmpty = false;
            return true;
        }
        Debug.Log("ERROR ADDING PAWN TO TILE");
        return false;
    }

    public void TakePawn()
    {
        pawnOnTile = null;
        isEmpty = true;
    }

    public PawnController GetPawn() {
        if (pawnOnTile != null) {
            return pawnOnTile;
        }
        Debug.Log("PAWN ON TILE NULL");
        return null;
    }

    public bool IsTileEmpty
    {
        get { return isEmpty; }
    }

    public void EnableHighlight() {
        _highlight.SetActive(true);
    }

    public void DisableHighlight()
    {
        _highlight.SetActive(false);
    }


}
