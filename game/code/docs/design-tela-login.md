# Spec visual — Tela de login / cadastro (`a-fronda`)

Fonte da verdade: `inspo/paleta.md` (seção "## Login / cadastro") + `inspo/briefing.md`
(portão de entrada; escolha de lado) + `inspo/visual.md` (idioma pixel Stardew).
Fonte técnica da tipografia: `game/code/src/Fronda.Client/Font/PixelFont.cs`
(glifo 5×7 procedural — todas as medidas de texto desta spec saem dela, ver §2.1 e §2.4).
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

### 2.1 Resolução lógica e escala da janela

- **Resolução lógica: 960×540 px (backbuffer lógico inalterado).** **A janela abre
  em escala inteira ×2 → 1920×1080.** Nunca escala fracionária: em MonoGame,
  `SpriteBatch.Begin` com **transform matrix de fator inteiro ×2**
  (`Matrix.CreateScale(2f)` ou matriz equivalente montada pela Anna),
  `SamplerState.PointClamp`, `PreferMultiSampling = false`, sem interpolação —
  cada pixel lógico vira exatamente 2×2 px de janela e o contorno de 1px sobrevive.
- Se a área de trabalho do monitor não comportar **1080px de altura** (janela não
  cabe), cair para **×1 (960×540)** — o fallback sancionado é o próximo inteiro
  para baixo, nunca valor fracionário entre os dois.
- Toda coordenada da §2.2 continua em **px lógicos**; a matriz de escala é apenas
  multiplicada por 2 na hora do draw. Nada no layout muda.
- Fonte: **uma família pixel por jogo, já existente: a procedural 5×7 do
  `PixelFont.cs`** — glifo 5 colunas × 7 linhas de base, **AlturaBase 7**,
  **AlturaComAcento 10** (3 linhas de diacrítico acima da base), cedilha com +2
  linhas abaixo da base (total 12 linhas), **avanço 6px por caractere**.
  Escalar = multiplicar cada pixel por N (`Rectangle` inteiros por pixel; sem
  interpolação). Escalas oficiais nesta tela: **título 5, corpo/labels/botões 3,
  subtítulo/rodapé 2** (medidas por elemento em §2.2, regras de encaixe em §2.4).
- Grade lógica: tudo encaixado em pixel inteiro; nada em subpixel.

### 2.2 Layout com medidas (px lógicos)

```
960×540 (fundo #120E10, noite)
├── silhueta do horizonte (base da tela)
│   ├── bloco esquerdo 200×72 @ (28,468): telhados apertados, cor Painel #1C1614
│   └── bloco direito  120×120 @ (772,420): torre, Painel #1C1614,
│       com filete 1px Ouro #C9A227 a 4px do topo (o castelo "aponta" gold)
├── painel 360×480 @ (300,30) — centro da tela (480,270)
└── rodapé de teclado, FORA do painel: texto 432×14 @ (264,518)
    ("tab navega - enter envia - esc volta", Texto Mudo `#9A958C`, escala 2;
    separador é " - " porque a fonte não tem meia-pilha; não colide com a
    silhueta — x 264–696 fica entre os blocos 228 e 772 — e dá à tela um chão)
