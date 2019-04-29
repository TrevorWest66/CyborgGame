using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactiveTarget : MonoBehaviour
{
	// creates the method that is called in the shoot class
	public void ReactToHit()
	{
		// checks if target has wandering ai script and if so sets its status to not alive
		WanderingAI behavoir = GetComponent<WanderingAI>();

		if (behavoir != null)
		{
			behavoir.setAlive(false);
			behavoir.turnOffNavMesh();
		}
		StartCoroutine(Die());
		
	}

	private IEnumerator Die()
	{
		// this is instant want to look into tweens later to make it fall slowly. 
		// tips over objecct waits 1.5 seconds then destroys it. 
		// the yield is what tells the coroutine to pause. 

		// creates a var for the x rotation

		this.transform.Rotate(-75, 0, 0);
		yield return new WaitForSeconds(1.5f);

		Destroy(this.gameObject);
		GameObject spawningThing = GameObject.Find("ControllerTestCenter");
		SceneController spawner = spawningThing.GetComponent<SceneController>();
		spawner.decreaseEnemyCount();
	}
}
