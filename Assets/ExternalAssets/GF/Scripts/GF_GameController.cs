using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using Mirror;

public class GF_GameController : MonoBehaviour {

	[Header ("Scene Selection")]
	public Scenes PreviousScene;
	//public Scenes NextScene;

	[Header ("Main Player", order = 1)]
	public Player_Attributes[] Players; 

	[Header ("Game Elements")]
	public Game_Dialogues Game_Elements;

    [Header("Play Area")]
	public GameObject PlayArea;

    [Header("Main Player")]
    public GameObject Main_Player;

    //public Transform Main_Player_Position;

    [Header ("SFX Objects")]
	public SFX_Objects SFX_Elements;

	[Header ("Level Information")]
	public int PlayableLevels = 6;
	public Level_Data[] Levels;
	[Header ("Gameover States")]
	public bool ReasonBased;
	[Tooltip ("Gameover information is optional. This will not appear if un-checked.")]
	public GameOver[] States;

	[Header ("Level End Delay")]
	public float GameWinDelay;
	public float GameLooseDelay;


	//Local Variables
	GameObject PlayerMain;
	GameObject AudioSource_Parent;
	GameObject FX_AudioSource;
	//Timer
	int minutes;
	int seconds;
	string time;
	private int currentLevel;
	private int currentPlayer;
	private int FinishCount = 0;
	private bool isTimerEnabled;
	private int Rewardamount = 0;
    public int totalAvalibleLevels;
    [HideInInspector]
	public bool TimerPaused = false;
    private Coroutine exitTimerCoroutine;

    #region debug

    [Header ("Debug Values")]
	[Range (1, 8)]
	public int StartLevel = 1;
	[Range (1, 2)]
	public int StartPlayer = 1;
	public int ObjectivesLeft = 0;
	public float LevelTime = 0.0f;

	#endregion

	void InitializeAudio (GameObject obj, string name){
		AudioSource_Parent = GameObject.Find ("SFXController");
		obj = new GameObject (name);
		obj.transform.position = AudioSource_Parent.transform.position;
		obj.transform.rotation = AudioSource_Parent.transform.rotation;
		obj.transform.parent = AudioSource_Parent.transform;
		obj.AddComponent<AudioSource> ();
		obj.GetComponent<AudioSource> ().priority = 128;
	}

	void Start () {

		//GameManager Variables Reset
		GameManager.Instance.GameStatus = null;
		Time.timeScale = 1;
		AudioListener.pause = false;


		if (!GameManager.Instance.Initialized) {
			InitializeGame ();
		}

		//InitializeLevel ();

		//Initialize Audio Sources
		InitializeAudio (FX_AudioSource, "FX_AudioSource");
		FX_AudioSource = GameObject.Find ("FX_AudioSource");
		totalAvalibleLevels = PlayableLevels;

        //SpawnPlayer();
        //SpawnPlayers ();
        //ActivateLevel ();
        //if (Levels [currentLevel - 1].Objectives.Length > 0) {
        //	ActivateFinishPoint ();
        //}

        //In-Game Timer
        //if (isTimerEnabled)
        //	InvokeRepeating ("GameTimer", 0, 1);
    }

    private void Update()
    {
		//Main_Player_Position = Main_Player.transform;
		if (Input.GetKey(KeyCode.Escape))
		{
			PauseGame();
		}
    }

    private void AddMiniGames()
    {
        GameManager.Instance.AddMiniGames(Levels, PlayableLevels);
    }

    void InitializeGame () {
		GameManager.Instance.Initialized = true;
		AddMiniGames();
    }

    private void ActivatePlayArea()
    {
        PlayArea.SetActive(true);
    }

    private void DeactivatePlayArea()
    {
        PlayArea.SetActive (false);
    }

	private void ActivatePlayer()
	{
		Main_Player.SetActive(true);
	}

    private void DeactivatePlayer()
    {
        Main_Player.SetActive(false);
    }

	private int PickRandomMiniGame()
	{
		int index;
		do
		{
			index = Random.Range(0, PlayableLevels);
		} while (GameManager.Instance.isValidLevel(index));
		return index + 1;
	}

