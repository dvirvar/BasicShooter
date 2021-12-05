using Newtonsoft.Json;
using UnityEngine;

public class PlayerLoadouts
{
    public Loadout loadout1;
    public Loadout loadout2;
    public Loadout loadout3;

    public PlayerLoadouts(Loadout loadout1, Loadout loadout2, Loadout loadout3)
    {
        this.loadout1 = loadout1;
        this.loadout2 = loadout2;
        this.loadout3 = loadout3;
    }

    public Loadout this[int index] =>
            index switch
            {
                0 => loadout1,
                1 => loadout2,
                2 => loadout3,
                _ => null,
            };
        
    

    public JSONObject asJsonObject()
    {
        var d = JSONObject.obj;
        d.AddField("loadouts", JSONObject.Create(JsonConvert.SerializeObject(this)));
        return d;
    }
}
