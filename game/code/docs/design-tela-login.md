# Spec visual — Tela de login / cadastro (`a-fronda`)

Fonte da verdade: `inspo/paleta.md` (seção "## Login / cadastro") + `inspo/briefing.md`
(portão de entrada; escolha de lado) + `inspo/visual.md` (idioma pixel Stardew).
Esta spec usa **somente os 10 hexes da paleta de login**; qualquer exceção está
marcada como *PENDENTE DE APROVAÇÃO NA PALETA*.

Paleta oficial da tela (e nada além dela):

| Nome | Hex | Uso |
| --- | --- | --- |
| Fundo | `#120E10` | canvas, noite |
| Painel | `#1C1614` | card, formulário |
| Borda | `#3A3230` | input, divisor, estado desligado |
| Texto | `#F3EDE0` | labels, títulos (marfim do reino) |
| Texto mudo | `#9A958C` | placeholder, ajuda (pedra) |
| Acento oposição | `#C41E3A` | CTA / lado oposição |
| Acento reino | `#1E3A8A` | CTA / lado reino |
| Ouro | `#C9A227` | marca, detalhe do logo |
| Erro | `#7A1418` | validação (vermelho sangue, não neon) |
| Foco | `#E8DCC8` | outline de acessibilidade |

---

## 1. Conceito / sensação

A tela é um **portão à noite**: um painel único e apertado no centro de um fundo
quase preto, como a fachada fechada de um prédio clandestino — e, no horizonte do
fundo, a silhueta de uma torre com um filete de ouro que não convida. Não existe
janela de "bem-vindo". Manter todos os elementos dentro de **um único painel
 fechado** reforça a etiqueta como grade: o formulário é o primeiro imóvel do
mundo, e imóvel aqui **manda**. Os dois acentos (vermelho/azul) aparecem **só**
onde existe escolha de lado; o resto da tela é noite + marfim + ouro de filete.
O detalhe político do layout: no modo cadastrar, o CTA fica **desligado** até o
jogador declarar de que lado está — **o jogo não abre a porta para quem não
escolheu**. Isso transforma o formulário em ato, não em burocracia.

Idioma técnico (não negociável): silhueta primeiro, contorno 1px, 2–3 degraus de
cor por elemento, sem gradiente suave, sem chibi, sem neon, sem rosa, sem roxo,
sem `#FF0000`/`#0000FF` puros.

---

## 2. Espaço de renderização e layout

### 2.1 Resolução lógica

- **Resolução lógica: 960×540 px**, com **escala inteira** (×2 → 1920×1080,
  ×3 → 2880×1620). Nunca escala fracionária: em MonoGame, `SpriteBatch` com
  transform matrix de fator inteiro e `PreferMultiSampling = false`, para
  preservar o contorno de 1px.
- Fonte: **uma família pixel por jogo**, render em múltiplos de 8px (base
  sugerida: fonte 8×8; corpo 16px = 2×, título 32px = 4×). Nome final da fonte
  é decisão da Anna.
- Grade lógica: tudo encaixado em pixel inteiro; nada em subpixel.

### 2.2 Layout com medidas (px lógicos)

```
960×540 (fundo #120E10, noite)
├── silhueta do horizonte (base da tela)
│   ├── bloco esquerdo 200×72 @ (28,468): telhados apertados, cor Painel #1C1614
│   └── bloco direito  120×120 @ (772,420): torre, Painel #1C1614,
│       com filete 1px Ouro #C9A227 a 4px do topo (o castelo "aponta" gold)
└── painel 360×480 centrado @ (300,30) — centro da tela (480,270)
```

Tudo dentro do painel usa padding interno de 24px (área útil: 312px de largura).

