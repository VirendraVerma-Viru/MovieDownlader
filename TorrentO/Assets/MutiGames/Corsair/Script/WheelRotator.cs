using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelRotator : MonoBehaviour
{
    public float speed = 100f;
    public GameController controllerScript;


    public GameObject dollorGO;
    public GameObject NormalBoat;
    float time = 0.1f;
    private bool moneyCreater = true;
    void Start()
    {
        moneyCreater = true;
    }
    // Update is called once per frame
    void Update()
    {
        

        if (gameObject.tag == "Money" && moneyCreater)
        {
            
            if (time < 0)
            {
                GameObject go = Instantiate(dollorGO, NormalBoat.transform.position, NormalBoat.transform.rotation);
                time = 0.1f;
            }
            if (gameObject.transform.eulerAngles.z <= 360 && gameObject.transform.eulerAngles.z > 350)
            {
                moneyCreater = false;
            }
            time -= Time.deltaTime;
            transform.Rotate(0f, 0f, speed * Time.deltaTime);
        }
        else
        {
            if (controllerScript.forward == true&&controllerScript.pause == false)
                transform.Rotate(0f, 0f, speed * Time.deltaTime);
            else if (controllerScript.pause == false)
                transform.Rotate(0f, 0f, -speed * Time.deltaTime);
        }
    }
}
