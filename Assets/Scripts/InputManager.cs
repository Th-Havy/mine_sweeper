using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GameManager))]
public class InputManager : MonoBehaviour
{
	GameManager gameManager;
	public bool gridInputEnabled = true;

	// Time interval to consider a double click
	public float doubleClickEpsilon = 0.3f;
	float lastClick = -1f;

	// Time interval to consider a double click
	public float doubleTouchEpsilon = 0.3f;
	float lastTouch = -1f;

	public float longTouchDuration = 0.5f;
	float beginTouchTime = -1f;
	bool longTouchDone = false;

	void Start ()
	{
		gameManager = GetComponent<GameManager> ();
	}
		
	void Update ()
	{
		int i = -1;
		int j = -1;
		ConvertToGrid(out i, out j);

		// If the mouse is within the grid
		if (gridInputEnabled && i >= 0 && i < gameManager.Rows && j >= 0 && j < gameManager.Columns)
		{
			#if UNITY_STANDALONE || UNITY_WEBGL
			if (DoubleClick())
			{
				gameManager.OnDoubleClick (i, j);
				return;
			}

			if (Input.GetButtonDown ("Click"))
			{
				gameManager.OnCellClick(i, j);
			}

			if (Input.GetButtonDown ("Flag"))
			{
				gameManager.Flag(i, j);
			}
			#endif

			#if UNITY_ANDROID
			if (DoubleTouch())
			{
				gameManager.OnDoubleClick (i, j);
				return;
			}

			if (ShortTouch())
			{
				gameManager.OnCellClick(i, j);
			}

			if (LongTouch())
			{
				gameManager.Flag(i, j);
			}
			#endif
		}
	}

	// Converts mouse position to grid coordinates
	void ConvertToGrid(out int i, out int j)
	{
		#if UNITY_STANDALONE || UNITY_WEBGL
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		#endif

		#if UNITY_ANDROID
		Touch[] touches = Input.touches;
		if (touches.Length == 0)
		{
			i = j = -1;
			return;
		}
		
		Ray ray = Camera.main.ScreenPointToRay(touches[0].position);
		#endif
		
		Vector2 pos = ray.origin;
		pos.y = -pos.y;

		i = Mathf.FloorToInt(pos.y);
		j = Mathf.FloorToInt(pos.x);
	}

	// Returns true if a double click was made
	bool DoubleClick()
	{
		if (Input.GetButtonDown ("Click"))
		{
			float newClick = Time.time;

			if (newClick - lastClick < doubleClickEpsilon)
			{
				lastClick = newClick;
				return true;
			}

			lastClick = newClick;
		}

		return false;
	}

	// Returns true if a double touch was made
	bool DoubleTouch()
	{
		Touch[] touches = Input.touches;
		if (touches.Length == 0)
			return false;
		
		if (touches[0].phase == TouchPhase.Began)
		{
			float newTouch = Time.time;

			if (newTouch - lastTouch < doubleTouchEpsilon)
			{
				lastTouch = newTouch;
				return true;
			}

			lastTouch = newTouch;
		}

		return false;
	}

	// Returns true if a short touch was made
	bool ShortTouch()
	{
		Touch[] touches = Input.touches;
		if (touches.Length == 0)
			return false;
		
		if (touches[0].phase == TouchPhase.Began)
		{
			beginTouchTime = Time.time;
			longTouchDone = false;
		}

		if (touches[0].phase == TouchPhase.Ended)
		{
			if (Time.time - beginTouchTime < longTouchDuration)
				return true;
		}

		return false;
	}

	// Returns true if a long touch was made
	bool LongTouch()
	{
		Touch[] touches = Input.touches;
		if (touches.Length == 0)
			return false;
		
		if (!longTouchDone && (touches[0].phase == TouchPhase.Stationary || touches[0].phase == TouchPhase.Moved))
		{
			if (Time.time - beginTouchTime > longTouchDuration)
			{
				longTouchDone = true;
				return true;
			}
		}

		return false;
	}

}