| Elemento | Tam (l×a) | Nota |
| --- | --- | --- |
| Painel | 360×480 @ (300,30) | bg Painel `#1C1614`, borda externa 1px `#3A3230`, centro da tela (480,270) |
| Título "a-fronda" | 32px, centrado, y≈78 | texto `#F3EDE0` + **detalhe em Ouro**: 1 fronda 8×16 px em `#C9A227` acoplada ao "a" e filete `#C9A227` 1px × 120px centrado abaixo do título (y≈96) |
| Alternância entrar/cadastrar | 2 tabs de 156×24, gap 8, y≈104 | ativa: texto `#F3EDE0` + filete inferior 2px Ouro `#C9A227`; inativa: texto `#9A958C`; divisor vertical 1px `#3A3230` entre as duas |
| Label de campo | 16px, `#F3EDE0`, gap 4 acima do input | — |
| Campo usuário | input 312×40 | bg Fundo `#120E10` (mais escuro que o painel = "recorte na parede"), borda 1px `#3A3230` |
| Campo senha | input 312×40, gap 16 | idem |
| Campo confirmar senha | input 312×40, gap 16 | **só no modo cadastrar** |
| Bloco "escolha seu lado" | label 312×16 (texto `#9A958C`) + 2 botões 152×56 lado a lado (gap 8) | **só no modo cadastrar**. Convenção de leitura: **Oposição à esquerda, Reino à direita** |
| CTA principal | 312×48 | "entrar" (modo entrar, gap 24 após campos) ou "criar conta" (modo cadastrar, gap 24 após a escolha de lado) |
| Link | 312×24 | texto 16px `#9A958C` + underline 1px `#9A958C`; "criar conta" no modo entrar / "já tenho conta" no modo cadastrar |
| Selo de erro | 312×24 (1 linha) ou 312×40 (2 linhas), gap 4 abaixo do campo com erro | fundo `#7A1418` + texto `#F3EDE0` (motivo em §3.5) |

### 2.3 Alternância entrar/cadastrar

Estado **padrão da tela é "entrar"** (a ação mais frequente). Comportamento:

- **tab ativa**: texto `#F3EDE0` + filete inferior 2px Ouro `#C9A227` (filete de
  marca; se a revisão considerar ouro demais, alternativo aprovável: filete `#F3EDE0`).
- **tab inativa**: texto `#9A958C`; hover: texto sobe para `#F3EDE0`.
- **foco**: outline 1px `#E8DCC8` externo.
- Trocar de modo **redefine o formulário**: limpa campos, selos de erro e a
  seleção de lado (a escolha é por conta nova, é recomeço de decisão).

---

## 3. Componentes e estados (hex por estado)

Contrastes medidos (luminância relativa, WCAG):

| Combinação | Contraste | Nota |
| --- | --- | --- |
| `#F3EDE0` sobre `#120E10` | **16,4:1** | AAA |
| `#F3EDE0` sobre `#1C1614` | **15,3:1** | AAA |
| `#9A958C` sobre `#120E10` | **6,4:1** | AA |
| `#9A958C` sobre `#1C1614` | **6,0:1** | AA |
| `#F3EDE0` sobre CTA vermelho `#C41E3A` | **5,0:1** | AA |
| `#F3EDE0` sobre CTA azul `#1E3A8A` | **8,9:1** | AAA |
| `#F3EDE0` sobre selo erro `#7A1418` | **9,3:1** | AAA |
| `#C41E3A` (borda/preenchimento) sobre `#1C1614` | **3,1:1** | passa 3:1 não-textual |
| `#1E3A8A` (borda) sobre `#1C1614` | **1,7:1** | **NÃO passa** — por isso reino usa insígnia, não borda (§4) |
| `#E8DCC8` (outline de foco) sobre `#120E10` | **14,1:1** | foco sempre visível |
| `#9A958C` sobre `#3A3230` (CTA desligado) | **4,2:1** | disabled é isento de AA |

### 3.1 Painel / card

| Estado | BG | Borda |
| --- | --- | --- |
| default | `#1C1614` | 1px `#3A3230` |
| foco | — (n/a: painel não recebe foco; foco mora nos filhos) | — |

### 3.2 Campo de texto (input)

| Estado | BG | Borda | Texto | Extra |
| --- | --- | --- | --- | --- |
| default | `#120E10` | 1px `#3A3230` | `#F3EDE0` | placeholder `#9A958C` |
| hover | `#120E10` | 1px `#F3EDE0` | `#F3EDE0` | mouse sobre campo não focado |
| foco | `#120E10` | 1px `#3A3230` (não muda) | `#F3EDE0` | **outline externo 1px `#E8DCC8`** (offset +1px do input, sem tocar na borda) |
| pressionado | n/a | — | — | input não tem esse estado separado do foco |
| disabled | `#1C1614` | 1px `#3A3230` | `#9A958C` | durante o request de rede |
| erro | `#120E10` | 1px `#C41E3A` (3,1:1) | `#F3EDE0` | + selo de erro abaixo (§3.5). **Nunca** borda `#7A1418`: sobre o escuro ela fica invisível |
| erro + foco | `#120E10` | 1px `#C41E3A` | `#F3EDE0` | outline `#E8DCC8` por cima — teclado nunca perde o indicador |
| sucesso (validação local ok) | `#120E10` | 1px `#3A3230` (neutro) | `#F3EDE0` | sucesso é **silêncio** do erro; sem verde/cor nova |
| loading (request em curso) | `#120E10` | 1px `#3A3230` | `#F3EDE0` | input travado; CTA mostra "enviando..." |

