using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyStickBind : KeyBindScript
{
    protected override void OnChange(Event e)
    {
        for (int i = 0; i < 20; i++)
        {
            if (Input.GetKey($"joystick button {i}"))
            {
                keyBindText.text = i.ToString();
                popup.isActive = false;
                isChanging = false;
            }
        }
    }

    protected override void popupWillShow()
    {
        popup.setText("Press Any Joystick Key");
    }
}
