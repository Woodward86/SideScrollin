using UnityEngine;
using UnityEngine.Networking;

public class NetworkManager_CameraControl : NetworkManager {

    [Header("Scene Camera Properties")]
    [SerializeField] Transform sceneCamera;
    [SerializeField] Transform playerCamera;

    public override void OnStartClient(NetworkClient client)
    {
        Debug.Log("Client Started");
    }


    public override void OnStartHost()
    {
        Debug.Log("Host Started");
    }


    public override void OnStopClient()
    {
        Debug.Log("Client Stopped");
    }


    public override void OnStopHost()
    {
        Debug.Log("Host Stopped");
    }

}
