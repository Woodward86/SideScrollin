using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoxCollider))]
public class PlayerHitbox : MonoBehaviour {

    float offset = 1f;
    BoxCollider col;

    void Awake()
    {
        WalkingController.OnFacingChange += RefreshFacing;
        col = GetComponent<BoxCollider>();
    }


    void RefreshFacing(FacingDirection fd)
    {
        //since the player model is getting localRotation to rotate this is always attached to the front
        //add more switch cases if there is need for facing up, down, toward screen or away from screen
        switch (fd)
        {
            default:
                col.center = Vector3.right * offset;
                Debug.Log(fd);
                break;
        }

    }

}
