using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[RequireComponent(typeof(GenerateGrid))]
[RequireComponent(typeof(BuildGrid))]
[RequireComponent(typeof(MusicManager))]
[RequireComponent(typeof(CameraManager))]
public class GameManager : MonoBehaviour
{
	public static int empty = 0;
	public static int mine = 9;
	
	[SerializeField]
	int columns = 10;
	[SerializeField]
	int rows = 10;
	[SerializeField]
	int numberOfMines = 10;
	[SerializeField]
	
	int[,] grid;
	bool[,] destroyedGrid;
	bool firstClick = true;
	int remainingCells; // Cells not clicked yet

	GenerateGrid generateGrid;
	BuildGrid buildGrid;
	MusicManager musicManager;
	CameraManager cameraManager;
	UIManager uIManager;
	AdManager adManager;

	public GameObject flagPrefab;

	// Use this for initialization
	void Start ()
	{
		rows = PlayerPrefs.GetInt ("Rows");
		columns = PlayerPrefs.GetInt ("Columns");
		numberOfMines = PlayerPrefs.GetInt ("Number Of Mines");

		grid = new int[rows, columns];
		destroyedGrid = new bool[rows, columns];

		firstClick = true;
		remainingCells = columns * rows;

		generateGrid = GetComponent<GenerateGrid> ();

		buildGrid = GetComponent<BuildGrid> ();
		buildGrid.BuildCells (columns, rows);

		musicManager = GetComponent<MusicManager> ();

		cameraManager = GetComponent<CameraManager> ();
		cameraManager.SetFraming (rows, columns);

		uIManager = GetComponent<UIManager> ();
		adManager = GetComponent<AdManager> ();
	}

	// Update is called once per frame
	void Update ()
	{
		if (remainingCells == numberOfMines)
		{
			StartCoroutine(Success());
		}
	}
	
	public void OnFirstClick (int i, int j)
	{
		generateGrid.RandGrid (grid, columns, rows, numberOfMines, i, j);
		buildGrid.BuildNumbers (grid, columns, rows);
		uIManager.SetStartTime (Time.time);
	}

	public void OnCellClick (int i, int j)
	{
		if (firstClick)
		{
			OnFirstClick(i, j);
			firstClick = false;
		}

		if (!destroyedGrid [i, j])
		{
			GameObject flag = GameObject.Find ("Flag [" + i.ToString() + ", " + j.ToString() + "]");

			// If we click a cell where we already put a flag, we don't want to destroy it
			if (flag)
			{
				musicManager.Play ("Flagged Cell");
				return;
			}

			musicManager.Play ("Cell Click");

			DestroyCell(i, j);
		}
	}

	public void OnDoubleClick(int i, int j)
	{
		if (destroyedGrid [i, j])
		{
			// If we don't have the exact number of flags surrounding the cell,
			// we can't destroy the remaining neighbour cells.
			if (CountNeighbourFlags(i, j) != grid[i, j])
			{
				return;
			}

			for (int u = i-1; u <= i+1; u++)
			{
				for (int v = j-1; v <= j+1; v++)
				{
					if (u >= 0 && u < rows && v >= 0 && v < columns && !(u == i && v == j))
					{
						// We go through all valid neighbour cells and destroy the non-flagged ones

						if (!destroyedGrid [u, v])
						{
							GameObject flag = GameObject.Find ("Flag [" + u.ToString() + ", " + v.ToString() + "]");

							if (flag)
							{
								continue;
							}

							DestroyCell(u, v);
						}
					}
				}
			}
		}
	}

	int CountNeighbourFlags(int i, int j)
	{
		int count = 0;

		for (int u = i-1; u <= i+1; u++)
		{
			for (int v = j-1; v <= j+1; v++)
			{
				if (u >= 0 && u < rows && v >= 0 && v < columns && !(u == i && v == j))
				{
					// We go through all valid neighbour cells and count the flagged ones

					if (!destroyedGrid [u, v])
					{
						GameObject flag = GameObject.Find ("Flag [" + u.ToString() + ", " + v.ToString() + "]");

						if (flag)
						{
							count++;
						}
					}
				}
			}
		}

		return count;
	}

