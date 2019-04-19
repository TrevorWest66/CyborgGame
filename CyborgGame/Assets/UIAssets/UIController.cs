using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIController : MonoBehaviour
{
	[SerializeField] private Text scoreLabel;
	[SerializeField] private SettingsPopUp settingsPopUp;
   

	private int _score;

	void Awake()
	{
		// declares which method responds to enemy hit
		Messenger.AddListener(GameEvent.ENEMY_HIT, OnEnemyHit);
		Messenger.AddListener(GameEvent.PLAYER_DIED, OnPlayerDeath);
	}

	void OnDestroy()
	{
		// when object is destroyed removes listener to avoid errors 
		Messenger.RemoveListener(GameEvent.ENEMY_HIT, OnEnemyHit);
		Messenger.RemoveListener(GameEvent.PLAYER_DIED, OnPlayerDeath);
	}

	void Start()
	{
		// set score to zero and then sets it to the score label
		_score = 0;
		scoreLabel.text = _score.ToString();

		settingsPopUp.Close();
	}

	private void OnEnemyHit()
	{



		// increments score when enemy is hit
		_score += 1;
		scoreLabel.text = _score.ToString();
	}

	public void OnOpenSettings()
	{
		settingsPopUp.Open();
	}

	public void OnPlayerDeath()
	{
		Messenger<float>.Broadcast(GameEvent.SEND_SCORE, _score);
	}
}