### 3.3 Botão facção — Oposição (`#C41E3A`) — **primário da tela**

| Estado | BG | Borda | Texto | Extra |
| --- | --- | --- | --- | --- |
| default (não selecionado) | `#1C1614` | **2px `#C41E3A`** | `#F3EDE0` | rótulo + insígnia 8×8 px `#C41E3A` à esquerda do rótulo |
| hover | `#C41E3A` | 2px `#C41E3A` | `#F3EDE0` | preencher no hover = antecipação da escolha |
| pressionado | `#7A1418` | 2px `#C41E3A` | `#F3EDE0` | `#7A1418` reutilizado como 2º degrau do pano vermelho (mesma função de "sombra do pano" na paleta da vila) |
| foco | `#1C1614` (ou o BG do estado atual de hover) | 2px `#C41E3A` | `#F3EDE0` | outline externo 1px `#E8DCC8` |
| selecionado | `#C41E3A` | 2px `#C41E3A` | `#F3EDE0` | insígnia vira 8×8 `#F3EDE0` (checado) |
| disabled | `#1C1614` | 1px `#3A3230` | `#9A958C` | insígnia `#9A958C` |
| erro | n/a | — | — | não existe erro semântico de facção; sem seleção = CTA desligado, não é erro |

### 3.4 Botão facção — Reino (`#1E3A8A`) — **escolha oposta da tela**

| Estado | BG | Borda | Texto | Extra |
| --- | --- | --- | --- | --- |
| default (não selecionado) | `#1C1614` | 1px `#3A3230` (neutra) | `#F3EDE0` | rótulo + **insígnia 8×8 px `#1E3A8A`** à esquerda. A cor azul vive na insígnia, não na borda: `#1E3A8A` como borda sobre painel dá só 1,7:1 (invisível, viola 3:1 não-textual) |
| hover | `#1E3A8A` | 1px `#1E3A8A` | `#F3EDE0` | preenchimento aparece no hover (texto 8,9:1) |
| pressionado | `#1E3A8A` | 1px `#1E3A8A` | `#F3EDE0` | bloco desce 1px; filete superior 1px `#120E10` como sombra embutida |
| foco | `#1C1614` (ou o BG do estado atual de hover) | 1px `#3A3230` | `#F3EDE0` | outline externo 1px `#E8DCC8` |
| selecionado | `#1E3A8A` | 1px `#1E3A8A` | `#F3EDE0` | insígnia vira 8×8 `#F3EDE0` (checado) |
| disabled | `#1C1614` | 1px `#3A3230` | `#9A958C` | insígnia `#9A958C` |

**Regra anti-arco-íris que coordena §3.3 e §3.4:** quando um lado está
selecionado, **o outro recua automaticamente** para o estado "escolha oposta":
contorno volta a 1px `#3A3230` (se o era 2px colorido) e a identidade do lado
ficará apenas na insígnia 8×8. Assim, num mesmo instante, os dois acentos
**nunca** disputam o mesmo peso visual.

### 3.5 Selo de erro (mensagem de validação)

| Estado | BG | Texto | Extra |
| --- | --- | --- | --- |
| visível | `#7A1418` | `#F3EDE0` 16px | ícone 8×8 px `#F3EDE0` ("!" pixelado) à esquerda do texto |
| corrigido / sair | (some) | — | desaparece sem transição na leva 1 |

**Desvio documentado (importante):** o pedido era "mensagens de erro na cor
`#7A1418`". Como **texto puro** sobre o painel, `#7A1418` daria **1,65:1**
(ilegível — viola a regra não negociável de contraste máximo). Adotado: a cor
Erro permanece presente como **bloco sólido** (fundo do selo, 9,3:1 com o texto
`#F3EDE0` em cima), e a borda do input em erro usa `#C41E3A` (3,1:1, passa 3:1
não-textual) porque `#7A1418` como borda fina também não passa. O vermelho
sangue não desaparece da tela — ele muda de papel, de tinta para placa.

### 3.6 CTA principal (entrar / criar conta)

