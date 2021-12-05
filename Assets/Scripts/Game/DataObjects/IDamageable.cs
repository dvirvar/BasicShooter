using UnityEngine;

[SerializeField]
public interface IDamageable
{
    void takeDamage(BulletInfo info, Bullet bulletObject);
}
