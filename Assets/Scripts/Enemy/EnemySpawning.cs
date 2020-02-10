using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawning : MonoBehaviour
{

    [SerializeField]
    GameObject enemyPref;
    [SerializeField]
    Transform[] spawnLocations;
    [SerializeField]
    float spawnRate = 2f;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("EnemySpawn", 0.5f, spawnRate);
    }

    public void EnemySpawn()
    {
        Instantiate(enemyPref, spawnLocations[Random.Range(0, spawnLocations.Length)].position, Quaternion.identity);
    }
}
