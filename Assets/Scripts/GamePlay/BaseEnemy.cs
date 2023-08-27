using UnityEngine;

public class BaseEnemy : BaseCharacter
{
    [SerializeField] private EnemyInfo enemyInfo;
    private bool _isCanMove;
    private CharacterMovement _characterMovement;


    protected virtual void Awake()
    {
        NavMeshAgent.isStopped = false;
        _isCanMove = true;
    }

    protected virtual void Update()
    {
        if (!_isCanMove)
            return;
        
    }
    
    protected Player CheckPlayerInRadius(float radiusAgro)
    {
        if (Physics.CheckSphere(transform.position, radiusAgro))
        {
            var hitColliders = Physics.OverlapSphere(transform.position, radiusAgro);

            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.TryGetComponent(out Player player))
                {
                    return player;
                }
            }
        }

        return null;
    }
    
    private protected void EnemyMoveToPlayer()
    {
        var player = CheckPlayerInRadius(5);
        if (player == null)
        {
            _characterMovement.StopMovement(NavMeshAgent);
            return;
        }

        _characterMovement.MovementToTheSelectionPosition(player.transform.position, enemyInfo.StoppingDistance, NavMeshAgent);
    }

    protected void BlockMove()
    {
        _isCanMove = false;
    }

    protected void AllowMove()
    {
        _isCanMove = true;
    }
}