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
    bool invokeRunning;
    float timePassed;
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
            resumedTime = (Time.time - timePassed);
            CancelInvoke();
            invokeRunning = false;
        }
        else if (!GameManager.paused && !invokeRunning)
        {
            invokeRunning = true;
            InvokeRepeating("EnemySpawn", Mathf.Clamp(resumedTime, 0, spawnRate), spawnRate);
        }
    }

    public void EnemySpawn()
    {
        if (GameManager.gameRunning && GameManager.enemiesLeftToSpawn > 0 && !GameManager.paused)
        {
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
