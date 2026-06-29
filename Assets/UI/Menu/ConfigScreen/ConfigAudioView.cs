using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.UI.Menu.ConfigScreen
{
    [System.Serializable]
    public class ConfigAudioView
    {

        private const float VOL_PADRAO_MASTER = 50f;
        private const float VOL_PADRAO_MUSICA = 50f;
        private const float VOL_PADRAO_SFX = 80f; 

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

            InicializarSliders();

            Button btnReset = root.Q<Button>("btn-reset-audio");
            if (btnReset != null)
            {
                btnReset.clicked -= ResetarParaPadrao;
                btnReset.clicked += ResetarParaPadrao;
            }
        }

        private void InicializarSliders()
        {
            if (AudioSettingsCore.Instance != null)
            {
                AplicarAoSlider(sliderMaster, AudioSettingsCore.Instance.MasterVolume * 100f, "txt-valor-master");
                AplicarAoSlider(sliderSFX, AudioSettingsCore.Instance.SfxVolume * 100f, "txt-valor-som");
                AplicarAoSlider(sliderMusica, AudioSettingsCore.Instance.MusicVolume * 100f, "txt-valor-musica");
            }

            sliderMaster?.RegisterValueChangedCallback(evt => AudioSettingsCore.Instance?.SetMasterVolume(evt.newValue / 100f));
            sliderSFX?.RegisterValueChangedCallback(evt => AudioSettingsCore.Instance?.SetSFXVolume(evt.newValue / 100f));
            sliderMusica?.RegisterValueChangedCallback(evt => AudioSettingsCore.Instance?.SetMusicVolume(evt.newValue / 100f));
        }

        private void AplicarAoSlider(Slider slider, float valor, string labelName)
        {
            if (slider == null) return;
            slider.SetValueWithoutNotify(valor);
            AtualizarLabelERastroAoAbrir(slider, labelName);
        }   

        private Slider ConfigurarSliderCompleto(string nomeSlider, string nomeLabel, string corClasse)
        {
            Slider slider = root.Q<Slider>(nomeSlider);
            Label label = root.Q<Label>(nomeLabel);

            if (slider != null)
            {
                slider.lowValue = 0f;
                slider.highValue = 100f;
                AdicionarRastroSlider(slider, corClasse);

                if (label != null)
                {
                    label.text = Mathf.RoundToInt(slider.value).ToString();
                    slider.RegisterValueChangedCallback(evt => label.text = Mathf.RoundToInt(evt.newValue).ToString());
                }
            }
            return slider;
        }

        private void AtualizarLabelERastroAoAbrir(Slider slider, string nomeLabel)
        {
            Label label = root.Q<Label>(nomeLabel);

            if (label != null)
            {
                label.text = Mathf.RoundToInt(slider.value).ToString();
            }

            var fillElement = slider.Q<VisualElement>("slider-fill");

            if (fillElement != null)
            {
                UpdateFill(slider, fillElement);
            }
        }

        private void AdicionarRastroSlider(Slider slider, string corClasse)
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

            if (range == 0)
                return;

            float percent = (slider.value - slider.lowValue) / range;
            percent = Mathf.Clamp01(percent);

            fillElement.style.width = Length.Percent(percent * 100f);
        }

        private void ResetarParaPadrao()
        {
            if (sliderMaster != null) sliderMaster.value = VOL_PADRAO_MASTER;
            if (sliderSFX != null) sliderSFX.value = VOL_PADRAO_SFX;
            if (sliderMusica != null) sliderMusica.value = VOL_PADRAO_MUSICA;

            if (AudioSettingsCore.Instance != null)
            {
                AudioSettingsCore.Instance.SetMasterVolume(VOL_PADRAO_MASTER / 100f);
                AudioSettingsCore.Instance.SetSFXVolume(VOL_PADRAO_SFX / 100f);
                AudioSettingsCore.Instance.SetMusicVolume(VOL_PADRAO_MUSICA / 100f);
            }

            AtualizarLabelERastroAoAbrir(sliderMaster, "txt-valor-master");
            AtualizarLabelERastroAoAbrir(sliderSFX, "txt-valor-som");
            AtualizarLabelERastroAoAbrir(sliderMusica, "txt-valor-musica");

            Debug.Log("Configurações de áudio resetadas para: 50, 80, 50.");
        }
    }
}