using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static bool gameRunning { get; set; }
    int currentWave;
    public static int enemiesLeft { get; set; }
    float spawnMod = 1;

    // Start is called before the first frame update
    void Start()
    {
        currentWave = 1;
        enemiesLeft = 1;
        gameRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(enemiesLeft <= 0)
        {
            NewRound();
        }

        if (!gameRunning)
        {
            GameEnd();
        }
    }

    void NewRound()
    {
        currentWave++;
        spawnMod += spawnMod * 0.1f; // Increase amount of enemies spawned each round
        enemiesLeft = Mathf.RoundToInt(currentWave * spawnMod);
    }

    void GameEnd()
    {
        foreach(Animator anim in FindObjectsOfType<Animator>())
        {
            anim.SetTrigger("End");
        }
    }
}
