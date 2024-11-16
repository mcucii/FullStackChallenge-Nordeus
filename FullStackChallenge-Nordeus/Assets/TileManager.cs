using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class TileManager : MonoBehaviour
{
    public int gridWidth = 30;
    public int gridHeight = 30;
    public GameObject tilePrefab;
    public float tileSize = 1.0f;
    public int maxHeight = 1000;
    public float scale = 10f;
    public int[,] heights;
    public CustomTile[,] tiles;

    private float xOffset;
    private float yOffset;

    private float waterPercentage = 0.6f;
    private float sandBound = 0.1f;
    private float landsBound = 0.4f;
    private float hillsBound = 0.6f;
    private float mountainsBound = 0.8f;


    void Awake()
    {
        xOffset = UnityEngine.Random.Range(0f, 100f);
        yOffset = UnityEngine.Random.Range(0f, 100f);
    }

  
    Color GetTileColor(float height)
    {
        Color color1 = new Color(0.8235f, 0.7059f, 0.5490f); // pesak
        Color color2 = new Color(0.4706f, 0.7059f, 0.3922f); // nizije
        Color color3 = new Color(0.5451f, 0.4667f, 0.3098f); // brda
        Color color4 = new Color(0.6627f, 0.6627f, 0.6627f); // planine
        Color color5 = new Color(1.0f, 1.0f, 1.0f); // vrhovi

        if (height == 0) // Voda
        {
            return new Color(0.0f, 0.2f, 0.5f);
        }
        else if (height <= sandBound*maxHeight) // Nizije
        {
            return color1;

        }
        else if (height <= landsBound * maxHeight) // Brezuljci
        {
            return color2;
        }
        else if (height <= hillsBound * maxHeight) // Brda
        {
            return color3;
        }
        else if (height <= mountainsBound * maxHeight) // Planine
        {
            return color4;
        }
        else // Vrhovi - sneg
        {
            return color5;
        }
    }


    public void GenerateRandomMap()
    {
        tiles = new CustomTile[gridWidth, gridHeight];

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector3 tilePosition = new Vector3(x * tileSize, y * tileSize, 0);

                GameObject newTile = Instantiate(tilePrefab, tilePosition, Quaternion.identity);
                newTile.transform.parent = this.transform;

                // perlinov sum za generisanje visine
                float xCoord = (float)x / gridWidth * scale + xOffset;
                float yCoord = (float)y / gridHeight * scale + yOffset;
                float heightValue = Mathf.PerlinNoise(xCoord, yCoord);

                CustomTile tile = newTile.AddComponent<CustomTile>();

                if (heightValue <= waterPercentage)
                    tile.height = 0;
                else
                {
                    // normalizovanje vrednosti kopna izmedju 0 i 1
                    float landHeight = (heightValue - waterPercentage) / (1 - waterPercentage);
                    tile.height = landHeight * maxHeight;   // visina kopna 0-1000
                }

                tile.pos = new Vector2(x, y);

                var spriteRenderer = newTile.GetComponent<SpriteRenderer>();
                spriteRenderer.color = GetTileColor(tile.height);
                if (spriteRenderer != null)
                {
                    Vector2 spriteSize = spriteRenderer.bounds.size;
                }

                tiles[x, y] = tile;
            }
        }
        //printTiles();    OK ispis tiles -> poklapa se sa mapom
    }

    public void GenerateCustomMap(int[,] heights)
    {
        if (heights.GetLength(0) != gridWidth || heights.GetLength(1) != gridHeight)
        {
            Debug.LogError("dims err");
            return;
        }

        tiles = new CustomTile[gridWidth, gridHeight];

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector3 tilePosition = new Vector3(x * tileSize, y * tileSize, 0);

                GameObject newTile = Instantiate(tilePrefab, tilePosition, Quaternion.identity);
                newTile.transform.parent = this.transform;

                CustomTile tile = newTile.AddComponent<CustomTile>();
                tile.height = heights[x, y]; // Visina se postavlja iz matrice

                tile.pos = new Vector2(x, y);

                var spriteRenderer = newTile.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    spriteRenderer.color = GetTileColor(tile.height);
                }

                tiles[x, y] = tile;
            }
        }
    }


    /*void printTiles()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                CustomTile tile = tiles[x, y];
                if (tile != null)
                {
                    Debug.Log($"Tile at ({x}, {y}) - Height: {tile.height}");
                }
                else
                {
                    Debug.Log($"Tile at ({x}, {y}) is null.");
                }
            }
        }
    }*/
}
