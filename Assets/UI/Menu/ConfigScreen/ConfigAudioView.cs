using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable] 
public class ConfigAudioView
{
    private const float VOL_PADRAO_MASTER = 100f;
    private const float VOL_PADRAO_MUSICA = 100f;
    private const float VOL_PADRAO_SFX = 100f;
    private VisualElement root;
    private Slider sliderMaster;
    private Slider sliderMusica;
    private Slider sliderSFX;

    public void Inicializar(VisualElement container)
    {
        root = container;
        
        sliderMaster = ConfigurarSliderCompleto("slider-master", "txt-valor-master", "cor-master");
        sliderSFX = ConfigurarSliderCompleto("slider-som", "txt-valor-som", "cor-sfx");
        sliderMusica = ConfigurarSliderCompleto("slider-musica", "txt-valor-musica", "cor-musica");

        if (sliderMaster != null)
        {
            float valorSalvoMaster = PlayerPrefs.GetFloat("VolumeMaster", VOL_PADRAO_MASTER / 100f) * 100f;
            
            sliderMaster.SetValueWithoutNotify(valorSalvoMaster);
            AtualizarLabelERastroAoAbrir(sliderMaster, "txt-valor-master");

            sliderMaster.RegisterValueChangedCallback(evt => 
            {
                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.SetMasterVolume(evt.newValue / 100f);
                }
            });
        }

        if (sliderSFX != null)
        {
            float valorSalvoSFX = PlayerPrefs.GetFloat("VolumeSFX", VOL_PADRAO_SFX / 100f) * 100f;
            
            sliderSFX.SetValueWithoutNotify(valorSalvoSFX);
            AtualizarLabelERastroAoAbrir(sliderSFX, "txt-valor-som");

            sliderSFX.RegisterValueChangedCallback(evt => 
            {
                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.SetSFXVolume(evt.newValue / 100f);
                }
            });
        }

        if (sliderMusica != null)
        {
            float valorSalvoMusica = PlayerPrefs.GetFloat("VolumeMusica", VOL_PADRAO_MUSICA / 100f) * 100f;
            
            sliderMusica.SetValueWithoutNotify(valorSalvoMusica);
            AtualizarLabelERastroAoAbrir(sliderMusica, "txt-valor-musica");

            sliderMusica.RegisterValueChangedCallback(evt => 
            {
                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.SetMusicVolume(evt.newValue / 100f);
                }
            });
        }

        Button btnReset = root.Q<Button>("btn-reset-audio");
        if (btnReset != null) btnReset.clicked += ResetarParaPadrao; 
    }

    private void AtualizarLabelERastroAoAbrir(Slider slider, string nomeLabel)
    {
        Label label = root.Q<Label>(nomeLabel);
        if (label != null) label.text = Mathf.RoundToInt(slider.value).ToString();

        var fillElement = slider.Q<VisualElement>("slider-fill");
        if (fillElement != null) UpdateFill(slider, fillElement);
    }

    private Slider ConfigurarSliderCompleto(string nomeSlider, string nomeLabel, string corClasse)
    {
        Slider slider = root.Q<Slider>(nomeSlider);
        Label label = root.Q<Label>(nomeLabel);

        if (slider != null)
        {
            adicionarRastroSlider(slider, corClasse);

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

    private void adicionarRastroSlider(Slider slider, string corClasse)
    {
        if (slider != null)
        {
            var tracker = slider.Q<VisualElement>(className: "unity-base-slider__tracker");
            if (tracker != null)
            {
                var fillElement = new VisualElement { name = "slider-fill" };
                
                fillElement.AddToClassList("meu-slider-rastro");
                fillElement.AddToClassList(corClasse); // Aplica a cor específica (verde, azul ou amarelo)
                fillElement.AddToClassList("elemento-pulsante"); // Tag para o script principal piscar

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
        if (sliderMaster != null)
        {
            sliderMaster.value = VOL_PADRAO_MASTER;
        }

        if (sliderSFX != null) 
        {
            sliderSFX.value = VOL_PADRAO_MUSICA; 
        }

        if (sliderMusica != null) 
        {
            sliderMusica.value = VOL_PADRAO_MUSICA; 
        }

        Debug.Log("Configurações de Áudio Resetadas");
    }
}