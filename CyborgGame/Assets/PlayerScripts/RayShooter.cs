using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Camera))]
[AddComponentMenu("Shooting Script/RayShooter")]
public class RayShooter : MonoBehaviour
{
	[SerializeField] private SettingsPopUp settingsPopUp;
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
		// enables or disables mouse lock and visibility also pauses game if mouse is visible
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			if (Cursor.visible == true)
			{
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
				Time.timeScale = 1f;
				settingsPopUp.Close();
			}
			else
			{
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
				Time.timeScale = 0f;
				settingsPopUp.Open();
			}
		}
		// enables shooing only if game isnt paused
		if (Time.timeScale != 0)
		{
			if ((Input.GetMouseButtonDown(0)) && (!EventSystem.current.IsPointerOverGameObject()))
			{
				// this creates a vector with the coridnates of the rays origin
				Vector3 point = new Vector3(_camera.pixelWidth / 2, _camera.pixelHeight / 2, 0);
				// creates ray object with origin cordinates
				Ray ray = _camera.ScreenPointToRay(point);
				// pases the ray and a new object with type RaycastHit
				if (Physics.Raycast(ray, out RaycastHit hit))
				{
					// this retrieves the object the ray hit
					GameObject hitObject = hit.transform.gameObject;
					ReactiveTarget target = hitObject.GetComponent<ReactiveTarget>();
					GameObject enemy = hit.transform.gameObject;
					WanderingAI lifeStatus = enemy.GetComponent<WanderingAI>();
					// this checks if the target is a person though the get component method and if it is
					// it returns a hit and if not it creates a sphere in the debug console.
					// get component returns null if the component isnt there.
					if ((target != null) && (lifeStatus.getAlive()))
					{
						target.ReactToHit();
						Messenger.Broadcast(GameEvent.ENEMY_HIT);
					}
					else
					{
						// starts the coroutine kicking control to it
						StartCoroutine(SphereIndicator(hit.point));
					}

				}
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
