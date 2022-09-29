using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenerateGrid : MonoBehaviour
{
	public void RandGrid (int [,] grid, int width, int height, int numberOfMines, int clickedRow, int clickedColumn)
	{
		RandMines (grid, width, height, numberOfMines, clickedRow, clickedColumn);
		CountNeighbours (grid, width, height);
	}

	void RandMines (int [,] grid, int width, int height, int numberOfMines, int clickedRow, int clickedColumn)
	{
		int remaining = numberOfMines;

		List<Vector2> forbiddenCells = new List<Vector2> ();

		for (int i = clickedRow-1; i <= clickedRow+1; i++)
		{
			for (int j = clickedColumn-1; j <= clickedColumn+1; j++)
			{
				forbiddenCells.Add(new Vector2((float)i, (float)j));
			}
		}
		
		while (remaining > 0)
		{
			int row = Random.Range (0, height);
			int column = Random.Range (0, width);

			if (forbiddenCells.Contains(new Vector2((float)row, (float)column)))
			{
				continue;
			}

			if (grid[row, column] == GameManager.mine)
			{
				continue;
			}

			grid[row, column] = GameManager.mine;
			remaining--;
		}
	}

	void CountNeighbours (int [,] grid, int width, int height)
	{
		for (int i = 0; i < height; i++)
		{
			for (int j = 0; j < width; j++)
			{
				if (grid[i, j] == GameManager.mine)
				{
					IncrementNeighbours(grid, width, height, i, j);
				}
			}
		}
	}

	void IncrementNeighbours(int [,] grid, int width, int height, int row, int column)
	{
		for (int i = row-1; i <= row+1; i++)
		{
			for (int j = column-1; j <= column+1; j++)
			{
				if (i >= 0 && i < height && j >= 0 && j < width)
				{
					if (i==row && j==column)
					{
						continue;
					}

					//Si un des voisins est une mine
					if (grid[i, j] == GameManager.mine)
					{
						continue;
					}

					grid[i, j]++;
				}
			}
		}
	}

}