```

Canvas, painel e silhueta: **coordenadas inalteradas**. A mudança de legibilidade
acontece DENTRO do painel e na fonte, nunca no palco:

- **Padding interno do painel: 16px** (antes 24). Área útil nova:
  **328×448 @ (316,46)–(644,494)**. O respiro interno foi o preço das escalas
  ampliadas; o painel segue 360×480, fechado, na mesma posição.
- **Layout é fluxo vertical**: os elementos são desenhados em sequência, de cima
  para baixo, nas posições `pos.Y` da tabela (ver §2.4 para o que é `pos.Y`).
  Quando um **selo de erro** aparece (§3.5), tudo abaixo do campo com erro
  **desce 32px** (selo 28 + gap 4) e a folga interna do painel absorve o
  deslocamento — o painel não muda de tamanho nunca.
- **Folga do fluxo por modo**: no modo **cadastrar** cabe **1 selo por vez**
  (folga interna final exata: 32px); mostrar sempre o **primeiro erro na ordem
  de tab** (§5) até ele ser corrigido. No modo **entrar** a folga interna é
  147px — os 2 selos dos 2 campos cabem ao mesmo tempo.
- **Tabelas de posição**: §2.2 dá as coordenadas por modo (`—` = elemento não
  existe no modo). Modo cadastrar é o mais denso e define o pior caso.

Tabela de elementos — todos os textos na fonte 5×7 (§2.1); `pos.Y` = topo da
base do glifo, e o slot do elemento já inclui a folga de acento/cedilha que a
tabela declara (cálculo em §2.4):

| Elemento | Tam (l×a) | Modo entrar | Modo cadastrar | Cor / nota |
| --- | --- | --- | --- | --- |
| Painel | 360×480 @ (300,30) | igual | igual | bg Painel `#1C1614`, borda externa 1px `#3A3230`, centro (480,270) |
| Título "a-fronda" | texto 240×35 (escala 5) | pos (360,46) | idem | `#F3EDE0`, centrado (insets 44/44); fronda 8×16 `#C9A227` acoplada ao "a"; **filete Ouro 1×120 @ (420,83)** |
| Subtítulo "meia-noite: escolha o lado" | texto 312×14 (escala 2) | pos (324,87) | idem | **novo**; `#9A958C`, centrado sob o filete |
| Tabs entrar / cadastrar | A 128×24 @ (316,105); B 192×24 @ (452,105), gap 8 | igual | idem | texto escala 3 (pos.Y = 106): "entrar" 108px e "cadastrar" 162px centrados na própria tab; ativa: `#F3EDE0` + filete inferior 2px Ouro (linhas 22–24); inativa: `#9A958C`; divisor 1px `#3A3230` no gap entre elas |
| Label "usuário" | slot 30×328 @ (316,133), pos.Y 142 | igual | idem | `#F3EDE0` escala 3 ("á" toma a folga de 9px do topo do slot) |
| Input usuário | 328×30 @ (316,167) | igual | idem | bg Fundo `#120E10` ("recorte na parede"), borda 1px `#3A3230`; estados §3.2 |
| Label "senha" | slot 21×328 @ (316,203), pos.Y 203 | igual | idem | idem label |
| Input senha | 328×30 @ (316,228) | igual | idem | idem input |
| Label "confirmar senha" | slot 27×328 @ (316,264), pos.Y 264 | — | igual | idem label; slot 27 = base 21 + **cedilha 6px** ("ç" desce, nunca invade o input) |
| Input confirmaSenha | 328×30 @ (316,295) | — | igual | idem input |
| Label "escolha seu lado" | slot 21×328 @ (316,329), pos.Y 329 | — | igual | **corrigido: `#F3EDE0`** (antes `#9A958C`) — é label, e a paleta manda `/Texto` em labels; `#9A958C` fica só para ajuda |
| Botões de facção | 2× 160×40 @ (316,354) e (484,354), gap 8 | — | igual | **Oposição à esquerda, Reino à direita**; texto escala 3 (pos.Y = 365), rótulos "oposição" 144px / "reino" 90px centrados; insígnia 8×8 no topo central do botão @ (76,2) rel.; estados §3.3/§3.4 |
| CTA principal | 328×36 | @ (316,282) (gap 24 após campos) | @ (316,402) (gap 8 após facção) | rótulo escala 3 (pos.Y = y+7): "entrar" 108px / "criar conta" 198px / "escolha seu lado" 288px (desligado) / "enviando..." 198px — todos ≤ 328px, centrados; estados §3.6 |
| Troca de modo (link) | slot 21×328, pos.Y 326 / 441 | @ (316,326) | @ (316,441) | texto 198px ("criar conta") ou 252px ("já tenho conta"), centrado, `#9A958C` + underline 1px `#9A958C` (underline 2px abaixo da base, na linha 22 do slot); estados §3.7 |
| Selo de erro | 328×28 | fluxo: gap 4 abaixo do campo com erro, tudo abaixo desce 32px | idem (1 por vez) | fundo `#7A1418` + texto `#F3EDE0` escala 3 (pos.Y = selo+3, ícone "!" 5×7 px à esquerda; motivo em §3.5) |
| Rodapé de teclado | texto 432×14 @ (264,518) | igual | idem | **novo**; `#9A958C` escala 2, fora do painel, sobre a noite (6,4:1, AA) |

### 2.3 Alternância entrar/cadastrar

Estado **padrão da tela é "entrar"** (a ação mais frequente). Comportamento:

- **tab ativa**: texto `#F3EDE0` + filete inferior 2px Ouro `#C9A227` (filete de
  marca; se a revisão considerar ouro demais, alternativo aprovável: filete `#F3EDE0`).
- **tab inativa**: texto `#9A958C`; hover: texto sobe para `#F3EDE0`.
- **foco**: outline 1px `#E8DCC8` externo.
- Trocar de modo **redefine o formulário**: limpa campos, selos de erro e a
  seleção de lado (a escolha é por conta nova, é recomeço de decisão).

