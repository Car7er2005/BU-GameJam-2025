using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class Warp : MonoBehaviour
{
    public PlayerMovement pm;

    public bool LargeMirror = false;    // if the large warp ability is unlocked
    public bool SmallMirror = false;    // if the small warp ability is unlocked
    // 0 = small, 1 = normal, 2 = large
    public int currentSize = 1; 

    private void Awake()
    {
        // Try to auto-assign PlayerMovement if not set in inspector
        if (pm == null)
        {
            pm = GetComponent<PlayerMovement>()
                 ?? GetComponentInParent<PlayerMovement>()
                 ?? GetComponentInChildren<PlayerMovement>();

            if (pm == null)
                Debug.LogError($"Warp: PlayerMovement reference not found on '{name}'. Assign it in the inspector or ensure a PlayerMovement exists in the scene.");
            else
                Debug.Log($"Warp: Auto-assigned PlayerMovement on '{pm.gameObject.name}'.");
        }
    }

    private void Start()
    {
        // Sync currentSize to transform scale to avoid inconsistent state
        var s = transform.localScale;
        // Heuristic based on expected scales used in this script
        if (Approximately(s, new Vector3(12f, 12f, 1f))) currentSize = 2;
        else if (Approximately(s, new Vector3(3f, 3f, 1f))) currentSize = 0;
        else currentSize = 1;
    }

    // Helper to compare vectors with tolerance
    private bool Approximately(Vector3 a, Vector3 b, float tol = 0.01f)
    {
        return Mathf.Abs(a.x - b.x) < tol && Mathf.Abs(a.y - b.y) < tol && Mathf.Abs(a.z - b.z) < tol;
    }

    void Update()
    {
        
    }

    public void WarpL(InputAction.CallbackContext context)
    {
        Debug.Log("WarpL action triggered");
        if (!context.performed) return;

        /*
        if (!LargeMirror)
        {
            Debug.Log("Large warp ability not unlocked.");
            return;
        }
        */

        if (currentSize == 1 && LargeMirror)
        {
            // Warp to large size
            transform.localScale = new Vector3(12f, 12f, 1f);
            currentSize = 2;
            if (pm != null) pm.JumpPower = 16f; // Increase jump power when large
            else Debug.LogWarning("WarpL: PlayerMovement (pm) is null; cannot set jump power.");
            Debug.Log("Warped to large size.");
        }
        else if (currentSize == 0 && SmallMirror)
        {
            // Warp to normal size from small
            transform.localScale = new Vector3(6.5f, 6.5f, 1f);
            currentSize = 1;
            if (pm != null) pm.JumpPower = 11f; // Reset jump power to normal
            else Debug.LogWarning("WarpL: PlayerMovement (pm) is null; cannot set jump power.");
            Debug.Log("Warped from small to normal size.");
        }
        else
        {
            Debug.Log("Already at largest size, or don't have the right mirror");
        }

        if(!pm.isFacingRight) // Maintain facing direction after warp
        {
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    public void WarpS(InputAction.CallbackContext context)
    {
        Debug.Log("WarpS action triggered");
        if (!context.performed) return;

        /*
        if (!SmallMirror)
        {
            Debug.Log("Small warp ability not unlocked.");
            return;
        }
        */

        if (currentSize == 1)
        {
            // Warp to small size
            transform.localScale = new Vector3(3f, 3f, 1f);
            currentSize = 0;
            if (pm != null) pm.JumpPower = 9f; // Decrease jump power when small
            else Debug.LogWarning("WarpS: PlayerMovement (pm) is null; cannot set jump power.");
            Debug.Log("Warped to small size.");
        }
        else if (currentSize == 2)
        {
            // Warp to normal size from large
            transform.localScale = new Vector3(6.5f, 6.5f, 1f);
            currentSize = 1;
            if (pm != null) pm.JumpPower = 11f; // Reset jump power to normal
            else Debug.LogWarning("WarpS: PlayerMovement (pm) is null; cannot set jump power.");
            Debug.Log("Warped from large to normal size.");
        }
        else
        {
            Debug.Log("Already at smallest size, or don't have the right mirror");
        }

        if (!pm.isFacingRight) // Maintain facing direction after warp
        {
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    public void setLargeMirror(bool isActive)
    {
        LargeMirror = isActive;
    }

    public void setSmallMirror(bool isActive)
    {
        SmallMirror = isActive;
    }
}
