using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeRunnerLogicScript : MonoBehaviour
{
    [SerializeField] private GameObject StartPoint;
    [SerializeField] private Sprite PlayerSprite;
    [SerializeField] private GameObject Player;

    private void OnEnable()
    {
        InitializeLevel();
    }

    private void InitializeLevel()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Player.transform.position = StartPoint.transform.position;
        Player.GetComponent<SpriteRenderer>().sprite = PlayerSprite;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        FindObjectOfType<GF_GameController>().ReturnFromMiniGame(true);
    }

    public void ExitGame()
    {
        FindObjectOfType<GF_GameController>().ReturnFromMiniGame(false);
    }
}
