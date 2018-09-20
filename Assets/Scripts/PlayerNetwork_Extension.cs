using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;


[System.Serializable]
public class ToggleEvenet : UnityEvent<bool> { }

public class PlayerNetwork_Extension : NetworkBehaviour
{
    [SerializeField] ToggleEvenet onToggleShared;
    [SerializeField] ToggleEvenet onToggleLocal;
    [SerializeField] ToggleEvenet onToggleRemote;

    void Start()
    {
        EnablePlayer();
    }


    void DisablePlayer()
    {
        onToggleShared.Invoke(false);

        if (isLocalPlayer)
        {
            onToggleLocal.Invoke(false);
        }
        else
        {
            onToggleRemote.Invoke(false);
        }
    }


    void EnablePlayer()
    {
        onToggleShared.Invoke(true);

        if (isLocalPlayer)
        {
            onToggleLocal.Invoke(true);
        }
        else
        {
            onToggleRemote.Invoke(true);
        }
    }
}
