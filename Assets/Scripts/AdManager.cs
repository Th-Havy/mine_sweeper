using UnityEngine;

public class AdManager : MonoBehaviour
{
	bool alreadyWon = false;
	bool adDisabled = false;

	void Start()
	{
		adDisabled = PlayerPrefs.GetInt ("Ad Disabled", 0) == 1;
		alreadyWon = PlayerPrefs.GetInt ("Already Won", 0) == 1;
	}

	public void ShowAd()
	{
		#if UNITY_ANDROID

		if (adDisabled || alreadyWon)
			return;

		if (UnityEngine.Advertisements.Advertisement.IsReady())
		{
			UnityEngine.Advertisements.Advertisement.Show();
		}
		#endif
	}
}
