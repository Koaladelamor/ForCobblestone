using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsScreen : MonoBehaviour
{

    public Text strengthValue;
    public Text staminaValue;
    public Text agilityValue;
    public Text intelligenceValue;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("DisplayGrodnarStats", 0.2f);
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
        DisplayStats(Game_Manager.Instance.GetGrodnarStats());
    }
    public void DisplayLanstarStats()
    {
        DisplayStats(Game_Manager.Instance.GetLanstarStats());
    }
    public void DisplaySigfridStats()
    {
        DisplayStats(Game_Manager.Instance.GetSigfridStats());
    }

    public void DisplayStats(List<Stat> characterStat) {
        for (int i = 0; i < characterStat.Count; i++)
        {
            switch (characterStat[i].attribute)
            {
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
        _text.text = _stat.value.ToString();
    }
}
