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

    // invokeRunning controls if a new Invoke needs to be started or if one is already running
    bool invokeRunning;
    // timePassed holds the time that the invoke last ran
    float timePassed;
    // resumedTime controls how long is needed before the next enemy needs to be spawned
    float resumedTime;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("EnemySpawn", 0.5f, spawnRate);
        invokeRunning = true;
    }

    private void Update()
    {
        if (GameManager.paused && invokeRunning)
        {
            // resumedTime determines how long has passed since the last invoke, at the time which the player pauses
            resumedTime = (Time.time - timePassed);
            CancelInvoke();
            invokeRunning = false;
        }
        else if (!GameManager.paused && !invokeRunning)
        {
            invokeRunning = true;
            // Restart invoke for EnemySpawn based on time remaining from last invoke, based on spawnRate
            InvokeRepeating("EnemySpawn", Mathf.Clamp(2-resumedTime, 0, spawnRate), spawnRate);
        }
    }

    public void EnemySpawn()
    {
        // If there are enemies left to spawn...
        if (GameManager.gameRunning && GameManager.enemiesLeftToSpawn > 0 && !GameManager.paused)
        {
            // Pick a spawnPoint using a random offset, whilst ensuring that the enemy spawns at least so far away from the centre of the level
            Vector3 spawnPoint = new Vector3(Random.Range(0, 21), 0, Random.Range(0, 21));
            while (Vector3.Distance(spawnPoint, transform.position) < 20 || Vector3.Distance(spawnPoint, transform.position) > 40)
            {
                spawnPoint = new Vector3(Random.Range(-20, 21), 0, Random.Range(-20, 21));
            }
            Instantiate(enemyPref, spawnPoint, Quaternion.identity);
            GameManager.enemiesLeftToSpawn--;
        }
        timePassed = Time.time;
    }
}
