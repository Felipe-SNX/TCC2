using UnityEngine;
using UnityEngine.UI;

public class ConfigManager : MonoBehaviour
{

    [Header("Abas")]
    [SerializeField] private GameObject painelAudio;
    [SerializeField] private GameObject painelControles;

    [Header("Controles de Áudio")]
    [SerializeField] private Slider sliderMaster;
    [SerializeField] private Slider sliderMusica;

    private GameObject menuAnterior;

    private void Start()
    {
        MostrarAbaAudio();

        if (sliderMaster != null) 
        {
            sliderMaster.value = AudioListener.volume; 
        }
    }

    public void AbrirConfiguracoes(GameObject origem)
    {
        menuAnterior = origem;           
        menuAnterior.SetActive(false);   
        gameObject.SetActive(true);     
    }

    public void FecharConfiguracoes()
    {
        if (menuAnterior != null)
        {
            menuAnterior.SetActive(true); 
        }
        
        gameObject.SetActive(false);
    }

        public void MostrarAbaAudio()
    {
        painelAudio.SetActive(true);      
        painelControles.SetActive(false);
    }

    public void MostrarAbaControles()
    {
        painelAudio.SetActive(false);
        painelControles.SetActive(true);  
    }

    public void SetMasterVolume(float valor)
    {
        AudioListener.volume = valor;
        Debug.Log($"Volume Master alterado para: {valor}");
    }

    public void SetMusicVolume(float valor)
    {
        // Aqui você pode salvar o valor para usar no seu Mixer 
        // ou passar direto para o AudioSource que toca a trilha sonora
        Debug.Log($"Volume da Música alterado para: {valor}");
        
        // Exemplo se você tiver um AudioSource de música:
        // musicaSource.volume = valor;
    }
}