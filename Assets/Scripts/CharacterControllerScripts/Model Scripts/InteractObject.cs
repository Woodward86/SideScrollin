using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractObject : MonoBehaviour {

    Material mat;
    // Use this for initialization
    void Start()
    {
        gameObject.layer = 10;
        mat = GetComponent<MeshRenderer>().material;
    }


    public void Interact()
    {
        mat.color = Color.green;
    }
}
