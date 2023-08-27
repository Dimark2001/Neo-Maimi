using UnityEngine;

public abstract class BaseAttackController : MonoBehaviour
{
    public abstract void PerformAttack(Vector3 dir);
}