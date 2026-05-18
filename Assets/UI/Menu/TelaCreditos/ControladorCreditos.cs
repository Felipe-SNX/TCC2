using System;
using UnityEngine;
using UnityEngine.UIElements;

public class ControladorCreditos : MonoBehaviour, IMenuPopup
{
    public Action AoFechar { get; set; }

    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        Button btnVoltar = root.Q<Button>("btn-voltar");

        if (btnVoltar != null) btnVoltar.clicked += FecharTela;
    }

    private void FecharTela()
    {
        AoFechar?.Invoke();
        gameObject.SetActive(false);
    }
}
