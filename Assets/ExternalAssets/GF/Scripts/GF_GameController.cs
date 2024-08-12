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
    }

    

    private void Update()
    {
		if (Input.GetKey(KeyCode.Escape) && !TimerPaused)
		{
            PauseGame();
		}
    }

    private void AddMiniGames()
    {
        GameManager.Instance.AddMiniGames(PlayableLevels);
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


	private int PickRandomMiniGame()
	{
		int index;
		do
		{
			index = Random.Range(0, PlayableLevels);
		} while (GameManager.Instance.isValidLevel(index));
		return index + 1;
	}

	public bool isMiniGameActive = false;

    public bool MoveToMiniGame(string gameName)
    {
        if (isMiniGameActive) return false;
        if (totalAvalibleLevels == 0) return false;
        isMiniGameActive = true;
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
        isMiniGameActive = false;
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
		if (TimerPaused)
		{
			ResumeGame();
		}
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //ActivatePlayer();
        FindObjectOfType<MainPlayerScript>().MiniGameReturn(Won);
		GameManager.Instance.setminiGameWinState(currentLevel - 1, Won);
		
    }

    private CursorLockMode CursorLockState = CursorLockMode.Confined;
    private bool Cursorvisible = false;

    private void ResetCursor()
    {
        Cursor.lockState = CursorLockState;
        Cursor.visible = Cursorvisible;
    }
	public void PauseGame()
	{
		TimerPaused = true;
		CursorLockState = Cursor.lockState;
		Cursorvisible = Cursor.visible;
		Game_Elements.PauseMenu.SetActive(true);
		Time.timeScale = 0.0f;
		AudioListener.pause = true;
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

    public void ResumeGame()
    {
        ResetCursor();
        Game_Elements.PauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
        AudioListener.pause = false;
        TimerPaused = false;
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

        if (Levels [currentLevel - 1].GiveReward) {
			if (Levels [currentLevel - 1].RewardLevels.Length == 0)
				Debug.LogError ("No Rewards have been defined in the inspector !");
		}

    }

	public void ShowInstruction () {
		Game_Elements.InstructionText.text = Levels [currentLevel - 1].Objectives [FinishCount].Instruction;
		FinishCount++;
    }

	public void MainMenu () {
		Game_Elements.LoadingScreen.SetActive (true);
		SceneManager.LoadScene (PreviousScene.ToString ());
    }
}