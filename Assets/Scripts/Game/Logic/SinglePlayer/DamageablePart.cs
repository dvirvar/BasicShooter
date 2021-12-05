using UnityEngine;
using System;

[RequireComponent(typeof(Collider))]
public class DamageablePart : MonoBehaviour, IDamageable
{
    public event Action<BulletInfo, Bullet> onDamage = delegate { };

    public void takeDamage(BulletInfo info, Bullet bulletObject)
    {
        onDamage(info, bulletObject);
    }
}
