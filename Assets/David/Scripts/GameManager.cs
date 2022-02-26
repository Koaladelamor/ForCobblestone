using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<WarriorsManager.WarriorProperties> Team;
    public List<WarriorsManager.WarriorProperties> BattleWild;
    public List<WarriorsManager.WarriorProperties> WildWarrior  = new List<WarriorsManager.WarriorProperties>();
    void Start()
    {

        Team.Add(WarriorsManager.GetWarriorById(0,  1));        
        Team.Add(WarriorsManager.GetWarriorById(1,  1));        
        Team.Add(WarriorsManager.GetWarriorById(2,  1));        
    }

    public void SetTeam(List<WarriorsManager.WarriorProperties> BattleTeam)
    {
        Team = BattleTeam;
    }
    public void AddWildWarrior(WarriorsManager.WarriorProperties warrior)
    {
        WildWarrior = new List<WarriorsManager.WarriorProperties>();
        WildWarrior.Add(warrior);
        GetComponent<BattleControl>().StartBattle(Team, WildWarrior);   
    }

    public void AddWarrior(WarriorsManager.WarriorProperties _warrior)
    {
        Team.Add(_warrior);
    }
}
