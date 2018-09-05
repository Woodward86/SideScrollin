using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

    [SerializeField] Transform player;
    [SerializeField] Vector3 offset;

	// TODO create ease in and out function
	void Update ()
    {
        transform.position = player.position + offset;
	}


}
