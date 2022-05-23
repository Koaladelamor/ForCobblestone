using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatController : MonoBehaviour
{
    public void IncreaseStat()
    {
        StatsScreen statsScreen = InventoryManager.Instance.m_statsScreen;
        UserInterface equipment = InventoryManager.Instance.GetCurrentEquipmentInterface();
        Attributes attr = Attributes.LAST_NO_USE;
        if (transform.parent.name == "Stamina") { attr = Attributes.STAMINA; }
        else if (transform.parent.name == "Strength") { attr = Attributes.STRENGHT; }
        else if (transform.parent.name == "Agility") { attr = Attributes.AGILITY; }
        else if (transform.parent.name == "Intelligence") { attr = Attributes.INTELLIGENCE; }

        if (attr == Attributes.LAST_NO_USE) {
            Debug.Log("ERROR attribute not set.");
            return;
        }

        int statMod = 0;
        if (GetComponentInChildren<Text>().text == "+") { statMod = 2; }
        else if (GetComponentInChildren<Text>().text == "-") { statMod = -2; }

        if (equipment.mInventory.type == InventoryType.GRODNAR)
        {
            GameStats.Instance.IncreaseStat("Grodnar", attr, statMod);
            statsScreen.DisplayStats(GameStats.Instance.GetGrodnarStats());
            if (GameStats.Instance.GetGrodnar()._attribute_points < 1) { statsScreen.DisableStatButtons(); }
        }
        else if (equipment.mInventory.type == InventoryType.LANSTAR)
        {
            GameStats.Instance.IncreaseStat("Lanstar", attr, statMod);
            statsScreen.DisplayStats(GameStats.Instance.GetLanstarStats());
            if (GameStats.Instance.GetLanstar()._attribute_points < 1) { statsScreen.DisableStatButtons(); }
        }
        else if (equipment.mInventory.type == InventoryType.SIGFRID)
        {
            GameStats.Instance.IncreaseStat("Sigfrid", attr, statMod);
            statsScreen.DisplayStats(GameStats.Instance.GetSigfridStats());
            if (GameStats.Instance.GetSigfrid()._attribute_points < 1) { statsScreen.DisableStatButtons(); }
        }

        
    }
}
