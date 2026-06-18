using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class ConfigGraphicsView
{
    private VisualElement root;
    private const int QUALIDADE_PADRAO = 3; 
    private const string MODO_TELA_PADRAO = "Tela Cheia";
    private DropdownField drpResolucao;
    private DropdownField drpModoTela;
    private DropdownField drpQualidade;
    private Resolution[] resolucoesDisponiveis;

    public void Inicializar(VisualElement container)
    {
        root = container;

        drpResolucao = root.Q<DropdownField>("drp-resolucao");
        drpModoTela = root.Q<DropdownField>("drp-modo-tela");
        drpQualidade = root.Q<DropdownField>("drp-qualidade");

        ConfigurarResolucao();
        ConfigurarModoJanela();
        ConfigurarQualidadeDropdown();

        Button btnReset = root.Q<Button>("btn-reset-graficos");
        if (btnReset != null) btnReset.clicked += ResetarParaPadrao; 
    }

    private void ConfigurarResolucao()
    {
        DropdownField drp = root.Q<DropdownField>("drp-resolucao");
        if (drp == null) return;

        resolucoesDisponiveis = Screen.resolutions;
        drp.choices.Clear();

        int indiceAtual = 0;
        for (int i = 0; i < resolucoesDisponiveis.Length; i++)
        {
            string opcao = $"{resolucoesDisponiveis[i].width}x{resolucoesDisponiveis[i].height}";
            drp.choices.Add(opcao);

            if (resolucoesDisponiveis[i].width == Screen.currentResolution.width &&
                resolucoesDisponiveis[i].height == Screen.currentResolution.height)
            {
                indiceAtual = i;
            }
        }

        drp.index = indiceAtual;
        drp.RegisterValueChangedCallback(evt => {
            var res = resolucoesDisponiveis[drp.index];
            Screen.SetResolution(res.width, res.height, Screen.fullScreenMode);
        });
    }

    private void ConfigurarModoJanela()
    {
        DropdownField drp = root.Q<DropdownField>("drp-modo-tela");
        if (drp == null) return;

        drp.choices = new List<string> { "Tela Cheia", "Janela" };
        drp.value = Screen.fullScreen ? "Tela Cheia" : "Janela";

        drp.RegisterValueChangedCallback(evt => {
            if (evt.newValue == "Tela Cheia")
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            else
                Screen.fullScreenMode = FullScreenMode.Windowed;
        });
    }

    private void ConfigurarQualidadeDropdown()
    {
        DropdownField drp = root.Q<DropdownField>("drp-qualidade");
        if (drp == null) return;

        List<string> qualidades = new List<string>(QualitySettings.names);
        drp.choices = qualidades;
        drp.index = QualitySettings.GetQualityLevel();

        drp.RegisterValueChangedCallback(evt => {
            QualitySettings.SetQualityLevel(drp.index, true);
            Debug.Log($"Qualidade alterada para: {evt.newValue}");
        });
    }

    private void ResetarParaPadrao()
    {
        QualitySettings.SetQualityLevel(QUALIDADE_PADRAO, true);
        if (drpQualidade != null) drpQualidade.index = QUALIDADE_PADRAO;

        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        if (drpModoTela != null) drpModoTela.value = MODO_TELA_PADRAO;

        Resolution nativa = Screen.resolutions[Screen.resolutions.Length - 1];
        Screen.SetResolution(nativa.width, nativa.height, true);
        
        if (drpResolucao != null) drpResolucao.index = drpResolucao.choices.Count - 1;

        Debug.Log("Configurações de Gráficos Resetadas");
    }
}