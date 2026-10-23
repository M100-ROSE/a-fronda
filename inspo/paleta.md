# Paletas de cores — a-fronda

Fonte da verdade para pixel art, UI e mapas. Os hexes **precisam** corresponder
ao nome e ao lado político. Não misturar as duas vilas.

## Mensagem que as cores precisam passar

O mundo tem dois lados (`ideal` na ficha do personagem):

| Lado | Política no jogo | Sensação visual | Família de cor |
| --- | --- | --- | --- |
| **`oposicao`** | ruptura, igualdade, povo, clandestinidade | calor, terra, bandeira, fumaça, uniforme de luta | vermelho, tijolo, ocre, oliva, carvão |
| **`reino`** | ordem, hierarquia, instituição, tradição | frio, pedra, marinha, ouro de autoridade, mármore | azul-marinho, ouro, marfim, grafite |

O jogo **não** usa o par neon CSS `#FF0000` × `#0000FF`. Esses dois puros gritam
“cor padrão de ferramenta”, não “mundo político em pixel art”.

Personagens históricos (retratos, fichas, sprites sóbrios) podem cair na paleta
**preto e branco**. Uniformes e símbolos de facção usam a paleta da vila.

---

## Vila revolucionária (`oposicao`)

Bandeira, povo, tijolo, terra, oliva de guerrilha. Tudo quente. Nada de verde-limão,
menta, cinza duplicado no lugar de vermelho, nem roxo no lugar de vermelho-escuro.

| # | Nome | Hex | Uso |
| --- | --- | --- | --- |
| 1 | Vermelho bandeira | `#C41E3A` | cor-âncora da oposição: faixas, ícones, destaque |
| 2 | Vermelho sangue | `#7A1418` | sombra do pano vermelho, vinho de luta |
| 3 | Tijolo | `#B55239` | paredes, telha, cidade clandestina |
| 4 | Ferrugem | `#8C3A22` | metal velho, portões, desgaste |
| 5 | Ocre | `#C4842A` | terra batida, lamparina, segundo destaque |
| 6 | Oliva guerrilha | `#4A5C32` | uniforme (Che, Fidel, Sankara), vegetação seca |
| 7 | Terra | `#5C3A2A` | chão, madeira, cabelo/sombra quente |
| 8 | Cinza fumaça | `#6B6560` | névoa urbana, concreto sujo |
| 9 | Papel sujo | `#E8DCC8` | panfleto, luz, texto sobre fundo escuro |
| 10 | Carvão | `#1A1210` | contorno, noite, silhueta |

**Não usar nesta vila:** `#00FF00`, `#90EE90`, `#C7FFD0`, `#800080`, `#0000FF`.

---

## Vila direita (`reino`)

Instituição, marinha, ouro, mármore. Tudo frio ou metálico. Nada de verde-limão
como “azul claro”, amarelo puro como “caramelo”, nem o vermelho-tijolo da oposição.

| # | Nome | Hex | Uso |
| --- | --- | --- | --- |
| 1 | Azul marinho | `#0B1D4A` | cor-âncora do reino: poder, hierarquia, noite palaciana |
| 2 | Azul real | `#1E3A8A` | bandeira, uniforme de gala, botões |
| 3 | Azul uniforme | `#2C5282` | farda, teto, tecido institucional |
| 4 | Azul pedra | `#5B7C99` | sombra clara, céu nublado da capital |
| 5 | Ouro autoridade | `#C9A227` | brasão, filete, destaque de status |
| 6 | Bronze | `#8B6914` | sombra do ouro, metal antigo |
| 7 | Marfim | `#F3EDE0` | mármore, documento, luz |
| 8 | Pedra | `#9A958C` | calçamento, coluna, parede nobre |
| 9 | Grafite | `#2F343B` | ferro, terno, contorno suave |
| 10 | Branco cerimonial | `#F7F5F0` | luva, camisa, destaque frio |

**Não usar nesta vila:** `#90EE90`, `#00FF00`, `#FFFF00`, `#B22222`, `#FF0000`.

---

## Ambiente — vila revolucionária

Cenário ao redor da oposição: arruamento de tijolo, terra, fumaça, lampião, mato
seco. Serve a tileset de `game/arts/mapa/`.

| # | Nome | Hex | Uso |
| --- | --- | --- | --- |
| 1 | Terra batida | `#6B4423` | chão, caminho |
| 2 | Tijolo queimado | `#8E3B2A` | parede, telha |
| 3 | Argamassa | `#C4B7A4` | junta, reboco rachado |
| 4 | Madeira | `#4A2E1F` | porta, caibro, caixa |
| 5 | Oliva seco | `#5A6B3A` | mato, lona, uniforme no cenário |
| 6 | Folha sombra | `#2F3B24` | vegetação profunda |
| 7 | Fumaça | `#7A7670` | céu sujo, poeira |
| 8 | Noite | `#1C1412` | céu noturno, vão |
| 9 | Lampião | `#E09A2B` | luz pontual, fogo |
| 10 | Brasa | `#D4452A` | fogo, destaque quente |
| 11 | Bandeira | `#C41E3A` | pano no mastro (igual à âncora da vila) |
| 12 | Linho | `#E6D7C3` | varal, papel, tenda clara |
| 13 | Ferrugem | `#7A3A28` | cano, telhado oxidado |
| 14 | Cinza telha | `#4E4A46` | sombra de telhado, asfalto velho |

