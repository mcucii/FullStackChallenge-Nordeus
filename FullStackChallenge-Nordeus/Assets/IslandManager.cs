using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class IslandManager : MonoBehaviour
{
    public float maxHeight;
    public Dictionary<CustomTile, CustomIsland> tile2island;
    public List<CustomIsland> islands;

    public TileManager tileManager;

    private bool[,] visited;

    void Start()
    {
        tile2island = new Dictionary<CustomTile, CustomIsland>();
    }
    

    void Dfs(CustomTile[,] tiles, bool[,] visited, int i, int j, CustomIsland currIsland)
    {
        if (i < 0 || j < 0 || i >= tileManager.gridWidth || j >= tileManager.gridHeight) return;

        if (visited[i,j]) return;  
        visited[i, j] = true;

        if (tileManager.tiles[i, j].height == 0) return;  // voda ne pripada ostrvu

        currIsland.tiles.Add(tiles[i, j]);
        tile2island.Add(tiles[i, j], currIsland);

        Dfs(tiles, visited, i + 1, j, currIsland);
        Dfs(tiles, visited, i, j + 1, currIsland);
        Dfs(tiles, visited, i - 1, j, currIsland);
        Dfs(tiles, visited, i, j - 1, currIsland);
    }

    public void DetectIslands()
    {
        // pronalazi sva ostrva i cuva ih u nizu islands, i takodje cuva u tile2island
        visited = new bool[tileManager.gridWidth , tileManager.gridHeight];
        for (int i = 0; i < tileManager.gridWidth; i++)
        {
            for (int j = 0; j < tileManager.gridHeight; j++)
            {
                // ako je polje kopno i nije poseceno --> novo ostrvo
                if (!visited[i, j] && tileManager.tiles[i, j].height > 0)
                {
                    CustomIsland newIsland = gameObject.AddComponent<CustomIsland>();
                    islands.Add(newIsland);

                    Dfs(tileManager.tiles, visited, i, j, newIsland);
                }
            }
        }

        //Debug.Log($"num islands: {islands.Count}");
    }

    public void CalculateHeights()
    {
        maxHeight = 0;
        foreach(CustomIsland isl in islands)
        {
            isl.CalculateAverageHeight();
            if(isl.avgHeight > maxHeight) 
                maxHeight = isl.avgHeight;
        }
    }

/*    void PrintIslands()
    {
        int k = 0;
        foreach(CustomIsland island in islands)
        {
            k++;
            Debug.Log($"OSTRVO {k}");
            for(int i = 0; i < island.tiles.Count; i++)
                Debug.Log($"Tile {island.tiles[i].pos} pripada ostrvu {k}");
        }
    }*/
}
