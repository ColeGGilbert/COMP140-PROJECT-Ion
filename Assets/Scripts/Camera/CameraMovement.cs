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
        // Calculates the planes/boundaries for the camera
        planes = GeometryUtility.CalculateFrustumPlanes(GetComponent<Camera>());
    }

    /// <summary>
    /// FixedUpdate() is used here to control manual movement when the controller is disabled
    /// The camera is rotated by the rotationSpeed based on the horizontal input from the player
    /// After changing the desired y rotation, Move() is run to affect the camera rotation
    /// </summary>
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
        // Re-calculates the planes/boundaries for the camera after moving
        planes = GeometryUtility.CalculateFrustumPlanes(GetComponent<Camera>());
    }
}
