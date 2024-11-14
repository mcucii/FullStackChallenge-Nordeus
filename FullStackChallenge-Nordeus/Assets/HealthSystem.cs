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
        Debug.Log($"{hearts.Length}");

        for(int i = 0; i < hearts.Length; i++)
        {
            // i  0 1 2
            //    s s s

            // ch   1 2 3
            // i = ch - 1
            
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
