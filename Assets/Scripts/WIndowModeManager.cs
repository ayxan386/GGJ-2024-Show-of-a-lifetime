using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WIndowModeManager : MonoBehaviour
{
    [SerializeField] TMP_Dropdown windowModes;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnValueChange()
    {
        int value = windowModes.value;

        if (value == 0)
        {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }
        if (value == 1)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }if (value == 2)
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
        Debug.Log(value);
    }

}
