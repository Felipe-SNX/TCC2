using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AbilityPulseEffect : MonoBehaviour
{
    public static AbilityPulseEffect Instance;

    [SerializeField] private Image top;
    [SerializeField] private Image bottom;
    [SerializeField] private Image left;
    [SerializeField] private Image right;

    [SerializeField] private float duration = 0.35f;
    [SerializeField] private float thickness = 120f;

    private void Awake()
    {
        Instance = this;

        SetAlpha(0f);
    }

    private void SetAlpha(float alpha)
    {
        Color c;

        c = top.color;
        c.a = alpha;
        top.color = c;

        c = bottom.color;
        c.a = alpha;
        bottom.color = c;

        c = left.color;
        c.a = alpha;
        left.color = c;

        c = right.color;
        c.a = alpha;
        right.color = c;
    }

    public void PlayPulse(Color color)
    {
        StopAllCoroutines();
        StartCoroutine(PulseRoutine(color));
    }

    private IEnumerator PulseRoutine(Color color)
    {
        top.color = color;
        bottom.color = color;
        left.color = color;
        right.color = color;

        float fadeInTime = duration * 0.3f;
        float fadeOutTime = duration * 0.7f;

        // Fade In
        float t = 0f;

        while (t < fadeInTime)
        {
            t += Time.unscaledDeltaTime;

            float alpha = Mathf.Lerp(0f, 0.8f, t / fadeInTime);

            SetAlpha(alpha);

            yield return null;
        }

        // Fade Out
        t = 0f;

        while (t < fadeOutTime)
        {
            t += Time.unscaledDeltaTime;

            float alpha = Mathf.Lerp(0.8f, 0f, t / fadeOutTime);

            SetAlpha(alpha);

            yield return null;
        }

        SetAlpha(0f);
    }
    public void PlayAbilityPulse(AbilityType ability)
    {
        switch (ability)
        {
            case AbilityType.Plant:
                PlayPulse(Color.green);
                break;

            case AbilityType.Dash:
                PlayPulse(Color.red);
                break;

            case AbilityType.Flashlight:
                PlayPulse(Color.yellow);
                break;
        }
    }
}