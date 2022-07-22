using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour, IDamageable
{
    [SerializeField] MonsterData _data;
    [SerializeField] AudioClip _attackSound;

    NavMeshAgent _agent;
    Animator _animator;
    AudioSource _audioSource;
    Healthbar _healthbar;

    GameObject _target;
    static GameObject _player;
    IDamageable _receiver;

    float _currentHealth;

    static int _attackHash = 0;
    static int _walkHash;
    static int _deadHash;

    WaitForSeconds _delay = new WaitForSeconds(0.1f);
    bool _enemyDetected = false;
    float _destinationRadius = 0;
    float _turnSpeed = 1.5f;
    Collider[] targetsBuffer = new Collider[100];

    public delegate void MonsterEvent();
    public static MonsterEvent monsterDied;

    
    void Awake()
    {
        if (_player == null) _player = GameObject.FindWithTag("Player");
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _healthbar = GetComponentInChildren<Healthbar>();

        _currentHealth = _data._health;
        _agent.speed = _data._movementSpeed;

        if (_attackHash == 0) {
            _attackHash = Animator.StringToHash("Attack");
            _walkHash = Animator.StringToHash("Walk");
            _deadHash = Animator.StringToHash("Dead");
        }

        StartCoroutine(CheckDestination());
    }

    void Start() {
        _healthbar?.InitializeHealthbar(_currentHealth);
    }

    void Update() {
        if (_target != null) TurnToTarget();
    }

    IEnumerator CheckDestination() {
        while (_currentHealth > 0) {
            // Check for enemies
            if (!_enemyDetected) AcquireTarget();

            var dist = Vector3.Distance(transform.position, _target.transform.position);
            _enemyDetected = _receiver != null && _receiver.Alive();

            // Animate Object
            _animator.SetBool(_walkHash, dist > _destinationRadius * 1.1f);
            _animator.SetBool(_attackHash, _enemyDetected && !_animator.GetBool(_walkHash));

            // Move to Destination
            if (dist > _destinationRadius) { _agent.SetDestination(_target.transform.position); }
            else _agent.SetDestination(transform.position);

            yield return _delay;
        }
    }



    void AcquireTarget () {
        Vector3 a = transform.localPosition;
        Vector3 b = a;
        b.y += 2f;
        int hits = Physics.OverlapCapsuleNonAlloc(
            a, b, _data._sightRange, targetsBuffer, _data.LayerMask);
        if (hits > 0) {
            for (int i = 0; i < hits; i++) {
                _target = targetsBuffer[i].GetComponent<Collider>().gameObject;
                _destinationRadius = _data._attackRange;
                _receiver = _target.gameObject.GetComponent<IDamageable>();
                _enemyDetected = true;
            }
            return;
        }

        _target = _player;
        _destinationRadius = _data._followDistance;
        _enemyDetected = false;
    }

    void TurnToTarget() {
        var desiredRotation = _target.transform.position - transform.position;
        desiredRotation.y = 0;
        var targetRotation = Quaternion.LookRotation(desiredRotation);
        var str = Mathf.Min (_turnSpeed * Time.deltaTime, 1);
        transform.rotation = Quaternion.Lerp (transform.rotation, targetRotation, str);
    }

    // Combat Functions

    // Called from animation event
    public void Attack() {
        _receiver.Damage(_data._damage);
        _audioSource.PlayOneShot(_attackSound, AudioManager.MasterVolume);
    }

    public void Damage(float damage) {
        if (!Alive()) return;

        _currentHealth -= damage;
        _healthbar?.UpdateHealthbar(_currentHealth);

        if (!Alive()) {
            _agent.enabled = false;
            _animator.SetBool(_deadHash, true);
            monsterDied();
            Invoke(nameof(DestroyThis), 1.5f);
        }
    }

    public bool Alive() {
        return _currentHealth > 0;
    }

    private void DestroyThis() { Destroy(gameObject); }
}
