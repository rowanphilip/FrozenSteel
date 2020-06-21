using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float move_speed;

    private Rigidbody rigid_body;
    private Vector3 first_clicked_location;
    private bool middle_click_down;

	// Use this for initialization
	void Start ()
    {
        rigid_body = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update ()
    {
		if (Input.GetKey("w") || Input.GetKey("up"))
        {
            rigid_body.AddRelativeForce(new Vector3(0, 0, move_speed));
        }
        if (Input.GetKey("a") || Input.GetKey("left"))
        {
            rigid_body.AddRelativeForce(new Vector3(-move_speed, 0, 0));
        }
        if (Input.GetKey("s") || Input.GetKey("down"))
        {
            rigid_body.AddRelativeForce(new Vector3(0, 0, -move_speed));
        }
        if (Input.GetKey("d") || Input.GetKey("right"))
        {
            rigid_body.AddRelativeForce(new Vector3(move_speed, 0, 0));
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
