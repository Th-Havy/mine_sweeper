using UnityEngine;
using System.Collections;

public class BuildGrid : MonoBehaviour
{
	public Transform origin;

	public GameObject gridCellBackground;
	public GameObject gridCellForeground;
	public GameObject mine;
	public GameObject[] numbers;

	public void BuildCells(int width, int height)
	{
		GameObject gridObject = new GameObject ("Grid");
		GameObject obj;

		for (int i = 0; i < height; i++)
		{
			for (int j = 0; j < width; j++)
			{
				GameObject cell = new GameObject("Cell [" + i.ToString() + ", " + j.ToString() + "]");
				cell.transform.parent = gridObject.transform;

				obj = (GameObject) Instantiate(gridCellBackground, origin.position + new Vector3(j, -i), origin.rotation);
				obj.name = "Grid Cell Background [" + i.ToString() + ", " + j.ToString() + "]";
				obj.transform.parent = cell.transform;

				obj = (GameObject) Instantiate(gridCellForeground, origin.position + new Vector3(j, -i), origin.rotation);
				obj.name = "Grid Cell Foreground [" + i.ToString() + ", " + j.ToString() + "]";
				obj.transform.parent = cell.transform;
			}
		}
	}

	public void BuildNumbers(int[,] grid, int width, int height)
	{
		GameObject obj;

		for (int i = 0; i < height; i++)
		{
			for (int j = 0; j < width; j++)
			{
				GameObject cell = GameObject.Find ("Cell [" + i.ToString() + ", " + j.ToString() + "]");

				if (grid[i, j] == GameManager.mine)
				{
					obj = (GameObject) Instantiate(mine, origin.position + new Vector3(j, -i), origin.rotation);
					obj.name = "mine [" + i.ToString() + ", " + j.ToString() + "]";
					obj.transform.parent = cell.transform;
				}
				else if (grid[i, j] != 0)
				{
					obj = (GameObject) Instantiate(numbers[grid[i, j] - 1], origin.position + new Vector3(j, -i), origin.rotation);
					obj.name = grid[i, j].ToString() + " [" + i.ToString() + ", " + j.ToString() + "]";
					obj.transform.parent = cell.transform;
				}
			}
		}
	}
}
