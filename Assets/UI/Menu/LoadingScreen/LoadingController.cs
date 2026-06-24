using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Assets.UI.Menu.LoadingScreen
{
    public class LoadingController : MonoBehaviour
    {
        [Header("Grids de Fundo")]
        [SerializeField] private GameObject gridFase1; 
        [SerializeField] private GameObject gridFase2; 

        private Color corFloresta;
        private Color corCaverna;

        private VisualElement root;
        private Label textoColorido;
        private VisualElement mascaraTitulo;
        private VisualElement barraPreenchimento;
        private VisualElement barraContainer;
        private Label promptText;

        private AsyncOperation operacaoCarregamento;
        private bool carregamentoCompleto = false;

        private void Awake()
        {
            ColorUtility.TryParseHtmlString("#FFB000", out corFloresta);
            ColorUtility.TryParseHtmlString("#00FFFF", out corCaverna);
        }

        private void Start()
        {
            if (gridFase1 != null && GlobalData.nextScene == "Map_Green")
            {
                gridFase1.SetActive(true);
            }
            else
            {
                if (gridFase2 != null) gridFase2.SetActive(true);
            }
        }

        private void OnEnable()
        {
            root = GetComponent<UIDocument>().rootVisualElement;

            textoColorido = root.Q<Label>("texto-colorido");
            mascaraTitulo = root.Q<VisualElement>("mascara-titulo");
            barraPreenchimento = root.Q<VisualElement>("barra-preenchimento");
            barraContainer = root.Q<VisualElement>("container-barra");
            promptText = root.Q<Label>("prompt-text");

            Color corEscolhida = (GlobalData.nextScene == "Map_Green") ? corFloresta : corCaverna;
            AplicarCores(corEscolhida);

            if (mascaraTitulo != null) mascaraTitulo.style.width = Length.Percent(0);
            if (barraPreenchimento != null) barraPreenchimento.style.width = Length.Percent(0);
            if (promptText != null) promptText.AddToClassList("oculto");

            StartCoroutine(CarregarCenaAssincronamente(GlobalData.nextScene));
        }
        private void AplicarCores(Color corTema)
        {
            if (textoColorido != null) textoColorido.style.color = corTema;
            
            if (barraPreenchimento != null) barraPreenchimento.style.backgroundColor = corTema;
            
            if (barraContainer != null) 
            {
                barraContainer.style.borderTopColor = corTema;
                barraContainer.style.borderBottomColor = corTema;
                barraContainer.style.borderLeftColor = corTema;
                barraContainer.style.borderRightColor = corTema;
            }

            if (promptText != null) promptText.style.color = Color.white; 
        }

        private void Update()
        {
            if (root == null || root.style.display == DisplayStyle.None) return;

            if (carregamentoCompleto && promptText != null)
            {
                float alpha = (Mathf.Sin(Time.time * 4f) + 1f) / 2f; 
                promptText.style.opacity = Mathf.Lerp(0.3f, 1f, alpha);

                if (Input.anyKeyDown)
                {
                    FinalizarCarregamento();
                }
            }
        }

        private IEnumerator CarregarCenaAssincronamente(string nomeCena)
        {
            yield return new WaitForSeconds((float)0.5); 

            operacaoCarregamento = SceneManager.LoadSceneAsync(nomeCena);
            
            operacaoCarregamento.allowSceneActivation = false; 

            while (!operacaoCarregamento.isDone)
            {
                float progresso = Mathf.Clamp01(operacaoCarregamento.progress / 0.9f);
                
                float porcentagem = progresso * 100f;
                if (barraPreenchimento != null) barraPreenchimento.style.width = Length.Percent(porcentagem);
                if (mascaraTitulo != null) mascaraTitulo.style.width = Length.Percent(porcentagem);

                if (operacaoCarregamento.progress >= 0.9f)
                {
                    carregamentoCompleto = true;
                    
                    if (promptText != null) promptText.RemoveFromClassList("oculto");
                    
                    yield break; 
                }

                yield return null; 
            }
        }

        private void FinalizarCarregamento()
        {
            if (operacaoCarregamento != null) operacaoCarregamento.allowSceneActivation = true;
        }
    }
}