using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class Item : MonoBehaviour
{
    public enum InteractionType
    {
        Pickup,
        Use,
        Plate
    }
    public enum ItemType
    {
        Static, equippable
    }
    [Header("Attributes")]
    public InteractionType interactType;
    public ItemType type;
    [Header("Exmaine")]
    public string descriptionText;
    [Header("Custom Events")]
    public UnityEvent customEvent;

    public void Interact()
    {
        switch (interactType)
        {
            case InteractionType.Pickup:
                //FindObjectOfType<InteractSystem>().PickUpItem(gameObject);
                gameObject.SetActive(false);
                // Implement pickup logic here
                break;
            case InteractionType.Use:
                Object.FindFirstObjectByType<InteractSystem>().UseItem(gameObject);
                // Implement use logic here
                break;            
            default:
                Debug.Log("Unknown interaction");
                break;
        }

        customEvent.Invoke();
    }

    
}
