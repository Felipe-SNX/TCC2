// INSTRUÇÕES: SISTEMA DE BACKGROUND COM TRANSIÇÃO DE CORES

== COMPONENTES CRIADOS ==

1. BackgroundManager.cs
   - Script principal que gerencia o background
   - Renderiza o background e controla transições de cores
   - Permite configurar tamanho e múltiplas cores

2. BackgroundConfig.cs
   - Scriptable Object com configurações do background
   - Armazena tamanho, cores, velocidade de transição
   - Criável via menu: Create > Platformer > Background Config

3. BackgroundProgressionController.cs
   - Sincroniza a progressão do background com o progresso do mapa
   - Pode rastrear posição do jogador e ajustar velocidade de cores

== COMO USAR ==

PASSO 1: Criar uma configuração de background
---------
1. Clique com direito em uma pasta (recomendado: Assets/Settings/Background)
2. Selecione: Create > Platformer > Background Config
3. Renomeie para algo descritivo (ex: "BackgroundConfig_Green_Level")

PASSO 2: Configurar a aparência do background
---------
1. Abra o BackgroundConfig que criou
2. Configure os seguintes parâmetros:

   BACKGROUND SIZE:
   - X, Y, Z: Tamanho do background em unidades de mundo
   - Recomendado: X = largura câmera (16), Y = altura câmera (10)

   COLOR CONFIGURATION:
   - Colors: Lista de cores para transicionar
   - Adicione quantas cores quiser (mínimo 1, máximo recomendado: 8)
   - As cores serão intercaladas durante o jogo

   TRANSITION SETTINGS:
   - colorTransitionDuration: Tempo (em segundos) entre cores
   - transitionSmoothness: Suavidade da transição (0 = linear, 1 = suave)

   DISPLAY SETTINGS:
   - sortingOrder: Ordem de renderização (-100 = bem atrás)
   - sortingLayerName: Nome da layer de renderização

PASSO 3: Criar o GameObject do background na cena
---------
1. Crie um novo GameObject vazio (Name: "Background")
2. Posicione na Z negativa (ex: Z = 5)
3. Adicione componente: BackgroundManager
4. Arraste o BackgroundConfig criado no campo "Config"
5. Clique em Play para testar!

PASSO 4: (Opcional) Adicionar progressão baseada no mapa
---------
1. No mesmo GameObject "Background", adicione: BackgroundProgressionController
2. Configure:
   - Target Transform: Arraste o Transform do jogador
   - Progression Start Value: 0 (onde começa)
   - Progression Max Value: 100 (onde a progressão máxima é atingida)
   - Use Position Progression: true (rastrear posição X do jogador)
   - Progression Axis: 0 (X = 0, Y = 1, Z = 2)

== EXEMPLOS DE CONFIGURAÇÃO ==

EXEMPLO 1: Dia → Entardecer → Noite
--
Colors:
  - Amarelo claro: (1, 1, 0.5)
  - Laranja: (1, 0.6, 0)
  - Roxo escuro: (0.4, 0, 0.6)
  - Azul noite: (0, 0, 0.3)
Transition Duration: 20 segundos

EXEMPLO 2: Verde Floresta (com variações)
--
Colors:
  - Verde claro: (0.2, 1, 0.2)
  - Verde normal: (0, 0.8, 0)
  - Verde escuro: (0, 0.5, 0)
Transition Duration: 15 segundos

EXEMPLO 3: Lava/Vulcão
--
Colors:
  - Cinza: (0.5, 0.5, 0.5)
  - Laranja: (1, 0.5, 0)
  - Vermelho: (0.8, 0, 0)
  - Roxo: (0.6, 0, 1)
Transition Duration: 8 segundos (mais rápido = mais caótico)

== CONTROLAR PELO CÓDIGO ==

// Obter referência do BackgroundManager
BackgroundManager bgManager = GetComponent<BackgroundManager>();

// Mudar tamanho do background
bgManager.SetBackgroundSize(new Vector3(20, 12, 1));

// Adicionar uma nova cor
bgManager.AddColor(Color.red);

// Mudar velocidade de progressão
bgManager.SetProgressionSpeed(2f); // 2x mais rápido

// Obter índice de cor atual
int colorIndex = bgManager.GetCurrentColorIndex();

// Obter progresso da transição (0 a 1)
float transitionProgress = bgManager.GetColorTransitionProgress();

== DICAS ==

1. Use cores complementares para transições suaves
2. Durações menores (5-10s) criam efeito mais dinâmico
3. Durações maiores (20-30s) criam atmosfera mais calma
4. Teste com diferentes suavidades de transição
5. O background fica sempre atrás de outros elementos por causa do sortingOrder negativo
6. Você pode ter múltiplos BackgroundManagers em cenas diferentes com configurações únicas

== TROUBLESHOOTING ==

Problema: Background não aparece
Solução: Verifique se tem componente SpriteRenderer, ou se a câmera está visualizando o GameObject

Problema: Cores não transitam
Solução: Verifique se tem mais de uma cor no BackgroundConfig e transitionDuration > 0

Problema: Background muito pequeno/grande
Solução: Ajuste o Vector3 backgroundSize no BackgroundConfig
