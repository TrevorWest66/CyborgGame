using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathShowScore : MonoBehaviour
{
	[SerializeField] private Text scoreAtDeath;
	[SerializeField] private Text playerName;

	// Start is called before the first frame update
	void Start()
	{
		
	}

	public void OnSubmitName()
	{
		PlayerPrefs.SetString("PlayerName", playerName.text);
		float value = PlayerPrefs.GetFloat("PlayerScore", 0);
		scoreAtDeath.text = value.ToString();
	}

	public void returnToMainMenu()
	{
		SceneManager.LoadScene(0);
	}
}
