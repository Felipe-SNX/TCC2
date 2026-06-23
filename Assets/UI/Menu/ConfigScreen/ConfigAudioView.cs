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

        private const string KEY_VOLUME_MASTER = "VolumeMaster";
        private const string KEY_VOLUME_MUSICA = "VolumeMusica";
        private const string KEY_VOLUME_SFX = "VolumeSFX";

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

            InicializarSliderMaster();
            InicializarSliderSFX();
            InicializarSliderMusica();

            Button btnReset = root.Q<Button>("btn-reset-audio");

            if (btnReset != null)
            {
                btnReset.clicked -= ResetarParaPadrao;
                btnReset.clicked += ResetarParaPadrao;
            }
        }

        private void InicializarSliderMaster()
        {
            if (sliderMaster == null)
                return;

            float valorSalvo = PlayerPrefs.GetFloat(KEY_VOLUME_MASTER, VOL_PADRAO_MASTER);

            sliderMaster.SetValueWithoutNotify(valorSalvo);
            AtualizarLabelERastroAoAbrir(sliderMaster, "txt-valor-master");

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.SetMasterVolume(valorSalvo / 100f);
            }

            sliderMaster.RegisterValueChangedCallback(evt =>
            {
                float valorPorcentagem = evt.newValue;

                PlayerPrefs.SetFloat(KEY_VOLUME_MASTER, valorPorcentagem);
                PlayerPrefs.Save();

                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.SetMasterVolume(valorPorcentagem / 100f);
                }

                AtualizarLabelERastroAoAbrir(sliderMaster, "txt-valor-master");
            });
        }

        private void InicializarSliderSFX()
        {
            if (sliderSFX == null)
                return;

            float valorSalvo = PlayerPrefs.GetFloat(KEY_VOLUME_SFX, VOL_PADRAO_SFX);

            sliderSFX.SetValueWithoutNotify(valorSalvo);
            AtualizarLabelERastroAoAbrir(sliderSFX, "txt-valor-som");

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.SetSFXVolume(valorSalvo / 100f);
            }

            sliderSFX.RegisterValueChangedCallback(evt =>
            {
                float valorPorcentagem = evt.newValue;

                PlayerPrefs.SetFloat(KEY_VOLUME_SFX, valorPorcentagem);
                PlayerPrefs.Save();

                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.SetSFXVolume(valorPorcentagem / 100f);
                }

                AtualizarLabelERastroAoAbrir(sliderSFX, "txt-valor-som");
            });
        }

        private void InicializarSliderMusica()
        {
            if (sliderMusica == null)
                return;

            float valorSalvo = PlayerPrefs.GetFloat(KEY_VOLUME_MUSICA, VOL_PADRAO_MUSICA);

            sliderMusica.SetValueWithoutNotify(valorSalvo);
            AtualizarLabelERastroAoAbrir(sliderMusica, "txt-valor-musica");

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.SetMusicVolume(valorSalvo / 100f);
            }

            sliderMusica.RegisterValueChangedCallback(evt =>
            {
                float valorPorcentagem = evt.newValue;

                PlayerPrefs.SetFloat(KEY_VOLUME_MUSICA, valorPorcentagem);
                PlayerPrefs.Save();

                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.SetMusicVolume(valorPorcentagem / 100f);
                }

                AtualizarLabelERastroAoAbrir(sliderMusica, "txt-valor-musica");
            });
        }

        private Slider ConfigurarSliderCompleto(string nomeSlider, string nomeLabel, string corClasse)
        {
            Slider slider = root.Q<Slider>(nomeSlider);
            Label label = root.Q<Label>(nomeLabel);

            if (slider != null)
            {
                slider.lowValue = 0f;
                slider.highValue = 100f;

                adicionarRastroSlider(slider, corClasse);

                if (label != null)
                {
                    label.text = Mathf.RoundToInt(slider.value).ToString();

                    slider.RegisterValueChangedCallback(evt =>
                    {
                        label.text = Mathf.RoundToInt(evt.newValue).ToString();
                    });
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

            if (range == 0)
                return;

            float percent = (slider.value - slider.lowValue) / range;
            percent = Mathf.Clamp01(percent);

            fillElement.style.width = Length.Percent(percent * 100f);
        }

        private void ResetarParaPadrao()
        {
            AplicarValorSliderMaster(VOL_PADRAO_MASTER);
            AplicarValorSliderSFX(VOL_PADRAO_SFX);
            AplicarValorSliderMusica(VOL_PADRAO_MUSICA);

            PlayerPrefs.Save();

            Debug.Log("Configurações de áudio resetadas.");
        }

        private void AplicarValorSliderMaster(float valorPorcentagem)
        {
            if (sliderMaster == null)
                return;

            sliderMaster.value = valorPorcentagem;

            PlayerPrefs.SetFloat(KEY_VOLUME_MASTER, valorPorcentagem);

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.SetMasterVolume(valorPorcentagem / 100f);
            }

            AtualizarLabelERastroAoAbrir(sliderMaster, "txt-valor-master");
        }

        private void AplicarValorSliderSFX(float valorPorcentagem)
        {
            if (sliderSFX == null)
                return;

            sliderSFX.value = valorPorcentagem;

            PlayerPrefs.SetFloat(KEY_VOLUME_SFX, valorPorcentagem);

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.SetSFXVolume(valorPorcentagem / 100f);
            }

            AtualizarLabelERastroAoAbrir(sliderSFX, "txt-valor-som");
        }

        private void AplicarValorSliderMusica(float valorPorcentagem)
        {
            if (sliderMusica == null)
                return;

            sliderMusica.value = valorPorcentagem;

            PlayerPrefs.SetFloat(KEY_VOLUME_MUSICA, valorPorcentagem);

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.SetMusicVolume(valorPorcentagem / 100f);
            }

            AtualizarLabelERastroAoAbrir(sliderMusica, "txt-valor-musica");
        }
    }
}