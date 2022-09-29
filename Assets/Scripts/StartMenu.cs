using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartMenu : MonoBehaviour
{
	int rows = 10;
	int columns = 10;
	int numberOfMines = 10;

	int minRows = 5;
	int minColumns = 5;
	int minNumberOfMines = 5;
	int maxNumberOfMines = 80;

	public InputField rowsInput;
	public InputField columnsInput;
	public Slider minesSlider;
	public Text mineText;

	void Start()
	{
		rows = PlayerPrefs.GetInt ("Rows", rows);
		columns = PlayerPrefs.GetInt ("Columns", columns);
		numberOfMines = PlayerPrefs.GetInt ("Number Of Mines", numberOfMines);

		UpdateMaxNumberOfMines ();

		rowsInput.text = rows.ToString ();
		columnsInput.text = columns.ToString ();

		minesSlider.minValue = (float)minNumberOfMines;
		minesSlider.maxValue = (float)maxNumberOfMines;
		minesSlider.value = numberOfMines;

		mineText.text = numberOfMines.ToString();
	}

	public void SetNumberOfMinesText()
	{
		mineText.text = minesSlider.value.ToString ();
	}

	public void SetNumberOfMines()
	{
		numberOfMines = (int)minesSlider.value;
	}

	public void SetRows()
	{
		int val = 0;

		if (!int.TryParse (rowsInput.text, out val))
		{
			rowsInput.text = rows.ToString ();
			return;
		}

		if (val >= minRows)
		{
			rows = val;
			UpdateMaxNumberOfMines ();
		}
		else
		{
			rowsInput.text = rows.ToString ();
		}
	}

	public void SetColumns()
	{
		int val = 0;
		
		if (!int.TryParse (columnsInput.text, out val))
		{
			columnsInput.text = columns.ToString ();
			return;
		}
		
		if (val >= minColumns)
		{
			columns = val;
			UpdateMaxNumberOfMines();
		}
		else
		{
			columnsInput.text = columns.ToString ();
		}
	}
		
	public void UpdateMaxNumberOfMines()
	{
		maxNumberOfMines = rows*columns - rows - columns;
		minesSlider.maxValue = maxNumberOfMines;

		if (numberOfMines > maxNumberOfMines)
		{
			numberOfMines = maxNumberOfMines;
			minesSlider.value = numberOfMines;
		}
	}

	public void OnPlayPressed()
	{
		PlayerPrefs.SetInt ("Rows", rows);
		PlayerPrefs.SetInt ("Columns", columns);
		PlayerPrefs.SetInt ("Number Of Mines", numberOfMines);

		UnityEngine.SceneManagement.SceneManager.LoadScene ("Game");
	}
}