	void DestroyCell(int i, int j)
	{
		GameObject GridCellForeground = GameObject.Find ("Grid Cell Foreground [" + i.ToString() + ", " + j.ToString() + "]");
		Destroy (GridCellForeground);

		remainingCells--;
		destroyedGrid[i, j] = true;

		if (grid[i, j] == mine)
		{
			StartCoroutine(MineClicked());
		}
		else if (grid[i, j] == empty)
		{
			musicManager.Play("Empty Cell");
			ProcessEmptyCell(i, j);
		}
	}

	IEnumerator MineClicked ()
	{
		musicManager.Play ("Mine");

		Destroy (GetComponent<InputManager> ());

		yield return new WaitForSeconds(2);

		adManager.ShowAd ();

		UnityEngine.SceneManagement.SceneManager.LoadScene ("Start Menu");
	}

	IEnumerator Success ()
	{
		musicManager.Play ("Cleared");

		Destroy (GetComponent<InputManager> ());

		PlayerPrefs.SetInt ("Already Won", 1);

		yield return new WaitForSeconds(2);

		UnityEngine.SceneManagement.SceneManager.LoadScene ("Start Menu");
	}

	void ProcessEmptyCell (int i, int j)
	{
		List<Vector2> emptyCellsToDestroy = new List<Vector2> ();
		List<Vector2> emptyCellsDestroyed = new List<Vector2> ();

		emptyCellsDestroyed.Add (new Vector2 ((float)i, (float)j));
		FindEmptyNeighboursCells (i, j, emptyCellsToDestroy, emptyCellsDestroyed);

		while (emptyCellsToDestroy.Count > 0)
		{
			int row = (int)emptyCellsToDestroy[0].x;
			int column = (int)emptyCellsToDestroy[0].y;

			FindEmptyNeighboursCells (row, column, emptyCellsToDestroy, emptyCellsDestroyed);

			emptyCellsDestroyed.Add(emptyCellsToDestroy[0]);
			emptyCellsToDestroy.RemoveAt(0);
		}
	}

	void FindEmptyNeighboursCells(int row, int column, List<Vector2> emptyCellsToDestroy, List<Vector2> emptyCellsDestroyed)
	{
		for (int i = row-1; i <= row+1; i++)
		{
			for (int j = column-1; j <= column+1; j++)
			{
				if (i >= 0 && i < rows && j >= 0 && j < columns)
				{
					if (!destroyedGrid [i, j])
					{
						DestroyCell(i, j);
					}
					
					// If one of the neighbours is empty
					if (grid[i, j] == empty)
					{
						if (emptyCellsToDestroy.Contains(new Vector2((float)i, (float)j))
						    || emptyCellsDestroyed.Contains(new Vector2((float)i, (float)j)))
						{
							continue;
						}

						emptyCellsToDestroy.Add(new Vector2((float)i, (float)j));
					}

				}
			}
		}
	}

	public void Flag(int i, int j)
	{
		GameObject gridCellForeground = GameObject.Find ("Grid Cell Foreground [" + i.ToString() + ", " + j.ToString() + "]");

		if (gridCellForeground)
		{
			musicManager.Play("Flag");
			GameObject flag = GameObject.Find ("Flag [" + i.ToString() + ", " + j.ToString() + "]");

			if (flag)
			{
				Destroy(flag);
				uIManager.remainingMines++;
			}
			else
			{
				flag = (GameObject) Instantiate(flagPrefab, new Vector3(j, -i), Quaternion.identity);
				flag.name = "Flag [" + i.ToString() + ", " + j.ToString() + "]";

				GameObject cell = GameObject.Find("Cell [" + i.ToString() + ", " + j.ToString() + "]"); 
				flag.transform.parent = cell.transform;

				uIManager.remainingMines--;
			}
		}
	}

	public int Columns
	{
		get
		{
			return columns; 
		}
	}

	public int Rows
	{
		get
		{
			return rows; 
		}
	}

	public int NumberOfMines
	{
		get
		{
			return numberOfMines;
		}
	}
}
