using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colorcollecterdestroyer : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {
        Destroy(col.gameObject);
    }
}
