using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class InputManager : MonoBehaviour
{
    public InputField inputField; 
    public TileManager tileManager;
    public IslandManager islandManager;

    public GameObject gameElements;

    public int[,] heights;

    public void LoadInput()
    {
        string inputText = inputField.text;

        string[] values = inputText.Split(new char[] { ' ', ',', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

        if (values.Length != tileManager.gridWidth * tileManager.gridHeight)
        {
            Debug.LogError($"Nepotpun unos. Generise se random mapa.");
            tileManager.GenerateRandomMap();
        } else
        {
            heights = new int[tileManager.gridWidth, tileManager.gridHeight];
            int index = 0;

            for (int x = 0; x < tileManager.gridWidth; x++)
            {
                for (int y = 0; y < tileManager.gridHeight; y++)
                {
                    if (int.TryParse(values[index], out int value))
                    {
                        if (value < 0 || value > 1000)
                        {
                            Debug.LogError($"Neispravna vrednost na poziciji {index}: {value}. Vrednost mora biti izmedju 0 i 1000. Generise se random mapa.");
                            tileManager.GenerateRandomMap();
                            return;
                        }
                        heights[x, y] = value;
                    }
                    else
                    {
                        Debug.LogError($"Neispravna vrednost na poziciji {index}: {values[index]}");
                        tileManager.GenerateRandomMap();
                        return;
                    }
                    index++;
                }
            }

            tileManager.GenerateCustomMap(heights);
        }

        gameElements.SetActive(true);

        islandManager.DetectIslands();
        islandManager.CalculateHeights();
    }
}
