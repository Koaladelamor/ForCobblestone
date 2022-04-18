
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class manager : MonoBehaviour
{
    public enum BattleStates { SELECT_ACTIONS, SELECT_ENEMY, ENEMY_ATTACK}
    public enum Actions { NONE , ATTACK, DEFEND }
    public BattleStates State;
    public Actions Action;
    public int Target;

    public Image LifeBar1;
    public Image LifeBar2;
    public Image LifeBar3;
    public Image LifeBar4;
    public Image LifeBar5;
    public Image LifeBar6;


    [System.Serializable]
    public class properties
    {
        public GameObject UI;
        public string name;
        public int TotalLife;
        public int Life;
        public int Damage;
        public int exp;
        public int nextlvlexp;
        public bool HasAttack;
    }

    public List<properties> Team;
    public List<properties> Enemies;

    private void Update()
    {
        LifeBar1.fillAmount = (float)Team[0].Life / (float)Team[0].TotalLife;
        LifeBar2.fillAmount = (float)Team[1].Life / (float)Team[1].TotalLife;
        LifeBar3.fillAmount = (float)Team[2].Life / (float)Team[2].TotalLife;
        LifeBar4.fillAmount = (float)Enemies[0].Life / (float)Enemies[0].TotalLife;
        LifeBar4.fillAmount = (float)Enemies[1].Life / (float)Enemies[1].TotalLife;
        LifeBar4.fillAmount = (float)Enemies[2].Life / (float)Enemies[2].TotalLife;

        BattleUpdate();

    }

    private void BattleUpdate() { 

        switch (State) {

          case BattleStates.SELECT_ACTIONS:

                for (int i = 0; i < Team.Count; i++)

                    if (Team[i].HasAttack == false){

                        if (Action == Actions.ATTACK){

                            if (Target == 1){
                                Enemies[0].Life = Enemies[Target - 1].Life - Team[i].Damage;
                                Target = 0;
                                Action = Actions.NONE;
                                Team[i].HasAttack = true;
                            }

                            if (Target == 2){
                                Enemies[Target - 1].Life = Enemies[Target - 1].Life - Team[i].Damage;
                                Target = 0;
                                Action = Actions.NONE;
                                Team[i].HasAttack = true;
                            }

                            if (Target == 3){
                                Enemies[Target - 1].Life = Enemies[Target - 1].Life - Team[i].Damage;
                                Target = 0;
                                Action = Actions.NONE;
                                Team[i].HasAttack = true;
                            }
                        }
                    }

         break;

        }
    }

    public void SelectActionAttack()
    {
        Action = Actions.ATTACK;
    }

    public void SelectTarget1()
    {
        Target = 1;
    }
    public void SelectTarget2()
    {
        Target = 2;
    }
    public void SelectTarget3()
    {
        Target = 3;
    }
}
