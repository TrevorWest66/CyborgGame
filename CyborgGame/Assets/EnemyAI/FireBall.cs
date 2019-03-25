using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
	public float speed = 10.0f;
	public int damage = 1;

	void Update()
    {
		// moves the ball forward. 
		transform.Translate(0, 0, speed * Time.deltaTime);
    }

	// this is called when the object collides with another
	// need to check is trigger in collider on fireball for this to work 
	// also need a rigid body component
	void OnTriggerEnter(Collider other)
	{
		PlayerChar player = other.GetComponent<PlayerChar>();
		// check if the object is the player
		if (player != null)
		{
			player.Hurt(damage);
		}
		// destory the fireball
		Destroy(this.gameObject);
	}
}
