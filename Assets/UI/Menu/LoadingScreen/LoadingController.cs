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

        private VisualElement root;
        private VisualElement mascaraTitulo;
        private VisualElement barraPreenchimento;
        private Label promptText;

        private AsyncOperation operacaoCarregamento;
        private bool carregamentoCompleto = false;

        private void Start()
        {
            if(gridFase1 != null && GlobalData.nextScene == "Map_Green")
            {
                gridFase1.SetActive(true);
            }
            else
            {
                gridFase2.SetActive(true);
            }
        }

        private void OnEnable()
        {
            root = GetComponent<UIDocument>().rootVisualElement;

            mascaraTitulo = root.Q<VisualElement>("mascara-titulo");
            barraPreenchimento = root.Q<VisualElement>("barra-preenchimento");
            promptText = root.Q<Label>("prompt-text");

            if (mascaraTitulo != null) mascaraTitulo.style.width = Length.Percent(0);
            if (barraPreenchimento != null) barraPreenchimento.style.width = Length.Percent(0);
            if (promptText != null) promptText.AddToClassList("oculto");

            StartCoroutine(CarregarCenaAssincronamente(GlobalData.nextScene));
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
            
            // Impede que a cena ligue automaticamente quando chegar em 100%
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