﻿using System.Collections;
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
            canMove = true;
            anim.enabled = true;
            CheckForFinish();
        }
        else if (GameManager.paused)
        {
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
        transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, speed);
        transform.LookAt(Vector3.zero);
    }

    void CheckForFinish()
    {
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
        if (anim.gameObject.transform.localScale.y == 10)
        {
            finish = true;
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
