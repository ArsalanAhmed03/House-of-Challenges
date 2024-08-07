using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseScreenScript : MonoBehaviour
{
    private Button ResumeButton;
    private Button ExitButton;
    private CursorLockMode originalLockState;
    private bool CursorVisible;
    

    void Start()
    {
        ResumeButton = GameObject.FindWithTag("ResumeButton").GetComponent<Button>();
        ExitButton = GameObject.FindWithTag("ExitButton").GetComponent<Button>();
        ResumeButton.onClick.AddListener(FindObjectOfType<GF_GameController>().ResumeGame);
        ExitButton.onClick.AddListener(EndGame);

    }

    public void PauseGame()
    {
        
        originalLockState = Cursor.lockState;
        CursorVisible = Cursor.visible;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    
    public void ResumeGame()
    {
        Cursor.lockState = originalLockState;
        Cursor.visible = CursorVisible;
    }

    public void EndGame()
    {
        //mainSceneController.EndGame();
    }

}
