using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButton : MonoBehaviour
{
    public Canvas menuCanvas;
    public GameObject mainCam;
    Camera menuCam;
    bool rotateCam;

    private void Start()
    {
        menuCam = GetComponent<Camera>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !GameManager.paused)
        {
            GetComponent<Animator>().SetTrigger("Switch");
            rotateCam = !rotateCam;
        }

        CheckRotation();
    }

    void CheckRotation()
    {
        if (menuCam.transform.rotation.x > 0 && rotateCam)
        {
            menuCam.transform.rotation = Quaternion.RotateTowards(menuCam.transform.rotation, Quaternion.Euler(0, mainCam.transform.rotation.y, mainCam.transform.rotation.z), 90f * Time.deltaTime);
        }
        else if (menuCam.transform.rotation.x < 90 && !rotateCam)
        {
            GameManager.paused = true;
            menuCam.transform.rotation = Quaternion.RotateTowards(menuCam.transform.rotation, Quaternion.Euler(90, 0, 0), 90f * Time.deltaTime);
        }
        else if (rotateCam && menuCam.transform.rotation.x < 1 && GameManager.paused)
        {
            GameManager.paused = false;
        }
        else if (!rotateCam && menuCam.transform.rotation.x > 1 && !GameManager.paused)
        {
            GameManager.paused = true;
        }
    }

    // Starts the game and moves the camera when button pushed
    public void OnMouseDown()
    {
        GetComponent<Animator>().SetTrigger("Switch");
        rotateCam = !rotateCam;
    }

    // CanvasSwitch runs on an animation event and enables/disables the menu canvas
    public void CanvasSwitch()
    {
        menuCanvas.enabled = !menuCanvas.enabled;
    }

    // CameraSwitch runs on an animation event and switches between menu and main cameras
    public void CameraSwitch()
    {
        mainCam.SetActive(!mainCam.activeSelf);
        menuCam.enabled = !menuCam.enabled;
    }
}
