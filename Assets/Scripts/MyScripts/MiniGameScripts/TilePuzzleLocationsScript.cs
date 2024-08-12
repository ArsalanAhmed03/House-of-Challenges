using UnityEngine;

public class PuzzleTileLocationsScript : MonoBehaviour
{
    [SerializeField] Sprite rightTile;
    private bool rightTileFound = false;

    private void OnEnable()
    {
        rightTileFound = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            FindAnyObjectByType<TilePuzzle>().notBlocked = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<SpriteRenderer>().sprite == rightTile)
        {
            FindAnyObjectByType<TilePuzzle>().notBlocked = false;
            other.gameObject.transform.position = transform.position;
            FindAnyObjectByType<TilePuzzle>().rightTiles++;
            Debug.Log(FindAnyObjectByType<TilePuzzle>().rightTiles);
            rightTileFound = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (rightTileFound && other.gameObject.GetComponent<SpriteRenderer>().sprite == rightTile)
        {
            rightTileFound = false;
            FindAnyObjectByType<TilePuzzle>().rightTiles--;
            Debug.Log(FindAnyObjectByType<TilePuzzle>().rightTiles);
        }
    }


}