### 2.4 Regras de encaixe de texto (contrato com `PixelFont.cs`)

Constantes da fonte (`PixelFont.cs`): `LarguraGlifo 5`, `AlturaBase 7`,
`AlturaComAcento 10` (3 linhas de diacrítico), cedilha +2 linhas ABAIXO da base,
`Avanco 6`. Em `Desenhar(lote, texto, posicao, cor, escala)`, o `posicao.Y`
recebido é o **topo da base** do glifo; a fonte desenhará sozinha…

- **acento**: linhas `pos.Y−3·N` até `pos.Y` (acima da base);
- **base**: `pos.Y` até `pos.Y+7·N`;
- **cedilha** (ç): `pos.Y+7·N` até `pos.Y+9·N` (abaixo da base).

Fórmulas para a Anna (N = escala):

1. **Largura do texto** = nº de caracteres × 6 × N (`LarguraTexto`).
2. **Altura de slot** = (3·N se a linha tiver acento) + 7·N (base sempre) +
   (2·N se a linha tiver ç). Sem nada disso extra: slot = 7·N.
   Com acento e ç juntos: 12·N.
3. **`pos.Y` do slot** = topo do slot + 3·N **se** a linha tem acento; senão
   topo do slot.
4. `MedirTexto` reporta só a base (7·N). O slot (folga de acento/cedilha) é
   responsabilidade da UI — **é ele que esta spec dimensiona por elemento**.
5. Sempre conferir: largura do texto + 2×padding interno ≤ largura do slot.

Medidas das classes de texto desta tela (alturas de slot já aplicadas em §2.2):

| Classe | Escala | Base | Com acento | Com ç | Onde |
| --- | --- | --- | --- | --- | --- |
| Título | 5 | 35 | 50 | 60 | título (sem acento: "a-fronda" usa 35) |
| Corpo/labels/botões | 3 | 21 | 30 | 36 | labels, tabs, inputs, botões, CTA, link, selo (label "usuário" usa 30; "confirmar senha" usa 27; rótulo "oposição" reserva 6px de cedilha dentro do botão de 40) |
| Subtítulo/rodapé | 2 | 14 | 20 | 28 | subtítulo e rodapé (nenhum tem acento: 14) |

Escalas interditadas nesta tela: 1 (ilegível em 1080p) e escala fracionária
(nunca existe — pixel inteiro, cada pixel lógico vira N×N de tela).

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

**Escrito dentro do campo (escala 3; recuo esquerdo 12px; `pos.Y` = topo do
input + 4 → base 21 dentro dos 30px do slot, 4 de teto e 5 de piso):**

| Estado | Escrito |
| --- | --- |
| vazio (placeholder, `#9A958C`) | usuário: "seu nome na vila" (288px) · senha: "segredo de oito" (270px) · confirmaSenha: "repita o segredo" (288px) |
| digitando, usuário (`#F3EDE0`) | em claro; charset §7 (`a-z0-9_-`) garante texto sem acento até 16 glifos = 288px + 12 de recuo ≤ interior do input |
| digitando, senha e confirmaSenha (`#F3EDE0`) | **cada caractere vira `*`** (glifo existe na fonte); o valor real vive na memória do front; máscara truncada na exibição em 16 `*` |
| caret (leva 1) | estático, 2×21 `#F3EDE0`, 2px à direita do último caractere/`*` |

### 3.3 Botão facção — Oposição (`#C41E3A`) — **primário da tela**

| Estado | BG | Borda | Texto | Extra |
| --- | --- | --- | --- | --- |
| default (não selecionado) | `#1C1614` | **2px `#C41E3A`** | `#F3EDE0` | rótulo "oposição" (144px) centrado na metade de baixo + **insígnia 8×8 px `#C41E3A` cravada no topo central** @ (76,2) rel. ao botão |
| hover | `#C41E3A` | 2px `#C41E3A` | `#F3EDE0` | preencher no hover = antecipação da escolha; **insígnia vira `#F3EDE0`** (não desaparece dentro do pano vermelho) |
| pressionado | `#7A1418` | 2px `#C41E3A` | `#F3EDE0` | `#7A1418` reutilizado como 2º degrau do pano vermelho (mesma função de "sombra do pano" na paleta da vila) |
| foco | `#1C1614` (ou o BG do estado atual de hover) | 2px `#C41E3A` | `#F3EDE0` | outline externo 1px `#E8DCC8` |
| selecionado | `#C41E3A` | 2px `#C41E3A` | `#F3EDE0` | insígnia vira 8×8 `#F3EDE0` (checado) |
| disabled | `#1C1614` | 1px `#3A3230` | `#9A958C` | insígnia `#9A958C` |
| erro | n/a | — | — | não existe erro semântico de facção; sem seleção = CTA desligado, não é erro |

