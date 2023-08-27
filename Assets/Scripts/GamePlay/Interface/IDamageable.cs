using UnityEngine;

public interface IDamageable
{
    public float Hp { get; set; }

    public void TakeDamage(GameObject source, int dmg);

    public void Dead();
}