using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public int numberOfCabbages;
	public int minutes;
	public Cabbage cabbagePrefab;
	public Player playerHQ;
	public Player playerLQ;
	public GameObject spawnPointsContainer;
	public GameObject UIMessagePanel;
	public Image UIMessageAvatarArea;
	public Text UIMessageTextArea;
	public GameObject UITimeLeftPanel;
	public Text UITimeLeftTextArea;
	public GameObject UIScorePanel;
	public GameObject UIPausePanel;
	public GameObject UIGameWonPanel;
	public GameObject UIGameLostPanel;

	public Text UIWinScoreText;
	public Text UILostScoreText;

	public List<Sprite> immagini = new List<Sprite>();
	public List<string> messaggi = new List<string>();
	private int listCounter = 0;
	private bool isLastMessageBeforePlay = false;

	private Player player;

	private float timeLeft;
	private bool isGameAlreadyStarted = false;
	private bool isGameInProgress = false;
	private bool isGameFinished = false;
	private AudioSource go_audioSource;
	private Rigidbody player_rigidbody;

	// Use this for initialization
	void Start()
	{
		if (immagini.Count != messaggi.Count)
		{
			Debug.LogError("\"Immagini\" e \"Messaggi\" devono contenere la stessa quantità di elementi!");
		}

		if (playerHQ.isActiveAndEnabled) {
			player = playerHQ;
		}
		else if (playerLQ.isActiveAndEnabled) {
			player = playerLQ;
		}
		else
		{
			Debug.LogError("Impossibile definire l'elemento \"Player\"!");
		}

		go_audioSource = gameObject.GetComponent<AudioSource>();
		player_rigidbody = player.GetComponent<Rigidbody>();

		List<Vector3> spawnPositions = new List<Vector3>();// = new Vector3[spawnPointsContainer.transform.childCount];
		int spawnElementIndex;

		// Inizializzo l'array delle posizioni.
		for (int i = 0; i < spawnPointsContainer.transform.childCount; i++)
		{
			if (spawnPointsContainer.transform.GetChild(i).gameObject.activeInHierarchy)
			{
				spawnPositions.Add(spawnPointsContainer.transform.GetChild(i).position);
			}
		}

		// Itero per "numberOfCabbages" elementi per renderizzare.
		for (int i = 0; i < numberOfCabbages && spawnPositions.Count > 0; i++)
		{
			spawnElementIndex = Random.Range(0, spawnPositions.Count);

			Instantiate(cabbagePrefab, spawnPositions[spawnElementIndex], Quaternion.identity);

			spawnPositions.RemoveAt(spawnElementIndex);
		}

		// Inizializzo il primo testo da far vedere.
		if (messaggi.Count > 0)
		{
			SetNextUIMessageItem();
		}

		// Inizializzo il tempo di gioco
		timeLeft = minutes * 60.0f;

		SetTimeToOutputString();
	}

	// Update is called once per frame
	void Update()
	{
		// Controllo gli input
		if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && !isGameInProgress && !isGameFinished)  // Inizio/Riprendo a giocare
		{
			if (!isLastMessageBeforePlay)
			{
				SetNextUIMessageItem();
			}
			else
			{
				StartGame();
			}
		}
		else if (
			((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && isGameFinished) ||
			(Input.GetKeyDown(KeyCode.R) && !isGameInProgress && isGameAlreadyStarted && !isGameFinished)
		)	// Ricarico la scena
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene("The_Viking_Village");
		}
		else if (Input.GetKeyDown(KeyCode.Escape) && isGameInProgress)  // Metto in pausa
		{
			PauseGame();
		}
		else if (Input.GetKeyDown(KeyCode.Escape) && isGameAlreadyStarted)  // Esco
		{
			Application.Quit();
		}

		// Faccio andare il timer
		if (isGameInProgress)
		{
			timeLeft -= Time.deltaTime;
			SetTimeToOutputString();
			if (timeLeft < 0)
			{
				GameOver();
			}
		}
	}

	void SetNextUIMessageItem()
	{
		if (!isLastMessageBeforePlay)
		{
			UIMessageAvatarArea.sprite = immagini[listCounter];
			UIMessageTextArea.text = messaggi[listCounter]
				.Replace("\\n", System.Environment.NewLine)  // Fix del bug di Unity per Windows che non permette di inserire "\n" direttamente nell'Inspector come carattere speciale.
				.Replace("{cabbagesNum}", numberOfCabbages.ToString())
				.Replace("{minutes}", minutes.ToString());
			listCounter++;

			isLastMessageBeforePlay = (listCounter == messaggi.Count);
		}
	}

	void SetTimeToOutputString()
	{
		System.TimeSpan time = System.TimeSpan.FromSeconds(timeLeft);

		int minutesExtended = time.Minutes + time.Hours * 60;

		string timeLeftFormatString = "La Sagra della Cassoeula inizia "
			+ (minutesExtended == 0 && time.Seconds == 0
				? "adesso"
				: "tra "
					+ (minutesExtended > 0 ? (minutesExtended == 1 ? "un minuto" : minutesExtended.ToString() + " minuti") : "")
					+ (minutesExtended > 0 && time.Seconds > 0 ? " e " : "")
					+ (time.Seconds > 0 ? (time.Seconds == 1 ? "un secondo" : time.Seconds.ToString() + " secondi") : "")
			)
			+ "!";

		//here backslash is must to tell that colon is
		//not the part of format, it just a character that we want in output
		/*UITimeLeftTextArea.text = string.Format("{0:D2}:{1:D2}",
				time.Minutes + time.Hours * 60,
				time.Seconds);*/

		UITimeLeftTextArea.text = timeLeftFormatString;
	}

	void StartGame()
	{
		isGameInProgress = true;
		player_rigidbody.isKinematic = !isGameInProgress;

		if (!isGameAlreadyStarted)    // Se sto iniziando a giocare
		{
			isGameAlreadyStarted = true;

			UIMessagePanel.SetActive(false);
			UITimeLeftPanel.SetActive(true);
			UIScorePanel.SetActive(true);
		}
		else    // Se sono in pausa e sto per giocare ancora
		{
			UIPausePanel.SetActive(false);
		}
	}
	void PauseGame()
	{
		FreezeGame();
		UIPausePanel.SetActive(true);
	}
	void FreezeGame()
	{
		isGameInProgress = false;
		player_rigidbody.isKinematic = !isGameInProgress;
	}

	// Gioco finito
	public void GameOver()
	{
		FreezeGame();
		isGameFinished = true;

		int cabbagesFound = player.getCabbagesFound();
		string finalText;

		switch (cabbagesFound)
		{
			case 0:
				finalText = "Mannaggia! non hai trovato neanche una foglia di verza per un brodino veloce!";
				break;
			case 1:
				finalText = "La sagra è stata un flop, ma almeno un mojito alla verza siamo riusciti a berlo!";
				break;
			default:
				finalText = "Peccato, hai trovato solo " + cabbagesFound.ToString() + " verze, ma poteva andare molto, molto peggio...";
				break;
		}
		UILostScoreText.text = finalText;

		UITimeLeftPanel.SetActive(false);
		UIScorePanel.SetActive(false);

		go_audioSource.Stop();
		UIGameLostPanel.SetActive(true);
		UIGameLostPanel.GetComponent<Animator>().Play("FinishedPanel");
		UIGameLostPanel.GetComponent<AudioSource>().Play();
	}
	public void GameWonByPlayer()
	{
		FreezeGame();
		isGameFinished = true;

		System.TimeSpan tempoImpiegato = System.TimeSpan.FromSeconds((minutes * 60.0f) - timeLeft);

		UIWinScoreText.text = UIWinScoreText.text
			.Replace("{m}", tempoImpiegato.Minutes.ToString())
			.Replace("{s}", tempoImpiegato.Seconds.ToString());

		UITimeLeftPanel.SetActive(false);
		UIScorePanel.SetActive(false);
		
		go_audioSource.Stop();
		UIGameWonPanel.SetActive(true);
		UIGameWonPanel.GetComponent<Animator>().Play("FinishedPanel");
		UIGameWonPanel.GetComponent<AudioSource>().Play();
	}
}
