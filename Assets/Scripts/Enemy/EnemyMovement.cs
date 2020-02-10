using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    float speed = 1f;
    Rigidbody rb;
    Animator anim;
    bool finish;
    Transform enemy;
    GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!finish && gm.gameRunning)
        {
            CheckForFinish();
        }
    }

    void FixedUpdate()
    {
        if (finish)
        {
            EnemyGameOver();
        }
        else if(gm.gameRunning)
        {
            Move();
        }
    }

    void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, speed);
        transform.LookAt(Vector3.zero);
    }

    void CheckForFinish()
    {
        if (transform.position == Vector3.zero)
        {
            gm.gameRunning = false;
            anim.SetTrigger("Expand");
            rb.useGravity = false;
            GetComponent<BoxCollider>().isTrigger = true;

            if (anim.gameObject.transform.localScale.y == 10)
            {
                finish = true;
            }
        }
    }

    void EnemyGameOver()
    {
        if (finish)
        {
            rb.constraints = RigidbodyConstraints.FreezePositionX;
            rb.velocity = new Vector3(0, 10, 0);
            if(transform.position.y > 30)
            {
                GetComponentInChildren<ParticleSystem>().gameObject.transform.parent = null;
                Destroy(gameObject);
            }
        }
    }
}
