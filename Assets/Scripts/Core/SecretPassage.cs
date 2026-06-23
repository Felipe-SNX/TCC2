using System.Collections;
using UnityEngine;

public class SecretPassage : MonoBehaviour
{
    private Renderer rend;
    
    [Header("Configurações do Fade")]
    public float fadeDuration = 0.5f;

    public float minimumAlpha = 0f;

    void Start()
    {
        // Pega automaticamente o SpriteRenderer ou TilemapRenderer do objeto
        rend = GetComponent<Renderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Quando o Player encostar na parede falsa, ela fica transparente
        if (collision.CompareTag("Player"))
        {
            StopAllCoroutines(); 
            StartCoroutine(FadeIn(minimumAlpha));
        }
    }

    IEnumerator FadeIn(float alphaAlvo)
    {
        Color currentColor = rend.material.color;
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            
            // Calcula a porcentagem do fade
            float novoAlpha = Mathf.Lerp(currentColor.a, alphaAlvo, time / fadeDuration);
            
            // Aplica a nova cor com a transparência alterada
            rend.material.color = new Color(currentColor.r, currentColor.g, currentColor.b, novoAlpha);
            
            yield return null; 
        }

        // Garante que terminou no valor exato no final
        rend.material.color = new Color(currentColor.r, currentColor.g, currentColor.b, alphaAlvo);
    }
}
