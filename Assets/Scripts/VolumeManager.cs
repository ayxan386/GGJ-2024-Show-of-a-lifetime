using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public string volumeParameterName;
    public Slider slider;

    void Start()
    {

        float volume = 0f;
        audioMixer.GetFloat(volumeParameterName, out volume);
        slider.value = Mathf.Pow(1, volume / 20f);
    }

    // This function is called whenever the value of the slider is changed
    public void OnValueChanged()
    {
        if(slider.value == 0f)
        {
            audioMixer.SetFloat(volumeParameterName, -80f);
        }
        else
        audioMixer.SetFloat(volumeParameterName, Mathf.Log10(slider.value) * 20f);
    }
}
