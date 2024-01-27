using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MouseSenstivityController : MonoBehaviour
{
    public Slider sensitivitySlider;
    public float sensitivityMultiplier = 5f;

    void Start()
    {
        // Set default sensitivity
        SetMouseSensitivity();
    }

    public void SetMouseSensitivity()
    {
        float sensitivity = sensitivitySlider.value * sensitivityMultiplier;
        // Update the mouse sensitivity
        PlayerPrefs.SetFloat("MouseSensitivity", sensitivity);
    }

}
