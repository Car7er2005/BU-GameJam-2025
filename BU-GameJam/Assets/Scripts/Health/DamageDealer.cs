using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private int damageAmount = 1; // Amount of damage to deal
    [SerializeField] private bool destroyOnHit = false;
    private PlayerMovement playerMovement;
    private AudioManager audioM;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerMovement = other.GetComponent<PlayerMovement>();
            if (playerMovement.cantGetHurt == false) {
                PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
                if (playerHealth != null && playerMovement != null)
                {
                    audioM.Play(AudioManager.SoundType.Hurt);
                    playerHealth.TakeDamage(damageAmount);
                    playerMovement.cantGetHurt = true;
                    playerMovement.cantGetHurtTimer = 1f;
                    if (destroyOnHit) Destroy(gameObject);
                }
            }
            
            
        }
        
    }
    void Start()
    {
        audioM = AudioManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