    public bool MoveToMiniGame(string gameName)
    {
        if (GameManager.Instance.isMiniGameActive) return false;
        if (totalAvalibleLevels == 0) return false;
        GameManager.Instance.isMiniGameActive = true;
        currentLevel = PickRandomMiniGame();
		InitializeLevel();
		DeactivatePlayArea();
		return true;
    }

	private IEnumerator DisableSplashScreens()
	{
        yield return new WaitForSeconds(GameWinDelay);
        Game_Elements.LevelComplete.SetActive(false);
        Game_Elements.LevelFailed.SetActive(false);
        GameManager.Instance.isMiniGameActive = false;
    }


    void UninitializeLevel()
    {
        Levels[currentLevel - 1].LevelObject.SetActive(false);
    }

    public void ReturnFromMiniGame(bool Won)
    {
        totalAvalibleLevels -= Won ? 1 : 0;
		ActivatePlayArea();
        Game_Elements.LevelComplete.SetActive(Won);
        Game_Elements.LevelFailed.SetActive(!Won);
        exitTimerCoroutine = StartCoroutine(DisableSplashScreens());
        UninitializeLevel();
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = false;
        //ActivatePlayer();
        FindObjectOfType<MainPlayerScript>().MiniGameReturn(Won);
		GameManager.Instance.setminiGameWinState(currentLevel - 1, Won);
    }

    private CursorLockMode CursorLockState;
    private bool Cursorvisible;

    private void ResetCursor()
    {
        Cursor.lockState = CursorLockState;
        Cursor.visible = Cursorvisible;
    }
    public void PauseGame()
    {
        if (Time.timeScale != 0.0f)
        {
            CursorLockState = Cursor.lockState;
            Cursorvisible = Cursor.visible;
            Game_Elements.PauseMenu.SetActive(true);
            Time.timeScale = 0.0f;
            AudioListener.pause = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            ResumeGame();
        }
    }

    public void ResumeGame()
    {
        ResetCursor();
        Game_Elements.PauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
        AudioListener.pause = false;
    }


    void InitializeLevel () {

		Game_Elements.LevelComplete.SetActive (false);
		Game_Elements.FinalComplete.SetActive (false);
		Game_Elements.LevelFailed.SetActive (false);
		Game_Elements.GameExit.SetActive (false);
		Game_Elements.LoadingScreen.SetActive (false);
		Game_Elements.PauseMenu.SetActive (false);
		Game_Elements.HelpScreen.SetActive (false);
        Levels[currentLevel - 1].LevelObject.SetActive(true);

        //Reset Players
        //if (Players.Length > 0) {
        //	for (int i = 0; i < Players.Length; i++) {
        //		Players [i].PlayerObject.SetActive (false);
        //		Players [i].PlayerControls.alpha = 0;
        //		Players [i].PlayerControls.interactable = false;
        //		Players [i].PlayerControls.blocksRaycasts = false;
        //          }
        //      } else if (Players.Length == 0) {
        //	Debug.LogError ("No Players have been assigned in the inspector !");
        //      }

        //Reset Finish Points
        //if (Levels [currentLevel - 1].Objectives.Length > 0) {
        //	for (int i = 0; i < Levels [currentLevel - 1].Objectives.Length; i++) {
        //		if (Levels [currentLevel - 1].Objectives [i].FinishPoint != null)
        //			Levels [currentLevel - 1].Objectives [i].FinishPoint.SetActive (false);
        //		if (Levels [currentLevel - 1].Objectives [i].Instruction == "")
        //			Debug.LogWarning ("Please write insctruction for Level->" + GameManager.Instance.CurrentLevel + " Objective->" + (i + 1) + " in the inspector !");
        //          }
        //      } else if (Levels [currentLevel - 1].Objectives.Length == 0) {
        //	Debug.LogError ("No Objectives have been defined in the inspector !");
        //      }

        //SpawnItems
        //if (Levels [currentLevel - 1].Items.Length > 0){
        //	for (int i = 0; i < Levels [currentLevel - 1].Items.Length; i++){
        //		SetPlayerPosition (Levels [currentLevel - 1].Items [i].Item, Levels [currentLevel - 1].Items [i].SpawnPoint);
        //	}
        //} 

        //spawn mini game player here

        if (Levels [currentLevel - 1].GiveReward) {
			if (Levels [currentLevel - 1].RewardLevels.Length == 0)
				Debug.LogError ("No Rewards have been defined in the inspector !");
		}

    }


