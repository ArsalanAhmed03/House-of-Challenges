using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager
{

    private static GameManager instance;

    private GameManager() { }

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameManager();
            }
            return instance;
        }
    }

    public bool Initialized = false;
    public int CurrentLevel = 1;
    public int CurrentPlayer = 1;
    public string GameStatus;
    public int Objectives;
    public int SessionStatus = 0;
    List<IndexBoolPair> miniGames = new List<IndexBoolPair>();

    public void setminiGameWinState(int index, bool state)
    {
        miniGames[index].setMiniGameWinState(state);
    }

    public bool isValidLevel(int index)
    {
        if (index > miniGames.Count - 1)
        {
            return false;
        }
        return (miniGames[index].miniGameWinState);
    }

    public void AddMiniGames(int PlayableLevels)
    {

        for (int i = 0; i < PlayableLevels; i++)
        {
            miniGames.Add(new IndexBoolPair(i, false));
        }

    }
}