using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPosition : MonoBehaviour
{
    private GameObject player;
    private Vector3 target;
    private bool playerCanMove;
    // Start is called before the first frame update
    void Start()
    {
        playerCanMove = true;
        player = GameObject.FindGameObjectWithTag("PlayerMap");
        transform.position = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCanMove)
        {
            if (InputManager.Instance.RightClickButtonPressed || InputManager.Instance.RightClickButtonHold)
            {
                target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                target.z = transform.position.z;

                transform.position = target;
            }
        }
    }

    public void SetMovement(bool movement) { playerCanMove = movement; }
}
