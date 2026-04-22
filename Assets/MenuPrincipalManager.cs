using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    [SerializeField] private string nomeLevel;
    [SerializeField] private GameObject painelMenuInicial;
    [SerializeField] private GameObject painelMenuConfiguracoes;
    [SerializeField] private GameObject painelMenuAudio;

    public void Jogar()
    {
        SceneManager.LoadScene(nomeLevel);
    }

    public void AbrirConfigurações()
    {
        painelMenuInicial.SetActive(false);
        painelMenuConfiguracoes.SetActive(true);
    } 

    public void FecharConfigurações()
    {
        painelMenuConfiguracoes.SetActive(false);
        painelMenuInicial.SetActive(true);
    } 

    public void AbrirMenuAudio()
    {
        painelMenuAudio.SetActive(true);
    }

    public void SairJogo()
    {
        Debug.Log("Sair do Jogo");
        Application.Quit();
    } 
}
