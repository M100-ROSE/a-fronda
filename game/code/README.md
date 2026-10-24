# Fronda.Client — tela de login/cadastro (leva 1) + back-end local (leva 2) + polimento (leva 3)

Tela de login/cadastro do jogo **a-fronda** em **MonoGame (C#)**. Sem
Content Pipeline. A leva 2 adicionou o **back-end local de autenticação**
(`Fronda.Auth`): sem servidor HTTP nem banco de dados — contas em um arquivo
JSON no disco, senha nunca em texto puro. A leva 3 é o **polimento da
interface** com o back-end real: caret piscando, dissolução discreta de
entrada/saída de cena, feedback de hover nos lados, tela de "bem-vindo" e o
contorno carvão aprovado.

## Como compilar

```powershell
cd game/code
dotnet build
```

## Como rodar

```powershell
dotnet run --project src/Fronda.Client
```

Janela fixa **960×540** (`Window.AllowUserResizing = false`, escala inteira ×1).
`SamplerState.PointClamp` — sem blur, pixel inteiro.

## Testar rapidamente

- **entrar**: campos vazios desligam o CTA; submit chama `AuthService` (leva 2,
  local) — sucesso dissolve a tela para a noite e abre a **cena de
  bem-vindo** (usuario + lado autenticados, nota "próxima tela do jogo"); erro
  mostra a placa com a mensagem PT-BR do back-end.
- **cadastrar**: aba/link trocam de modo; trocar limpa campos, erros e o lado
  escolhido. O CTA fica **desligado até escolher o lado** (rótulo "escolha seu
  lado"). Oposição → CTA vermelho; Reino → CTA azul.
- **teclado**: `Tab`/`Shift+Tab` ciclam (usuário → senha → confirmar →
  oposição → reino → CTA → link), `←`/`→` alternam o lado dentro do grupo,
  `Enter` submete (só com CTA habilitado), `Esc` volta ao modo entrar,
  `Espaço` ativa o botão focado.

## O que a leva 3 mudou na interface

| Item | Onde | Detalhe |
| --- | --- | --- |
| Caret piscando | `TextBox` | 30 frames aceso / 30 apagado, sem interpolação; ao ganhar foco recomeça aceso; pausa contando na perda de foco |
| Dissolução de entrada | `LoginScene` + `Widget.DegrauDissolucao` | 4 degraus discretos de COR (~200ms @60fps): painel, títulos, tabs e demais widgets misturam para a cor do fundo em degraus — o lote roda com `BlendState.Opaque`, então alpha não tem efeito; o "fade" pixel-art é feito degrau a degrau (equivalente visual aos 4 ditherings da spec §6) |
| Transição pós-êxito | `LoginScene` → `BemVindoScene` | sucesso dissolve a tela até "virar noite" (o degrau 0 torna a troca imperceptível) e a cena nova dissolve de volta; substituiu a mensagem placeholder "…conectado ao mundo" da leva 1 |
| Feedback de hover/foque dos lados | `FactionButton` | lado escolhido sobe 1px (micro-nudge, sem bounce), o outro recua 1px e o filete inferior 2px do escolhido intensifica o pano: `#7A1418` (2º degrau do pano vermelho, §3.3) na oposição e sombra embutida `#120E10` no reino (idioma de §3.4) |
| Contorno carvão | `Panel` | contorno externo 1px `#1A1210` **APROVADO**: a cor existe na paleta oficial (inspo/paleta.md, vila revolucionária nº 10 — uso "contorno, noite, silhueta") e o briefing sanciona contorno de 1px em "carvão ou grafite"; o substituto `#120E10` usado enquanto pendia a aprovação foi aposentado |
| Erro melhor | `ErrorBanner` + `TextBox` | a placa entra em 2 degraus de dissolução (metade da cor por ~3 frames, depois inteira) e o input recém-errado treme 1px para o lado por 3 frames, sem bounce |
| Pulso do título | `LoginScene` | filete de ouro 1×120 do logo pulsa em 2 degraus (40 cheio / 40 atenuado, sem interpolação) — escolhido EM VEZ da jornada de alpha (apenas um efeito, como pede a leva 3) |
| Ellipsis "…" | `PixelFont` + CTA/bem-vindo | glifo dedicado de 1 avanço de glifo; CTA usa "enviando…" e a cena de bem-vindo usa "…conectado ao mundo" (§6.1) |

## Contrato front ↔ back (leva 2, como está hoje)

- `Resultado.Sucesso` → `Jogador { Id, Usuario (normalizado/minúsculo), Lado }`.
- `credenciais_invalidas` chega **sempre com `ErroCampo = null`** → o cliente
  posiciona o selo **abaixo do CTA** (banner genérico; nunca revela se o
  usuário existe). Testes do back-end garantem os dois lados do acordo.
- Novo código possível: `usuario_invalido` no campo `usuario` — o cliente já
  mapeia qualquer erro do campo `usuario` para o selo abaixo do campo (na
  prática o filtro de caracteres do front evita chegar lá; sane de qualquer forma).
- Demais códigos iguais: `usuario_curto`, `usuario_longo`, `usuario_em_uso`,
  `senha_curta`, `senha_diferente`, `lado_obrigatorio`, `backend_ausente`.

## Decisões técnicas

- **TFM `net8.0`** com SDK 10: `MonoGame.Framework.DesktopGL` **3.8.2.1105**
  (pacote NuGet estável mais recente) distribui binários até net6.0/net8.0;
  net8.0 restaura e compila no SDK 10 sem problema. Se o MonoGame destravar
  net10, é só subir o TFM no csproj.
- **Fonte: `Font/PixelFont`** — malha 5×7 por glifo (3 linhas de diacrítico + 7
  base), gerada como pixels em runtime sobre textura 1×1 branca. Cobertura:
  A–Z, a–z, 0–9, pontuação básica (agora com **reticências "…"** dedicadas) e
  acentuação PT-BR (á à â ã ç é ê í ó ô õ ú ü + maiúsculas), composta
  sobrepondo diacrítico ao glifo-base. Avanço 6px, escala inteira.
  **Se futuramente trocarmos para SpriteFont/.mgcb**, todos os pontos de
  leitura passam por `PixelFont.Desenhar/MedirTexto` — basta trocar a classe.
  Nota: **nenhum asset do Content Pipeline existe nesta leva**.
- **Fades sem alpha**: `SpriteBatch` roda com `BlendState.Opaque` (preserva o
  contorno 1px), então a dissolução discreta é feita em degraus de COR
  (`Palette.DissolverPara`): mistura discreta da cor para o fundo sobre o qual
  o elemento pinta (Fundo p/ o painel e o que mora na noite; Painel p/ o que
  mora dentro do painel). Puro por degrau, sem interpolação — visual
  equivalente aos ditherings de §6.
- **Contorno de 1px carvão** `#1A1210` nos painéis/módulos de UI: sancionado
  pelo briefing ("contorno de 1px escuro — carvão ou grafite da paleta") e
  presente na seção da vila revolucionária da `inspo/paleta.md`.

## Mapa de módulos (`src/Fronda.Client/`)

| Pasta | Conteúdo |
| --- | --- |
| `Core/` | `GameApp` (jogo, 960×540, PointClamp, ponto único `TrocarCena`), `Scene` (abstrata), `SceneManager` (troca por corte; dissolve mora dentro das cenas), `Entrada` (diff de teclas, tecla→char) |
| `Rendering/` | `Palette` (as 10 cores da paleta de login + Carvão #1A1210 sancionado + `DissolverPara` de degraus), `ShapeBatch` (retângulos, contorno 1px, filetes; primitivos sobre `SpriteBatch`) |
| `Font/` | `PixelFont` (glifos 5×7 procedurais + acentos PT-BR compostos + reticências "…") |
| `UI/` | `Widget` (base OOP: bounds, foco, hierarquia, hit-test, `DegrauDissolucao`), `Label`, `Button` (base de estados), `FactionButton` (reino/oposição, insígnia 8×8, nudge/filete da leva 3), `BotaoCta` (§3.6), `BotaoTab` (§2.3), `LinkButton` (§3.7), `TextBox` (caret piscando 30/30, máscara de senha em bullets, trem de erro), `ToggleGroup` (escolha de lado), `Panel` (contorno carvão), `ErrorBanner` (placa `#7A1418` entrando em 2 degraus) |
| `Scenes/` | `LoginScene` (monta a tela, estados entrar↔cadastrar, validações locais, submit via `IAuthService`, dissolve de entrada/saída, pulso do filete do título), `BemVindoScene` (pós-login: jogador + lado + nota, dissolve de entrada, sem cor de facção §6.1) |
| `Auth/` | `NullAuthService` (stub da leva 1 que sempre falha; mantido só como fallback/testes — **não é mais o default**) |

## Mapa de módulos (`src/Fronda.Auth/`) — leva 2

| Pasta/Arquivo | Conteúdo |
| --- | --- |
| `Modelos/` | `Lado` (enum), `Jogador`, `Resultado` (contrato — **definição única**, era do cliente) |
| `IAuthService.cs` | contrato front ↔ back (assíncrono: permite troca futura por rede) |
| `Conta.cs` | modelo persistido (id sequencial, usuario, hash, salt, lado, dataCriacao) |
| `Hash/` | `IHasher` + `Pbkdf2Hasher` (PBKDF2-HMAC-SHA256, 150k iterações, salt 16B/conta, `FixedTimeEquals`) |
| `Armazenamento/` | `IContasRepository` + `JsonContasRepository` (JSON atômico, tolerante a arquivo ausente/corrompido) |
| `AuthService.cs` | implementação real de `IAuthService` (revalida tudo, não confia no front) |

O arquivo de contas vive em **`%APPDATA%\Fronda\usuarios.json`** (caminho
injetável por construtor — os testes usam o próprio). Testes: `tests/Fronda.Auth.Tests/`
(xunit, `dotnet test` na pasta `game/code`).

## Composição

Ponto único em `LoginScene`: `new AuthService(new Pbkdf2Hasher(), new JsonContasRepository())`.
No futuro, trocar por um serviço de rede é mudar essa linha só (mesma `IAuthService`).

Nenhum campo estático mutável global: todo estado vive nas instâncias (`GameApp`,
cenas, widgets — incluindo os contadores de frame de caret/trem/dissolução e o
degrau de cada widget).

## Validar local (mesmas regras que o back-end deve aplicar server-side)

- `usuario`: 3–20 caracteres, `a-z0-9_-` ("nome curto - 3 letras ou mais").
- `senha`: mínimo 8 ("senha curta - mínimo 8").
- `confirmaSenha`: igual à senha ("as senhas não coincidem").
- `lado`: obrigatório no cadastrar (CTA desligado sem escolha — não é erro).

Códigos curtos produzidos localmente: `usuario_curto`, `usuario_longo`,
`senha_curta`, `senha_diferente`. A falha genérica de credenciais devolve
`credenciais_invalidas` (campo null).

## Estado por leva

- **Leva 2 (back-end) — FEITA**: `Fronda.Auth` com `AuthService` local
  (JSON + PBKDF2), contrato assíncrono mantido; composição trocada em
  `LoginScene`. Falha genérica de credenciais devolve
  `{campo:null, codigo:"credenciais_invalidas"}` (antes o doc dizia campo
  "senha" — mudou para não revelar existência de conta).
- **Leva 3 (polimento) — FEITA**: contorno carvão do painel (#1A1210,
  sancionado pelo briefing), dissolução discreta na entrada da cena e na
  troca pós-login, caret piscando 30/30 frames com pausa no blur, nudge/filete
  nos botões de facção, trem de 1px no input errado, placa de erro em 2
  degraus, pulso sutil (2 degraus) no filete de ouro do título, reticências
  "…" dedicadas na fonte e `BemVindoScene` no lugar do placeholder.
- **Depois (rede)**: trocar o `AuthService` local por um serviço HTTP na
  composição de `LoginScene`, sem mudar nada mais no cliente; decidir a
  próxima tela do jogo que entra no lugar da `BemVindoScene`.
