using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

    [SerializeField] Transform player;
    [SerializeField] Vector3 offset;
    [SerializeField] float averageingBlend = .1f;

	// TODO create ease in and out function
	void Update ()
    {
        transform.position += (player.position + offset - transform.position) * averageingBlend * Time.deltaTime;
	}


}
