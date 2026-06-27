using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GlowEffect : MonoBehaviour
{
    [SerializeField]
    private Light2D glow;

    [SerializeField]
    private float speed = 2f;

    [SerializeField]
    private float minIntensity = .3f;

    [SerializeField]
    private float maxIntensity = .8f;


    private bool active = true;


    void Update()
    {
        if (!active || glow == null)
            return;


        glow.intensity =
            Mathf.Lerp(
                minIntensity,
                maxIntensity,
                (Mathf.Sin(Time.time * speed) + 1) / 2f
            );
    }


    public void DisableGlow()
    {
        active = false;

        if(glow != null)
        {
            glow.intensity = 0;
            glow.enabled = false;
        }
    }
}