using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuFactory : MonoBehaviour
{
    public ObjectManager objectm;
    public GameObject tank;
    private Vector3 creationPos;

    private float TimeOfLastProduction;
    private float RateOfProduction;

    private int tankNum;

    void Start()
    {
        creationPos = GameObject.FindGameObjectWithTag("Spawn").transform.position;

        RateOfProduction = 1;
        TimeOfLastProduction = Time.time;
    }

    void Update()
    {
        makeTank();

        if (tankNum > 100)
        {
            foreach (TankController t in objectm.GetAllTanks())
            {
                Destroy(t);
            }
        }
    }

    public void makeTank()
    {
        float current_time_seconds = Time.time;

        if (current_time_seconds - TimeOfLastProduction > (1.0f / RateOfProduction))
        { 
            StartCoroutine(Wait(1));
            GameObject new_tank = Instantiate(tank, creationPos, Quaternion.identity);
            objectm.RegisterNewTank(new_tank.transform.GetChild(0).gameObject.GetComponent<TankController>());
            TimeOfLastProduction = Time.time;
            tankNum++;
        }
    }

    public Vector3 GetPosition()
    {
        return this.transform.position;
    }

    public IEnumerator Wait(int time)
    {
        yield return new WaitForSeconds(time);
    }
}
