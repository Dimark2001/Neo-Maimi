using Cinemachine;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CharacterMovement))]
[RequireComponent(typeof(Rigidbody))]
public class Player : BaseCharacter, IDamageable, ITemporable
{
    public float Hp { get; set; }
    public float LifeTime { get; set; }

    [SerializeField] private BaseAttackController attackController;
    
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    private CharacterMovement _characterMovement;
    
    private int _blockInputCount = 0;
    private bool _isTakeDamage;

    private Plane _plane;
    private Camera _camera;
    
    public void TakeDamage(GameObject source, int dmg)
    {
        if (_isTakeDamage)
            return;
        _isTakeDamage = true;
        if (Hp <= 0)
        {
            Dead();
        }
    }
    
    public void AddLifetime(float time)
    {
        LifeTime += time;
    }

    public void RemoveLifetime(float time)
    {
        LifeTime -= time;
        if (LifeTime <= 0)
        {
            Dead();
        }
    }

    public void Dead()
    {
        BlockInput();
        playerRenderer.material.color = Color.grey;
        NavMeshAgent.enabled = false;
    }

    private void Awake()
    {
        SetupBaseComponent();
        
        virtualCamera.Follow = transform;
        LifeTime = 5f;
        
        void SetupBaseComponent()
        {
            _characterMovement = GetComponent<CharacterMovement>();
            NavMeshAgent = GetComponent<NavMeshAgent>();
            Rb = GetComponent<Rigidbody>();
            _plane = new Plane(Vector3.up, Vector3.zero);
            _camera = Camera.main;
        }
    }
    
    private void Update()
    {
        if (_blockInputCount != 0) return;
        
        Move();
        Attack();
        RemoveLifetime(Time.deltaTime);
    }

    private void LateUpdate()
    {
        if (_blockInputCount != 0) return;

        RotatePlayer(GetMouseAngle());
    }

    private void RotatePlayer(Vector3 angle)
    {
        transform.LookAt(angle);
    }

    private Vector3 GetMouseAngle()
    {
        var rayCam = _camera.ScreenPointToRay(Input.mousePosition);
        if (_plane.Raycast(rayCam, out var enter))
        {
            var hitPoint = rayCam.GetPoint(enter);
            var hitPointWithCharY = new Vector3(hitPoint.x, transform.position.y, hitPoint.z);
            return hitPointWithCharY;
        }

        return Vector3.zero;
    }

    private void Move()
    {
        _characterMovement.MovementOnDirection(GetDirectionMovement(), NavMeshAgent);
        
        Vector3 GetDirectionMovement()
        {
            var xRaw = Input.GetAxisRaw("Horizontal");
            var zRaw = Input.GetAxisRaw("Vertical");
            var moveDir = new Vector3(xRaw, 0, zRaw);
            return moveDir;
        }
    }

    private void Attack()
    {
        if (!Input.GetMouseButtonDown(0))
            return;
        AddLifetime(5f);
    }

    private void BlockInput()
    {
        _blockInputCount++;
    }

    private void UnBlockInput()
    {
        if (_blockInputCount <= 0)
        {
            _blockInputCount = 0;
            return;
        }
        _blockInputCount--;
    }
}
