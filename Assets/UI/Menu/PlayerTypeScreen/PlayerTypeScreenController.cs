using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using System;

[RequireComponent(typeof(UIDocument))]
public class PlayerTypeScreenController : MonoBehaviour, IMenuPopup
{
    public Action AoFechar { get; set; }

    private UIDocument uiDocument;
    
    private VisualElement selectionPanel;
    private VisualElement credentialsPanel;

    private TextField inputEmail;
    private TextField inputPin;
    private Toggle toggleLgpd;

    private Button btnEnthusiast;
    private Button btnRecommended;
    private Button btnBack;
    private Button btnSubmit;

    private Label lblErroEmail;
    private Label lblErroPin;
    private Label lblErroLgpd;

    public float velocidadePreenchimento = 50f;
    private VisualElement mascaraTitulo;
    private float progressoPreenchimento = 0f;
    private bool tituloCarregado = false;

    private void Awake()
    {
        uiDocument = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        if (uiDocument == null) return;

        VisualElement root = uiDocument.rootVisualElement;

        mascaraTitulo = root.Q<VisualElement>("mascara-titulo");
        if (mascaraTitulo != null) mascaraTitulo.style.width = Length.Percent(0);

        selectionPanel = root.Q<VisualElement>("selection-panel");
        credentialsPanel = root.Q<VisualElement>("credentials-panel");

        inputEmail = root.Q<TextField>("input_email");
        inputPin = root.Q<TextField>("input_pin");
        toggleLgpd = root.Q<Toggle>("toggle_lgpd");

        btnEnthusiast = root.Q<Button>("btn_enthusiast");
        btnRecommended = root.Q<Button>("btn_recommended");
        btnBack = root.Q<Button>("btn_back");
        btnSubmit = root.Q<Button>("btn_submit");

        lblErroEmail = root.Q<Label>("lbl_erro_email");
        lblErroPin = root.Q<Label>("lbl_erro_pin");
        lblErroLgpd = root.Q<Label>("lbl_erro_lgpd");

        if (btnEnthusiast != null) btnEnthusiast.clicked += OnEnthusiastClicked;
        if (btnRecommended != null) btnRecommended.clicked += OnRecommendedClicked;
        if (btnBack != null) btnBack.clicked += OnBackClicked;
        if (btnSubmit != null) btnSubmit.clicked += OnSubmitClicked;

        ResetScreen();

        ConfigurarComoModal();
    }

    private void Update()
    {
        if (!tituloCarregado && mascaraTitulo != null)
        {
            progressoPreenchimento += Time.deltaTime * velocidadePreenchimento;
            mascaraTitulo.style.width = Length.Percent(Mathf.Clamp(progressoPreenchimento, 0, 100));

            if (progressoPreenchimento >= 100f) tituloCarregado = true;
        }
    }

    private void ConfigurarComoModal()
    {
        selectionPanel?.RemoveFromClassList("hidden");
        credentialsPanel?.AddToClassList("hidden");
    }

    private void OnDisable()
    {
        if (btnEnthusiast != null) btnEnthusiast.clicked -= OnEnthusiastClicked;
        if (btnRecommended != null) btnRecommended.clicked -= OnRecommendedClicked;
        if (btnBack != null) btnBack.clicked -= OnBackClicked;
        if (btnSubmit != null) btnSubmit.clicked -= OnSubmitClicked;
    }

    private void ResetScreen()
    {
        selectionPanel?.RemoveFromClassList("hidden");
        credentialsPanel?.AddToClassList("hidden");

        if (lblErroEmail != null) lblErroEmail.style.display = DisplayStyle.None;
        if (lblErroPin != null) lblErroPin.style.display = DisplayStyle.None;
        if (lblErroLgpd != null) lblErroLgpd.style.display = DisplayStyle.None;

        if (inputEmail != null) inputEmail.value = "";
        if (inputPin != null) inputPin.value = "";
        if (toggleLgpd != null) toggleLgpd.value = false;
    }

    private void OnEnthusiastClicked()
    {
        GameSession.IsEnthusiast = true;
        GameSession.UserEmail = "entusiasta@anonimo.com";
        GameSession.UserPIN = "0000";

        DisableScreen();
    }

    private void OnRecommendedClicked()
    {
        selectionPanel?.AddToClassList("hidden");
        credentialsPanel?.RemoveFromClassList("hidden");
    }

    private void OnBackClicked()
    {
        ResetScreen();
    }

    private void OnSubmitClicked()
    {
        bool isFormValid = true;

        string email = inputEmail?.value?.Trim() ?? "";
        string pin = inputPin?.value?.Trim() ?? "";
        bool isLgpdChecked = toggleLgpd?.value ?? false;

        if (string.IsNullOrEmpty(email) || !email.Contains("@"))
        {
            if (lblErroEmail != null) lblErroEmail.style.display = DisplayStyle.Flex;
            isFormValid = false;
        }
        else
        {
            if (lblErroEmail != null) lblErroEmail.style.display = DisplayStyle.None;
        }

        if (string.IsNullOrEmpty(pin) || !Regex.IsMatch(pin, @"^\d+$"))
        {
            if (lblErroPin != null) lblErroPin.style.display = DisplayStyle.Flex;
            isFormValid = false;
        }
        else
        {
            if (lblErroPin != null) lblErroPin.style.display = DisplayStyle.None;
        }

        if (!isLgpdChecked)
        {
            if (lblErroLgpd != null) lblErroLgpd.style.display = DisplayStyle.Flex;
            isFormValid = false;
        }
        else
        {
            if (lblErroLgpd != null) lblErroLgpd.style.display = DisplayStyle.None;
        }

        if (isFormValid)
        {
            GameSession.IsEnthusiast = false;
            GameSession.UserEmail = email;
            GameSession.UserPIN = pin;

            DisableScreen();
        }
    }

    private void DisableScreen()
    {
        GlobalData.perguntaTipoJogadorMostrado = true;
        AoFechar?.Invoke(); 
        gameObject.SetActive(false); 
    }
}