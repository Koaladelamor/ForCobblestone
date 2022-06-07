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
    }
    public Stadistics Grodnar;
    public Stadistics Lanstar;
    public Stadistics Sigfrid;

    public Stadistics Spider;
    public Stadistics Worm;

    public Stadistics Boss;

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

        Grodnar = InitStats("Grodnar", 160, 5, 15, 20, 20, 2);
        Lanstar = InitStats("Lanstar", 75, 20, 35, 4, 6, 10);
        Sigfrid = InitStats("Sigfrid", 124, 15, 30, 16, 14, 20);

        Spider = InitStats("Spider", 75, 5, 15, 14, 10, 5);
        Worm = InitStats("Worm", 50, 5, 15, 8, 10, 5);

        Boss = InitStats("Boss", 300, 20, 45, 38, 50, 20);

        coins = 0;
    }
    
    public Stadistics GetGrodnar() { return Grodnar; }
    public Stadistics GetLanstar() { return Lanstar; }
    public Stadistics GetSigfrid() { return Sigfrid; }

    public List<Stat> GetGrodnarStats() { return Grodnar._stats; }
    public List<Stat> GetLanstarStats() { return Lanstar._stats; }
    public List<Stat> GetSigfridStats() { return Sigfrid._stats; }

    public List<Stat> GetSpiderStats() { return Spider._stats; }
    public List<Stat> GetWormStats() { return Worm._stats; }

    public List<Stat> GetBossStats() { return Boss._stats; }

    public Stadistics InitStats(string name, int health, int min_damage, int max_damage, int strenght, int stamina, int agility)
    {
        Stadistics player = new Stadistics();
        player._name = name;
        player._stats = SetPlayerStats(health, min_damage, max_damage, strenght, stamina, agility);
        player._level = 1;
        player._attribute_points = 0;
        player._required_xp = 500;
        player._current_xp = 0;

        return player;
    }

    public List<Stat> SetPlayerStats(int health, int min_damage, int max_damage, int strenght, int stamina, int agility)
    {
        List<Stat> statsList = new List<Stat>
        {

            new Stat(Attributes.CURR_HEALTH, health),
            new Stat(Attributes.MAX_HEALTH, health),
            new Stat(Attributes.MIN_DAMAGE, min_damage),
            new Stat(Attributes.MAX_DAMAGE, max_damage),
            new Stat(Attributes.STRENGHT, strenght),
            new Stat(Attributes.STAMINA, stamina),
            new Stat(Attributes.AGILITY, agility)

        };
        CalculateHealth(statsList, GetStamina(statsList));
        CalculateDamage(statsList, GetStrength(statsList));
        return statsList;
    }

    public void CalculateHealth(List<Stat> _stats, int _stamina) 
    {
        for (int i = 0; i < _stats.Count; i++)
        {
            if (_stats[i].attribute == Attributes.MAX_HEALTH) {
                _stats[i].mods = Mathf.RoundToInt(_stamina / 2);
                _stats[i].value = _stats[i].baseValue + _stats[i].mods;
            }
            else if (_stats[i].attribute == Attributes.CURR_HEALTH)
            {
                _stats[i].mods = Mathf.RoundToInt(_stamina / 2);
                _stats[i].value = _stats[i].baseValue + _stats[i].mods;
            }
        }
    }

    public void CalculateDamage(List<Stat> _stats, int _strength) {
        for (int i = 0; i < _stats.Count; i++)
        {
            if (_stats[i].attribute == Attributes.MIN_DAMAGE)
            {
                _stats[i].mods = Mathf.RoundToInt(_strength / 2);
                _stats[i].value = _stats[i].baseValue + _stats[i].mods;
            }
            else if (_stats[i].attribute == Attributes.MAX_DAMAGE)
            {
                _stats[i].mods = Mathf.RoundToInt(_strength / 2);
                _stats[i].value = _stats[i].baseValue + _stats[i].mods;
            }
        }
    }

    public int GetStamina(List<Stat> _stats) {
        for (int i = 0; i < _stats.Count; i++)
        {
            if (_stats[i].attribute == Attributes.STAMINA)
            {
                return _stats[i].value;
            }
        }
        return 0;
    }

    public int GetStrength(List<Stat> _stats)
    {
        for (int i = 0; i < _stats.Count; i++)
        {
            if (_stats[i].attribute == Attributes.STRENGHT)
            {
                return _stats[i].value;
            }
        }
        return 0;
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
                        Grodnar._stats[i].mods += addValue;
                        Grodnar._stats[i].value = Grodnar._stats[i].baseValue + Grodnar._stats[i].mods;
                        Grodnar._attribute_points += -1;
                        //Debug.Log(Grodnar._attribute_points);

                        if (Grodnar._stats[i].attribute == Attributes.STAMINA)
                        {
                            CalculateHealth(Grodnar._stats, GetStamina(Grodnar._stats));
                        }
                        else if (Grodnar._stats[i].attribute == Attributes.STRENGHT)
                        {
                            CalculateDamage(Grodnar._stats, GetStrength(Grodnar._stats));
                        }
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
                        Lanstar._stats[i].mods += addValue;
                        Lanstar._stats[i].value = Lanstar._stats[i].baseValue + Lanstar._stats[i].mods;
                        Lanstar._attribute_points += -1;
                        //Debug.Log(Lanstar._attribute_points);

                        if (Lanstar._stats[i].attribute == Attributes.STAMINA)
                        {
                            CalculateHealth(Lanstar._stats, GetStamina(Lanstar._stats));
                        }
                        else if (Lanstar._stats[i].attribute == Attributes.STRENGHT)
                        {
                            CalculateDamage(Lanstar._stats, GetStrength(Lanstar._stats));
                        }
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
                        Sigfrid._stats[i].mods += addValue;
                        Sigfrid._stats[i].value = Sigfrid._stats[i].baseValue + Sigfrid._stats[i].mods;
                        Sigfrid._attribute_points += -1;
                        //Debug.Log(Sigfrid._attribute_points);

                        if (Sigfrid._stats[i].attribute == Attributes.STAMINA)
                        {
                            CalculateHealth(Sigfrid._stats, GetStamina(Sigfrid._stats));
                        }
                        else if (Sigfrid._stats[i].attribute == Attributes.STRENGHT)
                        {
                            CalculateDamage(Sigfrid._stats, GetStrength(Sigfrid._stats));
                        }
                    }
                }
                break;
        }
    }

    public void AddXpToGrodnar(float xpAdded) {

        Grodnar._current_xp += xpAdded;
        if (LvlUp(Grodnar))
        {
            InventoryManager.Instance.SetLvlUpWarning(true);
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
            InventoryManager.Instance.SetLvlUpWarning(true);
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
            InventoryManager.Instance.SetLvlUpWarning(true);
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

            //Debug.Log(player._level);
            //Debug.Log(player._current_xp);
            //Debug.Log(player._required_xp);
            return true;
        }
        return false;
    }

    public void HealCharacters() {
        for (int i = 0; i < Grodnar._stats.Count; i++)
        {
            if (Grodnar._stats[i].attribute == Attributes.CURR_HEALTH)
            {
                Grodnar._stats[i].value = GetMaxHP(Grodnar._stats);
                break;
            }

        }

        for (int i = 0; i < Lanstar._stats.Count; i++)
        {
            if (Lanstar._stats[i].attribute == Attributes.CURR_HEALTH)
            {
                Lanstar._stats[i].value = GetMaxHP(Lanstar._stats);
                break;
            }
        }

        for (int i = 0; i < Sigfrid._stats.Count; i++)
        {
            if (Sigfrid._stats[i].attribute == Attributes.CURR_HEALTH)
            {
                Sigfrid._stats[i].value = GetMaxHP(Sigfrid._stats);
                break;
            }
        }
    }

    public void PotionHealGrodnar() {
        for (int i = 0; i < Grodnar._stats.Count; i++)
        {
            if (Grodnar._stats[i].attribute == Attributes.CURR_HEALTH)
            {
                if (Grodnar._stats[i].value < 1)
                {
                    InventoryManager.Instance.InteractableGrodnarButton(true);
                }
                Grodnar._stats[i].value += 100;
                if (Grodnar._stats[i].value > GetMaxHP(Grodnar._stats)) {
                    Grodnar._stats[i].value = GetMaxHP(Grodnar._stats);
                }
                InventoryManager.Instance.GrodnarEquipmentDisplay();
                InventoryManager.Instance.m_statsScreen.DisplayGrodnarStats();
                break;
            }
        }
    }

    public void PotionHealLanstar()
    {
        for (int i = 0; i < Lanstar._stats.Count; i++)
        {
            if (Lanstar._stats[i].attribute == Attributes.CURR_HEALTH)
            {
                if (Lanstar._stats[i].value < 1) {
                    InventoryManager.Instance.InteractableLanstarButton(true);
                }
                Lanstar._stats[i].value += 100;
                if (Lanstar._stats[i].value > GetMaxHP(Lanstar._stats))
                {
                    Lanstar._stats[i].value = GetMaxHP(Lanstar._stats);
                }
                InventoryManager.Instance.LanstarEquipmentDisplay();
                InventoryManager.Instance.m_statsScreen.DisplayLanstarStats();
                break;
            }
        }
    }

    public void PotionHealSigfrid()
    {
        for (int i = 0; i < Sigfrid._stats.Count; i++)
        {
            if (Sigfrid._stats[i].attribute == Attributes.CURR_HEALTH)
            {
                if (Sigfrid._stats[i].value < 1)
                {
                    InventoryManager.Instance.InteractableSigfridButton(true);
                }
                Sigfrid._stats[i].value += 100;
                if (Sigfrid._stats[i].value > GetMaxHP(Sigfrid._stats))
                {
                    Sigfrid._stats[i].value = GetMaxHP(Sigfrid._stats);
                }
                InventoryManager.Instance.SigfridEquipmentDisplay();
                InventoryManager.Instance.m_statsScreen.DisplaySigfridStats();
                break;
            }
        }
    }

    public void SetCurrentHP(List<Stat> stats, int newValue)
    {
        for (int i = 0; i < stats.Count; i++)
        {
            if (stats[i].attribute == Attributes.CURR_HEALTH)
            {
                stats[i].value = newValue;
            }
        }
    }

    public int GetMaxHP(List<Stat> stats)
    {
        for (int i = 0; i < stats.Count; i++)
        {
            if (stats[i].attribute == Attributes.MAX_HEALTH)
            {
                return stats[i].value;
            }
        }
        return -1;
    }

    public int GetCoins() { return coins; }

    public void SetCoins(int newAmount) { coins = newAmount; }

    public void AddCoins(int coinsToAdd) { coins += coinsToAdd; }

    public void SubtractCoins(int coinsToSubtract) { coins -= coinsToSubtract; }


    public void DestroyInstance() { Destroy(this.gameObject); }

}