---

## Ambiente — vila direita

Cenário do reino: pedra, mármore, jardim aparado, ferro, ouro em filete. Capital
ordenada, não bairro popular.

| # | Nome | Hex | Uso |
| --- | --- | --- | --- |
| 1 | Mármore | `#E8E2D4` | piso, fachada clara |
| 2 | Pedra talhada | `#8A8680` | muro, calçada |
| 3 | Pedra sombra | `#4A4742` | nicho, coluna |
| 4 | Azul bandeira | `#0B1D4A` | pano institucional (igual à âncora da vila) |
| 5 | Azul telhado | `#243868` | telha de ardósia, céu de capital |
| 6 | Ouro filete | `#C9A227` | portão, moldura, brasão no mapa |
| 7 | Ferro | `#2A2E33` | grade, lampião apagado |
| 8 | Jardim | `#2F4A32` | sebes aparadas, ordem vegetal |
| 9 | Folha clara | `#5E7A4A` | jardim ao sol |
| 10 | Céu institucional | `#6A8499` | horizonte, vidro |
| 11 | Areia de pátio | `#D4CBB8` | pátio interno |
| 12 | Sombra úmida | `#3A4A55` | água, capela, corredor |
| 13 | Tijolo nobre | `#7A4A3A` | só em edifício antigo — nunca como vermelho de luta |
| 14 | Marfim janela | `#F4EFE6` | luz interna, cortina |

---

## Personagens — preto e branco

Retrato histórico, ficha, sprite sóbrio. Uma rampa só, **sem nomes repetidos**.
O cinza frio (`#AEBEC7`) entra como granulação de foto, não como terceira “cinza”.

| # | Nome | Hex | Uso |
| --- | --- | --- | --- |
| 1 | Preto | `#0E0E0E` | contorno, pupilas, terno profundo |
| 2 | Carvão | `#252525` | sombra forte |
| 3 | Grafite | `#4A4A4A` | paletó, cabelo escuro |
| 4 | Cinza médio | `#6E6E6E` | volume, barba |
| 5 | Cinza papel | `#9B9B9B` | fundo de retrato |
| 6 | Cinza frio | `#AEBEC7` | granulação, foto de imprensa |
| 7 | Gelo | `#E0E0E0` | camisa, luz |
| 8 | Branco | `#F5F5F5` | highlight, papel |

Sepia de época (Mauá, Bonifácio) pode tingir essa rampa com o **ocre** e a **terra**
da vila revolucionária *só no fundo*, nunca como cor de facção do personagem.

---

## Login / cadastro

Tela de entrada do jogo: palco escuro em que os dois lados aparecem como escolha,
não como arco-íris. Rosa neon, amarelo puro e “gold” roxo **não** pertencem a este
mundo.

| # | Nome | Hex | Uso |
| --- | --- | --- | --- |
| 1 | Fundo | `#120E10` | canvas, noite |
| 2 | Painel | `#1C1614` | card, formulário |
| 3 | Borda | `#3A3230` | input, divisor |
| 4 | Texto | `#F3EDE0` | labels, títulos (marfim do reino) |
| 5 | Texto mudo | `#9A958C` | placeholder, ajuda (pedra) |
| 6 | Acento oposição | `#C41E3A` | CTA / lado oposição |
| 7 | Acento reino | `#1E3A8A` | CTA / lado reino |
| 8 | Ouro | `#C9A227` | marca, detalhe do logo |
| 9 | Erro | `#7A1418` | validação (vermelho sangue, não neon) |
| 10 | Foco | `#E8DCC8` | outline de acessibilidade |

Contraste alvo: texto `#F3EDE0` sobre `#120E10` (acima de AA). Botões de facção
nunca usam o mesmo vermelho e o mesmo azul na mesma hierarquia visual — um é
primário da tela, o outro é a escolha oposta.

---

## Regras de uso

1. **Lado político manda.** Personagem `oposicao` puxa a vila revolucionária;
   personagem `reino` puxa a vila direita. Retrato P&B é exceção consciente.
2. **Nome = hex.** Se o nome diz vermelho, o swatch é vermelho. Não relabelar.
3. **Pixel art:** no máximo a rampa da vila + 1 acento de ambiente por sprite.
4. **Mapa:** tileset da vila usa só a paleta de ambiente correspondente.
5. **UI:** login/cadastro é a única paleta que mistura os dois acentos, porque é
   a porta de escolha de lado.
6. Qualquer cor nova entra neste arquivo **antes** de ir para asset ou CSS.

---

## O que estava errado (revisão)

O rascunho anterior não passava a mensagem do jogo:

- Nomes mentiam o hex (“verde” era azul `#2094f3`, “azul claro” era verde
  `#90EE90`, “vermelho escuro” era roxo `#800080`).
- As duas vilas compartilhavam os mesmos swatches (`#808080`, `#90EE90`,
  `#B22222`, `#DAA520`, `#00FF00`), então os lados políticos não se distinguiam.
- Ambiente da oposição era um arco-íris Material Design (ciano, laranja, rosa).
- Login usava magenta `#FF00E9` e “gold” roxo, fora do universo.
- Personagens P&B repetiam o mesmo nome três vezes (`Cinza`, `Branco`).
