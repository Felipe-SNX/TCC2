using UnityEngine;

public class WaterCollectible : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent<PlayerState>(out PlayerState player))
            {
                player.CollectWater();               
                UIManager.Instance.ShowMessage("Você coletou água!"); 
                Debug.Log("Item de água coletado!");
                Destroy(gameObject);
            }
        }
    }
}