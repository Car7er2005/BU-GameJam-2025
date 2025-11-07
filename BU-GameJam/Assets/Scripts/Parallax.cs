using NUnit.Framework;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    Transform cam;
    Vector3 camStartPos;
    float dist;

    GameObject[] backgrounds;
    Material[] mats;
    float[] backSpeed;

    float farthestBack;

    [UnityEngine.Range(0f, 1f)]
    public float parallaxSpeed;

    void Start()
    {
        cam = Camera.main.transform;
        camStartPos = cam.position;

        int backCount = transform.childCount;
        mats = new Material[backCount];
        backSpeed = new float[backCount];
        backgrounds = new GameObject[backCount];

        for(int i = 0;i<backCount; i++)
        {
            backgrounds[i] = transform.GetChild(i).gameObject;
            mats[i] = backgrounds[i].GetComponent<Renderer>().material;
            
        }
        BackSpeedCalc(backCount);
    }

    void BackSpeedCalc(int backcount)
    {
        for (int i = 0; i < backcount; i++)     //find furthest back
        {
            if ((backgrounds[i].transform.position.z - cam.position.z) > farthestBack)
            {
                farthestBack = backgrounds[i].transform.position.z - cam.position.z;
            }
        }

        for (int i = 0; i < backcount; i++)     //calculate speed based on distance from camera
        {
            backSpeed[i] = 1 - ((backgrounds[i].transform.position.z - cam.position.z) / farthestBack);
        }
    }

    private void LateUpdate()
    {
        float distance = cam.position.x - camStartPos.x;
        transform.position = new Vector3(cam.position.x, transform.position.y, 0);

        for(int i = 0; i < backgrounds.Length; i++)
        {
            float parallax = distance * backSpeed[i] * parallaxSpeed;
            Vector3 bgTargetPos = new Vector3(backgrounds[i].transform.position.x + parallax, backgrounds[i].transform.position.y, backgrounds[i].transform.position.z);
            backgrounds[i].transform.position = bgTargetPos;

            /*
             * float speed = backSpeed[i] * parallaxSpeed;
             * mat[i].SetTextureOffset("_MainTex", new Vector2(distance * speed, 0));
             */
        }
    }
}
