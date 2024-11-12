using System.Collections.Generic;
using UnityEngine;

public class GameLogicManager : MonoBehaviour {
    public int lives = 3;

    public TileManager tileManager;
    public IslandManager islandManager;

    public GameOverScreen gameOverScreen;
    public WinScreen winScreen;

    void Update()
    {
        if(lives == 0) {
            GameOver();
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int x = Mathf.FloorToInt(worldPosition.x / tileManager.tileSize);
            int y = Mathf.FloorToInt(worldPosition.y / tileManager.tileSize);

            if (x >= 0 && x < tileManager.gridWidth && y >= 0 && y < tileManager.gridHeight)
            {
                CustomTile clickedTile = tileManager.tiles[x, y];
                CheckResult(clickedTile); 
            }
        }
    }

    void CheckResult(CustomTile clickedTile)
    {
        Debug.Log($"LIVES: {lives}");
        if (islandManager.tile2island.TryGetValue(clickedTile, out CustomIsland clickedIsland))
        {
            if (clickedIsland.avgHeight == islandManager.maxHeight)
            {
                Debug.Log("POBEDA! Kliknuli ste na ostrvo sa najvecom prosecnom visinom.");
                Win();
            }
            else
            {
                Debug.Log("Nije najvece ostrvo. TRY AGAIN.");
                lives--;
            }
        }
        else
        {
            Debug.Log("Ovo polje nije deo ostrva.");
            lives--;
        }
    }

    void GameOver()
    {
        gameOverScreen.setUp();
    }

    void Win()
    {
        winScreen.setUp();
    }
}
