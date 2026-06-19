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

        // Adiciona um pequeno respiro no topo do ScrollView
        root.style.paddingTop = 15;

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
        labelAcao.AddToClassList("labels"); 

        VisualElement wrapperDireita = new VisualElement();
        wrapperDireita.AddToClassList("tecla-wrapper-direita");

        VisualElement iconeElement = new VisualElement();
        
        iconeElement.style.width = ehLargo ? 140 : 50;
        iconeElement.style.height = 50;
        iconeElement.style.flexShrink = 0; 
        iconeElement.style.unityBackgroundScaleMode = ScaleMode.ScaleToFit; 

        if (ehLargo)
            iconeElement.AddToClassList("tecla-container-larga");
        else
            iconeElement.AddToClassList("tecla-container");

        iconeElement.AddToClassList("tecla-neon"); 
        iconeElement.AddToClassList("icone-cromatico"); 

        iconeElement.style.backgroundImage = new StyleBackground(icone);

        linha.Add(labelAcao);        
        wrapperDireita.Add(iconeElement); 
        linha.Add(wrapperDireita);

        root.Add(linha);
    }
}