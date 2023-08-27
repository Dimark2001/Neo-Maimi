using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/CreateEnemyData", order = 0)]
public class EnemyInfo : ScriptableObject
{
    [SerializeField] private float hp; 
    public float Hp => hp;
    
    [SerializeField] private float damage; 
    public float Damage => damage;
    
    [SerializeField] private float stoppingDistance; 
    public float StoppingDistance => stoppingDistance;
    
}