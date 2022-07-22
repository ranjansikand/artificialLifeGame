using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IDamageable
{
    private PlayerInput _playerInput;

    private CharacterController _characterController;
    private Animator _animator;

    public delegate void PlayerEvent();
    public static PlayerEvent monsterCreated;
    public static PlayerEvent playerDied;

    // Health
    int _currentHealth = 4;

    // Movement
    bool _canMove = true;
    float _walkSpeed = 5.25f;
    float _speedSmoothVelocity;
    Vector2 _currentMovementInput;
    Vector3 _appliedMovement;
    [SerializeField] LayerMask _obstacleLayers;

    // Monster
    bool _nearBlueprint = false;
    Blueprint _blueprint;
    int _selectedMonster = 0;
    
    int _spawnedMonsters = 0;
    const int _maxMonsters = 4;
    MonsterData[] _storedMonsters = new MonsterData[4];

    bool[] _monsterAvailable = { true, true, true, true };
    int[] _usesLeft = { 0, 0, 0, 0 };

    // Animation
    private int _walkHash;
    private int _spawnHash;


    private void Awake() {
        _playerInput = new PlayerInput();
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();

        _walkHash = Animator.StringToHash("Walk");
        _spawnHash = Animator.StringToHash("Spawn");

        _playerInput.Player.Move.performed += OnMovementInput;
        _playerInput.Player.Move.canceled += OnMovementInput;
        _playerInput.Player.Monster1.performed += SwitchMonster1;
        _playerInput.Player.Monster2.performed += SwitchMonster2;
        _playerInput.Player.Monster3.performed += SwitchMonster3;
        _playerInput.Player.Monster4.performed += SwitchMonster4;
        _playerInput.Player.Pickup.performed += OnPickup;
        _playerInput.Player.Spawn.performed += OnSpawn;

        MonsterController.monsterDied += SubtractMonster;
    }

    private void Update() {
        HandleMotion();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == 9) {
            _nearBlueprint = true;
            _blueprint = other.gameObject.GetComponent<Blueprint>();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.layer == 9) {
            _nearBlueprint = false;
            _blueprint = null;
        }
    }

    // Input Functions

    private void OnEnable() { _playerInput.Enable(); }

    private void OnDisable() { _playerInput.Disable(); }

    private void OnMovementInput(InputAction.CallbackContext context) {
        if (_canMove) { 
            _currentMovementInput = context.ReadValue<Vector2>().normalized;
        }
    }

    private void OnPickup(InputAction.CallbackContext context) {
        if (_nearBlueprint && _monsterAvailable[_selectedMonster] && _blueprint != null) {
            _storedMonsters[_selectedMonster] = _blueprint.Data;
            _usesLeft[_selectedMonster] = _blueprint.Uses;

            Slots.instance.UpdateSlot(_selectedMonster, _blueprint.Data);
            _blueprint.Collected();
        }
    }

    private void OnSpawn(InputAction.CallbackContext context) {
        if (_usesLeft[_selectedMonster] > 0 && _monsterAvailable[_selectedMonster]
            && _spawnedMonsters < _maxMonsters) {
            StartCoroutine(SpawnDelay());
            _usesLeft[_selectedMonster] -= 1;
        }
    }

    private void SwitchMonster1(InputAction.CallbackContext context) {  Select(1); }

    private void SwitchMonster2(InputAction.CallbackContext context) {  Select(2); }

    private void SwitchMonster3(InputAction.CallbackContext context) {  Select(3); }

    private void SwitchMonster4(InputAction.CallbackContext context) {  Select(4); }


    // Helper Functions

    void HandleMotion() {
        bool isWalking = _currentMovementInput != Vector2.zero;

        // Animation
        if (_animator.GetBool(_walkHash) != isWalking) 
            _animator.SetBool(_walkHash, isWalking);

        if (!isWalking) return;

        // Direction
        _appliedMovement = new Vector3(_currentMovementInput.x, 0, _currentMovementInput.y) * _walkSpeed;
        if (!Physics.Raycast(transform.position, _appliedMovement, 0.5f, _obstacleLayers))
            _characterController.Move(_appliedMovement * Time.deltaTime);

        // Rotation
        float targetRotation = Mathf.Atan2(_currentMovementInput.x, _currentMovementInput.y) * Mathf.Rad2Deg;
        transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(
            transform.eulerAngles.y, targetRotation, 
            ref _speedSmoothVelocity, 0.1f);
    }

    void FreezeMovement() {
        _canMove = false;
        _animator.SetBool(_walkHash, false);
        _currentMovementInput = Vector2.zero;
    }

    void Select(int number) {
        number--;
        _selectedMonster = number;
        Slots.instance.SelectSlot(number, _storedMonsters[_selectedMonster]);
    }

    // Coroutines to create or spawn monsters
    IEnumerator SpawnDelay() {
        FreezeMovement();
        _animator.SetBool(_spawnHash, true);

        int desiredMonsterIndex = _selectedMonster;  // In case user switches after spawn begins

        // UI depiction and actual wait time
        Slots.instance.CooldownSlot(desiredMonsterIndex, _storedMonsters[desiredMonsterIndex]._spawnTime);
        yield return new WaitForSeconds(_storedMonsters[desiredMonsterIndex]._spawnTime);

        Instantiate(_storedMonsters[desiredMonsterIndex]._monsterPrefab, 
            transform.position + transform.forward * 2, 
            Quaternion.identity);
        
        _animator.SetBool(_spawnHash, false);
        _canMove = true;
        AddMonster();
        StartCoroutine(CooldownDelay(desiredMonsterIndex));
    }

    IEnumerator CooldownDelay(int monster) {
        // If no uses left
        if (_usesLeft[monster] <= 0) {
            _storedMonsters[monster] = null;
            Slots.instance.UpdateSlot(monster, null);
            yield break;
        }

        // Deactivate monster
        _monsterAvailable[monster] = false;
        // UI representation of cooldown
        Slots.instance.CooldownSlot(monster, _storedMonsters[monster]._cooldown);
        yield return new WaitForSeconds(_storedMonsters[monster]._cooldown);
        _monsterAvailable[monster] = true;
    }

    // Damageable Interface for Combat
    public bool Alive() {
        return _currentHealth > 0;
    }

    public void Damage(float damage) {
        if (!_canMove) return;
        _currentHealth -= 1;
        PlayerHearts.instance.LoseHealth();
        
        if (_canMove && _currentHealth <= 0) {
            _canMove = false;
            _appliedMovement = Vector3.zero;
            _animator.SetBool("Dead", true);
        }
    }

    // Monster Tracking
    void AddMonster() {
        _spawnedMonsters += 1;
        monsterCreated();
        if (_spawnedMonsters > _maxMonsters) Debug.LogWarning("Too many monsters!");
    }

    void SubtractMonster() {
        _spawnedMonsters = Mathf.Max(_spawnedMonsters - 1, 0);
    }
}
