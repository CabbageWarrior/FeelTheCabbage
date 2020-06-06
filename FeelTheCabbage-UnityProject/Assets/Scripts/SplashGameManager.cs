using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SplashGameManager : MonoBehaviour
{

	public GameObject elem1, elem2, elem3;
	public float waitUntilElem1Rotate, waitUntilElem2Rotate, waitUntilElem3Rotate, waitUntilStartGame;
	private Transform go_transform;

	// Use this for initialization
	void Start()
	{
		StartCoroutine(StartGame(waitUntilStartGame));
	}

	IEnumerator StartGame(float waitUntilExec)
	{
		// Wait for waitUntilExec seconds
		yield return new WaitForSeconds(waitUntilExec);

		SceneManager.LoadScene("Main");
	}
}
