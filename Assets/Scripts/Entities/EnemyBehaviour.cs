using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Character_Stats))]
internal class EnemyBehaviour : Destroyable
{
    [field: SerializeField] internal float MoveRadius { get; private set; } = 10;
    [field: SerializeField] internal float AttackCooldown { get; private set; } = 3;

    protected Transform _playerTransform;
    protected CharacterController _characterController;
    protected CharacterStates _state;
    protected bool _isAgro = false;
    protected float _cooldown = 0;
    protected Character_Stats _stats;
    private void Awake()
    {
        base.Awake();
    }
    protected void Start()
    {
        base.Start();
        _playerTransform = GameManager.game_manager.player_transform;
        _characterController = GetComponent<CharacterController>();
        _stats = GetComponent<Character_Stats>();
    }
    protected void Update()
    {
        CheckCharacterState();

        if (_state == CharacterStates.Moving)
            Move();
        else if (_state == CharacterStates.Attacking)
            Attack();

        _cooldown -= Time.deltaTime;
    }

    protected void CheckCharacterState()
    {
        float _distanceFromPlayer = Vector2.Distance(transform.position, _playerTransform.position);
        if ((_distanceFromPlayer <= MoveRadius))
        {
            if (_distanceFromPlayer <= _stats.attack_distance)
                _state = CharacterStates.Attacking;
            else
                _state = CharacterStates.Moving;
        }
        else
            _state = CharacterStates.None;
    }

    protected virtual void Move()
    {
        Vector3 targetPosition = new Vector3(_playerTransform.position.x,
                                     0,
                                     _playerTransform.position.z);
        transform.LookAt(targetPosition);

        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        Vector3 movDir = transform.forward;
        movDir.y = 0;

        _characterController.Move(movDir * Time.deltaTime * _stats.movement_speed);
    }
    protected virtual void Attack()
    {
        Ray _ray = new Ray(transform.position, transform.forward * _stats.attack_distance * 4);
        RaycastHit _hit;
        Debug.DrawRay(transform.position, transform.forward * _stats.attack_distance * 4, Color.yellow);

        if (_cooldown <= 0)
        {
            if (Physics.Raycast(_ray, out _hit, _stats.attack_distance * 4))
            {
                if (_hit.collider.CompareTag("Player"))
                    _hit.collider.GetComponent<Character_Stats>().Reduce_Health(1);
                else
                    Debug.Log(_hit.collider.name);
            }
            _cooldown = AttackCooldown;
        }
    }
}