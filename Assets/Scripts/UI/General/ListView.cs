using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//TODO: Change it to be more efficient
public class ListView<INFO, DISPLAY> : TableView<INFO,DISPLAY> where DISPLAY : Display<INFO> where INFO : IEquatable<INFO>
{
    [SerializeField]
    private bool stackFormat = false;
    [SerializeField]
    private bool destroyElementsOnSizeExceeded = false;
    [SerializeField]
    private int elementsLifeSpan = -1;

    public override void addInfo(INFO info) {
        if (!stackFormat)
            base.addInfo(info);
        else
        {
            infosDisplay.Reverse();
            infosDisplay.Add(createAndFillInfoDisplay(info));
            infosDisplay.Reverse();
        }
    }

    protected override DISPLAY createAndFillInfoDisplay(INFO info) {
        DISPLAY display;
        if (!stackFormat)
        {
            display =  base.createAndFillInfoDisplay(info);
        }
        else
        {
            display = base.createAndFillInfoDisplay(info);
            display.transform.SetSiblingIndex(0);
        }
        if (elementsLifeSpan != -1) {
            if (gameObject.activeInHierarchy)
            {
                StartCoroutine(destroyGOOnDelay(display, elementsLifeSpan));
            } else
            {
                removeInfo(display.info);
            }
        }
        return display;
    }

    protected IEnumerator destroyGOOnDelay(DISPLAY display, int delay)
    {
        yield return new WaitForSeconds(delay);
        removeInfo(display.info);
    }
}
