using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPatrol : MonoBehaviour
{
    public float speed;
    public Transform[] patrolPoints;
    private int currentPatrolPoint;

    private bool enemyDetected;

    private bool patrolling;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        currentPatrolPoint = 0;
        patrolling = true;
        player = GameObject.FindGameObjectWithTag("Player");
        enemyDetected = false;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (enemyDetected = false)
        {*/
            patrol();
        /*}
        else
        {*/
            moveTowardsPlayer();
        //}
    }

    void moveTowardsPlayer()
    {
        if (GetComponentInChildren<EnemyVision>().playerDetected == true)
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

            if (transform.position == patrolPoints[1].position)
            {
                currentPatrolPoint = 0;
            }
        }
    }
    /*
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            enemyDetected = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            enemyDetected = false;
        }
    }
    */
}
