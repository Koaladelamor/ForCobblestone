using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolAI : MonoBehaviour
{
    public float speed;
    public GameObject[] patrolPoints;
    private int currentPatrolPoint;

    private bool patrolling;

    private GameObject m_player;

    // Start is called before the first frame update
    void Start()
    {

        currentPatrolPoint = 0;
        patrolling = true;
        m_player = GameObject.FindGameObjectWithTag("PlayerMap");
    }

    // Update is called once per frame
    void Update()
    {
        Patrol();
        MoveTowardsPlayer();
    }

    void MoveTowardsPlayer() {
        if (GetComponentInChildren<VisionRange>().playerDetected == true)
        {
            patrolling = false;
            transform.position = Vector3.MoveTowards(transform.position, m_player.transform.position, speed * Time.deltaTime);
        }
        else {
            patrolling = true;
        }
    }

    void Patrol() {
        if(patrolling) { 
            if (transform.position != patrolPoints[currentPatrolPoint].transform.position)
            {
                transform.position = Vector2.MoveTowards(transform.position, patrolPoints[currentPatrolPoint].transform.position, speed * Time.deltaTime);
            }
            else
            {
                currentPatrolPoint++;
            }

            if (transform.position == patrolPoints[3].transform.position)
            {
                currentPatrolPoint = 0;
            }
        }
    }
}
