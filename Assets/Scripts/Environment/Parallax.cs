using UnityEngine;


public class Parallax : MonoBehaviour
{
    private float lenght, startPos;
    public GameObject cam;
    public float parallaxEffect;
    void Start()
    {
        startPos = transform.position.x;
        lenght = GetComponent<SpriteRenderer>().bounds.size.x;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float dist = cam.transform.position.x * parallaxEffect;
        float movement = cam.transform.position.x * (1 - parallaxEffect);

        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z );

        if(movement > startPos + lenght)
        {
            startPos += lenght;
        }
        else if (movement < startPos - lenght)
        {
            startPos -= lenght;
        }
    }
}
