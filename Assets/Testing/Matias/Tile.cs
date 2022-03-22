using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    bool m_isEmpty;

    private void Start()
    {
        m_isEmpty = true;
    }

    public bool AddPawn(GameObject p_pawn)
    {
        if (p_pawn.CompareTag("Pawn") && m_isEmpty)
        {
            p_pawn.GetComponent<Pawn>().SetPosition(new Vector3(transform.position.x, transform.position.y, transform.position.z -1));
            m_isEmpty = false;
            return true;
        }
        Debug.Log("ERROR");
        return false;
    }

    public void TakePawn()
    {
        m_isEmpty = true;
    }

    public bool IsEmpty
    {
        get { return m_isEmpty; }
    }

}
