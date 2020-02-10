using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    float rotationSpeed;
    float yRot;

    void FixedUpdate()
    {
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            yRot += rotationSpeed;
        }
        else if (Input.GetAxisRaw("Horizontal") < 0)
        {
            yRot -= rotationSpeed;
        }
        transform.rotation = Quaternion.Euler(transform.rotation.x, yRot, transform.rotation.z);
    }
}
