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
            if (playerMovement != null && playerMovement.GetAnimator() != null)
            {
                playerMovement.GetAnimator().SetTrigger("isHurt");
                playerMovement.rb.AddForce(Vector2.up * playerMovement.JumpPower*(0.8f), ForceMode2D.Impulse);
            }

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
        // The player object is handled by the CheckpointManager after EndGame is called.
        // However, for the pause, we call the GameManager first.
        if (GameManager.Instance != null)
        {
            GameManager.Instance.EndGame();
        }

        // Deactivating the player object
        gameObject.SetActive(false);
    }
}