using System;
using Newtonsoft.Json;

[Serializable]
public class WeaponInfo: IEquatable<WeaponInfo>
{
    [UnityEngine.SerializeField] [JsonProperty] private WeaponID id;
    [JsonProperty] private WeaponType type;
    public ScopeID scopeID;

    public JSONObject asJSONObject()
    {
        JSONObject data = JSONObject.obj;
        data.AddField("weaponID", (int)id);
        data.AddField("scopeID", (int)scopeID);
        return data;
    }

    [JsonConstructor]
    public WeaponInfo(WeaponID id, WeaponType type, ScopeID scopeID)
    {
        this.id = id;
        this.type = type;
        this.scopeID = scopeID;
    }

    public WeaponID getID()
    {
        return id;
    }

    public WeaponType getWeaponType()
    {
        return type;
    }

    public bool Equals(WeaponInfo other)
    {
        return id == other.id;
    }

    public WeaponInfo copy()
    {
        var wi = new WeaponInfo(id,type,scopeID);
        return wi;
    }

    public override string ToString()
    {
        return $"id:{id},weaponType:{type},scopeID:{scopeID}";
    }
}
