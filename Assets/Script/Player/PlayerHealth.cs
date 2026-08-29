using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public TextMeshProUGUI hpText;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHPUI();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        Debug.Log("플레이어 체력: " + currentHealth);

        UpdateHPUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHPUI()
    {
        if (hpText != null)
        {
            hpText.text = "HP: " + currentHealth + " / " + maxHealth;
        }
    }
    void Die()
    {
        Debug.Log("플레이어 사망!");

        Time.timeScale = 0f;
    }
}