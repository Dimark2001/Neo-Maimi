using UnityEngine;

public interface IDamageable
{
    public float Hp { get; }

    public void TakeDamage(GameObject source, int dmg);

    public void Dead();
}