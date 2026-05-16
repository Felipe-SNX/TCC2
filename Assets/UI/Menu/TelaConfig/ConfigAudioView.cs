using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable] 
public class ConfigAudioView
{
    private const float VOL_PADRAO_MASTER = 100f;
    private const float VOL_PADRAO_MUSICA = 100f;
    private const float VOL_PADRAO_SOM = 100f;
    private VisualElement root;
    private Slider sliderMaster;
    private Slider sliderMusica;
    private Slider sliderSom;

    public void Inicializar(VisualElement container)
    {
        root = container;
        
        sliderMaster = ConfigurarSliderCompleto("slider-master", "txt-valor-master");
        sliderSom = ConfigurarSliderCompleto("slider-som", "txt-valor-som");
        sliderMusica = ConfigurarSliderCompleto("slider-musica", "txt-valor-musica");

        Button btnReset = root.Q<Button>("btn-reset-audio");
        if (btnReset != null) btnReset.clicked += ResetarParaPadrao; 
    }

    private Slider ConfigurarSliderCompleto(string nomeSlider, string nomeLabel)
    {
        Slider slider = root.Q<Slider>(nomeSlider);
        Label label = root.Q<Label>(nomeLabel);

        if (slider != null)
        {
            adicionarRastroSlider(slider);

            if (label != null)
            {
                label.text = Mathf.RoundToInt(slider.value).ToString();

                slider.RegisterValueChangedCallback(evt => {
                    label.text = Mathf.RoundToInt(evt.newValue).ToString();
                });
            }
        }
        return slider;
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

    private void ResetarParaPadrao()
    {
        if (sliderMaster != null) sliderMaster.value = VOL_PADRAO_MASTER;
        if (sliderMusica != null) sliderMusica.value = VOL_PADRAO_MUSICA;
        if (sliderSom != null) sliderSom.value = VOL_PADRAO_SOM;

        Debug.Log("Configurações de Áudio Resetadas");
    }
}