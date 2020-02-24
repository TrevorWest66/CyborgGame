using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSound : MonoBehaviour
{
	[SerializeField] private AudioSource backgroundMusic;
	[SerializeField] private AudioSource voice1;
	[SerializeField] private AudioSource voice2;
	[SerializeField] private AudioSource voice3;
	[SerializeField] private AudioSource voice4;
	[SerializeField] private AudioSource voice5;
	[SerializeField] private AudioSource voice6;
	[SerializeField] private AudioSource voice7;
	[SerializeField] private AudioSource voice8;
	private float _timer;
	private float _speakTimer = 10.0f;
	// Start is called before the first frame update
	void Start()
	{
		backgroundMusic.Play();
	}

	// Update is called once per frame
	void Update()
	{
		_timer += Time.deltaTime;
		if (_timer > _speakTimer)
		{
			int randomNumber = Random.Range(1, 8);
			if (randomNumber == 1)
			{
				voice1.Play();
			}
			else if (randomNumber == 2)
			{
				voice2.Play();
			}
			else if (randomNumber == 3)
			{
				voice3.Play();
			}
			else if (randomNumber == 4)
			{
				voice4.Play();
			}
			else if (randomNumber == 5)
			{
				voice5.Play();
			}
			else if (randomNumber == 6)
			{
				voice6.Play();
			}
			else if (randomNumber == 7)
			{
				voice7.Play();
			}
			else if (randomNumber == 8)
			{
				voice8.Play();
			}

			_timer = 0;
		}
	}
}