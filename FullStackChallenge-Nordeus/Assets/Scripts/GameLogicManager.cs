using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Init,
    Playing,
    Win,
    Lose
}

public class GameLogicManager : MonoBehaviour {

    public MapManager mapManager;
    public IslandManager islandManager;
    public HealthSystem healthSystem;

    public GameOverScreen gameOverScreen;
    public WinScreen winScreen;

    public GameState currentState = GameState.Init;


    void Update()
    {
        if (mapManager.tiles != null && currentState == GameState.Init)
        {
            currentState = GameState.Playing;
        }

        if (currentState != GameState.Playing)
        {
            return;
        }

        if (healthSystem.IsDead())
        {
            GameOver();
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Mathf.Abs(Camera.main.transform.position.z);

            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

            worldPosition += new Vector3(mapManager.tileSize / 2, mapManager.tileSize / 2, 0);
            worldPosition -= mapManager.transform.position;     // u slucaju da menjamo poziciju mape

            int x = Mathf.FloorToInt(worldPosition.x / mapManager.tileSize);
            int y = Mathf.FloorToInt(worldPosition.y / mapManager.tileSize);

            if (x >= 0 && x < mapManager.gridWidth && y >= 0 && y < mapManager.gridHeight)
            {
                //Debug.Log($"CLICK ON TILE {x}, {y}.");
                CustomTile clickedTile = mapManager.tiles[x, y];
                CheckResult(clickedTile);
            }
            else
            {
                Debug.Log("Click out of bounds.");
            }
        }      
    }

    void CheckResult(CustomTile clickedTile)
    {
        if (islandManager.tile2island.TryGetValue(clickedTile, out CustomIsland clickedIsland))
        {
            if (clickedIsland.avgHeight == islandManager.maxHeight)
            {
                Debug.Log("POBEDA! Kliknuli ste na ostrvo sa najvecom prosecnom visinom.");
                Win();
            }
            else
            {
                Debug.Log("Nije najvise ostrvo. TRY AGAIN.");
                healthSystem.TakeDamage();
            }
        }
        else
        {
            Debug.Log("VODA.");
            healthSystem.TakeDamage();
        }
    }

    void GameOver()
    {
        currentState = GameState.Lose;
        gameOverScreen.setUp();
    }

    void Win()
    {
        currentState = GameState.Win;
        winScreen.setUp();
    }
}
