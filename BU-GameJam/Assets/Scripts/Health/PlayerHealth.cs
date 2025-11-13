using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private int currentHealth;
    private HealthDisplay healthUI;
    private PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        currentHealth = maxHealth;
        healthUI = FindFirstObjectByType<HealthDisplay>();
    
        if (healthUI != null)
        {
            // This is the one, official call to set the UI at the start
            healthUI.UpdateHealthDisplay(currentHealth);
        }
        else
        {
            Debug.LogError("PlayerHealth could not find HealthDisplay on Start!");
        }
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        currentHealth = Mathf.Max(0, currentHealth);

        if (healthUI != null)
        {
            playerMovement.GetAnimator().SetTrigger("isHurt");
            healthUI.UpdateHealthDisplay(currentHealth);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);

        if (healthUI != null)
        {
            healthUI.UpdateHealthDisplay(currentHealth);
        }
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    void Die()
    {
        Debug.Log("Player has died.");
        gameObject.SetActive(false);
        GameManager.Instance.EndGame();
    }
}