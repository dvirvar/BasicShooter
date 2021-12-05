using UnityEngine;

public class Damageable : MonoBehaviour
{
    public int health { get; set; } = 100;

    private DamageablePart[] damageableParts;
    protected virtual void Awake()
    {
        damageableParts = GetComponentsInChildren<DamageablePart>();
        for (int i = 0; i < damageableParts.Length; i++)
        {
            damageableParts[i].onDamage += Damageable_onDamage;
        }
    }

    public virtual void Damageable_onDamage(BulletInfo info, Bullet bullet)
    {
        health -= info.damage;
        print(health);
    }

    protected virtual void OnDestroy()
    {
        for (int i = 0; i < damageableParts.Length; i++)
        {
            damageableParts[i].onDamage -= Damageable_onDamage;
        }
        damageableParts = null;
    }
}
