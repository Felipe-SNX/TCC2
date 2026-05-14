using System;
using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UIElements;

public class ControladorSelecaoFases : MonoBehaviour, IMenuPopup
{
    public Action AoFechar { get; set; }

    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        Button btnVerde = root.Q<Button>("btn-fase-verde");
        Button btnVermelha = root.Q<Button>("btn-fase-vermelha");
        Button btnAmarela = root.Q<Button>("btn-fase-amarela");
        Button btnVoltar = root.Q<Button>("btn-voltar");

        if (btnVerde != null) btnVerde.clicked += () => CarregarFase("Map_Green");
        if (btnVermelha != null) btnVermelha.clicked += () => CarregarFase("Map_Red");
        if (btnAmarela != null) btnAmarela.clicked += () => CarregarFase("Map_Yellow");

        if (btnVoltar != null) btnVoltar.clicked += FecharTela;
    }

    private void CarregarFase(string nomeDaCena)
    {
        Debug.Log($"Iniciando a cena: {nomeDaCena}");
        SceneManager.LoadScene(nomeDaCena);
    }

    private void FecharTela()
    {
        AoFechar?.Invoke();
        gameObject.SetActive(false);
    }
}