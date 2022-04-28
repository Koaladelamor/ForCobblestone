using UnityEngine;
using UnityEngine.UI;
using System;

public class HealthBar : MonoBehaviour
{
    private PawnController pawn;
    private Slider slider;


    void Start()
    {
        pawn = GetComponentInParent<RectTransform>().GetComponentInParent<PawnController>();
        slider = GetComponent<Slider>();
        slider.maxValue = (float)pawn.GetMaxHP();
        slider.value = (float)pawn.GetCurrentHP();
        OnHealthChange += UpdateHealthBar;
    }

    public event Action OnHealthChange;
    public void HealthChangeEvent()
    {
        if (OnHealthChange != null)
        {
            OnHealthChange();
        }
    }

    public void UpdateHealthBar() {
        slider.value = (float)pawn.GetCurrentHP();
    }
}
