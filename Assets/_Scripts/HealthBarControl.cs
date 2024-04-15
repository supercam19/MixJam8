using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarControl : MonoBehaviour
{
    Slider slider;

    public void Start()
    {
        slider = GetComponent<Slider>();
    }   

    public void SetHealth(int health)
    {
        slider.maxValue = 20;
        slider.value = health;
    }
}
