using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour
{
    [SerializeField] MonsterData _data;

    NavMeshAgent _agent;
    Animator _animator;

    GameObject _target;
    GameObject _player;
    IDamageable _receiver;

    float _currentHealth;

    int _attackHash;
    int _walkHash;

    WaitForSeconds _delay = new WaitForSeconds(0.1f);
    bool _enemyDetected = false;
    bool _enemyAlive = false;
    float _destinationRadius = 0;
    Collider[] targetsBuffer = new Collider[100];

    // Start is called before the first frame update
    void Awake()
    {
        _player = GameObject.FindWithTag("Player");
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();

        _currentHealth = _data._health;
        _agent.speed = _data._movementSpeed;

        _attackHash = Animator.StringToHash("Attack");
        _walkHash = Animator.StringToHash("Walk");

        StartCoroutine(CheckDestination());
    }

    IEnumerator CheckDestination() {
        while (_currentHealth > 0) {
            // Check for enemies
            if (!_enemyDetected) AcquireTarget();
            else StartCoroutine(CheckTarget());

            // Move to Destination
            if (Vector3.Distance(transform.position, _target.transform.position) > _destinationRadius) {
                _animator.SetBool(_attackHash, false);
                _animator.SetBool(_walkHash, true);

                _agent.enabled = true;
                _agent.SetDestination(_target.transform.position);
                
            } 
            else if (_agent.enabled == true) {
                _agent.enabled = false;
                _animator.SetBool(_walkHash, false);
                _animator.SetBool(_attackHash, _enemyDetected && _enemyAlive);
            }

            yield return _delay;
        }
    }

    IEnumerator CheckTarget() {
        _enemyAlive = true;

        // Check if there is an enemy to pursue/attack
        while (_enemyAlive) {
            if (!_receiver.Alive()) {
                _enemyAlive = false;
                _enemyDetected = false;
            }
            yield return _delay;
        }
    }

    void AcquireTarget () {
        Vector3 a = transform.localPosition;
        Vector3 b = a;
        b.y += 2f;
        int hits = Physics.OverlapCapsuleNonAlloc(
            a, b, _data._sightRange, targetsBuffer, _data._layerMask);
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

    // Called from animation event
    public void Attack() {
        _receiver.Damage(_data._damage);
    }
}
