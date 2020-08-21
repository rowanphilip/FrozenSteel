using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private int move_speed;

    private Rigidbody rigid_body;
    private Vector3 first_clicked_location;
    private bool middle_click_down;

    private float heightY;

    public GameObject Camera;

	// Use this for initialization
	void Start ()
    {
        move_speed = 20;

        rigid_body = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        //forward
        if (Input.GetKey("w") || Input.GetKey("up"))
        {
            rigid_body.AddRelativeForce(new Vector3(0, move_speed, 20));
        }
        //left
        if (Input.GetKey("a") || Input.GetKey("left"))
        {
            rigid_body.AddRelativeForce(new Vector3(-move_speed, 0, 0));
        }
        //backward
        if (Input.GetKey("s") || Input.GetKey("down"))
        {
            rigid_body.AddRelativeForce(new Vector3(0, -move_speed, -20));
        }
        //right
        if (Input.GetKey("d") || Input.GetKey("right"))
        {
            rigid_body.AddRelativeForce(new Vector3(move_speed, 0, 0));
        }
        //rotate
        if (Input.GetKey("q"))
        {
            //rotate left
            Camera.transform.Rotate(0, -1, 1);
        }
        else if (Input.GetKey("e"))
        {
            //rotate right
            Camera.transform.Rotate(0, 1, -1);
        }
        //zoom
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            //if (this.transform.position.y > 1)
            //{
                //zoom in
                Camera.transform.position += new Vector3(0, -4, 0);
            //}
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            //if (this.transform.position.y < 64)
            //{
                //zoom out
                Camera.transform.position += new Vector3(0, 4, 0);
            //}
        }

        // Middle click to drag around
        if (Input.GetMouseButtonDown(2))
        {
            if (!middle_click_down)
            {
                first_clicked_location = Input.mousePosition;
                middle_click_down = true;
            }

            Vector3 current_mouse_position = Input.mousePosition;

            Vector3 rotate_vector = current_mouse_position - first_clicked_location;

            rigid_body.AddRelativeTorque(rotate_vector);
        }
        else
        {
            middle_click_down = false;
        }

    }
}
