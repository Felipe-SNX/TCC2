using UnityEngine;

public class CameraMenuScroll : MonoBehaviour
{
    [Header("Configurações do Fundo")]
    [SerializeField] private float velocidade = 2f;
    [SerializeField] private float limiteY = 50f;
    
    private Vector3 posicaoInicial;
    private int direcao = 1;

    private void Start()
    {
        posicaoInicial = transform.position;
    }

    private void Update()
    {
        // Move a câmera para cima, multiplicando pela direção atual
        transform.Translate(Vector3.up * velocidade * direcao * Time.deltaTime);

        // Se a câmera passar do limite superior, muda a direção para baixo
        if (transform.position.y > limiteY)
        {
            direcao = -1; 
        }
        // Se a câmera descer além do ponto onde começou, muda a direção para cima
        else if (transform.position.y < posicaoInicial.y)
        {
            direcao = 1; 
        }
    }
}