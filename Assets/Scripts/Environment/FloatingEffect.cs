using UnityEngine;

public class FloatingEffect : MonoBehaviour
{

    public float speed = 2f;
    public float height = 0.5f;

    private Vector3 startPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float newY = startPosition.y + Mathf.Sin(Time.time * speed) * height;
        transform.position = new Vector3(transform.position.x, newY, 0);
        
    }
}
