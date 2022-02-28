using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [SerializeField] GameObject _highlight;
    public bool playerDraggableOnTile;
    bool isEmpty;

    private void Start()
    {
        //playerDraggableOnTile = false;
        isEmpty = true;
    }

    public Vector3 getTileTransform() {
        Vector3 position = this.transform.position;
        return position;
    }

    public bool AddPawn(GameObject p_pawn)
    {
        if ((p_pawn.CompareTag("Player") || p_pawn.CompareTag("Enemy")) && isEmpty)
        {
            p_pawn.GetComponent<PawnController>().SetPosition(new Vector3(transform.position.x, transform.position.y, transform.position.z - 1));
            isEmpty = false;
            return true;
        }
        Debug.Log("ERROR");
        return false;
    }

    public void TakePawn()
    {
        isEmpty = true;
    }

    public bool IsTileEmpty
    {
        get { return isEmpty; }
    }

    public void enableHighlight() {
        _highlight.SetActive(true);
    }

    public void disableHighlight()
    {
        _highlight.SetActive(false);
    }


}
