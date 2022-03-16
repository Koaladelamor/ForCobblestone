using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedCamera : MonoBehaviour
{

    private GameObject target;
    Vector3 offSet = new Vector3(0,0,-10);
    bool fixCameraOnPlayer;

    float cameraSpeed;

    float topBorder = 295;
    float bottomBorder = -300;
    float leftBorder = -530;
    float rightBorder = 533;

    void Start()
    {
        cameraSpeed = 325f;
        fixCameraOnPlayer = true;
        target = GameObject.FindGameObjectWithTag("PlayerMap");
        transform.position = target.transform.position + offSet;
        
    }

    private void Update()
    {
        if (InputManager.Instance.CameraButtonPressed)
        {
            fixCameraOnPlayer = !fixCameraOnPlayer;
        }

        float delta = Time.deltaTime;

        if (fixCameraOnPlayer)
        {
            if (target)
            {
                transform.position = new Vector3(
                    target.transform.position.x,
                    target.transform.position.y,
                    target.transform.position.z + offSet.z
                );
            }
        }


        else {
            if (InputManager.Instance.UpButtonHold)
            {
                transform.position += new Vector3(0, cameraSpeed * delta, 0);
                if (transform.position.y > topBorder) {
                    transform.position = new Vector3(transform.position.x, topBorder, offSet.z);
                }
            }
            if (InputManager.Instance.DownButtonHold)
            {
                transform.position += new Vector3(0, -cameraSpeed * delta, 0);
                if (transform.position.y < bottomBorder)
                {
                    transform.position = new Vector3(transform.position.x, bottomBorder, offSet.z);
                }
            }
            if (InputManager.Instance.LeftButtonHold)
            {
                transform.position += new Vector3(-cameraSpeed * delta, 0, 0);
                if (transform.position.x < leftBorder)
                {
                    transform.position = new Vector3(leftBorder, transform.position.y, offSet.z);
                }
            }
            if (InputManager.Instance.RightButtonHold)
            {
                transform.position += new Vector3(cameraSpeed * delta, 0, 0);
                if (transform.position.x > rightBorder)
                {
                    transform.position = new Vector3(rightBorder, transform.position.y, offSet.z);
                }
            }
        }
    }

}