### 3.4 Botão facção — Reino (`#1E3A8A`) — **escolha oposta da tela**

| Estado | BG | Borda | Texto | Extra |
| --- | --- | --- | --- | --- |
| default (não selecionado) | `#1C1614` | 1px `#3A3230` (neutra) | `#F3EDE0` | rótulo "reino" (90px) centrado na metade de baixo + **insígnia 8×8 px `#1E3A8A` cravada no topo central** @ (76,2) rel. ao botão. A cor azul vive na insígnia, não na borda: `#1E3A8A` como borda sobre painel dá só 1,7:1 (invisível, viola 3:1 não-textual) |
| hover | `#1E3A8A` | 1px `#1E3A8A` | `#F3EDE0` | preenchimento aparece no hover (texto 8,9:1); **insígnia vira `#F3EDE0`** |
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
| visível | `#7A1418` | `#F3EDE0` escala 3 (`pos.Y` = selo + 3) | selo **328×28** (largura da área útil): glifo "!" 5×7 na escala 3 (15×21) @ x+8, gap 8, mensagem à direita; sem acento: base 21 no `pos.Y` do selo+3 (3 de teto para o acento, 4 de piso); com acento, o diacrítico usa o teto (fórmulas §2.4) |
| corrigido / sair | (some) | — | desaparece sem transição na leva 1; o fluxo sobe de volta os 32px reservados (§2.2) |

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

Toda a §1–§5 é **leva 1 — obrigatória**. As escalas novas de §2.4 não mudam
nenhum degrau de dissolve: o dithering é por **pixel lógico** (o fade acontece
antes da matriz ×2), então continua idêntico com título 5×, corpo 3× e rodapé 2×.
O que segue é polimento da leva 3:

| Item | Leva | Nota |
| --- | --- | --- |
| Layout, painel, campos, tabs, botões de facção, CTA, link, todos os estados de §3, ordem de tab | **1** | pré-requisito |
| Selo de erro (§3.5) | **1** | sem isso a validação não comunica |
| Transição pós-login (§6.1) | **1** | placeholder apontando "próxima tela do jogo" |
| Silhueta do horizonte (2 blocos no fundo, §2.2) | **1** | primeiro sintoma do mundo |
| **Contorno externo de 1px carvão** no painel | **3** | *PENDENTE DE APROVAÇÃO NA PALETA*: `carvão #1A1210` não está na paleta de login; enquanto não aprovado, usar Fundo `#120E10` (quase idêntico e já permitido) |
| Fade do painel na entrada da tela (4 degraus de dithering 25→50→75→100%, ~200ms) | **3** | fade pixel-art, sem alpha suave |
| Fade na troca de modo entrar↔cadastrar (4 ditherings, ~100ms) | **3** | — |
| Cursor de texto piscando estilo pixel (caret 2×21 px `#F3EDE0` — acompanha a base da escala 3, §2.4 — 30 frames aceso / 30 apagado, sem interpolação) | **3** | leva 1 usa caret estático (§3.2) |
| Hover dos botões de facção com insígnia pulsando 1px (8×8 → 10×10 → 8×8 em 60 frames) | **3** | — |
| "Costura" interna do painel: linha 1px `#3A3230` a 8px da borda (quase moldura) | **3** | — |
| Horizonte respira: fumaça da vila (2px, 1 degrau `#3A3230`, ciclo 60 frames) e lampião na torre (1px `#C9A227` piscando a cada 240 frames) | **3** | — |
| Placeholder saindo com "dithering drop" (desaparece num degrau, sem fade suave) | **3** | — |
| Selo de erro entrando como "carimbo" (desce 1px + shake de 1 frame) | **3** | — |

### 6.1 Transição de sucesso (leva 1)

- **Leva 1**: fundo `#120E10` tela cheia + texto centrado "...conectado ao mundo"
  em `#F3EDE0` na **escala 3 (21px, largura 378px)** + filete Ouro `#C9A227`
  1px × 120px central. **Sem** cor de facção — a próxima tela decide o clima do
  lado. (Os três pontos são três glifos "."; o glifo único "…" da fonte é uso
  autorizado apenas na leva 3.)
