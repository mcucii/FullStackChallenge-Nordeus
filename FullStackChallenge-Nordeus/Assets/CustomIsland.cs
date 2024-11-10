using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class CustomIsland : MonoBehaviour
{
    public float avgHeight;
    public List<CustomTile> tiles;

    public CustomIsland()
    {
        avgHeight = 0;
        tiles = new List<CustomTile>();  // Initialize the list
    }

    public void CalculateAverageHeight()
    {
        float totalHeight = 0;
        foreach (CustomTile t in tiles)
        {
            totalHeight += t.height;
        }
        avgHeight = tiles.Count > 0 ? totalHeight / tiles.Count : 0;
    }
}
