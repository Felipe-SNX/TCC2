using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GlowEffect : MonoBehaviour
{
    [Header("Configurações de Luz")]
    [SerializeField] private Light2D glow;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float minIntensity = 0.3f;
    [SerializeField] private float maxIntensity = 0.8f;

    private bool isActive = true;

    void Update()
    {
        if (!isActive || glow == null) return;

        glow.intensity = Mathf.Lerp(
            minIntensity,
            maxIntensity,
            (Mathf.Sin(Time.time * speed) + 1f) / 2f
        );
    }

    public void DisableGlow()
    {
        isActive = false;

        if (glow != null)
        {
            glow.intensity = 0f;
            glow.enabled = false;
        }
    }
}