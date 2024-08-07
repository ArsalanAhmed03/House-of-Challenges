using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalanceBeamLogicScript : MonoBehaviour
{

    [SerializeField] GameObject spawnLocation;
    [SerializeField] GameObject Player;


    private void OnEnable()
    {
        InitializeLevel();
    }

    private void InitializeLevel()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Player.transform.position = spawnLocation.transform.position;
    }

    private void Update()
    {
        if(Player.transform.position.y <= -7)
        {
            FindObjectOfType<GF_GameController>().ReturnFromMiniGame(false);
        }
        else if (Player.transform.position.z >= 63)
        {
            FindObjectOfType<GF_GameController>().ReturnFromMiniGame(true);
        }
    }

}