    //void ActivatePlayer (int PlayerIndex){
    //	PlayerMain = Players [PlayerIndex].PlayerObject;
    //	/*
    //       * IMPORTANT NOTE: ONLY USE (PlayerMain) FOR REFERENCE OF SPECIFIC PLAYER TO GET ALL TYPES OF COMPONENT
    //       * TURN ON EVERYTHING WHICH CAN CONTROL THIS PLAYER
    //       * FOR EXAMPLE:
    //       * RIGIDBODY, CHARACTER CONTROLLER
    //       * RCC CONTROLLER
    //       * ANY FPS CONTROLLER OR TP CONTROLLER
    //       * ASSIGN TARGET TO MINIMAP IF ANY
    //       */
    //	PlayerMain.SetActive (true);
    //}

    //void DeactivatePlayer (int PlayerIndex, bool isActive){
    //	/*
    //       * IMPORTANT NOTE: ONLY USE (PlayerMain) FOR REFERENCE OF SPECIFIC PLAYER TO GET ALL TYPES OF COMPONENT
    //       * TURN ON EVERYTHING WHICH CAN CONTROL THIS PLAYER
    //       * FOR EXAMPLE:
    //       * RIGIDBODY, CHARACTER CONTROLLER
    //       * RCC CONTROLLER
    //       * ANY FPS CONTROLLER OR TP CONTROLLER
    //       */
    //	if (isActive){
    //		PlayerMain.SetActive (false);
    //	}
    //}

    void SwitchControls (){
		for (int i = 0; i < Players.Length; i++){
			if (i == currentPlayer){
				Players [i].PlayerObject.SetActive (true);
				Players [i].PlayerControls.alpha = 1;
				Players [i].PlayerControls.interactable = true;
				Players [i].PlayerControls.blocksRaycasts = true;
			} else{
				Players [i].PlayerControls.alpha = 0;
				Players [i].PlayerControls.interactable = false;
				Players [i].PlayerControls.blocksRaycasts = false;
			}
		}
	}


	void ActivateLevel () { //use this later
		for (int i = 0; i < Levels.Length; i++){
			if (i == currentLevel - 1){
				Levels [i].LevelObject.SetActive (true);
			} 
			//else{
			//	Destroy (Levels [i].PrimaryPlayer.SpawnPoint.gameObject);
			//	Destroy (Levels [i].LevelObject);
			//}
		}

		GameManager.Instance.Objectives = Levels [currentLevel - 1].Objectives.Length;
		//For Debug
		ObjectivesLeft = GameManager.Instance.Objectives;

		LevelTime = (Levels [currentLevel - 1].Minutes * 60) + Levels [currentLevel - 1].Seconds;
    }

	void ActivateFinishPoint () {
		if (FinishCount == 0) {
			if (Levels [currentLevel - 1].Objectives [FinishCount].FinishPoint != null)
				Levels [currentLevel - 1].Objectives [FinishCount].FinishPoint.SetActive (true);
			ShowInstruction ();
		} else if (FinishCount == Levels [currentLevel - 1].Objectives.Length){
			if (Levels [currentLevel - 1].Objectives [FinishCount - 1].FinishPoint != null)
				Levels [currentLevel - 1].Objectives [FinishCount - 1].FinishPoint.SetActive (false);
		} else {
			if (Levels [currentLevel - 1].Objectives [FinishCount - 1].FinishPoint != null)
				Levels [currentLevel - 1].Objectives [FinishCount - 1].FinishPoint.SetActive (false);
			if (Levels [currentLevel - 1].Objectives [FinishCount].FinishPoint != null)
				Levels [currentLevel - 1].Objectives [FinishCount].FinishPoint.SetActive (true);
			ShowInstruction ();
        }
    }

	public void ShowInstruction () {
		Game_Elements.InstructionText.text = Levels [currentLevel - 1].Objectives [FinishCount].Instruction;
		FinishCount++;
    }


