using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractSystem : MonoBehaviour
{
    public Transform detectionPoint;
    public const float detectionRadius = 0.5f;
    public LayerMask interactableLayer;

    public GameObject detectedObject;

    public InputAction interactAction;

    public bool isGrabbing;
    public bool isUsing;

    void Update()
    {
    }

    public void Interact(InputAction.CallbackContext context)
    {
        
        Debug.Log("Interact action triggered"); 
        if (context.performed && DetectObject())
            if (detectedObject.layer == LayerMask.NameToLayer("Interactable")) { 
                detectedObject.GetComponent<Item>().Interact();
                Debug.Log("Interacted");
            }
        else if(context.performed && !DetectObject())
            Debug.Log("No object to interact with");
    }

    bool DetectObject()
    {
        Collider2D obj = Physics2D.OverlapCircle(detectionPoint.position, detectionRadius, interactableLayer);
        if(obj != null)
        {
            Debug.Log("Object detected: " + obj.gameObject.name);
            detectedObject = obj.gameObject;
            return true;
        }
        else
        {
            Debug.Log("No object detected");
            detectedObject = null;
            return false;
        }
    }

    public void PickUpItem(GameObject item)
    {
        // Implement pickup logic here
    }

    public void UseItem(GameObject item)
    {
        CustomEvent.Trigger(item, "OnUse");
        
    }
}
