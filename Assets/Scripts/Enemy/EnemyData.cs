using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Data/EnemyData", order = 1)]
public class EnemyData : ScriptableObject {
    public float _health, 
        _damage,
        _speed;

    public float _attackRange;

    public LayerMask _layer;

    public float Health { get { return _health; }}
    public float Damage { get { return _damage; }}
    public float Speed { get { return _speed; }}

    public float AttackRange { get { return _attackRange; }}

    public LayerMask LayerMask { get { return _layer; }}
}