	//Dialogues Logic
	public void OnLevelCheck (int reasonIndex) {
		//For Debug
		ObjectivesLeft = GameManager.Instance.Objectives;

		if (GameManager.Instance.Objectives > 0 && GameManager.Instance.GameStatus != "Loose") {
			if (Levels [currentLevel - 1].Objectives.Length != 0)
				ActivateFinishPoint ();
			else
				Debug.LogWarning ("No Objectives have been defined in the inspector !");
        } else if (GameManager.Instance.Objectives == 0) {
			if (Levels [currentLevel - 1].Objectives.Length != 0)
				ActivateFinishPoint ();
			else
				Debug.LogWarning ("No Objectives have been defined in the inspector !");

			//Calculate Reward
			if (Levels [currentLevel - 1].GiveReward){
				GiveRewards ();
			}
			DisableAudio ();
			FX_AudioSource.GetComponent<AudioSource> ().PlayOneShot (SFX_Elements.LevelCompleteSFX);
			StartCoroutine (OnLevelStatus ());
        } else if (GameManager.Instance.GameStatus == "Loose") {
			DisableAudio ();
			if (ReasonBased)
				SetGameOverReason (reasonIndex);
			FX_AudioSource.GetComponent<AudioSource> ().PlayOneShot (SFX_Elements.LevelFailedSFX);
			StartCoroutine (OnLevelStatus ());
        }
    }

	void DisableAudio (){
		for (int i = 0; i < SFX_Elements.BGMusicLoops.Length; i++){
			SFX_Elements.BGMusicLoops [i].SetActive (false);
		}
	}

	void SetGameOverReason (int reasonIndex){
		if (States.Length > 0 && reasonIndex < States.Length){
			Game_Elements.ReasonObject.SetActive(true);
			Game_Elements.GameOverLogo.sprite = States[reasonIndex].Icon;
			Game_Elements.GameOverText.text = States[reasonIndex].Reason;
		} else{
			Debug.LogError ("Game over reason for index " + reasonIndex + " does not exist !");
		}
	}

	void GiveRewards () {
		if (Levels [currentLevel - 1].RewardLevels.Length > 0) {
			for (int i = 0; i < Levels [currentLevel - 1].RewardLevels.Length; i++) {
				//Give reward here
				//CalculateRewardAmount (i);
				//use main player script
            }
        } else {
			Debug.LogError ("No rewards have been defined in the inspector !");
        }
    }

	IEnumerator OnLevelStatus () {
		CancelInvoke ();
		//GameManager.Instance.PauseTimer ();
		//SFX_Elements.CountDown.SetActive (false);
		if (GameManager.Instance.GameStatus == "Loose") {
			yield return new WaitForSeconds (GameLooseDelay); 
			Game_Elements.LevelFailed.SetActive (true);
        } else {
			//UpdateLevel ();
			yield return new WaitForSeconds (GameWinDelay); 
			//if (currentLevel == PlayableLevels) {
			//	Game_Elements.FinalComplete.SetActive (true);
			//} else {
			//	Game_Elements.LevelComplete.SetActive (true);
			//}

            Game_Elements.LevelComplete.SetActive(true);
        }
        yield return new WaitForSeconds (2.0f);
		//Time.timeScale = 0; after returning from minigame
    }

	void UpdateLevel () {
		//calling the next level, don't need it in my game
    }

	

	//public void RetryLevel () {
	//	Game_Elements.LoadingScreen.SetActive (true);
	//	SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
 //   }

	//public void NextLevel () {
	//	if (currentLevel != PlayableLevels) {
	//		#if UNITY_EDITOR
	//		GameManager.Instance.SessionStatus = 1;
	//		#endif
	//		GameManager.Instance.CurrentLevel += 1;
	//		Game_Elements.LoadingScreen.SetActive (true);
	//		SceneManager.LoadScene (NextScene.ToString ());
 //       }
 //   }

	public void MainMenu () {
		Game_Elements.LoadingScreen.SetActive (true);
		SceneManager.LoadScene (PreviousScene.ToString ());
    }
}