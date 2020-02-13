using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    float rotationSpeed = 2.5f;
    float yRot;
    public Plane[] planes;

    void Start()
    {
        planes = GeometryUtility.CalculateFrustumPlanes(GetComponent<Camera>());
    }

    void FixedUpdate()
    {
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            yRot += rotationSpeed;
            Move();
        }
        else if (Input.GetAxisRaw("Horizontal") < 0)
        {
            yRot -= rotationSpeed;
            Move();
        }
    }

    void Move()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.x, yRot, transform.rotation.z);
        planes = GeometryUtility.CalculateFrustumPlanes(GetComponent<Camera>());
    }
}
