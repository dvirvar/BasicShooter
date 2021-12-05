using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BordersInputField : InputField
{
    public int maxValue;
    private string lastText;

    protected override void OnEnable()
    {
        base.OnEnable();
        lastText = textComponent.text;
        onValueChanged.AddListener(text =>
        {
            if (text.Length == 0)
            {
                lastText = text;
                return;
            }
            if (int.TryParse(text, out int numText))
            {
                if (numText > maxValue || numText < 0)
                {
                    removeLastCharacter();
                }
                else
                {
                    lastText = text;
                }
            }
            else
            {
                removeLastCharacter();
            }
        });
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        onValueChanged.RemoveAllListeners();
    }

    private void removeLastCharacter()
    {
        int indexOfLastText = text.IndexOf(lastText);
        if (text.Length > 1) {
            indexOfLastText += indexOfLastText > 0 ? -1 : 1;
        }
        text = text.Remove(indexOfLastText,1);
    }
}
