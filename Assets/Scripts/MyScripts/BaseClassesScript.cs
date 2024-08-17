using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorData
{
    public string name { get; set; }
    public GameObject sprite { get; set; }

    public ColorData(string name, GameObject sprite)
    {
        this.name = name;
        this.sprite = sprite;
    }
}

public class Position
{
    public Vector3 VectorPosition { get; set; }

    public Position(Vector3 vectorPosition)
    {
        VectorPosition = vectorPosition;
    }
}

public class ColorsClass
{
    public List<ColorData> AllColors = new List<ColorData>();
    public List<Position> AllColorsPositions = new List<Position>();
    public int colorsCount { get; private set; }

    public ColorsClass(GameObject allColors, GameObject allColorPositions)
    {
        colorsCount = allColors.transform.childCount;
        int colorPositionCount = allColorPositions.transform.childCount;

        for (int i = 0; i < colorsCount; i++)
        {
            AllColors.Add(new ColorData(allColors.transform.GetChild(i).name, allColors.transform.GetChild(i).gameObject));
        }

        for (int i = 0; i < colorPositionCount; i++)
        {
            Transform childTransform = allColorPositions.transform.GetChild(i).transform;
            Vector3 position = childTransform.position;
            AllColorsPositions.Add(new Position(position));
        }
    }

    public void PickColorPair(out GameObject chosenColor, out string trickColor, List<GameObject> TrickColors)
    {
        int index = Random.Range(0, colorsCount);
        chosenColor = AllColors[index].sprite;
        Debug.Log("Chosen Color " + AllColors[index].name);
        do
        {
            index = Random.Range(0, colorsCount);
        } while (AllColors[index].sprite == chosenColor);

        trickColor = AllColors[index].name;
        Debug.Log("Trick Color Name " + AllColors[index].name);

        while (TrickColors.Count < 2)
        {
            index = Random.Range(0, colorsCount);
            if (!TrickColors.Contains(AllColors[index].sprite) && chosenColor != AllColors[index].sprite)
            {
                TrickColors.Add(AllColors[index].sprite);
                Debug.Log("Trick Color " + AllColors[index].name);
            }
        }
    }

    public void RandomPosition(out Vector3 MainPosition, List<Vector3> TrickPositions)
    {
        int index = Random.Range(0, AllColorsPositions.Count);
        MainPosition = AllColorsPositions[index].VectorPosition;

        while (TrickPositions.Count < 2)
        {
            index = Random.Range(0, AllColorsPositions.Count);
            if (!TrickPositions.Contains(AllColorsPositions[index].VectorPosition) && AllColorsPositions[index].VectorPosition != MainPosition)
            {
                TrickPositions.Add(AllColorsPositions[index].VectorPosition);
            }
        }
    }
}

public class IndexBoolPair
{
    public int Index { get; set; }
    public bool miniGameWinState { get; set; }
    public string TriggerName { get; set; }



    public IndexBoolPair()
    {

    }
    public IndexBoolPair(int index, bool miniGameWinState, string TriggerName)
    {
        Index = index;
        this.miniGameWinState = miniGameWinState;
        this.TriggerName = TriggerName;
    }

    public void setMiniGameWinState(bool Win)
    {
        miniGameWinState = Win;
    }
}
