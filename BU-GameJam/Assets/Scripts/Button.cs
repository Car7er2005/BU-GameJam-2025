using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour
{
    public UnityEvent onActivate;
    public UnityEvent onDeactivate;

    public float deactivationDelay = 0f;

    int objectsInContact;
    Coroutine waitingToDeactivate;

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(deactivationDelay);
        
        if (onDeactivate != null)
            onDeactivate.Invoke();

        waitingToDeactivate = null;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        objectsInContact++;
        if(objectsInContact == 1)
        {
            if (waitingToDeactivate != null)
            {
                StopCoroutine(waitingToDeactivate);
                waitingToDeactivate = null;
            }
            else if (onActivate != null)
                onActivate.Invoke();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        objectsInContact--;
        if(objectsInContact == 0 && deactivationDelay > 0f)
        {
            waitingToDeactivate = StartCoroutine(Timer());
        }
        else if(objectsInContact == 0)
        {
            if (onDeactivate != null)
                onDeactivate.Invoke();
        }
    }
}
