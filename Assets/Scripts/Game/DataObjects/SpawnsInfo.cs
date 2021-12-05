using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnsInfo : MonoBehaviour
{
    private List<Spawn> spawns;
    private Dictionary<string,SpawnHelper> spawnHelpers;

    void Awake() {
        fillSpawns();
    }
    // Start is called before the first frame update
    void fillSpawns()
    {
        spawns = new List<Spawn>();
        spawnHelpers = new Dictionary<string, SpawnHelper>();
        foreach (SpawnHelper spawnHelper in this.GetComponentsInChildren<SpawnHelper>()) {
            spawns.Add(new Spawn(spawnHelper.gameObject.name, spawnHelper.gameObject.transform.position, spawnHelper.gameObject.transform.rotation.eulerAngles));
            spawnHelpers.Add(spawnHelper.gameObject.name, spawnHelper);
        }
    }

    public Dictionary<string,SpawnHelper> getSpawnHelpers() {
        return this.spawnHelpers;
    }

    public List<Spawn> getSpawns() {
        fillSpawns();
        return this.spawns;
    }

    public SpawnHelper GetSpawnHelper(string id) {
        return this.spawnHelpers[id];
    }
}
