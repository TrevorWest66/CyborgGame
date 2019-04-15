using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerChar : MonoBehaviour
{
	private int _maxHealth { get; set; }
	private float _currentHealth { get; set; }
	public Slider healthBar;
	private bool alive;

	void Awake()
	{
		Messenger<float>.AddListener(GameEvent.SEND_SCORE, setFinalScore);
	}

	void OnDestroy()
	{
		Messenger<float>.RemoveListener(GameEvent.SEND_SCORE, setFinalScore);
	}

	void Start()
    {
		_maxHealth = 5;
		_currentHealth = _maxHealth;

		healthBar.value = calculateHealth();
		alive = true;
    }

    // Update is called once per frame
    public void Hurt(int damage)
	{
		if (alive)
		{
			_currentHealth -= damage;
			healthBar.value = calculateHealth();
			Debug.Log(calculateHealth());
			Debug.Log("Health: " + _currentHealth + " max Health: " + _maxHealth);
			if (_currentHealth <= 0)
			{
				dead();
			}
		}
	}

	public float calculateHealth ()
	{
		return (_currentHealth / _maxHealth);
	}

	// kills charecter
	public void dead ()
	{
		Messenger.Broadcast(GameEvent.PLAYER_DIED);
		SceneManager.LoadScene(2);
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		alive = false;
	}

	public void setFinalScore (float score)
	{
		PlayerPrefs.SetFloat("HighScore", score);
	}
}
