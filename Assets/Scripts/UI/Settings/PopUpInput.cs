using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpInput : Popup
{
    public static KeyBindScript keyBindScript;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnGUI()
    {
        Event e = Event.current;
        if (e.isKey)
        {
            //Keyboard
            if (e.type == EventType.KeyUp)
            {
                //Debug.Log("Detected key up: " + e.keyCode);
            }
        }
        else if (e.isMouse)
        {
            //Mouse
            if (e.type == EventType.MouseUp)
            {
                //Debug.Log("Detected mouse up: " + e.button);
            }
        }
        else
        {
            //Joystick
            if (Input.GetAxis(StaticStrings.Input.horizontalMovement) != 0)
            {
                //print(StaticStrings.Input.horizontalMovement + $" {Input.GetAxis(StaticStrings.Input.horizontalMovement)}");
            }
            else if (Input.GetAxis(StaticStrings.Input.verticalMovement) != 0)
            {
                //print(StaticStrings.Input.verticalMovement + $" {Input.GetAxis(StaticStrings.Input.verticalMovement)}");
            }
            else if (Input.GetAxis(StaticStrings.Input.horizontalLook) != 0)
            {
                //print($"{StaticStrings.Input.horizontalLook} {Input.GetAxis(StaticStrings.Input.horizontalLook)}");
            }
            else if (Input.GetAxis(StaticStrings.Input.verticalLook) != 0)
            {
                //print($"{StaticStrings.Input.verticalLook} {Input.GetAxis(StaticStrings.Input.verticalLook)}");
            }
            else
            {
                for (int i = 0; i < 20; i++)
                {
                    if (Input.GetKey($"joystick button {i}"))
                    {
                        //print(i);
                    }
                }
            }
        }
    }
}
