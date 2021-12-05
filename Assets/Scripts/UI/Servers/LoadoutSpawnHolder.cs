using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadoutSpawnHolder : MonoBehaviour
{
    [SerializeField] private Text primaryName;
    [SerializeField] private Image primaryImage;
    [SerializeField] private Text secondaryName;
    [SerializeField] private Image secondaryImage;

    public void init(Loadout loadout)
    {
        primaryName.text = loadout.getPrimary().getID().ToString();
        secondaryName.text = loadout.getSecondary().getID().ToString();
    }
}
