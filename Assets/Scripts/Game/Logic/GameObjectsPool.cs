using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectsPool : MonoBehaviour
{
    public static GameObjectsPool instance { get; private set; }
    [SerializeField] private PoolObjectsPrefabs prefabs;
    private Dictionary<PoolObjectID, Queue<PoolObject>> objectsDictionary;

    private void Awake()
    {
        instance = this;
        objectsDictionary = new Dictionary<PoolObjectID, Queue<PoolObject>>();
        foreach (var item in prefabs.getPrefabs())
        {
            objectsDictionary.Add(item.id, new Queue<PoolObject>());
        }
    }

    public PoolObject get(PoolObjectID id)
    {
        if (objectsDictionary[id].Count == 0)
        {
            addObjects(id, 5);
        }
        var gm = objectsDictionary[id].Dequeue();
        gm.gameObject.SetActive(true);
        gm.onDequeue();
        return gm;
    }

    /// <summary>
    /// Return the pool object to the pool
    /// </summary>
    /// <param name="objectToReturn">The pool object</param>
    public void returnToPool(PoolObject objectToReturn)
    {
        objectToReturn.onEnqeue(false);
        objectToReturn.gameObject.SetActive(false);
        objectsDictionary[objectToReturn.id].Enqueue(objectToReturn);
    }

    /// <summary>
    /// Moves the pool object to the pool
    /// Means it will set the parent of the pool object to be the pool
    /// </summary>
    /// <param name="objectToMove">The pool object</param>
    public void moveToPool(PoolObject objectToMove)
    {
        returnToPool(objectToMove);
        objectToMove.gameObject.transform.SetParent(transform, false);
    }

    private void addObjects(PoolObjectID id, int times)
    {
        for (int i = 0; i < times; i++)
        {
            var gm = Instantiate(prefabs.getPrefab(id));
            gm.name = id.ToString();
            gm.onEnqeue(true);
            gm.gameObject.SetActive(false);
            objectsDictionary[gm.id].Enqueue(gm);
        }
    }
}

public abstract class PoolObject: MonoBehaviour
{
    public abstract PoolObjectID id { get; }

    public abstract void onEnqeue(bool isCreated);
    public abstract void onDequeue();
}