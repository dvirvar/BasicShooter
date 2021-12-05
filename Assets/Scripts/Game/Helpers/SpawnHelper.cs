using UnityEngine;
/// <summary>
/// Helps us to spawn an object on empty spot
/// </summary>
[RequireComponent(typeof(Collider))]
public class SpawnHelper : MonoBehaviour
{
    public Collider spawnedItemSize;
    private new Collider collider;
    private GameObject checker;
    private Collider checkerCollider;

    void Start()
    {
        collider = GetComponent<Collider>();
        checker = Instantiate(spawnedItemSize.gameObject);
        checker.transform.SetParent(transform);
        checker.SetActive(false);
        checkerCollider = checker.GetComponent<Collider>();
        checkerCollider.isTrigger = true;
    }

    public Vector3 getFreePoint()
    {
        int attempts = 0;
        bool cantSpawn = true;
        Vector3 randomPoint;
        do
        {
            randomPoint = getRandomPointInBounds();
            checker.transform.localPosition = randomPoint;
            Collider[] overlaps = Physics.OverlapBox(checkerCollider.bounds.center, checkerCollider.bounds.extents / 2, checker.transform.rotation);
            cantSpawn = !canSpawn(overlaps);
            attempts++;
        } while (!cantSpawn && attempts < 5);
        return randomPoint;
    }

    private bool canSpawn(Collider[] overlaps)
    {
        foreach(Collider overlap in overlaps)
        {
            if (overlap.name.ToLower() == "character")
            {
                return false;
            }
        }
        return true;
    }

    private Vector3 getRandomPointInBounds()
    {
        Bounds bounds = collider.bounds;
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            0,
            Random.Range(bounds.min.z, bounds.max.z));
    }

}
