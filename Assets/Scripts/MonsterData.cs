using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "Data/MonsterData", order = 0)]
public class MonsterData : ScriptableObject {
    // Info
    public string _monsterName = "Basic Monster";
    public string _description = "This is a basic friendly monster";
    public string _flavorText = "One part lizard, four parts eyes...";
    public GameObject _monsterPrefab;
    // Stats
    public float _health = 4,
        _damage = 4,
        _movementSpeed = 4,
        _sightRange = 4,
        _attackRange = 1.5F,
        _followDistance = 4;
    public float _cooldown = 10,
        _spawnTime = 2;
    
    public int _layer;

    public int LayerMask { get { return 1 << _layer; }}
}
