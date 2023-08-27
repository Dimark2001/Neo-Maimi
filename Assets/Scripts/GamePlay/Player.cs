using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CharacterMovement))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour, IDamageable
{
    public float Hp { get; }
    
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    private CharacterMovement _characterMovement;
    private NavMeshAgent _navMeshAgent;
    private Rigidbody _rb;
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

    public void Dead()
    {
        BlockInput();
        GetComponent<Collider>().enabled = false;
        _navMeshAgent.enabled = false;
    }

    private void Awake()
    {
        SetupBaseComponent();
        
        virtualCamera.Follow = transform;

        
        void SetupBaseComponent()
        {
            _characterMovement = GetComponent<CharacterMovement>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _rb = GetComponent<Rigidbody>();
            _plane = new Plane(Vector3.up, Vector3.zero);
            _camera = Camera.main;
        }
    }
    
    private void Update()
    {
        if (_blockInputCount != 0) return;
        
        Move();
    }

    private void LateUpdate()
    {
        RotatePlayer(GetMouseAngle());
    }

    private void RotatePlayer(Vector3 angle)
    {
        transform.LookAt(angle);
    }

    private Vector3 GetMouseAngle()
    {
        var rayCam = _camera.ScreenPointToRay(Input.mousePosition);
        //_plane = new Plane(Vector3.up, transform.position);
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
        _characterMovement.MovementOnDirection(GetDirectionMovement(), _navMeshAgent);
        
        Vector3 GetDirectionMovement()
        {
            var xRaw = Input.GetAxisRaw("Horizontal");
            var zRaw = Input.GetAxisRaw("Vertical");
            var moveDir = new Vector3(xRaw, 0, zRaw);
            return moveDir;
        }
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
