using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class SpawnUI : MonoBehaviour
{
    public OnSpawnPressed onSpawnPressed = new OnSpawnPressed();
    [SerializeField] protected Button spawnBtn;

    protected virtual void OnDestroy()
    {
        spawnBtn.onClick.RemoveAllListeners();
    }
}

public class OnSpawnPressed: UnityEvent<Loadout> { }