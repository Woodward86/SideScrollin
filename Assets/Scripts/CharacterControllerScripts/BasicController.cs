using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicController : MonoBehaviour
{

    public float speed = 6f;

    void Update ()
    {

        //if "right" button is pressed move right
        //ELSE if "left" button is pressed move left
		if(Input.GetKey(KeyCode.D))
        {
            transform.position += Vector3.right * Time.deltaTime * speed;
        }
        else if(Input.GetKey(KeyCode.A))
        {
            transform.position += Vector3.left * Time.deltaTime * speed;
        }
        
	}
}
