using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marti_Fixed_Camera : MonoBehaviour
{
    private GameObject target;
    Vector3 offSet = new Vector3(0, 0, -10);

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("PlayerMap");
        transform.position = target.transform.position + offSet;

    }

    void FixedUpdate()
    {
        if (target)
        {
            transform.position = new Vector3(
                target.transform.position.x + offSet.x,
                target.transform.position.y + offSet.y,
                target.transform.position.z + offSet.z
            );
        }
    }
}
