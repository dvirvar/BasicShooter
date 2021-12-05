using UnityEngine;

[SerializeField]
public abstract class GameMode: MonoBehaviour
{
    protected GameModeInfo gameModeInfo { get; set; }
}
