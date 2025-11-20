using UnityEngine;

public class Heal : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private int healAmount = 1; // Amount of damage to deal
    [SerializeField] private bool destroyOnHit = true;
    private PlayerMovement playerMovement;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerMovement = other.GetComponent<PlayerMovement>();
            
                PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
                if (playerHealth != null && playerMovement != null)
                {
                AudioManager.Instance.Play(AudioManager.SoundType.Drink);
                    playerHealth.Heal(healAmount);
                    if (destroyOnHit) Destroy(gameObject);
                }
            


        }

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
