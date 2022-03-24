using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_manager : MonoBehaviour
{
    public List<WarriorProperties> Team;
    public List<WarriorProperties> BattleWild;
    public List<WarriorProperties> WildWarrior  = new List<WarriorProperties>();
    void Start()
    {

        Team.Add(WarriorsManager.GetWarriorById(0,  1));        
        Team.Add(WarriorsManager.GetWarriorById(1,  1));        
        Team.Add(WarriorsManager.GetWarriorById(2,  1));        
    }

    public void SetTeam(List<WarriorProperties> BattleTeam)
    {
        Team = BattleTeam;
    }
    public void AddWildWarrior(WarriorProperties warrior)
    {
        WildWarrior = new List<WarriorProperties>();
        WildWarrior.Add(warrior);
        GetComponent<BattleControl>().StartBattle(Team, WildWarrior);   
    }

    public void AddWarrior(WarriorProperties _warrior)
    {
        Team.Add(_warrior);
    }
}
