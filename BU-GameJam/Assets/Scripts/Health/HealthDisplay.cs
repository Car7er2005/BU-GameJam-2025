using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField]
    private Image[] heartImages = new Image[3]; 

    // Use [SerializeField] to create the Inspector slot
    [SerializeField] 
    private Sprite fullHeartSprite; 

    public void UpdateHealthDisplay(int currentHealth)
    {
        // This is the safety check for the STALE REFERENCE bug.
        if (fullHeartSprite == null)
        {
            // If you see this, the Inspector reference is still broken.
            Debug.LogError("HealthDisplay Error: 'Full Heart Sprite' is null. Re-assign it in the Inspector!");
            return; 
        }

        int maxHealth = heartImages.Length;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        for (int i = 0; i < maxHealth; i++)
        {
            heartImages[i].sprite = fullHeartSprite; 

            if (i < currentHealth)
            {
                // Full opacity
                heartImages[i].color = new Color(1f, 1f, 1f, 1f); 
            }
            else
            {
                // Faded
                heartImages[i].color = new Color(1f, 1f, 1f, 0.3f); 
            }
        }
    }
}