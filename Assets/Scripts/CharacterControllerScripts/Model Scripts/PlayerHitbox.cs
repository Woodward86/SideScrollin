using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoxCollider))]
public class PlayerHitbox : MonoBehaviour {

    //collider position
    float offset = 1f;
    BoxCollider col;

    //collider duration
    float duration;

    void Awake()
    {
        WalkingController.OnFacingChange += RefreshFacing;
        WalkingController.OnInteract += StartCollisionCheck;
        col = GetComponent<BoxCollider>();
        col.enabled = false;
        gameObject.layer = 9;
    }


    void Update()
    {
        if (col.enabled)
        {
            duration -= Time.deltaTime;
            if (duration <= 0.0f)
            {
                col.enabled = false;
            }
        }
    }


    void StartCollisionCheck(float dur)
    {
        col.enabled = true;
        duration = dur;
    }


    void RefreshFacing(FacingDirection fd)
    {
        //since the player model is getting localRotation to rotate, this is always attached to the front
        //add more switch cases if there is need for facing up, down, toward screen or away from screen
        switch (fd)
        {
            default:
                col.center = Vector3.right * offset;
                //Debug.Log(fd);
                break;
        }

    }


    void OnTriggerEnter(Collider col)
    {
        col.GetComponent<InteractObject> ().Interact();
    }

}
