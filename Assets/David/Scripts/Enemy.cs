using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public Transform[] patrolPoints;
    private int currentPatrolPoint;
    public int enemyid;
    public int enemylevel;
    private bool patrolling;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        currentPatrolPoint = 0;
        patrolling = true;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        patrol();
        moveTowardsPlayer();
    }

    void moveTowardsPlayer()
    {
        if (GetComponentInChildren<VisionRange>().playerDetected == true)
        {
            patrolling = false;
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
        else
        {
            patrolling = true;
        }
    }

    void patrol()
    {
        if (patrolling)
        {
            if (transform.position != patrolPoints[currentPatrolPoint].position)
            {
                transform.position = Vector2.MoveTowards(transform.position, patrolPoints[currentPatrolPoint].position, speed * Time.deltaTime);
            }
            else
            {
                currentPatrolPoint++;
            }

            if (transform.position == patrolPoints[3].position)
            {
                currentPatrolPoint = 0;
            }
        }
    }
 
}
