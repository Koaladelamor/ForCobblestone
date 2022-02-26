using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{


    public enum States { NONE, ATTACK, DEFENSE, STUN }
    [System.Serializable]
    public class SkillProperties
    {

        public int IdSkill;
        public string Name;
        public string Description;

        public int Power;
        public int mana;

        public States State;
        public int StateValue;


    }
    static public List<SkillProperties> Skill = new List<SkillProperties>()
    {
   
        new SkillProperties(){
            IdSkill = 0, Name = "Golpaso", Description = "Un tremendo golpe", Power = 20, mana = 70, State = States.STUN, StateValue = 1
        },
     
        new SkillProperties(){
            IdSkill = 1, Name = "Mordisco", Description = "Un bocao", Power = 50, mana = 80, State = States.NONE, StateValue = 0
        },



    };


    static public SkillProperties GetSkillByID(int id)
    {
        for (int i = 0; i < Skill.Count; i++)
        {
            if (Skill[i].IdSkill == id) return CloneSkill(Skill[i]);
        }
        return null;
    }

    static public SkillProperties CloneSkill(SkillProperties skill)
    {
        SkillProperties NewSkill = new SkillProperties()
        {
            IdSkill = skill.IdSkill,
            Description = skill.Description,
            Name = skill.Name,
            mana = skill.mana,
            Power = skill.Power,
            State = skill.State,
            StateValue = skill.StateValue,
        };
        return NewSkill;
    }
}