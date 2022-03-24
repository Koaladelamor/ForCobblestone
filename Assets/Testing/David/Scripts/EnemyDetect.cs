    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetect : MonoBehaviour
{
    public int enemyid;
    public int enemylevel;

    void FixedUpdate()
    {
       
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            print("entra");
            Destroy(this.gameObject);
            other.GetComponent<Game_manager>().AddWildWarrior(WarriorsManager.GetWarriorById(enemyid,  enemylevel));
            
        }
    }
}
