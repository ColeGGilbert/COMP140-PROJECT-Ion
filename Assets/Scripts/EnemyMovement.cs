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

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        CheckForFinish();
    }

    void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, speed * Time.deltaTime);
        transform.LookAt(Vector3.zero);
    }

    void CheckForFinish()
    {
        if (transform.position == Vector3.zero)
        {
            anim.SetTrigger("Expand");
            if(anim.gameObject.transform.localScale.y == 10)
            {
                rb.useGravity = false;
                finish = true;
            }
        }

        if (finish)
        {
            rb.constraints = RigidbodyConstraints.FreezePositionX;
            rb.velocity = new Vector3(0, 20, 0);
        }
    }
}
