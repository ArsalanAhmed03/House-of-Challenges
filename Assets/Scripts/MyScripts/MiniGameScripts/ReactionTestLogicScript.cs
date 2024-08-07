using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ReactionTestLogicScript : MonoBehaviour
{
    [SerializeField] private GameObject allColors;
    [SerializeField] private GameObject allColorPositions;
    [SerializeField] private TextMeshProUGUI pickedColorName;
    [SerializeField] private TextMeshProUGUI pointCount;
    [SerializeField] private TextMeshProUGUI attemptsLeftCount;

    private ColorsClass colorPicker;
    private int playerPoint = 0;
    private int playerAttempts = 5;
    private GameObject chosenColor = null;
    private List<GameObject> trickColors = new List<GameObject>();
    private Coroutine timerCoroutine;

    private const int maxAttempts = 5;
    private const int winPoints = 8;

    private void Start()
    {
        Debug.Log("Start");
    }

    private void Update()
    {
        if (playerAttempts < 1)
        {
            GameLost();
        }
    }

    private void OnEnable()
    {
        InitializeLevel();
    }

    private void InitializeLevel()
    {
        playerPoint = 0;
        playerAttempts = maxAttempts;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        colorPicker = new ColorsClass(allColors, allColorPositions);
        PickNextColor();
    }

    private void OnDisable()
    {
        playerPoint = 0;
        playerAttempts = maxAttempts;
    }

    private Color GetColorFromName(string colorName)
    {
        switch (colorName)
        {
            case "DarkBlue":
                return new Color(0f, 0f, 1f);
            case "Grey":
                return Color.white;
            case "LightBlue":
                return new Color(0f, 0.9124765f, 1f);
            case "LightGreen":
                return new Color(0.422668f, 1f, 0f);
            case "Orange":
                return new Color(1f, 0.4044465f, 0f);
            case "Pink":
                return new Color(1f, 0f, 0.7878213f);
            case "Purple":
                return new Color(0.6476364f, 0.09748411f, 1f);
            case "Red":
                return Color.red;
            case "Yellow":
                return Color.yellow;
            default:
                return Color.white;
        }
    }

    private void AddClick()
    {
        if (chosenColor != null)
        {
            Debug.Log("Click Assigned On Chosen Color");
            chosenColor.GetComponent<Button>().onClick.AddListener(RightColorPicked);
        }

        foreach (var trickColor in trickColors)
        {
            Debug.Log("Click Assigned On Trick Color");
            trickColor.GetComponent<Button>().onClick.AddListener(RemovePoints);
        }
    }

    private void RemovePoints()
    {
        if (playerPoint > 0)
        {
            --playerPoint;
        }
    }

    private void WrongColorPicked()
    {
        Debug.Log("Wrong Color Called");

        if (playerPoint > 0)
        {
            --playerPoint;
        }
        if (playerAttempts > 0)
        {
            --playerAttempts;
        }
        RemoveClicks();
        UpdateScoreAttempts();
        PickNextColor();
    }

    private void RightColorPicked()
    {
        Debug.Log("Right Color Called");
        ++playerPoint;
        RemoveClicks();
        UpdateScoreAttempts();

        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }

        if (playerPoint < winPoints)
        {
            PickNextColor();
        }
        else
        {
            GameWon();
        }
    }

    private void UpdateScoreAttempts()
    {
        pointCount.text = playerPoint.ToString();
        attemptsLeftCount.text = playerAttempts.ToString();
    }

    private void RemoveClicks()
    {
        if (chosenColor != null)
        {
            chosenColor.GetComponent<Button>().onClick.RemoveAllListeners();
        }

        foreach (var trickColor in trickColors)
        {
            trickColor.GetComponent<Button>().onClick.RemoveAllListeners();
        }
    }

    private void PickNextColor()
    {
        SetAllColorsInactive();
        SetColorsActive();
    }

    private void SetAllColorsInactive()
    {
        foreach (var color in colorPicker.AllColors)
        {
            color.sprite.SetActive(false);
        }
    }

    private void SetColorsActive()
    {
        chosenColor = null;
        trickColors.Clear();
        Vector3 mainPosition = Vector3.zero;
        List<Vector3> trickPositions = new List<Vector3>();
        string trickColor = "";
        colorPicker.PickColorPair(out chosenColor, out trickColor, trickColors);
        colorPicker.RandomPosition(out mainPosition, trickPositions);
        chosenColor.SetActive(true);
        chosenColor.transform.position = mainPosition;

        for (int i = 0; i < trickColors.Count; i++)
        {
            trickColors[i].SetActive(true);
            trickColors[i].transform.position = trickPositions[i];
        }

        pickedColorName.text = chosenColor.name;
        pickedColorName.color = GetColorFromName(trickColor);
        AddClick();

        // Start the 0.8-second timer
        timerCoroutine = StartCoroutine(StartTimer());
    }

    private IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(0.8f);
        WrongColorPicked(); // Call wrongColorPicked after the timer finishes
    }

    private void GameWon()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }

        pickedColorName.text = "You Win";
        pickedColorName.color = Color.white;
        ReturnFromMiniGame(true);
    }

    private void GameLost()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }

        pickedColorName.text = "Try Again";
        pickedColorName.color = Color.white;
        ReturnFromMiniGame(false);
    }

    public void ExitMiniGame()
    {
        FindObjectOfType<GF_GameController>().ReturnFromMiniGame(false);
    }

    private void ReturnFromMiniGame(bool won)
    {
        FindObjectOfType<GF_GameController>().ReturnFromMiniGame(won);
    }
}