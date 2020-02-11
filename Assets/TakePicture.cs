using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakePicture : MonoBehaviour
{

    CameraMovement camMove;

    // Start is called before the first frame update
    void Start()
    {
        camMove = GetComponent<CameraMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            foreach (EnemyMovement em in FindObjectsOfType<EnemyMovement>())
            {
                if (GeometryUtility.TestPlanesAABB(camMove.planes, em.GetComponent<Collider>().bounds))
                {
                    em.GetComponentInChildren<Animator>().SetTrigger("End");
                }
            }
        }
    }
}
