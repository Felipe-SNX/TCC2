using UnityEngine;

public class WaterCollectible : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerState player = other.GetComponent<PlayerState>();

            if (player != null)
            {
                player.CollectWater();
                Destroy(gameObject);
            }
        }
    }
}