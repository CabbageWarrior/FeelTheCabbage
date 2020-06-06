using UnityEngine;
using System.Collections;

public class Heartbeat : MonoBehaviour
{

	public float duration;
	public float sizeIncrement;
	public float waitUntilBeat;
	private bool isScaling = false;
	private Transform go_transform;
	private AudioSource go_audioSource;

	// Use this for initialization
	void Start()
	{
		go_transform = gameObject.GetComponent<Transform>();
		go_audioSource = gameObject.GetComponent<AudioSource>();

		StartCoroutine(Beat(waitUntilBeat));
	}

	public IEnumerator Beat(float waitUntilExec)
	{
		// Wait for waitUntilExec seconds
		yield return new WaitForSeconds(waitUntilExec);

		// Check if we are scaling now, if so exit method to avoid overlap.
		if (isScaling)
			yield break;

		// Declare that we are scaling now.
		isScaling = true;

		// Grab the current time and store it in a variable.
		float startTime;
		Vector3 startScale = go_transform.localScale;

		go_audioSource.Play();

		for (int i = 0; i < 2; i++)
		{
			startTime = Time.time;
			while (Time.time - startTime < duration)
			{
				float amount = (Time.time - startTime) / duration;
				go_transform.localScale = Vector3.Lerp(startScale, startScale * sizeIncrement, amount);
				yield return null;
			}

			// Snap the scale to sizeIncrement.
			go_transform.localScale = startScale * sizeIncrement;

			// Leave the scale at 3 for 2 seconds (this can be changed at any time).
			//yield return new WaitForSeconds(2.0f);

			// Now for the scale down part.  Store the current time in the same variable.
			startTime = Time.time;

			while (Time.time - startTime < duration)
			{
				float amount = (Time.time - startTime) / duration;
				go_transform.localScale = Vector3.Lerp(startScale * sizeIncrement, startScale, amount);
				yield return null;
			}
		}

		// Snap the scale to initial.
		go_transform.localScale = startScale;

		// Declare that we are no longer modifing the scale.
		isScaling = false;
	}
}