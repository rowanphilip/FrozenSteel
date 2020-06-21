using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour {

    private int collision_count;

    CollisionDetection()
    {
        this.collision_count = 0;
    }

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void OnCollisionEnter(Collision collision)
    {
        this.collision_count++;
    }

    void OnCollisionExit(Collision collision)
    {
        this.collision_count--;
    }

    public bool IsColliding()
    {
        return this.collision_count != 0;
    }
}
