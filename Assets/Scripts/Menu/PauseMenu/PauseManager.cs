using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }

    [Header("Interface")]
    [SerializeField] private GameObject painelPause;
    
    public bool JogoEstaPausado { get; private set; } = false;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this.gameObject);
        else Instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (JogoEstaPausado)
            {
                RetomarJogo();
            }
            else
            {
                PausarJogo();
            }
        }
    }

    public void PausarJogo()
    {
        painelPause.SetActive(true);
        Time.timeScale = 0f; 
        JogoEstaPausado = true;
    }

    public void RetomarJogo()
    {
        painelPause.SetActive(false);
        Time.timeScale = 1f; 
        JogoEstaPausado = false;
    }

    public void VoltarAoMenu()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("Scene_MainMenu"); 
    }
}