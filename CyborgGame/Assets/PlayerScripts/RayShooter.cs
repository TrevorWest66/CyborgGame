﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Camera))]
[AddComponentMenu("Shooting Script/RayShooter")]
public class RayShooter : MonoBehaviour
{
	// [SerializeField] private SettingsPopUp settingsPopUp;
	[SerializeField] private GameObject sphere;
	[SerializeField] private ParticleSystem muzzleFlash;
	[SerializeField] private AudioSource gunShot;
	private Camera _camera;

	void Start()
    {
		// this gets the camera component for later use
		_camera = GetComponent<Camera>();

		// locks and hides the cursor 

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		
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
				// settingsPopUp.Close();
			}
			else
			{
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
				Time.timeScale = 0f;
				// settingsPopUp.Open();
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
				muzzleFlash.Play();
				gunShot.Play();
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
						StartCoroutine(SphereIndicator(hit));
					}

				}
			}
		}
    }
	private IEnumerator SphereIndicator(RaycastHit targetHit)
	{
		// creates a sphere for the visual indicator 
		Vector3 sphereSize = new Vector3(.1f, .1f, .001f);
		sphere.transform.localScale = sphereSize;
		Instantiate(sphere, targetHit.point, Quaternion.FromToRotation(Vector3.forward, targetHit.normal));
		// sets the position of the sphere to the vector cordinates passed in
		// sphere.transform.position = pos;

		// this yield causes the program to pause for one second than continue 
		yield return new WaitForSeconds(1);

		Destroy(sphere);
	}
 }
