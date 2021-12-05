using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : PoolObject
{
    public override PoolObjectID id => PoolObjectID.Bullet;
    public Rigidbody rb { get; private set; }
    public BulletInfo? info;
    private float timeElapsed = 0;
    
    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void onEnqeue(bool isCreated)
    {
        if (!isCreated)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            timeElapsed = 0;
            info = null;
        }
    }

    public override void onDequeue()
    {
        
    }
    
    private void Update()
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed > 6)
        {
            GameObjectsPool.instance.returnToPool(this);
        }
    }

    private void FixedUpdate()
    {
        rb.AddForce(Vector3.down * 0.1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.takeDamage(info.Value,this);
        }
        GameObjectsPool.instance.returnToPool(this);
    }
}
