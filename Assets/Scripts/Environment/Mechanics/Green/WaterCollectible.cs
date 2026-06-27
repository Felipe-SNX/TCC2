using UnityEngine;

public class WaterCollectible : MonoBehaviour
{
    private bool playerContact = false;
    private InputSystem_Actions controls;

    private void Awake()
    {
        controls = new InputSystem_Actions();

        controls.Player.Interact.performed += context => TryCollectWater();
        controls.Player.Discard.performed += context => DiscardWater();
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    private void TryCollectWater()
    {
        if (!playerContact) return;
        if (PlayerState.Instance == null) return;

        if (PlayerState.Instance.CurrentWaterStatus())
        {
            if (UIManager.Instance != null) UIManager.Instance.ShowMessage("Você já está carregando água!");
            return;
        }

        PlayerState.Instance.CollectWater();

        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowMessage("Você coletou água!");
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerAudioManager playerAudio = player.GetComponent<PlayerAudioManager>();
            if (playerAudio != null)
            {
                playerAudio.PlayCollectWater();
            }
        }
    }

    private void DiscardWater()
    {
        if (PlayerState.Instance == null || !PlayerState.Instance.CurrentWaterStatus()) return;

        PlayerState.Instance.DiscardWater();
        if (UIManager.Instance != null) UIManager.Instance.ShowMessage("Você descartou a água!");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerContact = true;
            if (UIManager.Instance != null) UIManager.Instance.ShowMessage("Pressione E para coletar água.");
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