using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour {

	private string nickname;
	private int maxScore;

	public GameScript gameScript;
	public GameObject planeGame;
	public GameObject panelMenu;
	public GameObject panelGameOver;
	public GameObject panelEditName;

	public Text textNameScore;
	public Text textLastScore;
	public Text textLastMaxScore;
	public Text textEditName;
	public InputField inputName;

	void Start () {
		// playerprefs load
		nickname = PlayerPrefs.GetString("name", "guest");
		maxScore = PlayerPrefs.GetInt ("maxscore", 0);
		UpdateTextNameScore ();
	}

	public void Play()
	{
		panelMenu.SetActive (false);
		planeGame.SetActive (true);
		gameScript.Retry ();
	}

	public void EditName()
	{
		textEditName.text = "Your actual name:\n" + nickname;
		panelEditName.SetActive (true);
	}

	public void SaveName()
	{
		if (inputName.text != "")
		{
			nickname = inputName.text;
			UpdateName ();
		}

		CancelName ();
	}

	public void CancelName()
	{
		inputName.text = "";
		panelEditName.SetActive (false);
	}

	public void GameWon(int score)
	{

	}

	public void GameOver(int score)
	{
		panelGameOver.SetActive (true);
		// save playerprefs
		if (score > maxScore)
		{
			maxScore = score;
			UpdateScore ();
		}
		UpdateLastScored(score);
	}

	public void Menu()
	{
		planeGame.SetActive (false);
		panelGameOver.SetActive (false);
		panelMenu.SetActive (true);
	}

	public void Retry()
	{
		panelGameOver.SetActive (false);
		gameScript.Retry ();
	}

	private void UpdateLastScored(int score)
	{
		textLastScore.text = "Your score: " + score;
		textLastMaxScore.text = "Your best score: " + maxScore;
	}

	private void UpdateScore ()
	{
		UpdateTextNameScore ();
		PlayerPrefs.SetInt ("maxscore", maxScore);
		PlayerPrefs.Save ();
	}

	private void UpdateName ()
	{
		UpdateTextNameScore ();
		PlayerPrefs.SetString ("name", nickname);
		PlayerPrefs.Save ();
	}

	private void UpdateTextNameScore () {
		textNameScore.text = "Hello, " + nickname + "! Your best score: " + maxScore;
	}
}
