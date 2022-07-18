using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInput _playerInput;

    private CharacterController _characterController;
    private Animator _animator;


    // Movement
    float _walkSpeed = 4.25f;
    float _speedSmoothVelocity;
    Vector2 _currentMovementInput;
    Vector3 _appliedMovement;

    // Monster
    bool _nearBlueprint = false;
    MonsterData _blueprint;
    int _selectedMonster = 1;
    MonsterData[] _storedMonsters = new MonsterData[4];
    bool[] _monsterAvailable = { false, false, false, false };

    // Animation
    private int _walkHash;


    private void Awake() {
        _playerInput = new PlayerInput();
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();

        _walkHash = Animator.StringToHash("Walk");

        _playerInput.Player.Move.performed += OnMovementInput;
        _playerInput.Player.Move.canceled += OnMovementInput;
        _playerInput.Player.Monster1.performed += SwitchMonster1;
        _playerInput.Player.Monster2.performed += SwitchMonster2;
        _playerInput.Player.Monster3.performed += SwitchMonster3;
        _playerInput.Player.Monster4.performed += SwitchMonster4;
    }

    private void Update() {
        HandleMotion();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == 9) {
            _nearBlueprint = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.layer == 9) {
            _nearBlueprint = false;
        }
    }

    // Input Functions

    private void OnEnable() { _playerInput.Enable(); }

    private void OnDisable() { _playerInput.Disable(); }

    private void OnMovementInput(InputAction.CallbackContext context) {
        _currentMovementInput = context.ReadValue<Vector2>().normalized;
    }

    private void OnPickup(InputAction.CallbackContext context) {
        if (_nearBlueprint) {
            // Open Pickup Menu
        }
    }

    private void OnSpawn(InputAction.CallbackContext context) {
        // Spawn Selected Monster

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

        // Direction
        _appliedMovement = new Vector3(_currentMovementInput.x, 0, _currentMovementInput.y) * _walkSpeed;
        _characterController.Move(_appliedMovement * Time.deltaTime);
    
        if (!isWalking) return;
        // Rotation
        float targetRotation = Mathf.Atan2(_currentMovementInput.x, _currentMovementInput.y) * Mathf.Rad2Deg;
        transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(
            transform.eulerAngles.y, targetRotation, 
            ref _speedSmoothVelocity, 0.1f);
    }

    void Select(int number) {
        number--;
        if (_storedMonsters[number] != null) _selectedMonster = number; 
    }

    IEnumerator SpawnDelay() {
        if (!_monsterAvailable[_selectedMonster]) yield break;

        yield return new WaitForSeconds(_storedMonsters[_selectedMonster]._spawnTime);
        Instantiate(_storedMonsters[_selectedMonster]._monsterPrefab, transform.forward * 2, Quaternion.identity);
    }

    IEnumerator CooldownDelay(int monster) {
        _monsterAvailable[monster] = false;
        yield return new WaitForSeconds(_storedMonsters[monster]._cooldown);
        _monsterAvailable[monster] = true;
    }
}