| Estado | BG | Borda | Texto | Extra |
| --- | --- | --- | --- | --- |
| default no modo **entrar** | `#3A3230` | — | `#F3EDE0` | neutro: no login ainda não existe lado político |
| hover (entrar) | `#3A3230` | outline externo 1px `#E8DCC8` | `#F3EDE0` | — |
| pressionado (entrar) | `#3A3230` | — | `#F3EDE0` | bloco desce 1px; filete 1px `#120E10` no topo como sombra |
| disabled (entrar) | `#1C1614` | 1px `#3A3230` | `#9A958C` | campos vazios ou request em curso |
| disabled no modo **cadastrar** | `#1C1614` | 1px `#3A3230` | `#9A958C` | **enquanto nenhum lado foi escolhido**; rótulo vira "escolha seu lado" — é a pressão política da tela |
| default (cadastrar) — lado **oposição** escolhido | `#C41E3A` | — | `#F3EDE0` | herda o acento do lado |
| default (cadastrar) — lado **reino** escolhido | `#1E3A8A` | — | `#F3EDE0` | herda o acento do lado |
| hover / pressionado (cadastrar) | BG do estado + degrau de sombra: `#7A1418` se oposição; desce 1px se reino | — | `#F3EDE0` | — |
| foco | qualquer BG atual | outline externo 1px `#E8DCC8` | `#F3EDE0` | — |
| loading | congela o BG anterior | — | `#F3EDE0` | rótulo "enviando..." com 3 pontos em loop |
| sucesso | §6.7 | — | — | — |

### 3.7 Link (troca de modo)

| Estado | Texto | Underline |
| --- | --- | --- |
| default | `#9A958C` | 1px `#9A958C` |
| hover | `#F3EDE0` | 1px `#F3EDE0` |
| foco | `#F3EDE0` | 1px `#F3EDE0` + outline externo 1px `#E8DCC8` |
| disabled (durante request) | `#9A958C` | sem underline |

### 3.8 Logo

| Estado | Cores |
| --- | --- |
| sempre | texto `#F3EDE0`; fronda e filete em Ouro `#C9A227`. **Nenhum acento de facção jamais no logo** — a marca é anterior aos lados |

---

## 4. Tratamento dos dois acentos (sem arco-íris)

Regra da paleta: **um é primário da tela, o outro é a escolha oposta** — nunca os
dois com o mesmo peso visual no mesmo instante.

**CTA primário desta tela: o lado `oposicao` (vermelho `#C41E3A`).** Justificativa:
criar conta na a-fronda é um ato de ruptura — entrar neste mundo de noite é
declarar-se contra o status quo, e o vermelho é a única cor da paleta com calor
de bandeira; por isso ele leva o tratamento de botão primário da área de escolha
(contorno de 2px permanente, preenchimento no hover). O reino permanece como
**escolha oposta**: sempre presente, mais recuada — exatamente como o castelo
aparece no horizonte da tela: visível, ordenado, sem convite (identidade via
insígnia 8×8, não via borda azul, que dá só 1,7:1 sobre o painel).

Distribuição para nunca virar arco-íris:

1. **Ouro** — só em 3 usos de marca: fronda/filete do logo e filete da tab ativa.
2. **Vermelho** — só em: botão facção oposição, borda de input em erro.
3. **Azul** — só em: insígnia e preenchimento do botão reino / CTA com lado reino escolhido.
4. **Numa mesma "vista" o jogador vê no máximo uma cor de facção em peso forte**;
   a outra, quando presente, está em peso baixo (insígnia 8×8 ou desligada).
5. Depois que um lado é confirmado, o outro **recua automaticamente** (§3.4) —
   a hierarquia acompanha a decisão, sempre assimétrica.

---

## 5. Foco / teclado (acessibilidade)

- **Outline de foco**: 1px `#E8DCC8`, **offset externo +1px** (não substitui a
  borda do componente; soma-se a ela). Sobre `#120E10`: 14,1:1.
- Todo controle interativo tem o mesmo outline: inputs, tabs, botões de facção,
  CTA e links.

**Ordem de tab** (modo cadastrar; o modo entrar é o subconjunto):

1. Campo usuário
2. Campo senha
3. Campo confirmaSenha
4. Botão facção **oposição**
5. Botão facção **reino**
6. CTA "criar conta"
7. Link "já tenho conta"

- `Tab` / `Shift+Tab` seguem a ordem acima, em ciclo.
- Dentro do grupo de facção (passo 4–5), `←`/`→` alternam a seleção sem sair do grupo.
- `Enter` submete **se o CTA estiver habilitado** (no modo cadastrar, exige lado escolhido).
- `Esc` no modo cadastrar volta para "entrar".
- Ao trocar de modo, o foco vai direto para o campo usuário (nada de foco perdido).
- Selos de erro aparecem **associados ao campo** com erro: campo, selo e foco
  contigo no mesmo lugar (a narração por leitor de tela do jogo é extensão
  futura, fora do escopo desta spec).

---

## 6. Leva 1 (obrigatória) vs. leva 3 (polimento)

Toda a §1–§5 é **leva 1 — obrigatória**. O que segue é polimento da leva 3:

