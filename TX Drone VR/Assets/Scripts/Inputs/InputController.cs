using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

    void Update()
    {
        float x = Input.GetAxis("Horizontal");

        Debug.Log("x : " + x);


    }
}
