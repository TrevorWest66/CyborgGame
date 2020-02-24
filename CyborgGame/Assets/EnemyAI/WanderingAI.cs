using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WanderingAI : MonoBehaviour
{
	public float speed = 5.0f;

	public float _fireballCastTimer = 2;
	private float _timer = 0;

	public float obstacleRange = 0.3f;
	private bool _alive;
	// create a prefab game object and then a var for the instance of the object
	[SerializeField] private GameObject fireballPrefab;
	private GameObject _fireBall;
	private Vector3 wayPointPos;
	private GameObject wayPoint;
	NavMeshAgent enemyChar;
	private float DistanceFromPlayerX;
	private float DistanceFromPlayerZ;
	
	void Start()
	{
		_alive = true;
		wayPoint = GameObject.Find("Player");
		enemyChar = GetComponent<NavMeshAgent>();
	}

    void Update()
    {
		DistanceFromPlayerX = Mathf.Abs(transform.position.x - wayPoint.transform.position.x);
		DistanceFromPlayerZ = Mathf.Abs(transform.position.z - wayPoint.transform.position.z);
		// only runs if object is alive
		if (_alive)
		{
			// need to stop enemies from floating here
			wayPointPos = new Vector3(wayPoint.transform.position.x, wayPoint.transform.position.y,
				wayPoint.transform.position.z);
			// sets the objects move speed
			// transform.Translate(0, 0, speed * Time.deltaTime);
			if ((Mathf.Abs(DistanceFromPlayerX) > 1) || (Mathf.Abs(DistanceFromPlayerZ) > 1))
			{
				enemyChar.SetDestination(wayPointPos);
				GetComponent<NavMeshAgent>().speed = speed;

				transform.LookAt(wayPointPos);
			}
			// this is where the attack will go
			else
			{
				enemyChar.SetDestination(transform.position);
			}


			// makes the ray
			Ray ray = new Ray(transform.position, transform.forward);
			RaycastHit hit;
			_timer += Time.deltaTime;
			// checks to see if the ray hit something
			if (Physics.SphereCast(ray, 0.75f, out hit))
			{
				// checks if the ray hit the player and if so and there is not a fireball then fires it.
				GameObject hitObject = hit.transform.gameObject;
				if (hitObject.GetComponent<PlayerChar>())
				{
					if (_timer >= _fireballCastTimer)
					{
						// sends the fireball forward after making it and causes it to spin
						_fireBall = Instantiate(fireballPrefab) as GameObject;
						// this places firball in fornt of char and  sends it forward 
						Vector3 _fireBallPositon = new Vector3(0, 1.5f, 2);
						// _fireBall.transform.position = transform.TransformPoint(Vector3.forward * 1.5f);
						_fireBall.transform.position = transform.TransformPoint(_fireBallPositon);
						_fireBall.transform.rotation = transform.rotation;
						_timer = 0;
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
	public void turnOffNavMesh()
	{
		// this gets called by RayShoot right after set alive and it turns off the nav mesh agent
		// and adds a rigid body thus allowing the enemy to fall over dead.
		enemyChar.GetComponent<NavMeshAgent>().enabled = false;
		Rigidbody body = gameObject.AddComponent<Rigidbody>() as Rigidbody;
	}
	public bool getAlive()
	{
		return _alive;
	}
}
