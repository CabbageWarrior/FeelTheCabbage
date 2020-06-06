using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainGameManager : MonoBehaviour {

	public GameObject infoPanel;
	public GameObject loadingPanel;
	public SpriteRenderer loadingImage;

	private bool skipBtnAlreadyPressed = false;

	// Use this for initialization
	/*void Start () {

	}*/

	// Update is called once per frame
	void Update() {
		// Controllo gli input
		if (!skipBtnAlreadyPressed && Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Space))  // Inizio a giocare
		{
			skipBtnAlreadyPressed = true;
			SceneManager.LoadScene("The_Viking_Village");
			infoPanel.SetActive(false);
			loadingPanel.SetActive(true);
		}
		else if (Input.GetKeyDown(KeyCode.Escape)) // Esco
			Application.Quit();

	}
}
