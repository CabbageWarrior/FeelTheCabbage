using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MonoBehaviour {
	public GameManager gameManager;
	public Text scoreText;
	private int cabbagesFound = 0;
	
	// Use this for initialization
	void Start () {
		UpdateScoreText();
	}

	// Update is called once per frame
	/*
	void Update () {
	
	}
	*/

	void OnTriggerEnter(Collider other)
	{
		switch (other.tag) {
			case "TokenCabbage":
				gameObject.GetComponent<AudioSource>().Play();
				Destroy(other.gameObject);
				AddPlayerPoint();
				break;
		}
	}

	void AddPlayerPoint() {
		cabbagesFound += 1;
		UpdateScoreText();
		if (cabbagesFound == gameManager.numberOfCabbages) {
			gameManager.GameWonByPlayer();
		}
	}

	void UpdateScoreText() {
		string textToShow;
		int numberOfCabbages = gameManager.numberOfCabbages;

		if (cabbagesFound == 0)
		{
			textToShow = "Trova " + (numberOfCabbages == 1 ? "l'Unica Verza" : "le " + numberOfCabbages.ToString() + " verze") + "!";
		}
		else if (cabbagesFound < numberOfCabbages)
		{
			textToShow = "Hai trovato " + (cabbagesFound == 1 ? "una verza" : cabbagesFound.ToString() + " verze") + " su " + gameManager.numberOfCabbages.ToString() + "!";
		}
		else {
			textToShow = "Hai trovato tutte le verze che ci servivano!";
		}
		scoreText.text = textToShow;
	}

	public int getCabbagesFound() {
		return cabbagesFound;
	}
}
