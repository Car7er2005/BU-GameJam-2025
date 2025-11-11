using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionSystem : MonoBehaviour
{
    public Transform detectionPoint;
    private const float detectionRadius = 0.2f;
    public LayerMask detectionLayer;

    public InputAction interactAction;
    void Update()
    {
        if(DetectObject())
        {
            Debug.Log("Object Detected");
        }
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (DetectObject())
            {
                Debug.Log("Interacted with object");
            }
        }
    }

    bool DetectObject()
    {        
        bool isDetected = Physics2D.OverlapCircle(detectionPoint.position, detectionRadius, detectionLayer);
        return isDetected;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(detectionPoint.position, detectionRadius);
    }

}
