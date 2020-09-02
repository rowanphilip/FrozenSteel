using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FactoryScript : MonoBehaviour
{
    public ObjectManager objectm;
    public GameObject tank;
    private Vector3 creationPos;
    public GameObject FUI;

    public Sprite noRecruit;
    public Sprite Recruit;

    public GameObject Selection;

    public Text Timer;

    private float CurrentTime;

    void Start()
    {
        this.transform.position = new Vector3(230, 1, 120);

        //To be used when selection is ready:
        makeInvisible();
        UnSelect();

        creationPos = GameObject.FindGameObjectWithTag("Spawn").transform.position;

        Selection.SetActive(false);

        CurrentTime = 0;
    }

    void Update()
    {
        if (CurrentTime > 0)
        {
            CurrentTime -= 1 * Time.deltaTime;
            FUI.transform.GetChild(1).GetComponent<Image>().sprite = noRecruit;
        }
        else
        {
            FUI.transform.GetChild(1).GetComponent<Image>().sprite = Recruit;
        }

        Timer.text = Mathf.RoundToInt(CurrentTime).ToString();
    }

    public void makeTank()
    { 
        if (Timer.text == "0")
        {
            GameObject new_tank = Instantiate(tank, creationPos, Quaternion.identity);
            objectm.RegisterNewTank(new_tank.transform.GetChild(0).gameObject.GetComponent<TankController>());
            CurrentTime = 20;
        }
    }

    public Vector3 GetPosition()
    {
        return this.transform.position;
    }

    public void makeVisible()
    {
        FUI.SetActive(true);
    }

    public void makeInvisible()
    {
        FUI.SetActive(false);
    }

    public void Select()
    {
        Selection.SetActive(true);
        makeVisible();
    }

    public void UnSelect()
    {
        Selection.SetActive(false);
        makeInvisible();
    }

    public IEnumerator Wait(int num)
    {
        yield return new WaitForSeconds(num);
    }
}
