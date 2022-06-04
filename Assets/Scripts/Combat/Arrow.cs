using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    private bool shoot;
    private float speed;

    private GFXController gfxArcher;

    // Start is called before the first frame update
    void Start()
    {
        shoot = false;
        speed = 250f;

        CombatManager combatManager = GameObject.FindGameObjectWithTag("CombatManager").GetComponent<CombatManager>();
        GameObject[] players = combatManager.GetPlayers();
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<PawnController>().character == PawnController.CHARACTER.LANSTAR) { 
                gfxArcher = players[i].GetComponent<GFXController>(); 
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (shoot) {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }
    }

    public void SetShoot(bool _shoot) { shoot = _shoot; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) {
            gfxArcher.AttackIsDone();
            Destroy(this.gameObject);
        }
    }
}
