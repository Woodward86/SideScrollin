using UnityEngine;
using UnityEngine.Networking;

public class FacingController : NetworkBehaviour
{

    public void Start()
    {
        WalkingController.OnFacingChange += RefreshFacing;
    }


    public void RefreshFacing(FacingDirection fd)
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
