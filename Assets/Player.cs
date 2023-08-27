using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class Player : BaseCharacter, IDamageable
    {
        public static Player Instance;
        public bool blockProtection;
        public bool blockAttack;

        [SerializeField] private CharacterMovement characterMovement;
        
        [SerializeField] public float timeInvulnerability;
        private CinemachineVirtualCamera _virtualCamera;

        private int _blockInputCount = 0;
        private bool _isTakeDamage;

        private Plane _plane;
        private Camera _camera;

        private void Awake()
        {
            if (Instance != null)
                Destroy(Player.Instance.gameObject);
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            _plane = new Plane(Vector3.up, Vector3.zero);
            _camera = Camera.main;
            _virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
            _virtualCamera.Follow = transform;
        }
        
        private void Update()
        {
            if (_blockInputCount != 0) return;

            RotatePlayer(GetMouseAngle());
            Move();
        }

        private void Move()
        {
            characterMovement.MovementOnDirection(GetDirectionMovement(), navMeshAgent);
        }

        private void RotatePlayer(Vector3 angle)
        {
            transform.LookAt(angle);
        }

        private Vector3 GetDirectionMovement()
        {
            var xRaw = 0;
            var zRaw = 0;
            var moveDir = new Vector3(xRaw, 0, zRaw);
            return moveDir;
        }

        public Vector3 GetMouseAngle()
        {
            var rayCam = _camera.ScreenPointToRay(new Vector3(0, 0, 0));
            _plane = new Plane(Vector3.up, transform.position);
            if (_plane.Raycast(rayCam, out var enter))
            {
                var hitPoint = rayCam.GetPoint(enter);
                var hitPointWithCharY = new Vector3(hitPoint.x, transform.position.y, hitPoint.z);
                return hitPointWithCharY;
            }

            return Vector3.zero;
        }

        private void Attack()
        {
            if (_blockInputCount != 0) return;
            if (blockAttack) return;
            blockAttack = true;
            SetWeaponPrefab(projectilePrefabs);

            var inVal = 0f;
            DOTween.To(() => inVal, x => inVal = x, 1, 0.3f).OnComplete(() => { });

            DOTween.To(() => inVal, x => inVal = x, 1, AttackCooldown).OnComplete(() => { blockAttack = false; });
        }

        public void TakeDamage(int dmg, GameObject source)
        {
            if (_isTakeDamage)
                return;
            _isTakeDamage = true;
            if (Hp <= 0)
            {
                KillPlayer();
            }
            else
            {
                ReturnNormalState();
            }
        }

        public void GetHp(int count)
        {
            Hp += count;
        }

        public override void KnockBack(Vector3 dir, float force)
        {
            if (isKnockBack)
                return;
            BlockInput();
            isKnockBack = true;
            rb.isKinematic = false;
            rb.AddForce(dir.normalized * force, ForceMode.Impulse);
            ReturnNormalState();
        }

        private void KillPlayer()
        {
            BlockInput();
            GetComponent<Collider>().enabled = false;
            navMeshAgent.enabled = false;
        }

        public void ResurrectPlayer()
        {
            Hp = maxHp;
            UnBlockInput();
            ReturnNormalState();
            GetComponent<Collider>().enabled = true;
            navMeshAgent.enabled = true;
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
        
        private void ReturnNormalState()
        {
            var inVal = 0f;
            DOTween.To(() => inVal, x => inVal = x, 1, timeInvulnerability).OnComplete(() =>
            {
                UnBlockInput();
                isKnockBack = false; 
                _isTakeDamage = false; 
                rb.isKinematic = true;
            });
        }
    }
