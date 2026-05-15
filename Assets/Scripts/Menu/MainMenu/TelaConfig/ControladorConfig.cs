using System;
using UnityEngine;
using UnityEngine.UIElements;

public class ControladorConfiguracoes : MonoBehaviour, IMenuPopup
{
    private const float VOL_PADRAO_MASTER = 100f;
    private const float VOL_PADRAO_MUSICA = 100f;
    private const float VOL_PADRAO_SOM = 100f;
    public Action AoFechar { get; set; }
    private Label tituloAba;
    private VisualElement conteudoAudio;
    private VisualElement conteudoControles;
    private Button btnAudio;
    private Button btnControles;
    private Slider sliderMaster;
    private Slider sliderMusica;
    private Slider sliderSom;
    private Color fillColor;

    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        ColorUtility.TryParseHtmlString("#FFD700", out fillColor);

        // --------------- Área da Tela Config  --------------- // 
        tituloAba = root.Q<Label>("titulo-aba");
        conteudoAudio = root.Q<VisualElement>("conteudo-audio");
        conteudoControles = root.Q<VisualElement>("conteudo-controles");

        btnAudio = root.Q<Button>("btn-aba-audio");
        btnControles = root.Q<Button>("btn-aba-controles");
        Button btnVoltar = root.Q<Button>("btn-voltar");

        if (btnAudio != null) btnAudio.clicked += MostrarAbaAudio;
        if (btnControles != null) btnControles.clicked += MostrarAbaControles;
        if (btnVoltar != null) btnVoltar.clicked += FecharTela; 

        // --------------- Área da Tela de Áudio --------------- // 
        sliderMaster = root.Q<Slider>("slider-master");
        adicionarRastroSlider(sliderMaster);
        Label txtValorMaster = root.Q<Label>("txt-valor-master");

        sliderSom = root.Q<Slider>("slider-som");
        adicionarRastroSlider(sliderSom);
        Label txtValorSom = root.Q<Label>("txt-valor-som");
        
        sliderMusica = root.Q<Slider>("slider-musica");
        adicionarRastroSlider(sliderMusica);
        Label txtValorMusica = root.Q<Label>("txt-valor-musica");

        Button btnReset = root.Q<Button>("btn-reset");
        if (btnReset != null) btnReset.clicked += ResetarParaPadrao; 

        // Evento que escuta a barra sendo arrastada
        if (sliderMaster != null && txtValorMaster != null)
        {
            sliderMaster.RegisterValueChangedCallback(evt => {
                txtValorMaster.text = Mathf.RoundToInt(evt.newValue).ToString();
            });
        }

        if (sliderSom != null && txtValorSom != null)
        {
            sliderSom.RegisterValueChangedCallback(evt => {
                txtValorSom.text = Mathf.RoundToInt(evt.newValue).ToString();
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

    private void ResetarParaPadrao()
    {
        if (sliderMaster != null) sliderMaster.value = VOL_PADRAO_MASTER;
        if (sliderMusica != null) sliderMusica.value = VOL_PADRAO_MUSICA;
        if (sliderSom != null) sliderSom.value = VOL_PADRAO_SOM;

        Debug.Log("Configurações de Áudio Resetadas");
    }

    private void adicionarRastroSlider(Slider slider)
    {
        if (slider != null)
        {
            var tracker = slider.Q<VisualElement>(className: "unity-base-slider__tracker");

            if (tracker != null)
            {
                var fillElement = new VisualElement
                {
                    name = "slider-fill"
                };
                fillElement.AddToClassList("meu-slider-rastro");

                tracker.Add(fillElement);

                slider.RegisterValueChangedCallback(evt => UpdateFill(slider, fillElement));

                UpdateFill(slider, fillElement);
            }
        }
    }

    private void UpdateFill(Slider slider, VisualElement fillElement)
    {
        float range = slider.highValue - slider.lowValue;
        
        if (range == 0) return; 

        float percent = (slider.value - slider.lowValue) / range;
        percent = Mathf.Clamp01(percent);

        fillElement.style.width = Length.Percent(percent * 100f);
    }
}