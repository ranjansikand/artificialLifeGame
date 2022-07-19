using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    public static ItemGenerator instance;
    void Awake() { instance = this; }


    [SerializeField] List<MonsterData> _monsterData = new List<MonsterData>();
    [SerializeField] List<GameObject> _blueprints = new List<GameObject>();

    [SerializeField] Vector2 _spawnInterval;
    [SerializeField] Vector2[] _corners = new Vector2[2];

    void Start() {
        StartCoroutine(SpawnBlueprints());
    }


    public void GenerateBlueprint(Vector3 location, int uses = 4) {
        GameObject newBlueprint = Instantiate(
            _blueprints[Random.Range(0,4)], 
            location,
            Quaternion.identity);
        
        newBlueprint.GetComponent<Blueprint>().SetData(
            _monsterData[Random.Range(0, _monsterData.Count)],
            uses);
    }

    IEnumerator SpawnBlueprints() {
        while (true) {
            Vector3 randomPos = new Vector3(
                Random.Range(_corners[0].x, _corners[1].x), 
                0,
                Random.Range(_corners[0].y, _corners[1].y)
            );

            if (Physics.Raycast(randomPos + transform.up*10, Vector3.down, 12.5f)) {
                yield return new WaitForSeconds(Random.Range(_spawnInterval.x, _spawnInterval.y));
                GenerateBlueprint(randomPos);
            }
            else {
                yield return null;
            }
        }
    }
}
