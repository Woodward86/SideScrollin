using UnityEngine;


[RequireComponent(typeof(Transform))]
public class FollowPlayer : MonoBehaviour
{

    [SerializeField] Transform player;
    [SerializeField] Vector3 offset;
    [SerializeField] float verticleBlend = 10f;
    [SerializeField] float horizontalBlend = 1.75f;
    [SerializeField] float depthBlend = 0f;


    // TODO when player reaches level boarders the camera needs to blend more towards ending
    void Update ()
    {
        float horizontalPosition = (player.position.x + offset.x - transform.position.x) * horizontalBlend;
        float verticalPosition = (player.position.y + offset.y - transform.position.y) * verticleBlend;
        float depthPosition = (player.position.z + offset.z - transform.position.z) * depthBlend;
        transform.position += new Vector3(horizontalPosition, verticalPosition, depthPosition) * Time.deltaTime;
	}


}
