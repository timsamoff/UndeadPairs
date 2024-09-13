using UnityEngine;
using UnityEngine.UI;

public class LoseHealth : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private float healthDecreasePercent = 10f;
    private float currentHealth = 100f;

    private void Start()
    {
        if (healthBar == null)
        {
            Debug.LogError("Health bar Slider not assigned.");
        }
        else
        {
            healthBar.value = currentHealth / 100f;
        }
    }

    public void ReduceHealth()
    {
        currentHealth -= healthDecreasePercent;
        currentHealth = Mathf.Clamp(currentHealth, 0, 100);

        // Update health bar UI
        if (healthBar != null)
        {
            healthBar.value = currentHealth / 100f;
        }

        // Check if health is at 0%
        if (currentHealth <= 0)
        {
            Debug.Log("Health has reached 0%!");
            // You can trigger a game over event here if desired
        }
    }
}