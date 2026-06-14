using System;
using UnityEngine;
using UnityEngine.UIElements;

public class CreditsController : MonoBehaviour, IMenuPopup
{
    public Action AoFechar { get; set; }
    private VisualElement root;

    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        Button btnVoltar = root.Q<Button>("btn-voltar");

        if (btnVoltar != null) btnVoltar.clicked += FecharTela;
    }

    void Start()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ConnectButtons(root);
        }
    }

    private void FecharTela()
    {
        AoFechar?.Invoke();
        gameObject.SetActive(false);
    }
}
