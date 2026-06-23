using UnityEngine;

namespace Assets.UI.Menu
{
    public class ChangeGridScript : MonoBehaviour
    {
        [Header("Grids de Fundo")]
        [SerializeField] private GameObject gridFase1;
        [SerializeField] private GameObject gridFase2;

        [Header("Configurações")]
        [SerializeField] private float tempoDeTroca = 30f; 

        private float cronometro = 0f;
        private bool fase1Ativa = true;

        private void Start()
        {
            if (gridFase1 != null) gridFase1.SetActive(true);
            if (gridFase2 != null) gridFase2.SetActive(false);
        }

        private void Update()
        {
            cronometro += Time.deltaTime;

            if (cronometro >= tempoDeTroca)
            {
                TrocarGrids();
                cronometro = 0f; 
            }
        }

        private void TrocarGrids()
        {
            fase1Ativa = !fase1Ativa;

            if (gridFase1 != null) gridFase1.SetActive(fase1Ativa);
            if (gridFase2 != null) gridFase2.SetActive(!fase1Ativa);

            CameraMenuScript.Instance?.IsChangeGrid();
        }
    }
}