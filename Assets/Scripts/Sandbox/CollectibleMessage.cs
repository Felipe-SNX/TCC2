using UnityEngine;

public class CollectibleMessage : MonoBehaviour
{
    [TextArea]
    public string messagePart;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Colidiu com player!");
            MessageManager.Instance.Collect(messagePart);
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (transform.position.y < Camera.main.transform.position.y - 10f)
        {
            Destroy(gameObject);
        }
    }
}
