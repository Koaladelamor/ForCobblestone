using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAI : MonoBehaviour
{
    public float speed;
    private GameObject enemy;

    private Vector3 offset;

    void Start()
    {
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        offset.x = -40;
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        float delta = Time.fixedDeltaTime;

        transform.position = Vector2.MoveTowards(transform.position, enemy.transform.position + offset, speed * delta);
    }
}
