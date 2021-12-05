using System;
using UnityEngine;

public class HitEventInfo : IEquatable<HitEventInfo>
{
    public string id;
    public Sprite previewSprite;
    public string description;
    public int reward;
    public bool isDeadPoints;

    public bool Equals(HitEventInfo other)
    {
        return other.id == id;
    }
}
