using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
	// serialized var for linking to prefab object 
	// this makes it so other scripts cant effect it but you can set it in unity
	[SerializeField] private GameObject enemyPrefab;
	// private var to keep track of instance of enemy in scene
	private GameObject _enemy;
	float spawnTime = 2.0f;
	float timer = 0;
	int numEnemies = 0;
	int maxEnemies = 3;
   
    void Update()
    {
		timer += Time.deltaTime;
		// if there is no enemy creats a new one
		if ((timer >= spawnTime) && (numEnemies < maxEnemies)) 
		{
			// iniates a new isntance of enemy
			_enemy = Instantiate(enemyPrefab) as GameObject;
			int randomXPoint = Random.Range(-110, -90);
			int randomZPoint = Random.Range(-110, -90);
			_enemy.transform.position = new Vector3(randomXPoint, 1, randomZPoint);
			float angle = Random.Range(0, 360);
			_enemy.transform.Rotate(0, angle, 0);
			numEnemies += 1;
			timer = 0;
		}
    }
	public void decreaseEnemyCount()
	{
		numEnemies -= 1;
	}
}
