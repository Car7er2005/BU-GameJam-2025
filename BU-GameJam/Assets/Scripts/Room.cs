using UnityEngine;

public class Room : MonoBehaviour
{

    public GameObject virtualCam;
    public Transform respawnPoint;

    public AudioManager.SoundType roomMusicTrack;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            virtualCam.SetActive(true);

            if (CheckpointManager.Instance != null && respawnPoint != null) {
                CheckpointManager.Instance.SetCheckpoint(respawnPoint.position, virtualCam);
            }

            if (AudioManager.Instance != null) {
                AudioManager.Instance.ChangeMusic(roomMusicTrack);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            virtualCam.SetActive(false);
        }
    }
}
