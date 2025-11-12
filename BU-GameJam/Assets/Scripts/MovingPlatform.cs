using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public List<Transform> points;  // Points the platform will move between
    public Transform platform;     // Reference to the platform itself
    int goalPointIndex = 0; // Index of the current target point
    public float speed = 2f;        // Speed of the platform
    [SerializeField] public bool active = true;     // Whether the platform is active

    void Update()
    {
        if (active)
        {
            MoveToNextPoint();
        }        
    }

    void MoveToNextPoint()
    {
        platform.position = Vector2.MoveTowards(platform.position, points[goalPointIndex].position, speed * Time.deltaTime);
        if (Vector2.Distance(platform.position, points[goalPointIndex].position) < 0.1f)
        {
            goalPointIndex = (goalPointIndex + 1) % points.Count; // Loop back to the first point
        }
    }

    void SetActive(bool x)
    {
        active = x;
    }
}
