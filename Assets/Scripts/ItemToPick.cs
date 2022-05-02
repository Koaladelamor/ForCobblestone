using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemToPick : MonoBehaviour
{
    public ItemObject item;
    private Audio_Manager Audio_Manager;

    private void Start()
    {
        Audio_Manager = GameObject.FindObjectOfType<Audio_Manager>();
    }
    void Pick()
    {
        //Cojer la llave
        Audio_Manager.PlayInstant(Audio_Manager.InstantAudios.PLAYERHURT);
    }
}
