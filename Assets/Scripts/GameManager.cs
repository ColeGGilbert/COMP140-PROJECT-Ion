using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO.Ports;

public class GameManager : MonoBehaviour
{
    // gameRunning determines if game is lost, paused determines if the player is on the menu screen
    public static bool gameRunning { get; set; }
    public static bool paused { get; set; }

    int currentWave;
    public static int enemiesLeftToSpawn { get; set; }

    // spawnMod affects number of enemies spawned per wave
    float spawnMod = 1;

    // newRoundInit restricts the script from initiating a new round more than once
    bool newRoundInit;

    Color newCol;

    // controllerActive enables the game to attempt to use the camera controller
    public bool controllerActive;
    // commPort determines which port the controller is plugged into
    public int commPort;

    private SerialPort serial = null;
    private bool connected = false;

    [SerializeField]
    TakePicture tp;

    // Holds values for camera rotation input from arduino
    float cameraX;
    float cameraY;
    float cameraZ;

    [SerializeField]
    Renderer floor;

    // Start is called before the first frame update
    void Start()
    {
        // If controller is enabled, attempt to connect using commPort
        if (controllerActive)
        {
            ConnectToSerial();
        }
        // Assign first wave variables
        currentWave = 1;
        enemiesLeftToSpawn = 1;
        gameRunning = true;
        newRoundInit = true;
    }

    void ConnectToSerial()
    {
        Debug.Log("Attempting Serial: " + commPort);

        // Connects to arduino serial for data transfer
        serial = new SerialPort("\\\\.\\COM" + commPort, 115200);
        serial.ReadTimeout = 50;
        serial.Open();
    }

    // Update is called once per frame
    void Update()
    {
        // Checks if there are no more enemies spawning and there are no enemies in the scene
        if (enemiesLeftToSpawn <= 0 && FindObjectsOfType<EnemyMovement>().Length <= 0 && newRoundInit && gameRunning) 
        {
            StartCoroutine(DelayNewRound());
        }

        if (!gameRunning)
        {
            // Stops all enemies from moving, and triggers game over sequence
            GameEnd(); 
        }

        // Changes the floor colour to indicate the change in wave
        if(floor.material.color != newCol)
        {
            floor.material.color = Color.Lerp(floor.material.color, newCol, .1f);
        }

        ArduinoInput();

        Reset();
    }

    void ArduinoInput()
    {
        if (controllerActive)
        {
            // Request the arduino to write to serial
            WriteToArduino("i");
            // Read from the arduino serial (timeout 50ms)
            string[] values = ReadFromArduino(50).Split(',');

            if (values.Length > 0)
            {
                if (values[0] == "1")
                {
                    tp.InitPicture();
                }

                // Debug for incoming values from arduino
                //Debug.Log(values[1]);
                //Debug.Log(values[2]);
                //Debug.Log(values[3]);
                
                cameraX = float.Parse(values[1]);
                cameraY = float.Parse(values[2]);
                cameraZ = float.Parse(values[3]);

                Camera.main.transform.eulerAngles = new Vector3(cameraX, -cameraY, -cameraZ);
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

    // At the start of a new round, remove existing enemies and images
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

        // Assign new wave variables
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
        // Pressing G resets the game, to level 1, in case the player gets stuck
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
