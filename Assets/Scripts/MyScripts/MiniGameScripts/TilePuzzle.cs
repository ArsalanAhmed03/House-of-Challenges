
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TilePuzzle : MonoBehaviour
{
    [SerializeField] private GameObject Tiles;
    [SerializeField] private GameObject IncorrectLocations;
    [SerializeField] private GameObject IncorrectLocationsCopy;
    [SerializeField] private GameObject CorrectLocations;
    [SerializeField] private Camera myCamera;

    //private variables
    private List<Sprite> PuzzleList = new List<Sprite>();
    private int tileCount;
    public bool leftMouseClicked = false;
    public bool notBlocked = true;
    public int rightTiles = 0;

    private void Update()
    {
        if (rightTiles != tileCount)
        {
            checkMouse();
            checkMouseClick();
            moveSprite();
        }
        else
        {
            FindObjectOfType<GF_GameController>().ReturnFromMiniGame(true);
        }
    }

    private void checkMouseClick()
    {
        leftMouseClicked = (Input.GetMouseButton(0) && notBlocked) ? true : false;
    }

    private void moveSprite()
    {
        if (leftMouseClicked && HitObject.collider != null)
        {

            Vector3 mousePosition = Input.mousePosition;

            mousePosition.z = myCamera.WorldToScreenPoint(HitObject.collider.transform.position).z; 

            Vector3 newPosition = myCamera.ScreenToWorldPoint(mousePosition);

            HitObject.collider.transform.position = new Vector3(newPosition.x, newPosition.y, HitObject.collider.transform.position.z);
        }
    }

    private RaycastHit HitObject;
    private void checkMouse()
    {

        Ray cameraRay = myCamera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(cameraRay.origin, cameraRay.direction * 100f, Color.red, 1f);
        if (!Physics.Raycast(cameraRay, out HitObject))
        {
            HitObject = default;
        }
    }

    private void OnEnable()
    {
        IntializeGame();
    }

    private void IntializeGame()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        addTiles();
        PlaceTiles();
        rightTiles = 0;
    }

    private void addTiles()
    {
        tileCount = Tiles.transform.childCount;

        for (int i = 0; i < tileCount; i++)
        {
            PuzzleList.Add(Tiles.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite);
        }
    }


    private void ReplaceTiles()
    {
        int index = 0;
        do
        {
            IncorrectLocations.transform.GetChild(index).position = IncorrectLocationsCopy.transform.GetChild(index).position;
            index++;

        } while (index < tileCount);
    }
    private void OnDisable()
    {
        removeTiles();
        ReplaceTiles();
    }

    private void removeTiles()
    {
        int number_of_removed = 0;
        do
        {
            IncorrectLocations.transform.GetChild(number_of_removed).GetComponent<SpriteRenderer>().sprite = default;
            number_of_removed++;

        } while (number_of_removed < tileCount);
    }

    private void PlaceTiles()
    {
        int number_of_placed = 0;

        do
        {
            int index = Random.Range(0, PuzzleList.Count);
            IncorrectLocations.transform.GetChild(number_of_placed).GetComponent<SpriteRenderer>().sprite = PuzzleList[index];
            PuzzleList.RemoveAt(index);
            number_of_placed++;

        } while (number_of_placed < tileCount);
    }

    public void ExitMiniGame()
    {
        FindObjectOfType<GF_GameController>().ReturnFromMiniGame(false);
    }

}
