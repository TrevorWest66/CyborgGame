using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathShowScore : MonoBehaviour
{
	public Text scoreAtDeath;

	// Start is called before the first frame update
	void start()
	{
		/*
		float value = PlayerPrefs.GetFloat("HighScore", 0);
		Debug.Log("High Score: " + value);
		scoreAtDeath.text = value.ToString();
		*/
		scoreAtDeath.text = "fuck this";
		Debug.Log("this is broken");
	}

	void update ()
	{
		scoreAtDeath.text = "fuck this";
	}
}
