using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class InputManager : MonoBehaviour
{
    public InputField inputField; 
    public TileManager tileManager;
    public IslandManager islandManager;

    public TextMeshProUGUI msgHowToPlay;

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
                        heights[x, y] = value; // Direktno smesti int vrednost
                    }
                    else
                    {
                        Debug.LogError($"Neispravna vrednost na poziciji {index}: {values[index]}");
                        return;
                    }
                    index++;
                }
            }

            tileManager.GenerateCustomMap(heights);
        }

        msgHowToPlay.gameObject.SetActive(true);

        islandManager.DetectIslands();
        islandManager.CalculateHeights();
    }
}
