using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Vector3 GetPosition()
    {
        return this.transform.position;
    }

    /*
    void OnCollisionEnter(Collision item)
    {
        if (item.gameObject.tag == "Projectile")
        {
            gameObject.GetComponent<MeshRenderer>().material = DullMetal;
        }
    }
    */
}
