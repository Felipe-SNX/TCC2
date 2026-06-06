using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class QuestionnaireUIController : MonoBehaviour
{

    [Header("Próxima Tela")]
    [SerializeField] private GameObject resultScreen;

    private UIDocument uiDocument;
    
    private Label q1Label;
    private Label q2Label;
    private Label lblValorQ1;
    private Label lblValorQ2;
    private SliderInt q1Slider;
    private SliderInt q2Slider;
    private Button submitButton;

    private void OnEnable()
    {
        uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;

        q1Label = root.Q<Label>("lbl_question_1");
        q2Label = root.Q<Label>("lbl_question_2");
        q1Slider = root.Q<SliderInt>("slider_q1");
        q2Slider = root.Q<SliderInt>("slider_q2");
        submitButton = root.Q<Button>("btn_submit");

        lblValorQ1 = root.Q<Label>("lbl_valor_q1");
        lblValorQ2 = root.Q<Label>("lbl_valor_q2");

        if (lblValorQ1 != null && q1Slider != null) 
            lblValorQ1.text = "Valor: " + q1Slider.value;
            
        if (lblValorQ2 != null && q2Slider != null) 
            lblValorQ2.text = "Valor: " + q2Slider.value;

        if (q1Slider != null)
        {
            q1Slider.RegisterValueChangedCallback(evt => {
                lblValorQ1.text = "Valor: " + evt.newValue;
            });
        }

        if (q2Slider != null)
        {
            q2Slider.RegisterValueChangedCallback(evt => {
                lblValorQ2.text = "Valor: " + evt.newValue;
            });
        }

        SetupDynamicQuestions();

        if (submitButton != null)
        {
            submitButton.clicked += SubmitQuestionnaire;
        }
    }

    private void SetupDynamicQuestions()
    {
        if (MetricsManager.Instance == null) return;

        string currentLevel = MetricsManager.Instance.GetNameLevel();
        string[] levelQuestions = QuestionSettings.GetQuestions(currentLevel);

        if (q1Label != null) q1Label.text = levelQuestions[0];
        if (q2Label != null) q2Label.text = levelQuestions[1];
    }

    private void SubmitQuestionnaire()
    {
        int score1 = q1Slider != null ? q1Slider.value : 0;
        int score2 = q2Slider != null ? q2Slider.value : 0;

        if (MetricsManager.Instance != null)
        {
            MetricsManager.Instance.SubmitDataWithSurvey(score1, score2);
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
        {
            submitButton.clicked -= SubmitQuestionnaire;
        }
    }
}