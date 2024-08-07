using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LogicScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pointCount;
    [SerializeField] private Canvas PlayerCanvas;
    [SerializeField] private TMP_InputField userInput;



    private List<string> correctAnswers = new List<string> { "apple", "macdonalds", "nike", "starbucks", "among us", "candy crush", "fortnite", "minecraft", "jarassic park", "the lion king", "titanic" };
    private List<bool> Picked = new List<bool> { false,false,false,false,false,false, false, false, false, false, false };
    private string correctAnswer;
    private bool levelPicked;
    private int options;
    Transform selectedOption;
    private void OnEnable()
    {
        InitializeLevel();
    }


    private void InitializeLevel()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        correctAnswer = "";
        levelPicked = false;
    }

    private void OnDisable()
    {
        for (int i = 0; i < Picked.Count; i++)
        {
            Picked[i] = false;
        }

        selectedOption.gameObject.SetActive(false);
        pointCount.text = "0";

    }

    private void Update()
    {
        if (CheckWin())
        {
            FindObjectOfType<GF_GameController>().ReturnFromMiniGame(true);
        }
        else
        {
            if (!levelPicked)
            {
                PickLevel();
            }

            if (levelPicked)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    Try();
                }
            }

        }
    }

    bool CheckWin()
    {
        int count = int.Parse(pointCount.text);
        if (count == 5)
        {
            return true;
        }
        return false;
    }

    private void PickLevel()
    {
        options = PlayerCanvas.transform.childCount;
        if (options <= 0) return;
        
        do {
            options = Random.Range(0, options - 1);
        } while (Picked[options]);

        selectedOption = PlayerCanvas.transform.GetChild(options);
        selectedOption.gameObject.SetActive(true);
        correctAnswer = correctAnswers[options];
        Picked[options] = true;
        levelPicked = true;

        return;
        
    }

    public void Try()
    {
        string user_attempt = userInput.text.ToLower();
        userInput.text = "";

        if (user_attempt == correctAnswer)
        {
            int count = int.Parse(pointCount.text) + 1;
            pointCount.text = count.ToString();

            selectedOption.gameObject.SetActive(false);
            levelPicked = false;

        }
    }

    public void ExitMiniGame()
    {
        FindObjectOfType<GF_GameController>().ReturnFromMiniGame(false);
    }
}

