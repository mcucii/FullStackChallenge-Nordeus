using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Playing,
    Win,
    Lose
}

public class GameLogicManager : MonoBehaviour {

    public TileManager tileManager;
    public IslandManager islandManager;
    public HealthSystem healthSystem;

    public GameOverScreen gameOverScreen;
    public WinScreen winScreen;

    public GameState currentState = GameState.Playing;


    void Update()
    {
        if (currentState == GameState.Playing)
        {
            if (healthSystem.IsDead())
            {
                GameOver();
            }

            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePosition = Input.mousePosition;
                mousePosition.z = Mathf.Abs(Camera.main.transform.position.z); // Dubina ravni pločica

                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
                worldPosition -= tileManager.transform.position; // Ukloni offset mreže

                int x = Mathf.FloorToInt(worldPosition.x / tileManager.tileSize);
                int y = Mathf.FloorToInt(worldPosition.y / tileManager.tileSize);

                if (x >= 0 && x < tileManager.gridWidth && y >= 0 && y < tileManager.gridHeight)
                {
                    //Debug.Log($"CLICK ON TILE {x}, {y}.");
                    CustomTile clickedTile = tileManager.tiles[x, y];
                    CheckResult(clickedTile);
                }
                else
                {
                    Debug.Log("Click out of bounds.");
                }
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
                Debug.Log("Nije najvece ostrvo. TRY AGAIN.");
                healthSystem.TakeDamage();
            }
        }
        else
        {
            Debug.Log("Ovo polje nije deo ostrva.");
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
