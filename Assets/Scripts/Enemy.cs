using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(Animator), typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] EnemyData _data;

    private Animator _animator;
    private NavMeshAgent _agent;
    private Healthbar _healthbar;

    GameObject _target;
    IDamageable _receiver;

    float _currentHealth;

    bool _enemyDetected = false;
    WaitForSeconds _delay = new WaitForSeconds(0.25f);
    Collider[] targetsBuffer = new Collider[100];
    float _turnSpeed = 2f;

    static int _attackHash = 0;
    static int _walkHash;
    static int _deadHash;

    private void Awake() {
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _healthbar = GetComponentInChildren<Healthbar>();

        if (_attackHash == 0) {
            _attackHash = Animator.StringToHash("Attack");
            _walkHash = Animator.StringToHash("Walk");
            _deadHash = Animator.StringToHash("Dead");
        }
    }

    private void Start() {
        _currentHealth = _data.Health;
        _agent.speed = _data.Speed;
        _healthbar?.InitializeHealthbar(_currentHealth);

        StartCoroutine(EnemyBehavior());
    }

    private void Update() {
        if (_target != null) TurnToTarget();
    }


    IEnumerator EnemyBehavior() {
        while (Alive()) {
            if (_enemyDetected && _target!= null && _receiver.Alive()) {
                if (Vector3.Distance(transform.position, _target.transform.position) > _data.AttackRange) {
                    _animator.SetBool(_attackHash, false);
                    _agent.SetDestination(_target.transform.position);
                } else {
                    _agent.SetDestination(transform.position);
                    _animator.SetBool(_attackHash, true);
                }
            }

            else {
                AcquireTarget();
                
                if (Random.Range(1, 5) > 3) {  // Prevent constant reassignment
                    Vector2 rand = Random.insideUnitCircle * 3;
                    _agent.SetDestination(transform.position + new Vector3(rand.x, 0, rand.y));
                }
            }

            if (!_agent.pathPending &&
                _agent.remainingDistance <= _agent.stoppingDistance && 
                !_agent.hasPath || _agent.velocity.sqrMagnitude == 0f)  {
                    // Not Moving
                    _animator.SetBool(_walkHash, false);
            } 
            else if (!_animator.GetBool(_walkHash)) {
                _animator.SetBool(_walkHash, true);
            }

            yield return _delay;
        }
    }


    void AcquireTarget () {
        Vector3 a = transform.localPosition;
        Vector3 b = a;
        b.y += 2f;
        int hits = Physics.OverlapCapsuleNonAlloc(
            a, b, 5, targetsBuffer, _data.LayerMask);
        if (hits > 0) {
            for (int i = 0; i < hits; i++) {
                _target = targetsBuffer[i].GetComponent<Collider>().gameObject;
                _receiver = _target.gameObject.GetComponent<IDamageable>();
                _enemyDetected = true;
            }
            return;
        }

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
    public bool Alive() {
        return _currentHealth > 0;
    }

    public void Damage(float damage) {
        if (!Alive()) return;

        _currentHealth -= damage;
        _healthbar?.UpdateHealthbar(_currentHealth);
        AcquireTarget();

        if (!Alive()) {
            _agent.enabled = false;
            _target = null;
            _animator.SetBool(_deadHash, true);
            Invoke(nameof(DestroyThis), 1.5f);
        }
    }

    void DestroyThis() { Destroy(gameObject); }
    
    // Animation Event
    public void Attack() {
        if (_target == null || !_receiver.Alive()) {
            _enemyDetected = false;
            _animator.SetBool(_attackHash, false);
            return;
        }

        if (Vector3.Distance(transform.position, _target.transform.position) < _data.AttackRange) {
            _receiver?.Damage(_data.Damage);
        }
    }
}
