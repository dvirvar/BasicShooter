using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsVC : ViewController
{
    GameInputManager gim;
    // Start is called before the first frame update
    void Start()
    {
        gim = GameInputManager.getInstance();
    }

    // Update is called once per frame
    void Update()
    {
        //if (gim.GetKeyDown("Attack")) {
        //    Debug.Log("Attack!");
        //}
    }
}
