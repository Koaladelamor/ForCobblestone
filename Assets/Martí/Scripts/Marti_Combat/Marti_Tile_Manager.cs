using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marti_Tile_Manager : MonoBehaviour
{
    bool isEmpty;

    private void Start()
    {
        isEmpty = true;
    }

    public Vector3 getTileTransform()
    {
        Vector3 position = this.transform.position;
        return position;
    }

    public bool AddPawn(GameObject p_pawn)
    {
        if ((p_pawn.CompareTag("Player") || p_pawn.CompareTag("Enemy")) && isEmpty)
        {
            p_pawn.GetComponent<Marti_Pawn_Controller>().SetPosition(new Vector3(transform.position.x, transform.position.y, transform.position.z - 1));
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
}
