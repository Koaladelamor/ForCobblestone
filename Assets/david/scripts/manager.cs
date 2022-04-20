
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class manager : MonoBehaviour
{
    public enum BattleStates { SELECT_ACTIONS, ENEMY_ATTACK}
    public enum Actions { NONE , ATTACK, DEFEND }
    public enum PanelActions { MUESTRATE , ESCONDETE}

    public BattleStates State;
    public Actions Action;
    public int Target;


    public PanelActions panel_action;
    public PanelActions panel_target;

    public GameObject Action_Panel;
    public Animator Action_Panel_Anim;

    public Text Action_Text;    

    public Image LifeBar1;
    public Image LifeBar2;
    public Image LifeBar3;
    public Image LifeBar4;
    public Image LifeBar5;
    public Image LifeBar6;


    [System.Serializable]
    public class properties
    {
    
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
        BattleUpdate();


   
    }

    private void BattleUpdate() { 

        switch (State) {

          case BattleStates.SELECT_ACTIONS:

                panel_action = PanelActions.MUESTRATE; 

                for (int i = 0; i < Team.Count; i++) { 

                    if (Team[i].HasAttack == false){
                        Action_Text.text = "Que deberia hacer " + Team[i].name + " ?"; 
                        if (Action == Actions.ATTACK){

                            panel_action = PanelActions.ESCONDETE;

                            panel_target = PanelActions.MUESTRATE;

                            if (Target == 1){
                                Attack(i);
                            }

                            if (Target == 2){
                                Attack(i);
                            }

                            if (Target == 3){
                                Attack(i);
                              
                            }
                        }
                        else
                        {
                            panel_target = PanelActions.ESCONDETE;
                        }
                        break;
                    }
                    if( i == 2 && Team[i].HasAttack == true) 
                    {
                        for (int j = 0; j < Team.Count; j++)
                        {
                            Team[j].HasAttack = false;
                        }
                        
                        ChangeState(BattleStates.ENEMY_ATTACK);
                    }
                }

         break;





          case BattleStates.ENEMY_ATTACK:

                for (int i = 0; i < Enemies.Count; i++)
                {
                  Team[Random.Range(0, 2)].Life -= Enemies[i].Damage;
                }
                UpdateUI();
                ChangeState(BattleStates.SELECT_ACTIONS);


        break;
        }
    }

    private void UpdateUI()
    {
        LifeBar1.fillAmount = (float)Team[0].Life / (float)Team[0].TotalLife;
        LifeBar2.fillAmount = (float)Team[1].Life / (float)Team[1].TotalLife;
        LifeBar3.fillAmount = (float)Team[2].Life / (float)Team[2].TotalLife;
        LifeBar4.fillAmount = (float)Enemies[0].Life / (float)Enemies[0].TotalLife;
        LifeBar5.fillAmount = (float)Enemies[1].Life / (float)Enemies[1].TotalLife;
        LifeBar6.fillAmount = (float)Enemies[2].Life / (float)Enemies[2].TotalLife;
    }

    private void Attack(int i)
    {
        Enemies[Target - 1].Life = Enemies[Target - 1].Life - Team[i].Damage;
        Target = 0;
        Action = Actions.NONE;
        Team[i].HasAttack = true;
        UpdateUI();
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

    private void ChangeState(BattleStates NewState)
    {
        State = NewState;
        BattleUpdate();
    }
}
