using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakePicture : MonoBehaviour
{

    CameraMovement camMove;
    Camera snapCam;
    [SerializeField]
    GameObject newImagePref;
    Transform canvas;
    [SerializeField]
    float distance = 20f;

    // Start is called before the first frame update
    void Start()
    {
        snapCam = GameObject.FindGameObjectWithTag("SnapCam").GetComponent<Camera>();
        camMove = GetComponent<CameraMovement>();
        snapCam.enabled = false;
        canvas = GameObject.Find("WorldCanvas").transform;
        snapCam.targetTexture = new RenderTexture(snapCam.pixelWidth, snapCam.pixelHeight, 24);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            distance = 20f;
            foreach (EnemyMovement em in FindObjectsOfType<EnemyMovement>())
            {
                if (GeometryUtility.TestPlanesAABB(camMove.planes, em.GetComponent<Collider>().bounds))
                {
                    if(Vector3.Distance(transform.position, em.transform.position) < distance)
                    {
                        distance = Vector3.Distance(transform.position, em.transform.position) - 3.5f;
                    }
                    em.GetComponentInChildren<Animator>().enabled = false;
                    Destroy(em);
                }
            }
            snapCam.enabled = true;
            GameObject image = Instantiate(newImagePref, transform.position + transform.forward * distance, transform.rotation);
            snapCam.Render();
            RenderTexture.active = snapCam.targetTexture;
            snapCam.targetTexture = new RenderTexture(snapCam.pixelWidth, snapCam.pixelHeight, 24);
            image.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, snapCam.pixelWidth / (40 + (distance*2.5f)));
            image.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, snapCam.pixelHeight / (40 + (distance*2.5f)));
            //image.GetComponent<FreezeImageInWorld>().ApplyImage(snapshot.EncodeToPNG());
            image.GetComponent<FreezeImageInWorld>().ApplyImage(RenderTexture.active);
            image.transform.SetParent(canvas);
            snapCam.enabled = false;
        }
    }
}
