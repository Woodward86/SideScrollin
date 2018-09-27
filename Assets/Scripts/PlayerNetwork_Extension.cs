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
    [SerializeField] float respawnTime = 5f;

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


    public void Die()
    {
        DisablePlayer();

        Invoke("Respawn", respawnTime);
    }


    void Respawn()
    {
        if(isLocalPlayer)
        {
            Transform spawn = NetworkManager.singleton.GetStartPosition();
            transform.position = spawn.position;
            transform.rotation = spawn.rotation;
        }
        EnablePlayer();
    }
}
