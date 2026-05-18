using System;
using UnityEngine;
using UnityEngine.UIElements;

public class ControladorConfiguracoes : MonoBehaviour, IMenuPopup
{
    public Action AoFechar { get; set; }

    [Header("Sub-Sistemas")]
    public ConfigAudioView viewAudio;
    public ConfigControlesView viewControles;
    public ConfigGraficosView viewGraficos;
    
    private Label tituloAba;
    private VisualElement conteudoAudio;
    private VisualElement conteudoControles;
    private VisualElement conteudoGraficos;
    private Button btnAudio;
    private Button btnControles;
    private Button btnGraficos;

    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        tituloAba = root.Q<Label>("titulo-aba");
        conteudoAudio = root.Q<VisualElement>("conteudo-audio");
        conteudoControles = root.Q<VisualElement>("conteudo-controles");
        conteudoGraficos = root.Q<VisualElement>("conteudo-graficos");

        ScrollView listaControles = root.Q<ScrollView>("lista-controles-container");

        if (viewControles != null && listaControles != null)
        {
            viewControles.Inicializar(listaControles);
        }

        btnAudio = root.Q<Button>("btn-aba-audio");
        btnControles = root.Q<Button>("btn-aba-controles");
        btnGraficos = root.Q<Button>("btn-aba-graficos");
        Button btnVoltar = root.Q<Button>("btn-voltar");

        if (viewAudio != null && conteudoAudio != null) viewAudio.Inicializar(conteudoAudio);
        if (viewControles != null && conteudoControles != null) viewControles.Inicializar(conteudoControles);
        if (viewGraficos != null && conteudoGraficos != null) viewGraficos.Inicializar(conteudoGraficos);

        if (btnAudio != null) btnAudio.clicked += MostrarAbaAudio;
        if (btnControles != null) btnControles.clicked += MostrarAbaControles;
        if (btnGraficos != null) btnGraficos.clicked += MostrarAbaGraficos;
        if (btnVoltar != null) btnVoltar.clicked += FecharTela; 

        MostrarAbaAudio();
    }

    private void FecharTela()
    {
        AoFechar?.Invoke(); 
        gameObject.SetActive(false);
    }

    private void MostrarAbaAudio()
    {
        EsconderAbas();
        tituloAba.text = "Áudio";
        conteudoAudio.style.display = DisplayStyle.Flex;
        btnAudio.AddToClassList("botao-ativo");
    }

    private void MostrarAbaControles()
    {
        EsconderAbas();
        tituloAba.text = "Controles";
        conteudoControles.style.display = DisplayStyle.Flex;
        btnControles.AddToClassList("botao-ativo");
    }

    private void MostrarAbaGraficos()
    {
        EsconderAbas();
        tituloAba.text = "Gráficos";
        conteudoGraficos.style.display = DisplayStyle.Flex;
        btnGraficos.AddToClassList("botao-ativo");
    }

    private void EsconderAbas()
    {
        conteudoAudio.style.display = DisplayStyle.None;
        conteudoControles.style.display = DisplayStyle.None;
        conteudoGraficos.style.display = DisplayStyle.None;

        btnGraficos.RemoveFromClassList("botao-ativo");
        btnControles.RemoveFromClassList("botao-ativo");
        btnAudio.RemoveFromClassList("botao-ativo");
    }

}