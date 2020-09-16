using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazePlayerCollider : MonoBehaviour
{
    public GameObject GameController;
    public GameObject PlayerDestroyer;
    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Obstacle")
        {
            //gameover
            GameController.GetComponent<MazeGameController>().GameOver();
            GameObject go = Instantiate(PlayerDestroyer, gameObject.transform.position, gameObject.transform.rotation);
            foreach (Transform child in go.transform)
            {
                child.transform.parent = null;
                child.GetComponent<Rigidbody>().AddExplosionForce(200, new Vector3(0, 1, 0), 100);
            }
            go.GetComponent<Rigidbody>().AddExplosionForce(100, Vector3.up, 5);
            Destroy(go);
        }
    }
}
