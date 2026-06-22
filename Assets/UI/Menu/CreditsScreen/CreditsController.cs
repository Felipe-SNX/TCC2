using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.UI.Menu.CreditsScreen
{
    public class CreditsController : MonoBehaviour, IMenuPopup
    {
        public Action AoFechar { get; set; }

        [Header("Animação do Título")]
        public float velocidadePreenchimento = 50f;
        private float progressoPreenchimento = 0f;

        private VisualElement root;
        private VisualElement mascaraTitulo;

        private void OnEnable()
        {
            root = GetComponent<UIDocument>().rootVisualElement;

            mascaraTitulo = root.Q<VisualElement>("mascara-titulo");

            // Reseta o título sempre que abrir a tela
            progressoPreenchimento = 0f;
            if (mascaraTitulo != null) mascaraTitulo.style.width = Length.Percent(0);

            Button btnVoltar = root.Q<Button>("btn-voltar");
            if (btnVoltar != null) btnVoltar.clicked += FecharTela;
        }

        void Start()
        {
            if (AudioManager.Instance != null && root != null)
            {
                AudioManager.Instance.ConnectButtons(root);
            }
        }

        private void Update()
        {
            if (root == null || root.style.display == DisplayStyle.None) return;

            // Animação de Carregamento do Título "Créditos"
            if (mascaraTitulo != null && progressoPreenchimento < 100f)
            {
                progressoPreenchimento += Time.deltaTime * velocidadePreenchimento;
                mascaraTitulo.style.width = Length.Percent(Mathf.Clamp(progressoPreenchimento, 0, 100));
            }
        }

        private void FecharTela()
        {
            AoFechar?.Invoke();
            gameObject.SetActive(false);
        }
    }
}