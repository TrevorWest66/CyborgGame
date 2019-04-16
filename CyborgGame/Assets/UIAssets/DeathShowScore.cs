using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathShowScore : MonoBehaviour
{
	[SerializeField] private Text scoreAtDeath;

	// Start is called before the first frame update
	void Start()
	{
		float value = PlayerPrefs.GetFloat("PlayerScore", 0);
		scoreAtDeath.text = value.ToString();
	}

	public void OnSubmitName(string name)
	{
		PlayerPrefs.SetString("PlayerName", name);
	}

	public void returnToMainMenu()
	{
		SceneManager.LoadScene(0);
	}
}
