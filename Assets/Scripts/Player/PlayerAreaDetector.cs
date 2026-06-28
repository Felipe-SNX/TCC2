using UnityEngine;

public class PlayerAreaDetector : MonoBehaviour
{
    [Header("Detecção")]
    [SerializeField] private LayerMask vineAreaLayer;
    [SerializeField] private float areaCheckRadius = 0.25f;

    public bool IsInWater { get; private set; }
    public bool IsInVine { get; private set; }
    private int waterContacts = 0;
    private int vineContacts = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("WaterArea")) { waterContacts++; IsInWater = waterContacts > 0; }
        if (other.CompareTag("VineArea")) { vineContacts++; IsInVine = vineContacts > 0; }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("WaterArea")) { waterContacts = Mathf.Max(0, waterContacts - 1); IsInWater = waterContacts > 0; }
        if (other.CompareTag("VineArea")) { vineContacts = Mathf.Max(0, vineContacts - 1); IsInVine = vineContacts > 0; }
    }
    
    public bool IsTouchingVine() => Physics2D.OverlapCircle(transform.position, areaCheckRadius, vineAreaLayer);
}