using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    [SerializeField] private Slider forceSlider;
    [SerializeField] private Text forceText;

    private void Start()
    {
        forceSlider.value = forceSlider.maxValue / 2;
    }

    public void UpdateForceForceText(float force)
    {
        forceText.text =$"force {force}";
    }
}
