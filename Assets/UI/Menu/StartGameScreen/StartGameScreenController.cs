using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.UI.Menu.StartGameScreen
{
    public class StartScreenController : MonoBehaviour
    {
        [SerializeField] private GameObject mainMenuObject; 
        
        private VisualElement root;
        private VisualElement titleMask;
        private Label coloredTitle;
        private Label promptText;
        
        [Header("Cores e Carregamento")]
        private Color[] paleta = new Color[] { Color.green, Color.blue, Color.yellow, Color.red };
        private int currentColorIndex = 0;
        private float timeColor = 0;
        public float velocidadeTrocaCor = 0.4f; 
        private float fillDuration = 0;
        private bool tituloCarregado = false;

        [Header("Transição Suave")]
        public float velocidadeFade = 3f; 
        private bool estaSaindo = false;
        private float opacidadeTela = 1f;

        void Start()
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.ConnectButtons(root);
                AudioManager.Instance.PlayMenuMusic();
            }
        }

        private void OnEnable()
        {
            root = GetComponent<UIDocument>().rootVisualElement;
            titleMask = root.Q<VisualElement>("mascara-titulo");
            coloredTitle = root.Q<Label>("titulo-frente");
            promptText = root.Q<Label>("prompt-text");

            fillDuration = 0f;
            tituloCarregado = false;
            estaSaindo = false;
            opacidadeTela = 1f;
            
            if (root != null) root.style.opacity = opacidadeTela; 
            if (titleMask != null) titleMask.style.width = Length.Percent(0);
            if (coloredTitle != null) coloredTitle.style.opacity = 1f; 
        }

        private void Update()
        {
            if (root == null) return;

            if (estaSaindo)
            {
                HandleExitTransition();
                return;
            }

            AnimateTitleColor();
            AnimateTitleMask();
            AnimatePromptText();
            CheckForInput();
        }

        private void HandleExitTransition()
        {
            opacidadeTela -= Time.deltaTime * velocidadeFade;
            root.style.opacity = opacidadeTela;

            float zoom = Mathf.Lerp(1.2f, 1f, opacidadeTela);
            if (coloredTitle != null)
            {
                coloredTitle.style.scale = new StyleScale(new Scale(new Vector2(zoom, zoom)));
            }

            if (opacidadeTela <= 0f)
            {
                ProsseguirParaMenu();
            }
        }

        private void AnimateTitleColor()
        {
            timeColor += Time.deltaTime * velocidadeTrocaCor;
            if (timeColor >= 1f) 
            { 
                timeColor = 0; 
                currentColorIndex = (currentColorIndex + 1) % paleta.Length; 
            }
            
            Color corFinal = Color.Lerp(paleta[currentColorIndex], paleta[(currentColorIndex + 1) % paleta.Length], timeColor);
            if (coloredTitle != null) 
            {
                coloredTitle.style.color = corFinal;
            }
        }

        private void AnimateTitleMask()
        {
            if (!tituloCarregado)
            {
                fillDuration += Time.deltaTime * 50f; 
                if (titleMask != null) 
                {
                    titleMask.style.width = Length.Percent(Mathf.Clamp(fillDuration, 0, 100));
                }

                if (fillDuration >= 100f)
                {
                    tituloCarregado = true;
                }
            }
            else if (coloredTitle != null)
            {
                coloredTitle.style.opacity = 1f; 
            }
        }

        private void AnimatePromptText()
        {
            if (promptText != null)
            {
                float alpha = (Mathf.Sin(Time.time * 3f) + 1f) / 2f; 
                promptText.style.opacity = Mathf.Lerp(0.2f, 0.8f, alpha);
            }
        }

        private void CheckForInput()
        {
            if (Input.anyKeyDown)
            {
                estaSaindo = true;
                // AudioManager.Instance.PlaySFX("StartGame");
            }
        }

        private void ProsseguirParaMenu()
        {
            mainMenuObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}