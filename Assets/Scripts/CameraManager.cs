using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour
{
	public Camera mainCamera;

	// Use this for initialization
	void Start ()
	{

	}

	public void SetFraming(int rows, int columns)
	{
		mainCamera.transform.position = new Vector3 ((float)columns / 2f, (float)-rows / 2f, -10f);
		mainCamera.orthographicSize = (float)System.Math.Max(rows, columns) * (0.5f + 0.2f);
	}
}
