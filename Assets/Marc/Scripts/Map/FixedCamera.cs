using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedCamera : MonoBehaviour
{
    enum Direction { UP, DOWN, LEFT, RIGHT, NONE }
    Direction mDirection;
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
        mDirection = Direction.NONE;
        cameraSpeed = 325f;
        fixCameraOnPlayer = true;
        target = GameObject.FindGameObjectWithTag("PlayerMap");
        transform.position = target.transform.position + offSet;
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
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
            if (Input.GetKey(KeyCode.W))
            {
                transform.position += new Vector3(0, cameraSpeed * delta, 0);
                if (transform.position.y > topBorder) {
                    transform.position = new Vector3(transform.position.x, topBorder, offSet.z);
                }
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.position += new Vector3(0, -cameraSpeed * delta, 0);
                if (transform.position.y < bottomBorder)
                {
                    transform.position = new Vector3(transform.position.x, bottomBorder, offSet.z);
                }
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.position += new Vector3(-cameraSpeed * delta, 0, 0);
                if (transform.position.x < leftBorder)
                {
                    transform.position = new Vector3(leftBorder, transform.position.y, offSet.z);
                }
            }
            if (Input.GetKey(KeyCode.D))
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
