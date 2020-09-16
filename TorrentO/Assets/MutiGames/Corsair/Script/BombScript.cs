using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    float speed = 200f;
    
    void Update()
    {
        transform.Translate(0, 0, 0.01f);
        //transform.position+=transform.position+new Vector3(0f,0f,0.0001f);
    }
}
