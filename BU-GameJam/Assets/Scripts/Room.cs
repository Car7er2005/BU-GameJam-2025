using UnityEngine;

public class Room : MonoBehaviour
{

    public GameObject virtualCam;
    private Vector3 playerPos;
    private bool checkpointSet = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            virtualCam.SetActive(true);
            if (!checkpointSet) {
                playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
                CheckpointManager.Instance.SetPlayerPos(playerPos);
                checkpointSet = true;
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
