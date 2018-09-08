using UnityEngine;

public class PlayerCollision : MonoBehaviour
{

    // TODO move collision logic for grounded and wall slide states from player movement to here

    [SerializeField] PlayerMovement movement;

    void OnCollisionEnter(Collision collisionInfo)
    {
        if(collisionInfo.collider.tag == "Obstacle")
        {
            movement.enabled = false;
        }
    }

}
