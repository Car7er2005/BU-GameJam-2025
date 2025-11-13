using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    // [Start is called once before the first execution of Update after the MonoBehaviour is created]
    [SerializeField] private GameObject deathPanel;
    public static GameManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // CRITICAL: Keep GameManager active across scene loads for reset tracking
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    // --- EndGame is now the first step of the reset process ---
    public void EndGame()
    {
        Debug.Log("Game Over");

        // This sets up the paused state and shows the UI
        deathPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    // --- REMOVE THE OLD RestartGame() METHOD ---
    // The functionality is now handled by CheckpointManager.InitiateSceneReset()

    // --- Update Method ---
    void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            if (CheckpointManager.Instance != null)
            {
                // This call starts the sequence: Set flag -> Reload Scene -> Warp Player on load
                CheckpointManager.Instance.InitiateSceneReset();
            }
        }
    }
}