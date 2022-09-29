using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour
{
	public Text timeLabel;
	public Text mineLabel;

	public int remainingMines = 0;

	InputManager inputManager;
	float startTime = -1f;

	// Use this for initialization
	void Start ()
	{
		startTime = -1f;
		remainingMines = PlayerPrefs.GetInt ("Number Of Mines", 0);
		timeLabel.text = "Time: 0s";
		mineLabel.text = "Remaining Mines: " + remainingMines.ToString ();

		inputManager = GetComponent<InputManager> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (startTime >= 0f)
		{
			float playTime = Time.time - startTime;

			timeLabel.text = "Time: " + Mathf.Floor (playTime).ToString () + "s";
		}

		mineLabel.text = "Remaining Mines: " + remainingMines.ToString ();
	}

	// Called when the quit button is clicked
	public void OnButtonQuitClick()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene ("Start Menu");
	}

	// Called when the restart button is clicked
	public void OnButtonRestartClick()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene ("Game");
	}

	// Called when the help button is clicked
	public void OnButtonHelpClick()
	{
		inputManager.gridInputEnabled = false;
	}

	public void onButtonQuitHelpClick()
	{
		inputManager.gridInputEnabled = true;
	}

	public void SetStartTime(float time)
	{
		startTime = time;
	}
}
