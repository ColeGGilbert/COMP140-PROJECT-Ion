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

    // Variables that control charge usage for the camera
    int pictureCharges = 3;
    int maxPictureCharges = 3;
    float pictureRechargeResetTimer = 2.25f;
    float pictureRechargeTimer;

    // Start is called before the first frame update
    void Start()
    {
        // Finds and assigns references for snapshot camera and camMove
        snapCam = GameObject.FindGameObjectWithTag("SnapCam").GetComponent<Camera>();
        camMove = GetComponent<CameraMovement>();
        snapCam.enabled = false;
        canvas = GameObject.Find("WorldCanvas").transform;
        snapCam.targetTexture = new RenderTexture(snapCam.pixelWidth, snapCam.pixelHeight, 24);
        pictureRechargeTimer = pictureRechargeResetTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            InitPicture();
        }

        ChargeCamera();
    }

    public void InitPicture()
    {
        // If the game hasn't ended...
        if (GameManager.gameRunning)
        {
            Debug.Log("Charges: " + pictureCharges);

            if (pictureCharges > 0)
            {
                // Looks for the nearest enemy in the bounds of the camera (using the camera planes in CameraMovement)
                // Once the nearest enemy is found, the image projection is created slightly in front of the enemy
                distance = 20f;
                foreach (EnemyMovement em in FindObjectsOfType<EnemyMovement>())
                {
                    if (GeometryUtility.TestPlanesAABB(camMove.planes, em.GetComponent<Collider>().bounds))
                    {
                        if (Vector3.Distance(transform.position, em.transform.position) < distance)
                        {
                            distance = Vector3.Distance(transform.position, em.transform.position) - 3.5f;
                        }
                        // Disables enemy movement and animation
                        em.GetComponentInChildren<Animator>().enabled = false;
                        Destroy(em);
                    }
                }
                if (distance <= 0)
                {
                    distance += 0.1f;
                }

                pictureCharges--;

                CaptureImage();
            }
        }
    }

    void CaptureImage()
    {
        // Enables snapshot camera
        snapCam.enabled = true;
        // Creates an image in the world space
        GameObject image = Instantiate(newImagePref, transform.position + transform.forward * distance, transform.rotation);

        // Forces the snapshot camera to render for accuracy
        snapCam.Render();

        // Assigns the current RenderTexture as the snapshot camera's targetTexture
        RenderTexture.active = snapCam.targetTexture;

        // creates a new RenderTexture attatched to the snapshot camera, equal to the width and height of the camera to simulate the camera bounds
        snapCam.targetTexture = new RenderTexture(snapCam.pixelWidth, snapCam.pixelHeight, 24);
        image.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, snapCam.pixelWidth / (40 + (distance * 2.5f)));
        image.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, snapCam.pixelHeight / (40 + (distance * 2.5f)));
        //image.GetComponent<FreezeImageInWorld>().ApplyImage(snapshot.EncodeToPNG());
        image.GetComponent<FreezeImageInWorld>().ApplyImage(RenderTexture.active);
        image.transform.SetParent(canvas);
        snapCam.enabled = false;
    }

    void ChargeCamera()
    {
        // If the current number of charges isn't at max, recharges the camera by reducing the remaining timer
        if (pictureCharges < maxPictureCharges)
        {
            pictureRechargeTimer -= Time.deltaTime;
            if(pictureRechargeTimer <= 0)
            {
                pictureCharges++;
                pictureRechargeTimer = pictureRechargeResetTimer;
            }
        }
    }
}
