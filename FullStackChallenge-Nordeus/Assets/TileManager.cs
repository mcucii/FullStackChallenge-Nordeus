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
    public float waterPercentage;
    public float scale = 10f;  // Kontrola za šum


    void Start()
    {
        GenerateTiles();
    }

    void GenerateTiles()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector3 tilePosition = new Vector3(x * tileSize, y * tileSize, 0);

                GameObject newTile = Instantiate(tilePrefab, tilePosition, Quaternion.identity);
                newTile.transform.parent = this.transform; 

                // Koristi Perlinov šum za generisanje visine
                float xCoord = (float)x / gridWidth * scale;
                float yCoord = (float)y / gridHeight * scale;
                float heightValue = Mathf.PerlinNoise(xCoord, yCoord);

                //float value = UnityEngine.Random.Range(0f, 1f);

                CustomTile tile = newTile.AddComponent<CustomTile>();
                
                if (heightValue <= waterPercentage)
                    tile.height = 0;
                else
                {
                    tile.height = heightValue / waterPercentage;
                }
                    
                
                var spriteRenderer = newTile.GetComponent<SpriteRenderer>();
                if (tile.height == 0)
                    spriteRenderer.color = new Color(0f, 0f, 0.5f); // vodaaa
                else
                {
                    Color lowGreen = new Color(0f, 0.5f, 0f); // niže visine
                    Color highColor = new Color(0.7f, 0.7f, 0.7f); // više visine

                    float minHeight = 1;
                    float maxHeight = 1 / waterPercentage;

                    Debug.Log(tile.height);
                    float normalizedHeight = Mathf.InverseLerp(minHeight, maxHeight, tile.height);
                    Debug.Log(normalizedHeight);
                    spriteRenderer.color = Color.Lerp(lowGreen, highColor, normalizedHeight);
                }
                    
            }
        }
    }
}
