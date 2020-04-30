using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton : MonoBehaviour
{
    // Closes the game when the button is pushed
    public void OnMouseDown()
    {
        Application.Quit();
    }
}
