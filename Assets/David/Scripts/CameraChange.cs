using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChange : MonoBehaviour
{
    public bool change = false;
    public Vector3 pos1;
    public Vector3 pos2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (change == false)
        {
            this.gameObject.transform.position = pos1;
        }
        else
        {
            this.gameObject.transform.position = pos2;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            change = !change;
        }
    }
}
