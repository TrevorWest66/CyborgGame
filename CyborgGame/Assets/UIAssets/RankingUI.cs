using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingUI : MonoBehaviour
{
	[SerializeField] private Text Player1Name;
	[SerializeField] private Text Player2Name;
	[SerializeField] private Text Player3Name;
	[SerializeField] private Text Player4Name;
	[SerializeField] private Text Player5Name;

	[SerializeField] private Text Player1Score;
	[SerializeField] private Text Player2Score;
	[SerializeField] private Text Player3Score;
	[SerializeField] private Text Player4Score;
	[SerializeField] private Text Player5Score;

	void Start()
    {
		// initialize the scoreboard values
		setScoreBoard();

		// create vars for player name and score
		string _playerName = PlayerPrefs.GetString("PlayerName");
		float _playerScore = PlayerPrefs.GetFloat("PlayerScore");

		// checks if its the high score if so sets it
		if (float.Parse(Player1Score.text) < _playerScore)
		{
			PlayerPrefs.SetFloat("Score1", _playerScore);
			PlayerPrefs.SetString("Name1", _playerName);

			// bumps down the reamining scores
			PlayerPrefs.SetFloat("Score5", float.Parse(Player4Score.text));
			PlayerPrefs.SetString("Name5", Player4Name.text);
			PlayerPrefs.SetFloat("Score4", float.Parse(Player3Score.text));
			PlayerPrefs.SetString("Name4", Player3Name.text);
			PlayerPrefs.SetFloat("Score3", float.Parse(Player2Score.text));
			PlayerPrefs.SetString("Name3", Player2Name.text);
			PlayerPrefs.SetFloat("Score2", float.Parse(Player1Score.text));
			PlayerPrefs.SetString("Name2", Player1Name.text);

			// clears player score after it has been set
			PlayerPrefs.SetFloat("PlayerScore", 0);

		}
		// checks if second highest
		else if (float.Parse(Player2Score.text) < _playerScore)
		{
			// sets second place
			PlayerPrefs.SetFloat("Score2", _playerScore);
			PlayerPrefs.SetString("Name2", _playerName);

			// bumps down the rest
			PlayerPrefs.SetFloat("Score5", float.Parse(Player4Score.text));
			PlayerPrefs.SetString("Name5", Player4Name.text);
			PlayerPrefs.SetFloat("Score4", float.Parse(Player3Score.text));
			PlayerPrefs.SetString("Name4", Player3Name.text);
			PlayerPrefs.SetFloat("Score3", float.Parse(Player2Score.text));
			PlayerPrefs.SetString("Name3", Player2Name.text);

			// clears the player score
			PlayerPrefs.SetFloat("PlayerScore", 0);
		}
		// checks if third place
		else if (float.Parse(Player3Score.text) < _playerScore)
		{
			// sets third place
			PlayerPrefs.SetFloat("Score3", _playerScore);
			PlayerPrefs.SetString("Name3", _playerName);

			// moces reamining scores down
			PlayerPrefs.SetFloat("Score5", float.Parse(Player4Score.text));
			PlayerPrefs.SetString("Name5", Player4Name.text);
			PlayerPrefs.SetFloat("Score4", float.Parse(Player3Score.text));
			PlayerPrefs.SetString("Name4", Player3Name.text);

			// clears score
			PlayerPrefs.SetFloat("PlayerScore", 0);
		}
		// checks if fourth
		else if (float.Parse(Player4Score.text) < _playerScore)
		{
			// sets fourth
			PlayerPrefs.SetFloat("Score4", _playerScore);
			PlayerPrefs.SetString("Name4", _playerName);

			// moves 4th to 5th
			PlayerPrefs.SetFloat("Score5", float.Parse(Player4Score.text));
			PlayerPrefs.SetString("Name5", Player4Name.text);

			// clears player score
			PlayerPrefs.SetFloat("PlayerScore", 0);
		}
		// checks if 5th
		else if (float.Parse(Player5Score.text) < _playerScore)
		{
			// sets the 5th score
			PlayerPrefs.SetFloat("Score5", _playerScore);
			PlayerPrefs.SetString("Name5", _playerName);

			// clears the players score
			PlayerPrefs.SetFloat("PlayerScore", 0);
		}
	}

	void Update()
	{
		// keeps the scoreboard updated
		setScoreBoard();
	}

	// function for setting score board
	private void setScoreBoard()
	{
		Player1Name.text = PlayerPrefs.GetString("Name1", "---");
		Player1Score.text = PlayerPrefs.GetFloat("Score1", 0).ToString();

		Player2Name.text = PlayerPrefs.GetString("Name2", "---");
		Player2Score.text = PlayerPrefs.GetFloat("Score2", 0).ToString();

		Player3Name.text = PlayerPrefs.GetString("Name3", "---");
		Player3Score.text = PlayerPrefs.GetFloat("Score3", 0).ToString();

		Player4Name.text = PlayerPrefs.GetString("Name4", "---");
		Player4Score.text = PlayerPrefs.GetFloat("Score4", 0).ToString();

		Player5Name.text = PlayerPrefs.GetString("Name5", "---");
		Player5Score.text = PlayerPrefs.GetFloat("Score5", 0).ToString();
	}

	// function to reset the high scores
	public void resetHighScores()
	{
		PlayerPrefs.SetFloat("Score1", 0);
		PlayerPrefs.SetString("Name1", "---");
		PlayerPrefs.SetFloat("Score2", 0);
		PlayerPrefs.SetString("Name2", "---");
		PlayerPrefs.SetFloat("Score3", 0);
		PlayerPrefs.SetString("Name3", "---");
		PlayerPrefs.SetFloat("Score4", 0);
		PlayerPrefs.SetString("Name4", "---");
		PlayerPrefs.SetFloat("Score5", 0);
		PlayerPrefs.SetString("Name5", "---");
	}
}
