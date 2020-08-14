using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    private int health;

    public GameObject EnemyHealth;

    public ParticleSystem explosion;

    public GameObject Camera;

    // Use this for initialization
    void Start ()
    {
        health = 10;
        explosion.Stop();
    }
	
	// Update is called once per frame
	void Update ()
    {
        EnemyHealth.transform.LookAt(Camera.transform);
    }

    public Vector3 GetPosition()
    {
        return this.transform.position;
    }

    void OnCollisionEnter(Collision item)
    {
        if (item.gameObject.tag == "Projectile")
        {
            if (health == 1)
            {
                EnemyHealth.transform.localScale += new Vector3(0, -10, 0);
                explosion.Play();
                EnemyHealth.SetActive(false);
            }
            else
            {
                health--;
                EnemyHealth.transform.localScale += new Vector3(0, -10, 0);
            }
        }
    }

    public IEnumerator Wait(int seconds)
    {
        yield return new WaitForSeconds(seconds);
    }
}
