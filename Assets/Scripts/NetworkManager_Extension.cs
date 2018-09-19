using UnityEngine;
using UnityEngine.Networking;

public class NetworkManager_Extension : NetworkManager {

    [Header("Scene Camera Properties")]
    protected Camera sc;
    protected Camera pc;

    //TODO: need to move the camera stuff onto a player based command not client

    public override void OnStartClient(NetworkClient client)
    {
        Debug.Log("Client Started");

        //set camera
        pc = GameObject.FindGameObjectWithTag("PlayerCamera").GetComponent<Camera>() as Camera;
        sc = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>() as Camera;
        sc.enabled = false;
        pc.enabled = true;
    }


    public override void OnStartHost()
    {
        Debug.Log("Host Started");
    }


    public override void OnStopClient()
    {
        Debug.Log("Client Stopped");

        //set camera
        pc.enabled = false;
        sc.enabled = true;
    }


    public override void OnStopHost()
    {
        Debug.Log("Host Stopped");
    }

}
