﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static bool gameRunning { get; set; }
    int currentWave;
    public static int enemiesLeftToSpawn { get; set; }
    float spawnMod = 1;
    bool newRoundInit;
    Color newCol;
    float distance;

    [SerializeField]
    Renderer floor;

    // Start is called before the first frame update
    void Start()
    {
        currentWave = 1;
        enemiesLeftToSpawn = 1;
        gameRunning = true;
        newRoundInit = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemiesLeftToSpawn <= 0 && FindObjectsOfType<EnemyMovement>().Length <= 0 && newRoundInit) // Checks if there are no more enemies spawning and there are no enemies in the scene
        {
            StartCoroutine(DelayNewRound());
        }

        if (!gameRunning)
        {
            GameEnd(); // Stops all enemies from moving, and triggers game over sequence
        }

        if(floor.material.color != newCol)
        {
            floor.material.color = Color.Lerp(floor.material.color, newCol, .1f);
        }

        Reset();
    }

    void AffectGrain()
    {
        foreach (EnemyMovement em in FindObjectsOfType<EnemyMovement>())
        {
            distance = 20f;
            if (Vector3.Distance(transform.position, em.transform.position) < distance)
            {
                distance = Vector3.Distance(transform.position, em.transform.position);
            }
            em.GetComponentInChildren<Animator>().enabled = false;
            Destroy(em);
        }
    }

    void NewRound()
    {
        foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(enemy);
        }
        foreach(GameObject image in GameObject.FindGameObjectsWithTag("Image"))
        {
            Destroy(image);
        }
        currentWave++;
        spawnMod += spawnMod * 0.1f; // Increase amount of enemies spawned each round
        enemiesLeftToSpawn = Mathf.RoundToInt(currentWave * spawnMod);
        newCol = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        newRoundInit = true;    
    }

    void GameEnd()
    {
        foreach(Animator anim in FindObjectsOfType<Animator>()) // Gets all the animator components in the scene
        {
            anim.SetTrigger("End");
        }
    }

    void Reset()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            foreach(EnemyMovement em in FindObjectsOfType<EnemyMovement>())
            {
                Destroy(em.gameObject);
            }
            currentWave = 1;
            enemiesLeftToSpawn = 1;
            gameRunning = true;
            newRoundInit = true;
        }
    }

    IEnumerator DelayNewRound()
    {
        newRoundInit = false;
        yield return new WaitForSeconds(2.5f);
        NewRound();
    }
}