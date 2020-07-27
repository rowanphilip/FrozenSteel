using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostScript : MonoBehaviour
{
    public void loadGame()
    {
        Debug.Log("Button Pressed");
        Application.LoadLevel("Game");
    }
}
