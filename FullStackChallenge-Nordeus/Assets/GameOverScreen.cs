using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public void setUp()
    {
        gameObject.SetActive(true);
    }
    
    public void Restart()
    {
        SceneManager.LoadScene("Game");
    }
}
