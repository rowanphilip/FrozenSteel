using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecruitScript : MonoBehaviour
{
    public GameObject factory;

    public void click()
    {
        factory.GetComponent<FactoryScript>().makeTank();
    }
}
