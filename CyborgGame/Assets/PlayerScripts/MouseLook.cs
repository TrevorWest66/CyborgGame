using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
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
	public float horizonSens = 5.0f;

	public float vertSens = 5.0f;
	public float minVert = -45.0f;
	public float maxVert = 45.0f;
	// sets the baseline angle for the camera
	private float _rotationX = 0;

    void Update()
    {
		if (axes == RotationAxes.MouseX)
		{
			transform.Rotate(0, Input.GetAxis("Mouse X") * horizonSens, 0);
		}
		else if (axes == RotationAxes.MouseY)
		{
			// this increments the roation with mouse movement
			// it is -= because the camera needs to be inverted to feel normal
			_rotationX -= Input.GetAxis("Mouse Y") * vertSens;
			// keeps the camera from going too high or too low
			_rotationX = Mathf.Clamp(_rotationX, minVert, maxVert);

			// this takes the gathered data and applies it, done automatically in the rotation method
			float rotationY = transform.localEulerAngles.y;
			transform.localEulerAngles = new Vector3(_rotationX, rotationY, 0);
		}
		else
		{
			// same as above handles the verticle rotation
			_rotationX -= Input.GetAxis("Mouse Y") * vertSens;
			_rotationX = Mathf.Clamp(_rotationX, minVert, maxVert);

			// handles the horixontal rotation getting input then incrementing
			float delta = Input.GetAxis("Mouse X") * horizonSens;
			float rotationY = transform.localEulerAngles.y + delta;

			transform.localEulerAngles = new Vector3(_rotationX, rotationY, 0);
		}
        
    }
}
