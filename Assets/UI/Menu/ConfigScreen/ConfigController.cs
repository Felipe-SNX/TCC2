using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.UI.Menu.ConfigScreen
{
    public class ConfigController : MonoBehaviour, IMenuPopup
    {
        public Action AoFechar { get; set; }

        [Header("Sub-Sistemas")]
        public ConfigAudioView viewAudio;
        public ConfigControlsView viewControles;
        public ConfigGraphicsView viewGraficos;

        [Header("Animação de Pulso")]
        public float velocidadePulso = 5f;

        [Header("Animação de Carregamento")]
        public float velocidadePreenchimento = 50f; 
        private float progressoPreenchimento = 0f;
        
        private VisualElement root;
        private Label tituloAba;
        private VisualElement conteudoAudio;
        private VisualElement conteudoControles;
        private VisualElement conteudoGraficos;
        private Button btnAudio;
        private Button btnControles;
        private Button btnGraficos;

        // --- Referências de Animação ---
        private VisualElement mascaraTitulo;
        private List<VisualElement> elementosOpacidade = new();
        private List<VisualElement> bordasGraficos = new();
        
        private void OnEnable()
        {
            root = GetComponent<UIDocument>().rootVisualElement;

            tituloAba = root.Q<Label>("titulo-aba");
            conteudoAudio = root.Q<VisualElement>("conteudo-audio");
            conteudoControles = root.Q<VisualElement>("conteudo-controles");
            conteudoGraficos = root.Q<VisualElement>("conteudo-graficos");

            ScrollView listaControles = root.Q<ScrollView>("lista-controles-container");

            if (viewControles != null && listaControles != null)
            {
                viewControles.Inicializar(listaControles);
            }

            btnAudio = root.Q<Button>("btn-aba-audio");
            btnControles = root.Q<Button>("btn-aba-controles");
            btnGraficos = root.Q<Button>("btn-aba-graficos");
            Button btnVoltar = root.Q<Button>("btn-voltar");

            if (viewAudio != null && conteudoAudio != null) viewAudio.Inicializar(conteudoAudio);
            if (viewControles != null && conteudoControles != null) viewControles.Inicializar(conteudoControles);
            if (viewGraficos != null && conteudoGraficos != null) viewGraficos.Inicializar(conteudoGraficos);

            if (btnAudio != null) btnAudio.clicked += MostrarAbaAudio;
            if (btnControles != null) btnControles.clicked += MostrarAbaControles;
            if (btnGraficos != null) btnGraficos.clicked += MostrarAbaGraficos;
            if (btnVoltar != null) btnVoltar.clicked += FecharTela; 

            ConfigurarAnimacaoCromatica();

            MostrarAbaAudio();

            progressoPreenchimento = 0f;
            if (mascaraTitulo != null) mascaraTitulo.style.width = Length.Percent(0);
        }

        void Start()
        {
            if (UIAudioManager.Instance != null)
            {
                UIAudioManager.Instance.ConnectButtons(root);
            }
        }

        private void Update()
        {
            if (root == null || root.style.display == DisplayStyle.None) return;

            float pulso = (Mathf.Sin(Time.time * velocidadePulso) + 1f) / 2f; 
            
            float opacidadeForte = Mathf.Lerp(0.4f, 1f, pulso);
            float opacidadeFraca = Mathf.Lerp(0.2f, 0.8f, pulso);

            foreach (var el in elementosOpacidade)
            {
                if (el != null) el.style.opacity = opacidadeForte;
            }

            Color cianoPulso = new(0f, 1f, 1f, opacidadeFraca);
            foreach (var borda in bordasGraficos)
            {
                if (borda != null) 
                {
                    borda.style.borderBottomColor = cianoPulso;
                    
                    borda.style.borderTopColor = Color.clear;
                    borda.style.borderLeftColor = Color.clear;
                    borda.style.borderRightColor = Color.clear;
                }
            }

            if (mascaraTitulo != null && progressoPreenchimento < 100f)
            {
                progressoPreenchimento += Time.deltaTime * velocidadePreenchimento;
                mascaraTitulo.style.width = Length.Percent(Mathf.Clamp(progressoPreenchimento, 0, 100));
            }
        }

        private void ConfigurarAnimacaoCromatica()
        {
            mascaraTitulo = root.Q<VisualElement>("mascara-titulo");

            elementosOpacidade = root.Query<VisualElement>(className: "elemento-pulsante").ToList();
            bordasGraficos = root.Query<VisualElement>(className: "borda-cromatica").ToList();
        }

        private void FecharTela()
        {
            AoFechar?.Invoke(); 
            gameObject.SetActive(false);
        }

        private void MostrarAbaAudio()
        {
            EsconderAbas();
            tituloAba.text = "Audio";
            conteudoAudio.style.display = DisplayStyle.Flex;
            btnAudio.AddToClassList("aba-ativa"); 
        }

        private void MostrarAbaControles()
        {
            EsconderAbas();
            tituloAba.text = "Controles";
            conteudoControles.style.display = DisplayStyle.Flex;
            btnControles.AddToClassList("aba-ativa"); 
        }

        private void MostrarAbaGraficos()
        {
            EsconderAbas();
            tituloAba.text = "Graficos";
            conteudoGraficos.style.display = DisplayStyle.Flex;
            btnGraficos.AddToClassList("aba-ativa"); 
        }

        private void EsconderAbas()
        {
            conteudoAudio.style.display = DisplayStyle.None;
            conteudoControles.style.display = DisplayStyle.None;
            conteudoGraficos.style.display = DisplayStyle.None;

            btnGraficos.RemoveFromClassList("aba-ativa"); 
            btnControles.RemoveFromClassList("aba-ativa"); 
            btnAudio.RemoveFromClassList("aba-ativa"); 
        }
    }
}