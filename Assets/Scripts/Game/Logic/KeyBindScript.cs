using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Handles the key binding changes.
/// Keyboard & Mouse :
/// Movement -> Only keyboard
/// Firing & Aiming -> everything
/// Joystick
/// Movement & Aiming -> Only Stick
/// Fire & Player Interaction -> Everthing except sticks
/// </summary>
public abstract class KeyBindScript : MonoBehaviour
{
    protected Button keyBindBtn;
    protected Text keyBindText;
    protected bool isChanging = false;
    public Popup popup;
    public KeyCodeGroup keyCodeGroup;
    
    // Start is called before the first frame update
    protected void Start()
    {
        keyBindBtn = GetComponent<Button>();
        keyBindText = keyBindBtn.GetComponentInChildren<Text>();
        keyBindBtn.onClick.AddListener(() => {
            isChanging = true;
            popupWillShow();
            popup.isActive = true;
        });
    }
    protected abstract void popupWillShow();

    protected void OnGUI() {
        if (!isChanging)
        {
            return;
        }
        Event e = Event.current;
        if (e.keyCode == KeyCode.Escape) {
            isChanging = false;
            popup.isActive = false;
            return;
        }
        OnChange(e);
    }
    protected abstract void OnChange(Event e);
}

public enum KeyCodeGroup { 
    movement,firing
}











/*void OnGUI()
{
    curr = Event.current;
    if (isChanging)
    {
        if (!this.isKeyBoard && (curr.isMouse || curr.isKey))
        {
            btnText.text = curr.keyCode.ToString();
            isChanging = false;
        }
        else if (curr.isKey)
        {
            btnText.text = curr.keyCode.ToString();
            isChanging = false;
        }
        else if (curr.isMouse)
        {
            btnText.text = "Error";
            isChanging = false;
        }
    }
}
*/