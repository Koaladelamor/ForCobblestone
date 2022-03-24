using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStats : MonoBehaviour
{
    [SerializeField]
    public struct Stadistics {
        public string _name;
        public List<Stat> _stats;
        public float _required_xp;
        public float _current_xp;
        public float _dodge;
        public float _speed;//increases speed on map
    }
    public Stadistics Grodnar = new Stadistics();
    public Stadistics Lanstar = new Stadistics();
    public Stadistics Sigfrid = new Stadistics();

    static GameStats mInstance;

    static public GameStats Instance
    {
        get { return mInstance; }
        private set { }
    }

    private void Start()
    {
        //Singleton
        if (mInstance == null)
        {
            mInstance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

        SetStats(Grodnar, "Grodnar", 150, 5, 10, 18, 2, 1);
        SetStats(Lanstar, "Lanstar", 100, 20, 16, 10, 10, 5);
        SetStats(Sigfrid, "Sigfrid", 80, 15, 12, 5, 20, 15);
    }
    public Stadistics GetGrodnar() { return Grodnar; }
    public Stadistics GetLanstar() { return Lanstar; }
    public Stadistics GetSigfrid() { return Sigfrid; }

    public List<Stat> GetGrodnarStats() { return Grodnar._stats; }
    public List<Stat> GetLanstarStats() { return Lanstar._stats; }
    public List<Stat> GetSigfridStats() { return Sigfrid._stats; }

    public void SetStats(Stadistics player, string name, int health, int damage, int strenght, int stamina, int agility, int intelligence) 
    {
        player._name = name;
        player._stats = SetPlayerStats(health, damage, strenght, stamina, agility, intelligence);
        player._required_xp = 500;
        player._current_xp = 0;
        player._dodge = 1;
        player._speed = 1;
        
    }
    public List<Stat> SetPlayerStats(int health, int damage, int strenght, int stamina, int agility, int intelligence)
    {
        List<Stat> statsList = new List<Stat>
        {
            new Stat(Attributes.HEALTH, health),
            new Stat(Attributes.BASE_DAMAGE, damage),
            new Stat(Attributes.STRENGHT, strenght),
            new Stat(Attributes.STAMINA, stamina),
            new Stat(Attributes.AGILITY, agility),
            new Stat(Attributes.INTELLIGENCE, intelligence)
        };
        return statsList;
    }
}
