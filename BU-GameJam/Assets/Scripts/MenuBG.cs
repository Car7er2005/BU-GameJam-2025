using UnityEngine;

public class MenuBG : MonoBehaviour
{
    private float startPos, length;
    public float parallaxEffect;


    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate()
    {
        float movement = startPos - transform.position.x;

        transform.position += Vector3.left * parallaxEffect * Time.fixedDeltaTime;

        if(transform.position.x <= -length)
        {
            transform.position += new Vector3(length, 0f, 0f);
        }
    }
}
