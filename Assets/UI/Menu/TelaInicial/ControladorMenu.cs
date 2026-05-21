using System;
using UnityEngine;
using UnityEngine.UIElements;

public class ControladorMenu : MonoBehaviour, IMenuPopup
{
    public Action AoFechar { get; set; }
    [SerializeField] private GameObject objetoConfiguracoes;
    [SerializeField] private GameObject objetoSelecaoFases;
    [SerializeField] private GameObject objetoCreditos;

    private VisualElement root;

    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        Button btnOpcoes = root.Q<Button>("btn-opcoes");
        Button btnCreditos = root.Q<Button>("btn-creditos");
        Button btnJogar = root.Q<Button>("btn-jogar");
        Button btnSair = root.Q<Button>("btn-sair");

        if (btnJogar != null) btnJogar.clicked += () => AbrirTela(objetoSelecaoFases);
        if (btnCreditos != null) btnCreditos.clicked += () => AbrirTela(objetoCreditos);
        if (btnOpcoes != null) btnOpcoes.clicked += () => AbrirTela(objetoConfiguracoes);
        if (btnSair != null) btnSair.clicked += SairDoJogo;
    }

    void Start()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMenuMusic();
        }
    }

    private void AbrirTela(GameObject objetoFase)
    {
        if (objetoFase != null)
        {
            var popup = objetoFase.GetComponent<IMenuPopup>();

            if (popup != null)
            {
                root.style.display = DisplayStyle.None;

                popup.AoFechar = () => { 
                    root.style.display = DisplayStyle.Flex; 
                };

                objetoFase.SetActive(true);
            }
            else 
            {
                Debug.LogError($"O objeto {objetoFase.name} não tem um script que implementa IMenuPopup!");
            }
        }
    }

    private void SairDoJogo()
    {
        Debug.Log("Saindo do jogo...");
        Application.Quit();

        // Esta parte avisa a Unity para parar o "Play" se estiver testando no Editor
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}