using System;
using UnityEngine;
using UnityEngine.UIElements;

public class ControladorConfiguracoes : MonoBehaviour, IMenuPopup
{
    public Action AoFechar { get; set; }
    private Label tituloAba;
    private VisualElement conteudoAudio;
    private VisualElement conteudoControles;
    private Button btnAudio;
    private Button btnControles;

    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        tituloAba = root.Q<Label>("titulo-aba");
        conteudoAudio = root.Q<VisualElement>("conteudo-audio");
        conteudoControles = root.Q<VisualElement>("conteudo-controles");

        btnAudio = root.Q<Button>("btn-aba-audio");
        btnControles = root.Q<Button>("btn-aba-controles");
        Button btnVoltar = root.Q<Button>("btn-voltar");

        if (btnAudio != null) btnAudio.clicked += MostrarAbaAudio;
        if (btnControles != null) btnControles.clicked += MostrarAbaControles;
        if (btnVoltar != null) btnVoltar.clicked += FecharTela; 

        Slider sliderMaster = root.Q<Slider>("slider-master");
        Label txtValorMaster = root.Q<Label>("txt-valor-master");
        
        Slider sliderMusica = root.Q<Slider>("slider-musica");
        Label txtValorMusica = root.Q<Label>("txt-valor-musica");

        // Evento que escuta a barra sendo arrastada
        if (sliderMaster != null && txtValorMaster != null)
        {
            sliderMaster.RegisterValueChangedCallback(evt => {
                txtValorMaster.text = Mathf.RoundToInt(evt.newValue).ToString();
            });
        }

        if (sliderMusica != null && txtValorMusica != null)
        {
            sliderMusica.RegisterValueChangedCallback(evt => {
                txtValorMusica.text = Mathf.RoundToInt(evt.newValue).ToString();
            });
        }

        MostrarAbaAudio();
    }

    private void FecharTela()
    {
        AoFechar?.Invoke(); 
        gameObject.SetActive(false);
    }

    private void MostrarAbaAudio()
    {
        tituloAba.text = "Áudio";
        
        conteudoAudio.style.display = DisplayStyle.Flex;
        conteudoControles.style.display = DisplayStyle.None;

        btnAudio.AddToClassList("botao-ativo");
        btnControles.RemoveFromClassList("botao-ativo");
    }

    private void MostrarAbaControles()
    {
        tituloAba.text = "Controles";
        
        conteudoAudio.style.display = DisplayStyle.None;
        conteudoControles.style.display = DisplayStyle.Flex;

        btnControles.AddToClassList("botao-ativo");
        btnAudio.RemoveFromClassList("botao-ativo");
    }
}