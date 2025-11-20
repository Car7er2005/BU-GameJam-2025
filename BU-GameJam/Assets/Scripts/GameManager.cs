using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject deathPanel;
    public static GameManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void EndGame()
    {
        deathPanel.SetActive(true);
        Time.timeScale = 0f;
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
            // If the game is paused (DeathPanel active) AND R is pressed, initiate reset.
            if (Time.timeScale == 0f && deathPanel.activeSelf)
            {
                // This call starts the sequence: Unpause Time -> Reload Scene -> Warp Player
                if (CheckpointManager.Instance != null)
                {
                    // Call the Checkpoint Manager to handle the full scene reset and warp
                    CheckpointManager.Instance.InitiateSceneReset();
                }
            }
        }
    }
}