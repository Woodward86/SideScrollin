using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

    [SerializeField] Transform player;
    [SerializeField] Vector3 offset;
    [SerializeField] float averageingBlend = .1f;

	// TODO make vertical camera movement and horizontal movement separate controls
    // TODO when player reaches level boarders the camera needs to blend more towards ending
	void Update ()
    {
        transform.position += (player.position + offset - transform.position) * averageingBlend * Time.deltaTime;
	}


}
