using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    }

}
