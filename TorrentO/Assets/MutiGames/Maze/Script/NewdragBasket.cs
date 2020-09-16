using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewdragBasket : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        
          Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
          //print(touchPosition);
          touchPosition.z = 0f;
          
          gameObject.transform.position = touchPosition;
    }
}
