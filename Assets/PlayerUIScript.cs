using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIScript : MonoBehaviour
{
    public GameObject BuildScreen;
    public GameObject buildButton;

    public SelectionController selectControl;

    public Texture2D FactoryCursor;

    // Start is called before the first frame update
    void Start()
    {
        //To be implemented in second prototype
        /*
        BuildScreen.SetActive(false);
        buildButton.SetActive(true);
        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //To be implemented in second prototype
    /*
    public void openMenu()
    {
        buildButton.SetActive(false);
        BuildScreen.SetActive(true);
    }

    public void buildFactory()
    {
        Cursor.SetCursor(FactoryCursor, Vector2.zero, CursorMode.Auto);
    }
    */
}
