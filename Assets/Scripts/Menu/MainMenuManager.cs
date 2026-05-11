using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    [Header("Painéis Principais")]
    [SerializeField] private GameObject painelMenuInicial;
    [SerializeField] private GameObject painelSelecaoFases;
    [SerializeField] private GameObject painelMenuConfiguracoes;
    [SerializeField] private GameObject painelMenuCreditos;

    [Header("Sub-painéis")]
    [SerializeField] private GameObject painelMenuAudio; 
    
    public void CarregarFase(string nomeDaFase)
    {
        SceneManager.LoadScene(nomeDaFase);
    }

    public void AbrirSelecaoFases()
    {
        painelMenuInicial.SetActive(false);
        painelSelecaoFases.SetActive(true);
    }

    public void AbrirConfiguracoes()
    {
        painelMenuInicial.SetActive(false);
        painelMenuConfiguracoes.SetActive(true);
    }

    public void AbrirMenuCreditos()
    {
        painelMenuInicial.SetActive(false);
        painelMenuCreditos.SetActive(true);
    }

    public void VoltarParaMenuInicial()
    {
        painelSelecaoFases.SetActive(false);
        painelMenuConfiguracoes.SetActive(false);
        painelMenuCreditos.SetActive(false);
        painelMenuAudio.SetActive(false); 

        painelMenuInicial.SetActive(true);
    }

    public void AbrirMenuAudio()
    {
        painelMenuAudio.SetActive(true);          
    }

    public void FecharMenuAudio()
    {
        painelMenuAudio.SetActive(false);         
        painelMenuConfiguracoes.SetActive(true);  
    }

    public void SairJogo()
    {
        Debug.Log("Sair do Jogo");
        Application.Quit();
    } 
}