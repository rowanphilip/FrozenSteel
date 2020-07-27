using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{  
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision item)
    {
        Destroy(gameObject);
    }
}
