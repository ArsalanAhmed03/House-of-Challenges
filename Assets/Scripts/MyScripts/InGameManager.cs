using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    //[SerializeField] private GameObject MiniGames;
    //[SerializeField] private GameObject MainGamePrafab;
    //[SerializeField] private GameObject PauseScreen;
    ////public List<GameObject> playerCamera;

    //private void Start()
    //{
    //    LoadPlayer();
    //    AddMiniGames();
    //}
    //private void Update()
    //{   
    //    CheckGamePause();
    //}

    //private void AddMiniGames()
    //{
    //    MyGameManager.Instance.AddMiniGames(MiniGames);
    //}


    //public void LoadPlayer()
    //{
    //    if (MyGameManager.Instance.currentPlayerCount < 1)
    //    {
    //        MyGameManager.Instance.currentPlayerCount++;
    //        FindObjectOfType<NetworkManager>().StartHost();
    //    }
    //    else if (MyGameManager.Instance.currentPlayerCount < MyGameManager.Instance.maxPlayerCount)
    //    {
    //        MyGameManager.Instance.currentPlayerCount++;
    //        FindObjectOfType<NetworkManager>().StartClient();
    //    }
    //    else
    //    {
    //        Debug.Log("Max Players Already Exist");
    //    }
    //}


    //public void EndGame()
    //{
    //    ResumeGame();
    //    FindObjectOfType<NetworkManager>().StopHost();
    //}


    //void CheckGamePause()
    //{
    //    if (!Input.GetKeyDown(KeyCode.Escape)) return;

    //    if (MyGameManager.Instance.isPaused)
    //    {
    //        ResumeGame();
    //    }

    //    else
    //    {
    //        PauseGame();
    //    }
    //}


    //public void ResumeGame()
    //{
    //    FindObjectOfType<PauseScreenScript>().ResumeGame();
    //    MyGameManager.Instance.isPaused = false;
    //    PauseScreen.SetActive(false);
    //}


    //public void PauseGame()
    //{
    //    PauseScreen.SetActive(true);
    //    MyGameManager.Instance.isPaused = true;
    //    FindObjectOfType<PauseScreenScript>().PauseGame();
    //}


    //public void MoveToMiniGame(string gameName)
    //{
    //    if (MyGameManager.Instance.isMiniGameActive) return;
    //    for (int i = 0; i < MiniGames.transform.childCount; i++)
    //    {
    //        if (MiniGames.transform.GetChild(i).name == gameName)
    //        {
    //            MiniGames.transform.GetChild(i).gameObject.SetActive(true);
    //        }
    //    }
    //    MainGamePrafab.SetActive(false);
    //    MyGameManager.Instance.isMiniGameActive = true;
    //}


    //public void ReturnFromMiniGame(string gameName, bool Won)
    //{
    //    MainGamePrafab.SetActive(true);
    //    GameObject.FindGameObjectWithTag(gameName).SetActive(false);
    //    MyGameManager.Instance.isMiniGameActive = false;
    //    FindObjectOfType<PlayerScript>().MiniGameReturn(Won);
    //    MyGameManager.Instance.setminiGameWinState(gameName, Won);
    //}
}
