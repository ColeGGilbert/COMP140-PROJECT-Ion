using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    float speed = .05f;
    Rigidbody rb;
    Animator anim;
    bool finish;
    Transform enemy;

    public bool canMove;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gameRunning && !GameManager.paused)
        {
            // Enables movement and animation
            canMove = true;
            anim.enabled = true;
            CheckForFinish();
        }
        else if (GameManager.paused)
        {
            // Disables movement and animation
            canMove = false;
            anim.enabled = false;
        }
        else
        {
            CheckGameEnd();
        }
    }

    void FixedUpdate()
    {
        if (finish)
        {
            EnemyGameOver();
        }
        else if(GameManager.gameRunning && canMove && !GameManager.paused)
        {
            Move();
        }
    }

    void Move()
    {
        // Moves enemy towards the centre of the level
        transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, speed);
        transform.LookAt(Vector3.zero);
    }

    void CheckForFinish()
    {
        // If the enemy reaches the centre, end game and start end animation
        if (transform.position == Vector3.zero)
        {
            GameManager.gameRunning = false;
            anim.SetTrigger("Expand");
            rb.useGravity = false;
            GetComponent<BoxCollider>().isTrigger = true;
        }
    }

    void CheckGameEnd()
    {
        // Once the enemy finishes it's animation, the game ends
        if (anim.gameObject.transform.localScale.y == 10)
        {
            finish = true;
        }
    }

    void EnemyGameOver()
    {
        if (finish)
        {
            // Enemy starts moving upwards
            rb.constraints = RigidbodyConstraints.FreezePositionX;
            rb.velocity = new Vector3(0, 10, 0);
            if(transform.position.y > 30)
            {
                // Seperates the particle system from the enemy to avoid being destroyed along with the enemy
                GetComponentInChildren<ParticleSystem>().gameObject.transform.parent = null;
                Destroy(gameObject);
            }
        }
    }
}
