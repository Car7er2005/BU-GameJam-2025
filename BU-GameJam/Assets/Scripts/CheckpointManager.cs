using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointManager : MonoBehaviour
{

    public static CheckpointManager Instance;
    private GameObject player;
    private Vector3 playerSpawnPos;
    private Vector3 cameraTargetPos;
    private GameObject activeCameraObject;
    private bool shouldRespawn = false;
    public void SetPlayerPos(Vector3 pos) {
        playerSpawnPos = pos;
    }
    public void SetCameraPos(Vector3 pos)
    {
        cameraTargetPos = pos;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
        // 1. Find the new player and the new VCam object in the freshly loaded scene
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            player.transform.position = playerSpawnPos;
            player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;

            // Re-activate the VCam by searching the scene for the saved VCam object name
            if (activeCameraObject != null)
            {
                GameObject newVcam = GameObject.Find(activeCameraObject.name);
                if (newVcam != null)
                {
                    newVcam.SetActive(true);
                }
            }
        }

        // 3. Reset the flag and time
        shouldRespawn = false;
        Time.timeScale = 1f;
        // Optionally: Restore health here
    }

    // (Keep SetCheckpoint logic the same)
    public void SetCheckpoint(Vector3 newPlayerPos, GameObject vCamObject)
    {
        playerSpawnPos = newPlayerPos;
        activeCameraObject = vCamObject;
    }
}
