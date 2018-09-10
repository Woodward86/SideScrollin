using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacingController : MonoBehaviour
{

    void Awake()
    {
        WalkingController.OnFacingChange += RefreshFacing;
    }


    void RefreshFacing(FacingDirection fd)
    {
        switch (fd)
        {
            case FacingDirection.Left:
                transform.localRotation = Quaternion.Euler(0.0f, 180f, 0f);
                break;

            default:
                transform.localRotation = Quaternion.Euler(0.0f, 0f, 0f);
                break;
        }

    }

}
