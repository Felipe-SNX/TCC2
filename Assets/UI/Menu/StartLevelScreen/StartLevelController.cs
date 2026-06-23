using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.UI.Menu.StartLevelScreen
{
    public class StartLevelController : MonoBehaviour
    {
        [Header("Configurações da Fase")]
        public string nomeDaFase = "Fase 1: O Início";
        public string corFase = "Verde";

        private Label txtNomeFase;
        private Label txtAnuncio;
        private VisualElement banner;
        private VisualElement root;

        private void OnEnable()
        {
            root = GetComponent<UIDocument>().rootVisualElement;
            
            txtNomeFase = root.Q<Label>("txt-nome-fase");
            txtAnuncio = root.Q<Label>("txt-anuncio");
            banner = root.Q<VisualElement>(className: "banner-anuncio"); 

            if (txtNomeFase != null)
            {
                txtNomeFase.text = nomeDaFase.ToUpper(); 
            }

            Color corDaFase = ObterCorDaFaseAtual();

            AplicarCorDoBanner(corDaFase);

            Time.timeScale = 0f; 
            StartCoroutine(SequenciaReadyGo(corDaFase));
        }

        private IEnumerator SequenciaReadyGo(Color corDaFase)
        {
            txtAnuncio.text = "PREPARA";
            txtAnuncio.style.color = new StyleColor(Color.white);
            
            yield return new WaitForSecondsRealtime(1.5f); 

            txtAnuncio.text = "VAI!";
            
            txtAnuncio.style.color = new StyleColor(corDaFase);
            
            txtAnuncio.style.scale = new StyleScale(new Vector2(1.1f, 1.1f));

            yield return new WaitForSecondsRealtime(1f);

            root.style.display = DisplayStyle.None; 
            Time.timeScale = 1f; 
        }

        private Color ObterCorDaFaseAtual()
        {
            if (corFase.Equals("Verde")) return new Color(0.2f, 0.8f, 0.2f); 
            if (corFase.Equals("Vermelho")) return new Color(0.9f, 0.2f, 0.2f); 
            if (corFase.Equals("Azul")) return new Color(0.2f, 0.6f, 1.0f); 
            if (corFase.Equals("Amarelo")) return new Color(1.0f, 0.85f, 0.0f); 

            Color corPadrao;
            ColorUtility.TryParseHtmlString("#FFD700", out corPadrao);
            return corPadrao; 
        }

        private void AplicarCorDoBanner(Color cor)
        {
            if (banner != null)
            {
                banner.style.borderTopColor = new StyleColor(cor);
                banner.style.borderBottomColor = new StyleColor(cor);
            }
        }
    }
}