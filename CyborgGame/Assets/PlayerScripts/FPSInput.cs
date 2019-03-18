using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// makes it so script requires char controller and then adds component to list in unity
[RequireComponent (typeof(CharacterController))]
[AddComponentMenu("Control Script/FPS Input")]
public class FPSInput : MonoBehaviour
{
	public float moveSpeed = 5.0f;
	public float gravity = -9.8f;
	// creats char controller object
	private CharacterController _charController;
	void Start()
    {
		// extends this to anything attached to the object in game
		_charController = GetComponent<CharacterController>();
    }

	void Update()
    {
		// get movement input
		float deltaX = Input.GetAxis("Horizontal") * moveSpeed;
		float deltaZ = Input.GetAxis("Vertical") * moveSpeed;
		// creates a vector for movement
		Vector3 movement = new Vector3(deltaX, 0, deltaZ);
		// this is done so that diagnol movement isnt done at increased speed
		movement = Vector3.ClampMagnitude(movement, moveSpeed);
		movement.y = gravity;

		// insures movement is frame rate independent
		movement *= Time.deltaTime;
		// this converts the vector which is in local space to global space
		// so that it can be passed to the move method in the line after
		movement = transform.TransformDirection(movement);
		_charController.Move(movement);
        
    } 
}
