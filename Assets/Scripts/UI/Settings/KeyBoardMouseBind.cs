using UnityEngine;

public class KeyBoardMouseBind : KeyBindScript
{
    protected override void OnChange(Event e) {
        if (e.isKey)
        {
            //Keyboard
            if (e.type == EventType.KeyUp)
            {
                keyBindText.text = e.keyCode.ToString();
                popup.isActive = false;
                isChanging = false;
            }
        }
        else if (e.isMouse && keyCodeGroup != KeyCodeGroup.movement)
        {
            //Mouse
            if (e.type == EventType.MouseUp)
            {
                keyBindText.text = e.button.ToString();
                popup.isActive = false;
                isChanging = false;
            }
        }
    }

    protected override void popupWillShow()
    {
        if (keyCodeGroup == KeyCodeGroup.firing)
        {
            popup.setText("Press Any key");
        }
        else {
            popup.setText("Press Any Keyboard key");
        }
    }
}
