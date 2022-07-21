using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] GameObject[] _enemies;
    [SerializeField] Vector2 _spawnInterval;
    [SerializeField] Vector3[] _spawnPositions = new Vector3[4];


    void Start() {
        StartCoroutine(SpawnEnemies());
    }


    void GenerateEnemy(Vector3 location) {
        GameObject newBlueprint = Instantiate(
            _enemies[Random.Range(0, _enemies.Length)], 
            location,
            Quaternion.identity);
    }

    IEnumerator SpawnEnemies() {
        while (true) {
            yield return new WaitForSeconds(Random.Range(
                _spawnInterval.x, 
                _spawnInterval.y)
            );

            GenerateEnemy( _spawnPositions[Random.Range(0, _spawnPositions.Length)] );
        }
    }
}
