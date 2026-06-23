using UnityEngine;

namespace Assets.UI.Menu
{
    public class CameraMenuScript : MonoBehaviour
    {
        public static CameraMenuScript Instance { get; private set; }

        [Header("Configurações do Fundo")]
        [SerializeField] private float velocidade = 2f;
        [SerializeField] private float limiteY = 80f;
        [SerializeField] private float limiteX = 80f;

        private bool changeGrid = true;
        
        private Vector3 posicaoInicial;
        private int direcao = 1;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        private void Start()
        {
            posicaoInicial = transform.position;
        }

        private void Update()
        {
            if (changeGrid)
            {
                MoveX();
            }
            else
            {
                MoveY();
            }
        }

        private void MoveX()
        {
            transform.Translate(Vector3.right * velocidade * direcao * Time.deltaTime);

            if (transform.position.x > limiteX)
            {
                direcao = -1; 
            }
    
            else if (transform.position.x < posicaoInicial.x)
            {
                direcao = 1; 
            }
        }

        private void MoveY()
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

        public void IsChangeGrid()
        {
            changeGrid = !changeGrid;
            transform.position = posicaoInicial;
            direcao = 1;
        }
    }
}