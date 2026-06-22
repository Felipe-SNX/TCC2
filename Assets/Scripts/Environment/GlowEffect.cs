using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GlowEffect : MonoBehaviour
{
    [SerializeField]
    private Light2D glow;

    [SerializeField]
    private float speed = 2f;

    [SerializeField]
    private float min = .3f;

    [SerializeField]
    private float max = .8f;


    void Update()
    {
        glow.intensity =
            Mathf.Lerp(
                min,
                max,
                (Mathf.Sin(Time.time * speed) + 1) / 2
            );
    }


    public void DisableGlow()
    {
        glow.enabled = false;
    }
}