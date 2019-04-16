using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
	void Awake()
	{
		Messenger<float>.AddListener(GameEvent.SPEED_CHANGED, OnSpeedChanged);
	}

	void OnDestroy()
	{
		Messenger<float>.RemoveListener(GameEvent.SPEED_CHANGED, OnSpeedChanged);
	}

	void Start()
	{
		// this makes the camera unaffected by physics
		Rigidbody body = GetComponent<Rigidbody>();
		if (body != null)
		{
			body.freezeRotation = true;
		}
	}
    public enum RotationAxes
	{
		MouseXandY = 0,
		MouseX = 1,
		MouseY = 2,
	}
	public RotationAxes axes = RotationAxes.MouseXandY;
	public float sensitivityX = 5.0f;
	public float sensitivityY = 5.0f;
	public float minVert = -45.0f;
	public float maxVert = 45.0f;
	// sets the baseline angle for the camera
	private float _rotationX = 0;

    void Update()
    {
		// enables rotation only if game isnt paused
		if (Time.timeScale != 0)
		{
			if (axes == RotationAxes.MouseX)
			{
				transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
			}
			else if (axes == RotationAxes.MouseY)
			{
				// this increments the roation with mouse movement
				// it is -= because the camera needs to be inverted to feel normal
				_rotationX -= Input.GetAxis("Mouse Y") * sensitivityY;
				// keeps the camera from going too high or too low
				_rotationX = Mathf.Clamp(_rotationX, minVert, maxVert);

				// this takes the gathered data and applies it, done automatically in the rotation method
				float rotationY = transform.localEulerAngles.y;
				transform.localEulerAngles = new Vector3(_rotationX, rotationY, 0);
			}
			else
			{
				// same as above handles the verticle rotation
				_rotationX -= Input.GetAxis("Mouse Y") * sensitivityY;
				_rotationX = Mathf.Clamp(_rotationX, minVert, maxVert);

				// handles the horixontal rotation getting input then incrementing
				float delta = Input.GetAxis("Mouse X") * sensitivityX;
				float rotationY = transform.localEulerAngles.y + delta;

				transform.localEulerAngles = new Vector3(_rotationX, rotationY, 0);
			}
		}
    }

	public void OnSpeedChanged(float value)
	{
		sensitivityX = value;
		sensitivityY = value;
		Debug.Log("speed changed: " + value);
	}
}
