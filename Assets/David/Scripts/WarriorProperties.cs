using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class WarriorProperties
{
    public int WarriorID;
    public string Name;
    public int Level;



    public int BaseLife;
    public int Life;

    public int TotalLife;

    public int Exp;

    public int NextLevelExp;
    public int PrevLevelExp;



    public int BaseAtk;
    public int BaseDef;
    public int BaseSpeed;

    public int Atk;
    public int Def;
    public int Speed;

    [System.Serializable]
    public class MySkillProperties
    {
        public SkillManager.SkillProperties Skill;
        public bool Active;
        public bool CanLearn;
        public bool Canceled;
    }
    public List<MySkillProperties> MySkills;

    public class SkillbyLevelProperties
    {
        public SkillManager.SkillProperties Skill;
        public int Level;
    }
    public List<SkillbyLevelProperties> SkillbyLevel;

    public WarriorProperties CloneWarrior()
    {
        WarriorProperties NewWarrior = new WarriorProperties()
        {
            Name = this.Name,
            BaseLife = this.BaseLife,
            BaseAtk = this.BaseAtk,
            BaseDef = this.BaseDef,
            BaseSpeed = this.BaseSpeed,
            Level = this.Level,
            Life = this.Life,
            TotalLife = this.TotalLife,
            Atk = this.Atk,
            Def = this.Def,
            Speed = this.Speed,
            Exp = this.Exp,
            NextLevelExp = this.NextLevelExp,
            PrevLevelExp = this.PrevLevelExp
        };

        return NewWarrior;
    }
}