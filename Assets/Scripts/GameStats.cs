using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStats : MonoBehaviour
{
    [SerializeField]
    public struct Stadistics {
        public string _name;
        public List<Stat> _stats;
        public int _level;
        public int _attribute_points;
        public float _required_xp;
        public float _current_xp;
        public float _dodge;
        public float _speed;//increases speed on map
    }
    public Stadistics Grodnar;
    public Stadistics Lanstar;
    public Stadistics Sigfrid;

    private int coins;

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

        Grodnar = InitStats("Grodnar", 150, 5, 10, 18, 2, 1);
        Lanstar = InitStats("Lanstar", 100, 20, 16, 10, 10, 5);
        Sigfrid = InitStats("Sigfrid", 80, 15, 12, 5, 20, 15);

        coins = 0;
    }

    public Stadistics GetGrodnar() { return Grodnar; }
    public Stadistics GetLanstar() { return Lanstar; }
    public Stadistics GetSigfrid() { return Sigfrid; }

    public List<Stat> GetGrodnarStats() { return Grodnar._stats; }
    public List<Stat> GetLanstarStats() { return Lanstar._stats; }
    public List<Stat> GetSigfridStats() { return Sigfrid._stats; }

    public Stadistics InitStats(string name, int health, int damage, int strenght, int stamina, int agility, int intelligence)
    {
        Stadistics player = new Stadistics();
        player._name = name;
        player._stats = SetPlayerStats(health, damage, strenght, stamina, agility, intelligence);
        player._level = 1;
        player._attribute_points = 0;
        player._required_xp = 500;
        player._current_xp = 0;
        player._dodge = 1;
        player._speed = 1;

        return player;
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

    public void IncreaseStat(string name, Attributes attr, int addValue) {
        switch (name)
        {
            default:
                break;

            case "Grodnar":
                if (Grodnar._attribute_points < 1)
                {
                    Debug.Log("Grodnar 0 attribute points");
                    return;
                }
                for (int i = 0; i < Grodnar._stats.Count; i++)
                {
                    if (Grodnar._stats[i].attribute == attr)
                    {
                        Grodnar._stats[i].value += addValue;
                        Grodnar._attribute_points += -1;
                        Debug.Log(Grodnar._attribute_points);
                    }
                }
                break;

            case "Lanstar":
                if (Lanstar._attribute_points < 1)
                {
                    Debug.Log("Lanstar 0 attribute points");
                    return;
                }
                for (int i = 0; i < Lanstar._stats.Count; i++)
                {
                    if (Lanstar._stats[i].attribute == attr)
                    {
                        Lanstar._stats[i].value += addValue;
                        Lanstar._attribute_points += -1;
                        Debug.Log(Lanstar._attribute_points);
                    }
                }
                break;

            case "Sigfrid":
                if (Sigfrid._attribute_points < 1)
                {
                    Debug.Log("Sigfrid 0 attribute points");
                    return;
                }
                for (int i = 0; i < Sigfrid._stats.Count; i++)
                {
                    if (Sigfrid._stats[i].attribute == attr)
                    {
                        Sigfrid._stats[i].value += addValue;
                        Sigfrid._attribute_points += -1;
                        Debug.Log(Sigfrid._attribute_points);
                    }
                }
                break;
        }
    }

    public void AddXpToGrodnar(float xpAdded) {

        Grodnar._current_xp += xpAdded;
        if (LvlUp(Grodnar))
        {
            Grodnar._level++;
            Grodnar._current_xp -= Grodnar._required_xp;
            Grodnar._required_xp += 700;
            Grodnar._attribute_points += 2;
            Debug.Log(Grodnar._level);
            Debug.Log(Grodnar._current_xp);
            Debug.Log(Grodnar._required_xp);
        }

    }

    public void AddXpToLanstar(float xpAdded)
    {

        Lanstar._current_xp += xpAdded;
        if (LvlUp(Lanstar))
        {
            Lanstar._level++;
            Lanstar._current_xp -= Lanstar._required_xp;
            Lanstar._required_xp += 700;
            Lanstar._attribute_points += 2;
            Debug.Log(Lanstar._level);
            Debug.Log(Lanstar._current_xp);
            Debug.Log(Lanstar._required_xp);
        }

    }

    public void AddXpToSigfrid(float xpAdded)
    {

        Sigfrid._current_xp += xpAdded;
        if (LvlUp(Sigfrid))
        {
            Sigfrid._level++;
            Sigfrid._current_xp -= Sigfrid._required_xp;
            Sigfrid._required_xp += 700;
            Sigfrid._attribute_points += 2;
            Debug.Log(Sigfrid._level);
            Debug.Log(Sigfrid._current_xp);
            Debug.Log(Sigfrid._required_xp);
        }

    }

    public bool LvlUp(Stadistics player) {
        if (player._current_xp >= player._required_xp)
        {

            Debug.Log(player._level);
            Debug.Log(player._current_xp);
            Debug.Log(player._required_xp);
            return true;
        }
        return false;
    }

    public int GetCoins() { return coins; }

    public void SetCoins(int newAmount) { coins = newAmount; }

    public void AddCoins(int coinsToAdd) { coins += coinsToAdd; }

    public void SubtractCoins(int coinsToSubtract) { coins -= coinsToSubtract; }


}
