using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject deathPanel;
    public static GameManager Instance;

    void Awake()
    {
        // If a GameManager already exists (e.g. from a previous scene load), 
        // we destroy THIS new one to prevent duplicates.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return; // Stop doing anything else
        }

        // Otherwise, I am the manager
        Instance = this;

        // IMPORTANT: Do NOT use DontDestroyOnLoad(gameObject);
        // We WANT this manager to die when the scene reloads so a new one
        // can be born and link to the new DeathPanel automatically.
    }

    public void EndGame()
    {
        if (deathPanel != null)
        {
            deathPanel.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            Debug.LogError("GameManager: Death Panel is missing! Check the Inspector.");
        }
    }

    // Called by the UI "Restart" button (Full Reset)
    public void RestartGame()
    {
        deathPanel.SetActive(false);
        Time.timeScale = 1f;
        // This reloads the scene completely (good for game over)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Called by 'R' key (Checkpoint Reset)
    public void ResetToLastCheckpoint()
    {
        if (CheckpointManager.Instance != null)
        {
            // This tells the manager to handle the reload + warp logic
            CheckpointManager.Instance.InitiateSceneReset();
        }
        deathPanel.SetActive(false);
        Time.timeScale = 1f;
    }
    void Update()
    {
        // We use GetKey instead of wasPressedThisFrame because timeScale = 0 might skip frames.
        // However, since the Input System is polled by Unity, let's use the robust check:

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            //if (Time.timeScale == 0f && deathPanel.activeSelf)
            //{
                if (CheckpointManager.Instance != null)
                {
                    // Call the Checkpoint Manager to handle the full scene reset and warp
                    CheckpointManager.Instance.InitiateSceneReset();
                }
            //}
        }
    }
}