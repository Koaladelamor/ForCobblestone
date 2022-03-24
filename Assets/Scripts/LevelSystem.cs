using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    public float current_xp;
    public float required_xp;

    public int attribute_points;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LevelUp() { 
        
    }

    public void AddXp(float _xp) {
        current_xp += _xp;
    }
}
