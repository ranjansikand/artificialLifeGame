using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "ArtificialLifeGame/MonsterData", order = 0)]
public class MonsterData : ScriptableObject {
    // Info
    public string _enemyName = "Basic Monster";
    public string _description = "This is a basic friendly monster";
    public GameObject _monsterPrefab;
    // Stats
    public float _health = 4,
        _damage = 4,
        _movementSpeed = 4,
        _sightRange = 4,
        _attackRange = 1,
        _followDistance = 4;
    public float _cooldown = 10,
        _spawnTime = 2;
    
    public int _layerMask;
}
