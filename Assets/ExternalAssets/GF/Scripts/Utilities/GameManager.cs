using UnityEngine;
using System.Collections.Generic;
using Mirror;

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

    public int SessionStatus = 0;
    List<IndexBoolPair> miniGames = new List<IndexBoolPair>();



    public int isValidLevel(string TriggerName)
    {
        for (int i = 0; i < miniGames.Count; i++)
        {
            if (miniGames[i].TriggerName == TriggerName && !miniGames[i].miniGameWinState)
            {
                return i;
            }
        }
        return -1;
    }


    public void AddMiniGames(int PlayableLevels, GameObject MiniGameTriggers)
    {
        int triggersCount = MiniGameTriggers.transform.childCount;
        List<string> Triggers = new List<string>();
        for (int i = 0; i < triggersCount; i++)
        {
            Triggers.Add(MiniGameTriggers.transform.GetChild(i).transform.GetChild(0).name);
            Debug.Log(MiniGameTriggers.transform.GetChild(i).transform.GetChild(0).name);
            
        }
        Debug.Log("check");
        for (int i = 0; i < triggersCount; i++)
        {
            int index = Random.Range(0, Triggers.Count);
            miniGames.Add(new IndexBoolPair(i, false, Triggers[index]));
            Triggers.RemoveAt(index);
        }

    }



    public void setminiGameWinState(int index, bool state)
    {
        miniGames[index].setMiniGameWinState(state);
    }



}