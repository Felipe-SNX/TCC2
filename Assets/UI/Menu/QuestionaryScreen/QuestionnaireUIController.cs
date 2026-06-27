using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.UI.Menu.QuestionaryScreen
{
    public class QuestionnaireUIController : MonoBehaviour
    {
        [Header("Fluxo de Telas")]
        [SerializeField] private GameObject resultScreen;
        [SerializeField] private float velocidadePreenchimento = 50f;

        private VisualElement root;
        private VisualElement mascaraTitulo;
        private float progressoPreenchimento = 0f;

        private SliderInt q1Slider;
        private Label q1ValorLabel;
        private Button submitButton;

        private void OnEnable()
        {
            var uiDocument = GetComponent<UIDocument>();
            root = uiDocument.rootVisualElement;

            mascaraTitulo = root.Q<VisualElement>("mascara-titulo");
            q1Slider = root.Q<SliderInt>("slider_q1");
            q1ValorLabel = root.Q<Label>("lbl_valor_q1");
            submitButton = root.Q<Button>("btn_submit");

            progressoPreenchimento = 0f;
            if (mascaraTitulo != null) mascaraTitulo.style.width = Length.Percent(0);

            q1Slider?.RegisterValueChangedCallback(evt => {
                if (q1ValorLabel != null) q1ValorLabel.text = evt.newValue.ToString();
            });

            if (submitButton != null) submitButton.clicked += OnSubmit;
        }

        private void Start()
        {
            if (UIAudioManager.Instance != null)
            {
                UIAudioManager.Instance.ConnectButtons(root);
            }
        }

        private void Update()
        {
            if (mascaraTitulo != null && progressoPreenchimento < 100f)
            {
                progressoPreenchimento += Time.deltaTime * velocidadePreenchimento;
                mascaraTitulo.style.width = Length.Percent(Mathf.Clamp(progressoPreenchimento, 0, 100));
            }
        }

        private void OnSubmit()
        {
            int scoreCor = q1Slider != null ? q1Slider.value : 3;

            string emailFinal = GameSession.UserEmail;
            int pinFinal = int.TryParse(GameSession.UserPIN, out int pin) ? pin : 0;

            if (MetricsManager.Instance != null)
            {
                MetricsManager.Instance.SubmitDataWithSurvey(scoreCor, emailFinal, pinFinal);
            }

            TransitionToResult();
        }

        private void TransitionToResult()
        {
            gameObject.SetActive(false);
            if (resultScreen != null)
            {
                resultScreen.SetActive(true);
            }
        }

        private void OnDisable()
        {
            if (submitButton != null) submitButton.clicked -= OnSubmit;
        }
    }
}