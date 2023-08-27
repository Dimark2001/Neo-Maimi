using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class BaseCharacter : MonoBehaviour
{
    [SerializeField] protected Renderer playerRenderer;
    protected NavMeshAgent NavMeshAgent;
    protected Rigidbody Rb;
}
