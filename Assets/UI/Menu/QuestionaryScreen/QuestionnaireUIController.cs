using UnityEngine;
using UnityEngine.UIElements;
using System.Text.RegularExpressions;

public class QuestionnaireUIController : MonoBehaviour
{
    [Header("Fluxo de Telas")]
    [SerializeField] private GameObject resultScreen;
    private UIDocument uiDocument;
    private TextField inputEmail;
    private TextField inputPin;
    private Label erroEmail;
    private Label erroPin;
    private Label q1ValorLabel;
    private SliderInt q1Slider;
    private Button submitButton;

    private VisualElement root;

    private void OnEnable()
    {
        uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;

        inputEmail = root.Q<TextField>("input_email");
        inputPin = root.Q<TextField>("input_pin");
        erroEmail = root.Q<Label>("lbl_erro_email");
        erroPin = root.Q<Label>("lbl_erro_pin");

        q1ValorLabel = root.Q<Label>("lbl_valor_q1");
        q1Slider = root.Q<SliderInt>("slider_q1");
        submitButton = root.Q<Button>("btn_submit");

        // Atualiza o valor do texto ao arrastar o slider
        if (q1Slider != null && q1ValorLabel != null)
        {
            q1Slider.RegisterValueChangedCallback(evt => {
                q1ValorLabel.text = evt.newValue.ToString();
            });
        }

        if (inputEmail != null)
        {
            // Verifica se a chave "EmailUsuario" já existe salva na máquina
            if (PlayerPrefs.HasKey("EmailUsuario"))
            {
                // Injeta o valor salvo direto no campo de texto
                inputEmail.value = PlayerPrefs.GetString("EmailUsuario");
            }
        }

        if (submitButton != null)
        {
            submitButton.clicked += ValidateAndSubmit;
        }
    }

    void Start()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ConnectButtons(root);
        }
    }

    private void ValidateAndSubmit()
    {
        bool formularioValido = true;

        // Validação do E-mail 
        string emailRegexPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        if (string.IsNullOrWhiteSpace(inputEmail.text) || !Regex.IsMatch(inputEmail.text, emailRegexPattern))
        {
            erroEmail.style.display = DisplayStyle.Flex; 
            formularioValido = false;
        }
        else
        {
            erroEmail.style.display = DisplayStyle.None; 
        }

        // Validação do PIN 
        if (string.IsNullOrWhiteSpace(inputPin.text) || !int.TryParse(inputPin.text, out int pinNumber))
        {
            erroPin.style.display = DisplayStyle.Flex; 
            formularioValido = false;
        }
        else
        {
            erroPin.style.display = DisplayStyle.None; 
        }

        if (!formularioValido)
        {
            return;
        }

        string emailFinal = inputEmail.text;
        int pinFinal = int.Parse(inputPin.text);
        int scoreCor = q1Slider.value;

        PlayerPrefs.SetString("EmailUsuario", emailFinal);
        PlayerPrefs.Save();  
        Debug.Log("[SISTEMA] E-mail salvo localmente: " + emailFinal);

        if (MetricsManager.Instance != null)
        {
            MetricsManager.Instance.SubmitDataWithSurvey(scoreCor, emailFinal, pinFinal); 
        }

        gameObject.SetActive(false); 

        if (resultScreen != null)
        {
            resultScreen.SetActive(true);
        }
    }

    private void OnDisable()
    {
        if (submitButton != null)
            submitButton.clicked -= ValidateAndSubmit;
    }
}