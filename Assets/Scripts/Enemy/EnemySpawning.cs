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
        if (GameManager.gameRunning && GameManager.enemiesLeft > 0)
        {
            Vector3 spawnPoint = new Vector3(Random.Range(0, 21), 0, Random.Range(0, 21));
            while (Vector3.Distance(spawnPoint, transform.position) < 20 || Vector3.Distance(spawnPoint, transform.position) > 40)
            {
                spawnPoint = new Vector3(Random.Range(0, 21), 0, Random.Range(0, 21));
            }
            Instantiate(enemyPref, spawnPoint, Quaternion.identity);
            GameManager.enemiesLeft--;
        }
    }
}
