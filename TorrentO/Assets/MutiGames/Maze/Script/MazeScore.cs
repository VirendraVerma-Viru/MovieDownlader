using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeScore : MonoBehaviour
{
    public GameObject GameController;
    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Obstacle")
        {
            GameController.GetComponent<MazeGameController>().AddScore();
            Destroy(col.gameObject);
        }
    }
}
