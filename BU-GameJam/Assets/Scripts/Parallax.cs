using NUnit.Framework;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float startPos, length;
    public GameObject cam;
    public float parallaxEffect;

    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate()
    {
        // calculate the distance the camera has moved multiplied by the parallax effect
        float dist = cam.transform.position.x * parallaxEffect;
        float movement = cam.transform.position.x * (1 - parallaxEffect);

        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);

        // if the background has reached the end of its length, ajust its position for infinite scrolling
        if (movement > startPos + length) startPos += length;
        else if (movement < startPos - length) startPos -= length;
    }
}
