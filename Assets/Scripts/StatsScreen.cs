using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsScreen : MonoBehaviour
{
    public Text healthValue;
    public Text damageValue;
    public Text strengthValue;
    public Text staminaValue;
    public Text agilityValue;
    public Text intelligenceValue;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("DisplayGrodnarStats", 1f);
    }

    // Update is called once per frame
    /*void Update()
    {
        
    }*/

    public void HideDisplay() {
        GetComponent<Image>().enabled = false;
        Text[] texts = GetComponentsInChildren<Text>();
        foreach (Text text in texts)
        {
            text.enabled = false;
        }
    }
    public void ShowDisplay()
    {
        GetComponent<Image>().enabled = true;
        Text[] texts = GetComponentsInChildren<Text>();
        foreach (Text text in texts)
        {
            text.enabled = true;
        }
    }

    public void DisplayGrodnarStats() {
        DisplayStats(GameStats.Instance.GetGrodnarStats());
    }
    public void DisplayLanstarStats()
    {
        DisplayStats(GameStats.Instance.GetLanstarStats());
    }
    public void DisplaySigfridStats()
    {
        DisplayStats(GameStats.Instance.GetSigfridStats());
    }

    public void DisplayStats(List<Stat> characterStat) {
        if (characterStat == null) {
            Debug.Log("ERROR Cannot display stats.");
            return;
        }
        for (int i = 0; i < characterStat.Count; i++)
        {
            switch (characterStat[i].attribute)
            {
                case Attributes.HEALTH:
                    UpdateText(healthValue, characterStat[i]);
                    break;
                case Attributes.BASE_DAMAGE:
                    UpdateText(damageValue, characterStat[i]);
                    break;
                case Attributes.STRENGHT:
                    UpdateText(strengthValue, characterStat[i]);
                    break;
                case Attributes.STAMINA:
                    UpdateText(staminaValue, characterStat[i]);
                    break;
                case Attributes.AGILITY:
                    UpdateText(agilityValue, characterStat[i]);
                    break;
                case Attributes.INTELLIGENCE:
                    UpdateText(intelligenceValue, characterStat[i]);
                    break;
                case Attributes.LAST_NO_USE:
                    break;
                default:
                    break;
            }
        }
    }

    public void UpdateText(Text _text, Stat _stat) {
        if (_text == null) {
            Debug.Log(string.Concat("ERROR Failed to update ", _stat.attribute));
            return; 
        }
        _text.text = _stat.value.ToString();
    }
}
