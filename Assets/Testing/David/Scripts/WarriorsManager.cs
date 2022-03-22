using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarriorsManager : MonoBehaviour
{
    static public int MaxTeam = 3;
    static public int MaxLevel = 100;

    
    static public List<WarriorProperties> Warrior = new List<WarriorProperties>()
    {

        new WarriorProperties(){
           WarriorID=0, Name = "Grodnar",    BaseLife = 60, BaseAtk = 30, BaseDef = 50, BaseSpeed = 30,
            SkillbyLevel = new List<WarriorProperties.SkillbyLevelProperties>
                {
                 new WarriorProperties.SkillbyLevelProperties()  {Skill =  SkillManager. GetSkillByID(0), Level = 1,},
                },

        },
          new WarriorProperties(){
           WarriorID=1, Name = "Sigfred",    BaseLife = 60, BaseAtk = 30, BaseDef = 50, BaseSpeed = 30,
            SkillbyLevel = new List<WarriorProperties.SkillbyLevelProperties>
                {
                 new WarriorProperties.SkillbyLevelProperties()  {Skill =  SkillManager. GetSkillByID(0), Level = 1,},
                },

        },
            new WarriorProperties(){
           WarriorID=2, Name = "Arquero",    BaseLife = 60, BaseAtk = 30, BaseDef = 50, BaseSpeed = 30,
            SkillbyLevel = new List<WarriorProperties.SkillbyLevelProperties>
                {
                 new WarriorProperties.SkillbyLevelProperties()  {Skill =  SkillManager. GetSkillByID(0), Level = 1,},
                },

        },

         new WarriorProperties(){
            WarriorID=3, Name = "Lobo",    BaseLife = 20, BaseAtk = 50, BaseDef = 20, BaseSpeed = 50,
            SkillbyLevel = new List<WarriorProperties.SkillbyLevelProperties>
                {
                 new WarriorProperties.SkillbyLevelProperties()  {Skill =  SkillManager. GetSkillByID(1), Level = 1,},
                },

        },


    };


    static public WarriorProperties GetWarriorById(int id,  int Level = -1)
    {
        for (int i = 0; i < Warrior.Count; i++)
        {
            if (Warrior[i].WarriorID == id)
            {
                WarriorProperties TempWarrior = Warrior[i].CloneWarrior();


                if (Level != -1)
                {
                    TempWarrior.Level = Level;
                    TempWarrior.TotalLife = GetHPStats(TempWarrior.BaseLife, TempWarrior.Level);
                    TempWarrior.Life = TempWarrior.TotalLife;

                    TempWarrior.Atk = GetOtherStats(TempWarrior.BaseAtk, TempWarrior.Level);
                    TempWarrior.Def = GetOtherStats(TempWarrior.BaseDef, TempWarrior.Level);
                    TempWarrior.Speed = GetOtherStats(TempWarrior.BaseSpeed, TempWarrior.Level);


                    TempWarrior.PrevLevelExp = (int)(TempWarrior.Level * 0.1f);
                    TempWarrior.NextLevelExp = (int)((TempWarrior.Level + 1) * 0.1f);
                    TempWarrior.Exp = TempWarrior.PrevLevelExp;




                }



                return TempWarrior;
            }
        }
        return null;
    }
    static public int GetHPStats(int Base, int Level)
    {
        int result = ((((Base) * 2) * Level) / 100) + Level + 10;
        return result;

    }

    static public int GetOtherStats(int Base, int Level)
    {
        int result = ((((Base) * 2) * Level) / 100) + 5;
        return result;
    }

}