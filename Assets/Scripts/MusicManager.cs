using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
	public AudioClip cellClick;
	public AudioClip cleared;
	public AudioClip emptyCell;
	public AudioClip mine;
	public AudioClip flag;
	public AudioClip flaggedCell;

	public AudioClip mainMusic;

	AudioSource audioSource;

	void Start ()
	{
		audioSource = GetComponent<AudioSource> ();
		audioSource.clip = mainMusic;
		audioSource.loop = true;
		audioSource.Play ();
	}

	public void Play(string clip)
	{
		switch (clip)
		{
			case "Cell Click":
				audioSource.PlayOneShot(cellClick);
				break;
			case "Cleared":
				audioSource.PlayOneShot(cleared);
				break;
			case "Empty Cell":
				audioSource.PlayOneShot(emptyCell);
				break;
			case "Mine":
				audioSource.PlayOneShot(mine);
				break;
			case "Flag":
				audioSource.PlayOneShot(flag);
				break;
			case "Flagged Cell":
				audioSource.PlayOneShot(flaggedCell);
				break;
			default:
				Debug.Log("Error: no clip named " + clip);
				break;
		}
	}
}
