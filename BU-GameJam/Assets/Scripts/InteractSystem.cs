using UnityEngine;
using UnityEngine.InputSystem;

public class InteractSystem : MonoBehaviour
{
    public Transform detectionPoint;
    public const float detectionRadius = 0.5f;
    public LayerMask interactableLayer;

    public InputAction interactAction;

    void Update()
    {
        
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if(context.performed && DetectObject())
            Debug.Log("Interacted");
        else if(context.performed && !DetectObject())
            Debug.Log("No object to interact with");
    }

    bool DetectObject()
    {
        bool isDetected = Physics2D.OverlapCircle(detectionPoint.position,detectionRadius, interactableLayer);
        return isDetected;
    }
}