- **Leva 3**: fade em dithering até a próxima tela do jogo.

---

## 7. Contratos que o back-end vai precisar

Nomes de campo são o contrato front ↔ back:

| Campo | Tipo | Modo | Validação sugerida |
| --- | --- | --- | --- |
| `usuario` | string | entrar + cadastrar | 3–16 caracteres, `a-z0-9_-` (mensagem: "nome de 3 letras") |
| `senha` | string | entrar + cadastrar | mínimo 8 caracteres (mensagem: "no mínimo 8") |
| `confirmaSenha` | string | **só cadastrar** | deve igualar `senha` (mensagem: "senhas não batem") |
| `lado` | enum | **só cadastrar** | `reino` \| `oposicao`; obrigatório para criar conta (CTA desligado sem ele). No login o campo **não** é enviado |

- **Endpoints sugeridos**:
  - `POST /auth/registrar` — recebe `{usuario, senha, confirmaSenha, lado}`
  - `POST /auth/entrar` — recebe `{usuario, senha}`
- **Respostas de erro padronizadas**: `{ campo?: "senha"|"usuario"|"lado", codigo: "...", mensagem: "..." }`,
  com códigos curtos (`usuario_curto`, `senha_diferente`, `credenciais_invalidas`,
  `usuario_em_uso`). O front mapeia código → mensagem PT-BR → campo, para posicionar
  o selo de erro no lugar certo.
- **Restrição tipográfica do selo** (§2.4/§3.5): mensagem de **1 linha, no
  máximo 16 glifos (288px)** — cabem exatamente no selo de 328×28 junto ao
  ícone "!". Sem travessão longo "—" (a fonte não o tem; usar "-"). Acentos
  comuns (ã, á, í) são bem-vindos; entra na folga de acento do slot.
  Mensagens sancionadas: "nome de 3 letras", "no mínimo 8", "senhas não batem",
  "dados errados", "nome em uso".
- O back-end **não** decide estilo, cor ou layout; erros de formulário locais
  (campo vazio, senhas diferentes) não geram request.
- `lado` deve ser armazenado com a conta e retornado em `GET /sessao` para o
  front decidir o clima da próxima tela.

---

## Checklist de aceite visual (leva 1)

**Paleta e contrastes**

1. Nenhum hex fora da paleta de login em qualquer estado de qualquer componente —
   a ampliação de texto (§2.1/§2.4) **não cria hex novo**: `#F3EDE0` e `#9A958C`
   prosseguem nos mesmos fundos (15,3:1 e 6,0:1 sobre o painel; 6,4:1 do rodapé
   sobre a noite) e todos os textos seguem ≥ AA na tabela de §3.
2. `#7A1418` nunca aparece como texto fino sobre escuro — sempre como placa (§3.5).
3. Os dois acentos nunca disputam o mesmo peso visual no mesmo instante (§4) —
   a insígnia cravada no topo dos botões de facção não muda a distribuição.

**Janela e escala (novificação desta revisão)**

4. A janela abre em **1920×1080 (escala inteira ×2)** com transform matrix
   inteira, `PointClamp` e `PreferMultiSampling = false`; fallback sancionado é
   ×1 (960×540). Nenhuma escala fracionária em tela alguma.
5. Escalas conferidas por classe no draw: **título 5, corpo/labels/botões 3,
   subtítulo/rodapé 2** (§2.4); `MedirTexto` nunca é lido como altura de slot
   (ela só mede a base de 7·N).

**Escrito na tela (novidade desta revisão)**

6. Subtítulo "meia-noite: escolha o lado" e rodapé "tab navega - enter envia -
   esc volta" presentes nas posições de §2.2, nas cores/slot de lá declarados.
7. Todos os slots respeitam folga de acento/cedilha (§2.4: label "usuário" 30,
   "confirmar senha" 27, rótulo "oposição" com 6px de cedilha dentro do botão
   de 40) — nenhum diacrítico cortado por borda ou fora do slot.

**Layout, estados e teclado**

8. CTA "criar conta" fica desligado até haver lado escolhido, com o rótulo
   "escolha seu lado" explicando (§3.6).
9. Todo controle focado mostra o outline `#E8DCC8`; ordem de tab conforme §5.
10. Tudo em pixel inteiro; painel centrado exato em (480,270); troca de modo
    limpa campos, erros e seleção de lado.
11. Fluxo de selo: no modo cadastrar, no máximo 1 selo visível (tudo abaixo
    desce 32px e continua dentro do painel); no modo entrar, os 2 selos cabem.
