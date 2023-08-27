using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public abstract class BaseCharacter : MonoBehaviour
{
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public Renderer characterRenderer;
    [HideInInspector] public float timeShoot = 0;
    
    public List<GameObject> projectilePrefabs;

    public int maxHp;
    [SerializeField] private int hp;

    public int Hp
    {
        get { return hp; }
        set
        {
            if (value <= maxHp) hp = value;
            else hp = maxHp;
        }
    }

    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private float protectionCooldown = 3;
    
    public float AttackCooldown
    {
        get
        {
            if(attackCooldown > 0.2) return attackCooldown;
            return 0.2f;
        } 
        set
        {
            if (value > 0.2f) attackCooldown = value;
            else attackCooldown = 0.2f;
        }
    }

    public float ProtectionCooldown
    {
        get
        {
            if(protectionCooldown > 0.5) return protectionCooldown;
            return 0.5f;
        } 
        set
        {
            if (value > 0.5f) protectionCooldown = value;
            else protectionCooldown = 0.5f;
        }
    }

    public bool isKnockBack = false;

    private GameObject _weaponPrefab;
    private void OnValidate()
    {
        AttackCooldown = attackCooldown;
        ProtectionCooldown = protectionCooldown;
    }

    public void SetWeaponPrefab(List<GameObject> weapons, int index)
    {
        _weaponPrefab = weapons[index];
    }

    public void SetWeaponPrefab(List<GameObject> weapons)
    {
        _weaponPrefab = weapons.First();
    }

    /*public T GetWeaponPrefab<T>() where T: 
    {
        if (_weaponPrefab == null)
            return null;
    
        return (T)_weaponPrefab;
    }*/

    public abstract void KnockBack(Vector3 dir, float force);
}