| Item | Leva | Nota |
| --- | --- | --- |
| Layout, painel, campos, tabs, botões de facção, CTA, link, todos os estados de §3, ordem de tab | **1** | pré-requisito |
| Selo de erro (§3.5) | **1** | sem isso a validação não comunica |
| Transição pós-login (§6.1) | **1** | placeholder apontando "próxima tela do jogo" |
| Silhueta do horizonte (2 blocos no fundo, §2.2) | **1** | primeiro sintoma do mundo |
| **Contorno externo de 1px carvão** no painel | **3** | *PENDENTE DE APROVAÇÃO NA PALETA*: `carvão #1A1210` não está na paleta de login; enquanto não aprovado, usar Fundo `#120E10` (quase idêntico e já permitido) |
| Fade do painel na entrada da tela (4 degraus de dithering 25→50→75→100%, ~200ms) | **3** | fade pixel-art, sem alpha suave |
| Fade na troca de modo entrar↔cadastrar (4 ditherings, ~100ms) | **3** | — |
| Cursor de texto piscando estilo pixel (caret 2×16 px `#F3EDE0`, 30 frames aceso / 30 frames apagado, sem interpolação) | **3** | leva 1 usa caret estático |
| Hover dos botões de facção com insígnia pulsando 1px (8×8 → 10×10 → 8×8 em 60 frames) | **3** | — |
| "Costura" interna do painel: linha 1px `#3A3230` a 8px da borda (quase moldura) | **3** | — |
| Horizonte respira: fumaça da vila (2px, 1 degrau `#3A3230`, ciclo 60 frames) e lampião na torre (1px `#C9A227` piscando a cada 240 frames) | **3** | — |
| Placeholder saindo com "dithering drop" (desaparece num degrau, sem fade suave) | **3** | — |
| Selo de erro entrando como "carimbo" (desce 1px + shake de 1 frame) | **3** | — |

### 6.1 Transição de sucesso (leva 1)

- **Leva 1**: fundo `#120E10` tela cheia + texto centrado "…conectado ao mundo"
  em `#F3EDE0` 16px + filete Ouro `#C9A227` 1px × 120px central. **Sem** cor de
  facção — a próxima tela decide o clima do lado.
- **Leva 3**: fade em dithering até a próxima tela do jogo.

---

## 7. Contratos que o back-end vai precisar

Nomes de campo são o contrato front ↔ back:

| Campo | Tipo | Modo | Validação sugerida |
| --- | --- | --- | --- |
| `usuario` | string | entrar + cadastrar | 3–16 caracteres, `a-z0-9_-` (mensagem: "nome curto — 3 letras ou mais") |
| `senha` | string | entrar + cadastrar | mínimo 8 caracteres |
| `confirmaSenha` | string | **só cadastrar** | deve igualar `senha` (mensagem: "as senhas não coincidem") |
| `lado` | enum | **só cadastrar** | `reino` \| `oposicao`; obrigatório para criar conta (CTA desligado sem ele). No login o campo **não** é enviado |

- **Endpoints sugeridos**:
  - `POST /auth/registrar` — recebe `{usuario, senha, confirmaSenha, lado}`
  - `POST /auth/entrar` — recebe `{usuario, senha}`
- **Respostas de erro padronizadas**: `{ campo?: "senha"|"usuario"|"lado", codigo: "...", mensagem: "..." }`,
  com códigos curtos (`usuario_curto`, `senha_diferente`, `credenciais_invalidas`,
  `usuario_em_uso`). O front mapeia código → mensagem PT-BR → campo, para posicionar
  o selo de erro no lugar certo.
- O back-end **não** decide estilo, cor ou layout; erros de formulário locais
  (campo vazio, senhas diferentes) não geram request.
- `lado` deve ser armazenado com a conta e retornado em `GET /sessao` para o
  front decidir o clima da próxima tela.

---

## Checklist de aceite visual (leva 1)

1. Nenhum hex fora da paleta de login em qualquer estado de qualquer componente.
2. Todos os textos ≥ AA nas combinações da tabela de contrastes (§3).
3. Os dois acentos nunca disputam o mesmo peso visual no mesmo instante (§4).
4. CTA "criar conta" fica desligado até haver lado escolhido, com rótulo explicando.
5. Todo controle focado mostra o outline `#E8DCC8`; ordem de tab conforme §5.
6. `#7A1418` nunca aparece como texto fino sobre escuro — sempre como placa (§3.5).
7. Tudo em pixel inteiro; painel centrado exato em (480,270); troca de modo limpa
   campos, erros e seleção de lado.
