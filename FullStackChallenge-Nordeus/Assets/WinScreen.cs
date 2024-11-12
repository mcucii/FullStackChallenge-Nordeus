using UnityEngine;
using UnityEngine.SceneManagement;


public class WinScreen : MonoBehaviour
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
