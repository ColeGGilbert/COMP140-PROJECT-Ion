using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FreezeImageInWorld : MonoBehaviour
{

    RawImage thisImage;

    // Start is called before the first frame update
    void Start()
    {
        thisImage = GetComponent<RawImage>();
    }

    private void FixedUpdate()
    {
        // Fades the camera out over time
        thisImage.color -= new Color(0, 0, 0, .01f);
    }

    public void ApplyImage(Texture image)
    {
        // Assigns the snapshot image to the ui image
        GetComponent<RawImage>().texture = image;
    }
}
