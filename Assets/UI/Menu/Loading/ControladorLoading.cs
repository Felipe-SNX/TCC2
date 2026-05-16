using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class ControladorLoading : MonoBehaviour
{
    private VisualElement barraPreenchimento;
    private Label txtPorcentagem;
    private bool carregamentoConcluido = false;

    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        barraPreenchimento = root.Q<VisualElement>("barra-preenchimento");
        txtPorcentagem = root.Q<Label>("txt-porcentagem");

        string cenaAlvo = string.IsNullOrEmpty(DadosGlobais.proximaCena) ? "Map_Green" : DadosGlobais.proximaCena;

        StartCoroutine(CarregarCenaAsync(cenaAlvo));
    }

    private IEnumerator CarregarCenaAsync(string nomeCena)
    {
        yield return new WaitForSecondsRealtime(0.5f);
        
        AsyncOperation operacao = SceneManager.LoadSceneAsync(nomeCena);
        operacao.allowSceneActivation = false;

        while (!operacao.isDone)
        {
            float progresso = Mathf.Clamp01(operacao.progress / 0.9f);
            
            if (operacao.progress < 0.9f)
            {
                barraPreenchimento.style.width = Length.Percent(progresso * 100);
                txtPorcentagem.text = Mathf.RoundToInt(progresso * 100) + "%";
            }
            else
            {
                barraPreenchimento.style.width = Length.Percent(100);
                txtPorcentagem.text = "PRESSIONE QUALQUER TECLA";

                if (!carregamentoConcluido)
                {
                    yield return new WaitForSecondsRealtime(0.1f);
                    carregamentoConcluido = true;
                }

                if (carregamentoConcluido && (Input.anyKeyDown || Input.GetMouseButtonDown(0)))
                {
                    operacao.allowSceneActivation = true;
                }
            }

            yield return null;
        }
    }
}