using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    public Transform player;
    public Text scoreText;

    // TODO make score dependent on something other then distance
	// Update is called once per frame
	void Update ()
    {
        scoreText.text = player.position.x.ToString("0");
	}
}
