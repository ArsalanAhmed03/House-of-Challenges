using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GF_MainMenu : MonoBehaviour{

    [Header("Scene Selection")]
    public Scenes NextScene;

    [Header("UI Panels")]
    public GameObject ExitDialogue;


    void Start(){

        Time.timeScale = 1;
        AudioListener.pause = false;

        if (!GameManager.Instance.Initialized) {
            InitializeGame();
        }

        InitializeUI();
    }

    void InitializeGame() {
        GameManager.Instance.Initialized = true;
    }

    void InitializeUI() {
        ExitDialogue.SetActive(false);
    }

    public void PlayBtn(){
        SceneManager.LoadScene(NextScene.ToString());
        
    }

    public void Exit(){
        Application.Quit();
    }
}
