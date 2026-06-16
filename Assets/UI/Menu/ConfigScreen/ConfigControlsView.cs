using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable] 
public class ConfigControlsView
{
    private VisualElement root;
    public void Inicializar(VisualElement container)
    {
        root = container;
        root.Clear(); 

        GerarLinha(Resources.Load<Sprite>("IconesControles/keyboard/keyboard-solid/keyboard-wasd"), "Movimentação", true);

        // Linhas normais para as teclas individuais
        GerarLinha(Resources.Load<Sprite>("IconesControles/keyboard/keyboard-solid/space"), "Pular / Saltar", false);
        GerarLinha(Resources.Load<Sprite>("IconesControles/keyboard/keyboard-solid/shift"), "Dash", false);
        GerarLinha(Resources.Load<Sprite>("IconesControles/keyboard/keyboard-solid/e"), "Ação", false);
        GerarLinha(Resources.Load<Sprite>("IconesControles/keyboard/keyboard-solid/q"), "Descartar", false);
    }

    private void GerarLinha(Sprite icone, string acao, bool ehLargo)
        {
            if (icone == null) return;

            VisualElement linha = new VisualElement();
            linha.AddToClassList("linha-controle");

            Label labelAcao = new Label(acao);
            labelAcao.AddToClassList("texto-acao");

            VisualElement wrapperDireita = new VisualElement();
            wrapperDireita.AddToClassList("tecla-wrapper-direita");

            VisualElement iconeElement = new VisualElement();
            if (ehLargo)
                iconeElement.AddToClassList("tecla-container-larga");
            else
                iconeElement.AddToClassList("tecla-container");


            iconeElement.style.backgroundImage = new StyleBackground(icone);

            linha.Add(labelAcao);         
            wrapperDireita.Add(iconeElement); 
            linha.Add(wrapperDireita);

            root.Add(linha);
        }
}