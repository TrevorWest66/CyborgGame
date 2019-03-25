using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingAI : MonoBehaviour
{
	public float speed = 3.0f;
	public float obstacleRange = 0.5f;
	private bool _alive;
	// create a prefab game object and then a var for the instance of the object
	[SerializeField] private GameObject fireballPrefab;
	private GameObject _fireBall;
	private Vector3 wayPointPos;
	private GameObject wayPoint;

	void Start()
	{
		_alive = true;
		wayPoint = GameObject.Find("Player");
	}

    void Update()
    {
		// only runs if object is alive
		if (_alive)
		{
			// need to stop enemies from floating here
			wayPointPos = new Vector3(wayPoint.transform.position.x, wayPoint.transform.position.y,
				wayPoint.transform.position.z);
			// sets the objects move speed
			// transform.Translate(0, 0, speed * Time.deltaTime);
			transform.position = Vector3.MoveTowards(transform.position, wayPointPos, speed * Time.deltaTime);

			transform.LookAt(wayPointPos);

			// makes the ray
			Ray ray = new Ray(transform.position, transform.forward);
			RaycastHit hit;
			// checks to see if the ray hit something
			if (Physics.SphereCast(ray, 0.75f, out hit))
			{
				// checks if the ray hit the player and if so and there is not a fireball then fires it.
				GameObject hitObject = hit.transform.gameObject;
				if (hitObject.GetComponent<PlayerChar>())
				{
					if (_fireBall == null)
					{
						// sends the fireball forward after making it and causes it to spin
						_fireBall = Instantiate(fireballPrefab) as GameObject;
						// this places firball in fornt of char and  sends it forward 
						_fireBall.transform.position = transform.TransformPoint(Vector3.forward * 1.5f);
						_fireBall.transform.rotation = transform.rotation;
					}
				}
				// checks the distance to the object
				// need to figure out here how to avoid them getting stuck
				/*else if (hit.distance < obstacleRange)
				{
					// gets a random angle and then turns that much and continues moving
					float angle = Random.Range(-110, 110);
					transform.Rotate(0, angle, 0);
				}
				*/
			}
		}
    }

	// creats a setter for alive.
	public void setAlive(bool alive)
	{
		_alive = alive;
	}
	public bool getAlive()
	{
		return _alive;
	}
}
