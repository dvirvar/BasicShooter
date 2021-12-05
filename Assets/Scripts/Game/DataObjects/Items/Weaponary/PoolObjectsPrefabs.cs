using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pool objects prefabs", menuName = "Pool/Objects prefabs")]
public class PoolObjectsPrefabs : ScriptableObject
{
    [SerializeField] private List<PoolObjectPrefab> prefabs;
    public PoolObject getPrefab(PoolObjectID id)
    {
        return prefabs.Find(poolObject => poolObject.id == id).prefab;
    }

    public List<PoolObjectPrefab> getPrefabs()
    {
        return prefabs;
    }
}
[System.Serializable]
public struct PoolObjectPrefab
{
    public PoolObjectID id;
    public PoolObject prefab;
}