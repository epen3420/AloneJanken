using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.InputSystem.Controls;
using TMPro;

public class ObservePressKey : MonoBehaviour
{
    [SerializeField]
    private TMP_Text displayKeyNameText;


    void Update()
    {
        var keyboard = Keyboard.current;

        if (keyboard == null)
        {
            enabled = false;
            return;
        }

        string displayKeyStr = "";
        foreach (KeyControl keyControl in keyboard.allKeys)
        {
            if (keyControl == null) break;

            if (keyControl.isPressed)
            {
                displayKeyStr += $"{keyControl.displayName}, ";
            }
        }


        displayKeyNameText.SetText(displayKeyStr);
    }
}
