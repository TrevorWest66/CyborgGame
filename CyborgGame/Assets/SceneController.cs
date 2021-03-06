﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
	// serialized var for linking to prefab object 
	// this makes it so other scripts cant effect it but you can set it in unity
	[SerializeField] private GameObject enemyPrefab;
	// private var to keep track of instance of enemy in scene
	private GameObject _enemy;
	private Vector3 playerPosition;
	public int charRotation = 0;
	float spawnTime = 1.0f;
	float timer = 0;
	float gameTime = 0;
	int numEnemies = 0;
	float maxEnemies = 7.0f;
	public float enemySpeed = 3.0f;

	void Update()
    {
		gameTime += Time.deltaTime;
		timer += Time.deltaTime;
		// if there is no enemy creats a new one
		if ((timer >= spawnTime) && (numEnemies < maxEnemies)) 
		{
			// iniates a new isntance of enemy
			int randomXPoint = Random.Range(-120, -80);
			int randomZPoint = Random.Range(-120, -80);
			playerPosition = GameObject.Find("Player").transform.position;
			double xDistanceFromPlayer = Mathf.Abs(playerPosition.x - randomXPoint);
			double zDistanceFromPlayer = Mathf.Abs(playerPosition.z - randomZPoint);
			if ((xDistanceFromPlayer > 10)&& (zDistanceFromPlayer > 10))
			{
				_enemy = Instantiate(enemyPrefab) as GameObject;
				_enemy.transform.position = new Vector3(randomXPoint, 0, randomZPoint);
				float angle = Random.Range(0, 360);
				_enemy.transform.Rotate(charRotation, angle, 0);
				numEnemies += 1;
				timer = 0;
			}
		}
		maxEnemies = (gameTime / 4) + 7;
		if (gameTime >= 20)
		{
			spawnTime = .5f;
		}
    }

	public void decreaseEnemyCount()
	{
		numEnemies -= 1;
	}

}
