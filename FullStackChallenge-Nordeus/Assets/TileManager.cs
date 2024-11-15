using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class TileManager : MonoBehaviour
{
    public int gridWidth = 30;
    public int gridHeight = 30;
    public GameObject tilePrefab;
    public float tileSize = 1.0f;

    public float waterPercentage = 0.6f;
    public float lowLandsBound = 0.3f;    
    public float highLandsBound = 0.5f;
    public float hillsBound = 0.7f;
    public float mountainsBound = 0.9f;

    public float scale = 10f;

    public CustomTile[,] tiles;    // matrica tiles

    private float xOffset;
    private float yOffset;


    void Awake()
    {
        xOffset = UnityEngine.Random.Range(0f, 100f);
        yOffset = UnityEngine.Random.Range(0f, 100f);

        GenerateTiles();
    }

    Color GetTileColor(float height)
    {
        Color color1 = new Color(0.3f, 0.5f, 0.2f); // nizije
        Color color2 = new Color(0.4f, 0.6f, 0.3f); // brezuljci
        Color color3 = new Color(0.7f, 0.7f, 0.5f); // brda
        Color color4 = new Color(0.6f, 0.6f, 0.6f); // planine
        Color color5 = new Color(1.0f, 1.0f, 1.0f); // vrhovi

        if (height == 0) // Voda
        {
            return new Color(0.0f, 0.2f, 0.5f);
        }
        else if (height <= lowLandsBound) // Nizije
        {
            float t = (height - 0) / (lowLandsBound - 0);
            return Color.Lerp(color1, color2, t);
        }
        else if (height <= highLandsBound) // Brezuljci
        {
            float t = (height - lowLandsBound) / (highLandsBound - lowLandsBound);
            return Color.Lerp(color2, color3, t);
        }
        else if (height <= hillsBound) // Brda
        {
            float t = (height - highLandsBound) / (hillsBound - highLandsBound);
            return Color.Lerp(color3, color4, t);
        }
        else if (height <= mountainsBound) // Planine
        {
            float t = (height - hillsBound) / (mountainsBound - hillsBound);
            return Color.Lerp(color4, color5, t);
        }
        else // Vrhovi - sneg
        {
            return new Color(1.0f, 1.0f, 1.0f); 
        }
    }


    void GenerateTiles()
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
                    tile.height = landHeight;
                }

                tile.pos = new Vector2(x, y);

                var spriteRenderer = newTile.GetComponent<SpriteRenderer>();
                spriteRenderer.color = GetTileColor(tile.height);
                if (spriteRenderer != null)
                {
                    Vector2 spriteSize = spriteRenderer.bounds.size;
                    Debug.Log($"Tile sprite size in world units: {spriteSize.x} x {spriteSize.y}");
                }

                tiles[x, y] = tile;
            }
        }
        //ispisiTiles();    OK ispis tiles -> poklapa se sa mapom
    }


    /*void ispisiTiles()
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
