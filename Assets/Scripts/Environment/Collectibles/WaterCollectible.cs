using UnityEngine;

public class WaterCollectible : MonoBehaviour
{
    private bool playerContact = false;
    private InputSystem_Actions controls;

    void Awake()
    {
        controls = new InputSystem_Actions();
        
        controls.Player.Interact.performed += context => TryCollectWater();
        controls.Player.Discard.performed += context => DiscardWater();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void TryCollectWater()
    {        
        if (!playerContact) return;

        if (PlayerState.Instance != null && !PlayerState.Instance.CurrentWaterStatus())
        {
            PlayerState.Instance.CollectWater();               
            UIManager.Instance.ShowMessage("Você coletou água!"); 
            Debug.Log("Item de água coletado!");
        }
    }

    private void DiscardWater()
    {        
        if (PlayerState.Instance != null)
        {
            PlayerState.Instance.DiscardWater();               
            UIManager.Instance.ShowMessage("Você descartou a água!"); 
            Debug.Log("Item de água descartado!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Entrou na área de colisão com a água!");
            playerContact = true;  
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerContact = false;
        }
    }
}