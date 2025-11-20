using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointManager : MonoBehaviour
{

    public static CheckpointManager Instance;
    private GameObject player;
    private Vector3 playerSpawnPos;
    private string activeCameraName;
    private bool shouldRespawn = false;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void SetCheckpoint(Vector3 newPlayerPos, GameObject vCamObject) {
        playerSpawnPos = newPlayerPos;
        if (vCamObject != null) {
            activeCameraName = vCamObject.name;

        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Execute the teleport/reset ONLY if the reset process was initiated (e.g., player died)
        if (shouldRespawn)
        {
            ResetPlayerState();
        }
    }

    public void InitiateSceneReset()
    {
        // 1. Set the flag to true
        shouldRespawn = true;

        // 2. Immediately start the scene reload
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ResetPlayerState()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            // 1. Teleport Player
            player.transform.position = playerSpawnPos;

            // 2. Stop Physics (prevent sliding after teleport)
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null) rb.linearVelocity = Vector2.zero;

            // 3. Re-activate the VCam by name
            if (!string.IsNullOrEmpty(activeCameraName))
            {
                // Find the NEW camera in the NEW scene that has the SAME name
                GameObject newVcam = GameObject.Find(activeCameraName);
                if (newVcam != null)
                {
                    newVcam.SetActive(true);
                }
            }
        }

        shouldRespawn = false;
        Time.timeScale = 1f;
    }
}
