using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colorchangerplayercollider : MonoBehaviour
{
    public GameObject GameController;
    public GameObject PlayerDestroyer;
    public int colorCode = 0;

    public Material[] colors;
    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Obstacle")
        {
            if (col.gameObject.GetComponent<ColorCollectorBallDetail>().colorCode == colorCode)
            {
                //add score
                GameController.GetComponent<ColorCollectorGameController>().OnAddScore();
                Destroy(col.gameObject);
            }
            else
            {
                //gameover
                GameController.GetComponent<ColorCollectorGameController>().OnGameOver();

                GameObject go = Instantiate(PlayerDestroyer,gameObject.transform.position,gameObject.transform.rotation);
                foreach (Transform child in go.transform)
                {
                    child.transform.GetComponent<Renderer>().material = colors[colorCode];
                    child.transform.parent = null;
                    child.GetComponent<Rigidbody>().AddExplosionForce(200, new Vector3(0,1,0), 100);
                }
                go.GetComponent<Rigidbody>().AddExplosionForce(100, Vector3.up, 5);
                Destroy(go);
                Destroy(gameObject);
            }
        }
        else if (col.tag == "ColorChanger")
        {
            colorCode = col.gameObject.GetComponent<ColorCollectorBallDetail>().colorCode;
            //chnage color i have code
            GameController.GetComponent<ColorCollectorGameController>().OnChangeColor(colorCode);
        }
    }
}
