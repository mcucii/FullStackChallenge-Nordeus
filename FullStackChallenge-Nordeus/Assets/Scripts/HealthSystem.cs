using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {

        for(int i = 0; i < hearts.Length; i++)
        {   
            if(i < currentHealth)
            {
                hearts[i].sprite = fullHeart;
            } else
            {
                hearts[i].sprite = emptyHeart;
            }
        }
    }

    public void TakeDamage()
    {
        currentHealth --;
    }

    public bool IsDead()
    {
        return currentHealth == 0;
    }
}
