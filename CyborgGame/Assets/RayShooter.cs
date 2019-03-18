using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[AddComponentMenu("Shooting Script/RayShooter")]
public class RayShooter : MonoBehaviour
{
	private Camera _camera;

    void Start()
    {
		// this gets the camera component for later use
		_camera = GetComponent<Camera>();

		// locks and hides the cursor 
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
    }

	void OnGUI()
	{
		int size = 12;
		// creates label point in center of screen shifted slightly becasue of label this is needed
		float posX = (_camera.pixelWidth / 2) - (size / 4);
		float posY = (_camera.pixelHeight / 2) - (size / 2);
		GUI.Label(new Rect(posX, posY, size, size), "*");
	}

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
		{
			// this creates a vector with the coridnates of the rays origin
			Vector3 point = new Vector3(_camera.pixelWidth / 2, _camera.pixelHeight / 2, 0);
			// creates ray object with origin cordinates
			Ray ray = _camera.ScreenPointToRay(point);
			// pases the ray and a new object with type RaycastHit
			if (Physics.Raycast(ray, out RaycastHit hit))
			{
				// starts the coroutine kicking control to it
				StartCoroutine(SphereIndicator(hit.point));
			}
		}
    }
	private IEnumerator SphereIndicator(Vector3 pos)
	{
		// creates a sphere for the visual indicator 
		GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		// sets the position of the sphere to the vector cordinates passed in
		sphere.transform.position = pos;

		// this yield causes the program to pause for one second than continue 
		yield return new WaitForSeconds(1);

		Destroy(sphere);
	}
 }
