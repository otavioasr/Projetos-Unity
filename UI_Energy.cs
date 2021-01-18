using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Energy : MonoBehaviour
{
    [SerializeField]
    public Slider slider;
    public int energy = 10;

    private void Start()
    {        
        slider.maxValue = energy;
        slider.value = energy;

        DecreaseEnergy (5);
    }

    public void DecreaseEnergy (int amount)
    {
        slider.value -= amount;
    }

    public void IncreaseEnergy (int amount)
    {
        if ((slider.value + amount) < slider.maxValue)
            slider.value += amount;
    }

    public bool Empty ()
    {
        return slider.value == 0;
    }
}
