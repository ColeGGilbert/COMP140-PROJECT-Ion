using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO.Ports;

public class GameManager : MonoBehaviour
{
    public static bool gameRunning { get; set; }
    int currentWave;
    public static int enemiesLeftToSpawn { get; set; }
    float spawnMod = 1;
    bool newRoundInit;
    Color newCol;
    public bool controllerActive;
    public int commPort;
    private SerialPort serial = null;
    private bool connected = false;
    float distance;
    [SerializeField]
    TakePicture tp;
    float cameraX;
    float cameraY;
    float cameraZ;

    [SerializeField]
    Renderer floor;

    // Start is called before the first frame update
    void Start()
    {
        if (controllerActive)
        {
            ConnectToSerial();
        }
        currentWave = 1;
        enemiesLeftToSpawn = 1;
        gameRunning = true;
        newRoundInit = true;
    }

    void ConnectToSerial()
    {
        Debug.Log("Attempting Serial: " + commPort);

        serial = new SerialPort("\\\\.\\COM" + commPort, 115200);
        serial.ReadTimeout = 50;
        serial.Open();
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

        ArduinoInput();

        Reset();
    }

    /*void AffectGrain()
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
    }*/

    void ArduinoInput()
    {
        if (controllerActive)
        {
            WriteToArduino("i");
            string[] values = ReadFromArduino(50).Split(',');
            foreach(string s in values)
            {
                Debug.Log(s);
            }

            if (values.Length > 0)
            {
                if (values[0] == "1")
                {
                    tp.InitPicture();
                }

                //Debug.Log(values[1]);
                //Debug.Log(values[2]);
                //Debug.Log(values[3]);
                
                cameraX = float.Parse(values[1]);
                cameraY = float.Parse(values[2]);
                cameraZ = float.Parse(values[3]);

                Camera.main.transform.eulerAngles = new Vector3(cameraX, -cameraY, -cameraZ);
                Debug.Log(Camera.main.transform.eulerAngles);
            }
        }
    }

    void WriteToArduino(string message)
    {
        serial.WriteLine(message);
        serial.BaseStream.Flush();
    }

    public string ReadFromArduino(int timeout = 0)
    {
        serial.ReadTimeout = timeout;
        try
        {
            return serial.ReadLine();
        }
        catch(System.TimeoutException e)
        {
            return null;
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
