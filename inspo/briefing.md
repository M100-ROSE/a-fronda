# Briefing visual — a-fronda

Fonte da verdade para **sensação**. Cores oficiais ficam em [`paleta.md`](paleta.md).
Fichas dos personagens ficam em `game/arts/personas/<slug>/<slug>.json`. Este arquivo
diz o que a arte precisa **fazer o jogador sentir**, não a biografia.

Toda arte (sprite, tileset, interior, UI de mapa) deve ser lida contra estas três
perguntas:

1. Que lado político isto pertence (`oposicao` ou `reino`)?
2. Que sentimento fica depois de dois segundos de tela?
3. Isto ainda parece pixel art no idioma do Stardew Valley (técnica), sem copiar o
   aconchego seguro da fazenda?

---

## Estilo de pixel art — idioma Stardew Valley

O jogo usa o **mesmo vocabulário técnico** do Stardew Valley, não o mesmo humor
pastoral. Stardew é a gramática; a-fronda é o texto.

### O que copiar da técnica

- **Vista 3/4** (de cima e um pouco de lado): chão legível, teto visível, personagem
  sempre inteiro.
- **Silhueta primeiro.** Em 16×32 / 32×48 o jogador reconhece o personagem sem ler
  o rosto. Chapéu, boina, fuzil, bolsa, cadeira de vime, charuto, pérolas, roca —
  um único objeto-âncora por sprite.
- **Contorno de 1 px** escuro (`carvão` ou `grafite` da paleta), nunca neon.
- **Poucas cores por sprite** (rampa da vila + 1 acento). Sem gradiente suave; a
  forma vem de 2–3 degraus de sombra.
- **Idle vivo:** respiração, peso mudando de pé, tecido, fumaça de charuto, papel
  que treme. Stardew ensina que o mundo respira mesmo parado.
- **Tiles com personalidade:** cada piso, cadeira e janela conta classe e lado
  político. Nada de tile “genérico de RPG”.
- **Luz de hora do dia:** manhã quente na vila revolucionária; céu nublado e
  azul-pedra na vila do reino; no castelo, luz que **não esquenta**.
- **Legibilidade de UI no cenário:** placas, bandeiras e portas precisam ler à
  distância, como o festival e as lojas do Stardew.

### O que não copiar do Stardew

- Aconchego automático (lareira = segurança, cama = save feliz, NPC sorridente).
- Saturação de colheita eterna, flores demais, céu sempre limpo.
- Proporção “fofa” que desmente gravidade política (cabeça enorme, olhos brilho
  kawaii, bounce de pular).
- Ouro que parece tesouro de aventura. Aqui ouro é **autoridade** ou **cela**.

Proporção alvo dos personagens: **cerca de 1:5 a 1:6 cabeças** (adulto legível,
não chibi). Expressão no centro da silhueta e na pose, não em pixels de olho.

---

## Vilas

As duas vilas são o chão habitável do mundo. Ambas usam a técnica Stardew
(tiles, interiores, NPCs andando). Os sentimentos **não** são iguais.

### Vila revolucionária (`oposicao`)

Paleta: vermelho bandeira, tijolo, ocre, oliva, terra, fumaça, papel sujo.

**Deve passar:** calor humano, urgência, improvisação, risco compartilhado,
dignidade pobre, noite que trabalha.

| Sensação | Como a arte entrega |
| --- | --- |
| Acolhimento tenso | Luz de lampião, varal, panfleto, mesa com gente demais — nunca vazio “de loja” |
| Luta ainda aberta | Bandeira um pouco gasta, não museu; tinta fresca na parede |
| Clandestinidade | Beeco, janela alta, porta dos fundos, sombra onde se conversa |
| Terra e corpo | Chão batido, tijolo queimado, mão na massa, uniforme sujo de uso |
| Esperança sem inocência | Uma planta no peitoril, uma criança, um jornal — e ferrugem ao lado |

O jogador pode **descansar o olho** aqui, mas não deve sentir que o perigo
acabou. O perigo está do outro lado da serra, no castelo.

### Vila direita (`reino`)

Paleta: azul-marinho, ouro, marfim, pedra, ferro, jardim aparado.

**Deve passar:** ordem, hierarquia visível, limpeza que custa, orgulho
institucional, frieza educada.

| Sensação | Como a arte entrega |
| --- | --- |
| Ordem | Calçada alinhada, sebe cortada, janelas iguais, relógio |
| Hierarquia | Degrau, brasão, guarda na porta, ouro só em filete (nunca espalhado) |
| Respeito que aperta | Interior claro e correto, cadeiras duras, silêncio visual |
| Tradição | Pedra talhada, mármore, retrato ancestral — vivo, não ruína |
| Distância de classe | Rua larga demais, jardim que se olha e não se pisa |

Esta vila **não** é o castelo. É a cidade que acredita no rei. Pode ser bela.
A beleza convence. O medo mora no castelo, para onde esta vila aponta.

---

## Áreas

Novas áreas só entram com pedido explícito. Hoje existem as duas vilas e o
castelo abaixo.

### Castelo do Rei César

Sede do poder central. **Apoiado pelo lado `reino`.** Não é masmorra de ossos e
não é palácio de conto. É uma prisão que se disfarça de cerimônia.

**Deve passar, o tempo todo:**

1. **Perigo** — institucional, não jump-scare. Alguém sempre pode ver você.
   Guardas como móveis. Alabardas que viram parte da arquitetura. Ouro que corta
   (filete fino como lâmina). Sombras longas ao meio-dia.
2. **Não estar seguro** — nenhum canto é “save”. Cama de gala que não convida a
   deitar. Janela alta demais para o chão. Porta aberta que parece armadilha.
   Corredor que continua igual depois da curva. O jogador não encontra um
   tapete de Stardew que diga “você chegou em casa”.
3. **Prisão psicológica** — você pode andar, mas o espaço pensa por você.
   Etiqueta como grade: tapete vermelho que obriga o caminho, retratos do rei
   que olham, relógio que marca a sua demora, salão belo demais para falar
   baixo. A beleza **prende**. Mármore frio. Eco visual (salas repetidas).
   Jardim interno podado até não restar vida. Luz de vitral que não esquenta,
   só pinta o chão de azul-marinho e ouro.

**Como construir no idioma Stardew:** mesmos tiles de interior (parede, piso,
mobília, NPC), mas:

- Teto mais alto que o da vila (escala que encolhe o personagem).
- Repetição deliberada de portas e colunas (labirinto cortês).
- Paleta só de **ambiente vila direita** + ouro autoridade; vermelho entra
  apenas como tapete/cerimônia, nunca como bandeira de luta.
- NPCs do reino aqui ficam mais rígidos: idle mínimo, mãos à frente, olhar
  para o corredor — não para o jogador.
- Fora: muralha, fosso, céu sempre nublado (`azul pedra`). Dentro: silêncio de
  salão, lareira que não aconchega, trono visível cedo demais (você já está
  sendo recebido, logo está sendo julgado).

O Rei César, até haver ficha, existe como **pressão espacial**: brasão, trono
vazio ou ocupado, voz da arquitetura. Não inventar biografia. Personagens
`reino` neste mapa servem o castelo; personagens `oposicao` neste mapa estão
em risco, mesmo convidados.

---

## Personagens — oposição (`oposicao`)

Paleta de vila revolucionária e/ou rampa P&B de retrato. Objeto-âncora único.

### Carlos Marighella (`carlos-marighella`)

- **Sensação:** obstinação clandestina, cidade que engole, poeta de pasta suja.
- **Arte:** baixo, atarracado, camisa arregaçada, papel e máquina. Luz de
  lampião, não de palanque. Não heroizar o fuzil; heroizar o trabalho noturno.

### Karl Marx (`karl-marx`)

- **Sensação:** fúria erudita, sala de leitura, pobreza de quem não para de
  escrever.
- **Arte:** barba ainda escura (29 anos), jornais, charuto, ombros de quem
  carrega mesa. Peso intelectual, não pose de estátua.

### Friedrich Engels (`friedrich-engels`)

- **Sensação:** lealdade prática, elegância de comerciante que viu a fábrica.
- **Arte:** porte militar jovem, relatórios, bengala. O contraste rico/pobre
  mora no objeto (relatório de operário na mão limpa).

### Che Guevara (`che-guevara`)

- **Sensação:** asma e marcha, austero, internacional, rosto já cansado.
- **Arte:** boina e estrela como âncora; verde-oliva terroso; olhos fundos.
  Mito reconhecível sem pôster pop.

### Fidel Castro (`fidel-castro`)

- **Sensação:** volume, oratória, controle, austeridade de farda.
- **Arte:** silhueta alta e larga, quepe, charuto. Presença que ocupa o tile.

### Leonel Brizola (`leonel-brizola`)

- **Sensação:** tribuna, impulso, povo na calçada, coragem à mostra.
- **Arte:** terno de palanque ou camisa da Legalidade com arma — escolher uma
  âncora por sprite. Gesto amplo, sobrancelha.

### Rosa Luxemburgo (`rosa-luxemburgo`)

- **Sensação:** fogo miúdo, jornal na mão, recusa de calar, claudicação como
  verdade do corpo.
- **Arte:** baixa, pasta, *Die Rote Fahne*, vinho e carvão. Energia maior que
  o tamanho.

### Mao Tsé-Tung (`mao-tse-tung`)

- **Sensação:** paciência de mapa, campo, túnica que vira bandeira humana.
- **Arte:** Zhongshan, pinta, estrela. Massa calma, perigosa na quietude.

### Malcolm X (`malcolm-x`)

- **Sensação:** fio tenso, ironia, orgulho, recusa de ajoelhar.
- **Arte:** óculos pretos, terno, cobre e carvão. Postura de auditório mesmo
  parado.

### Rosa Parks (`rosa-parks`)

- **Sensação:** dignidade quieta, recusa cotidiana, força sem grito.
- **Arte:** casaco, luvas, bolsa no colo, olhar firme. O banco / o ônibus
  como objeto-mundo, não como piada.

### Carlos Lamarca (`carlos-lamarca`)

- **Sensação:** disciplina que desertou, frio de planejamento, isolamento.
- **Arte:** oliva e cáqui, FAL ou camisa aberta — magro, alerta. Sem glamour
  de ação.

### Iara Iavelberg (`iara-iavelberg`)

- **Sensação:** inteligência afetiva, elegância sob perseguição, ruptura de
  classe.
- **Arte:** cabelo longo, gola alta, ferrugem e creme. Beleza que não anula o
  risco.

### Fred Hampton (`fred-hampton`)

- **Sensação:** juventude que organiza, calor de coalizão, punho e aula.
- **Arte:** boina, couro, punho. Sorriso raro; seriedade que ainda é jovem.

### Huey P. Newton (`huey-p-newton`)

- **Sensação:** trono de vime, filosofia armada, gravidade de retrato.
- **Arte:** a cadeira, o fuzil e a lança — uma pose. Preto absoluto, silêncio.

### Martin Luther King Jr. (`martin-luther-king-jr`)

- **Sensação:** autoridade moral, púlpito, contenção, peso da liderança.
- **Arte:** terno, microfone, gesto contido. Luz de igreja/comício, não de
  super-herói.

### Tupac Shakur (`tupac-shakur`)

- **Sensação:** intensidade periférica, contradição, vocação e ferida.
- **Arte:** bandana, corrente, tatuagem como leitura à distância. Não virar
  sticker; manter o corpo tenso.

### Vladimir Lênin (`vladimir-lenin`)

- **Sensação:** método, clandestinidade, calva e barba como marca, urgência
  histórica.
- **Arte:** terno ou boné de operário. Vermelho só na faixa. Olhos pequenos e
  acordados.

### Maximilien de Robespierre (`maximilien-de-robespierre`)

- **Sensação:** virtude fria, incorruptível, Terror como sala limpa.
- **Arte:** casaca azul-clara, palidez, óculos, lenço. Beleza setecentista que
  não aquece — primo distante do castelo, mas do lado da ruptura.

### Nelson Mandela (`nelson-mandela`)

- **Sensação:** paciência de 27 anos, reconciliação que ainda é firmeza,
  camisa que desarma o palácio.
- **Arte:** camisa Madiba (terra e azul) ou terno. Porte ereto, idade como
  honra, não como fraqueza.

### Mahatma Gandhi (`mahatma-gandhi`)

- **Sensação:** austeridade do corpo, recusa mansa, autoridade de quem jejuou.
- **Arte:** branco cru, roca, bastão, óculos redondos. O vazio do tecido é o
  poder.

### Thomas Sankara (`thomas-sankara`)

- **Sensação:** energia honesta, anti-luxo, bicicleta contra o palácio.
- **Arte:** camuflagem e boina vermelha, ou camiseta e violão. Vermelho só na
  boina.

### Kwame Nkrumah (`kwame-nkrumah`)

- **Sensação:** independência imediata, kente como nação, carisma de praça.
- **Arte:** ouro-verde-vermelho do tecido, bastão. Orgulho continental, não
  fantasia.

### Patrice Lumumba (`patrice-lumumba`)

- **Sensação:** dignidade ferida, discurso do dia da independência, tensão de
  quem já está cercado.
- **Arte:** terno estreito, postura ereta, poucos adereços. Magreza como
  urgência.

---

## Personagens — reino (`reino`)

Paleta de vila direita / institucional. No **castelo**, estes sprites ganham
rigidez extra (idle menor, luz mais fria). Fora do castelo podem ter jardim,
tribuna, gabinete.

### Edmund Burke (`edmund-burke`)

- **Sensação:** oratória inflamada a serviço da herança, horror à ruptura.
- **Arte:** casaca ampla, papéis, gesto de Câmara. Calor irlandês em paleta
  fria de instituição.

### Michael Oakeshott (`michael-oakeshott`)

- **Sensação:** ceticismo elegante, livro, recusa do entusiasmo.
- **Arte:** tweed, sentado, expressão contida. Quietude acadêmica.

### Roger Scruton (`roger-scruton`)

- **Sensação:** campo inglês, beleza herdada, polêmica afetuosa.
- **Arte:** tweed, cachimbo, cão ou piano. Oikophilia visível (casa, cerca,
  árvore), nunca ouro de trono.

### Alexis de Tocqueville (`alexis-de-tocqueville`)

- **Sensação:** melancolia aristocrática, caderno de viagem, medo da igualdade
  que sufoca a liberdade.
- **Arte:** esguio, sobrecasaca, caderno. Distância educada.

### Winston Churchill (`winston-churchill`)

- **Sensação:** teatro de guerra, teimosia, charuto, V da vitória que também é
  peso.
- **Arte:** tronco atarracado, homburg ou macacão de sirene. Expansivo — no
  castelo, o expansivo vira pressão.

### Margaret Thatcher (`margaret-thatcher`)

- **Sensação:** corte, bolsa como cetro, ferro sob pérola.
- **Arte:** tailleur azul-real, penteado rígido, pérolas. A bolsa é a âncora.
  Nenhuma suavidade de “dama”.

### Ronald Reagan (`ronald-reagan`)

- **Sensação:** palco solar, otimismo de rancho, autoridade que sorri.
- **Arte:** marinho e gravata vermelha, ou jeans e cavalo. O sorriso é arma
  institucional, não aconchego Stardew.

### Konrad Adenauer (`konrad-adenauer`)

- **Sensação:** jardim como refúgio de um duro, reconstrução, idade que governa.
- **Arte:** rosto anguloso, terno, roseira. Verde só no jardim, nunca oliva de
  guerrilha.

### Robert Schuman (`robert-schuman`)

- **Sensação:** silêncio de gabinete, pasta, Europa feita de papel.
- **Arte:** franzino, terno apagado, pasta de couro. Poder que não se exibe.

### John Howard (`john-howard`)

- **Sensação:** subúrbio metódico, obstinação sem carisma, paletó abotoado.
- **Arte:** óculos retangulares, gravata, caminhada de agasalho só se for o
  sprite de rua — o de gabinete é rígido.

### Brian Mulroney (`brian-mulroney`)

- **Sensação:** acordo de bastidor, voz, sorriso de palco.
- **Arte:** ombros largos, lenço, gravata. Calor de negociador dentro do frio
  do reino.

### Barry Goldwater (`barry-goldwater`)

- **Sensação:** deserto, franqueza, Oeste, rádio e sol.
- **Arte:** prata e areia, gravata-bolo ou chapéu. O reino aqui é horizonte,
  não mármore — no castelo, o deserto some e ele fica deslocado de propósito.

### Friedrich Hayek (`friedrich-hayek`)

- **Sensação:** aviso do caminho da servidão, prata, isolamento erudito.
- **Arte:** alto, magro, óculos finos, terno três peças. Frieza de biblioteca.

### Milton Friedman (`milton-friedman`)

- **Sensação:** didática combativa, caneta, gosto de explicar no ar.
- **Arte:** baixo, compacto, óculos grossos, vivaz. Energia de debate, não de
  tropa.

### Ludwig von Mises (`ludwig-von-mises`)

- **Sensação:** rigor vienense, recusa de conceder, seminário.
- **Arte:** bigode, colarinho alto, livros. Austero até o tile.

### Thomas Sowell (`thomas-sowell`)

- **Sensação:** ironia contida, disciplina, livro como escudo.
- **Arte:** ereto, óculos, terno. Pouco gesto; o peso está no olhar baixo para
  a página e de volta para o jogador.

### Walter Williams (`walter-williams`)

- **Sensação:** sala de aula, giz, fala direta.
- **Arte:** tons terrosos de professor (bege, laranja queimado) sobre grafite
  institucional. Microfone ou giz como âncora.

### Roberto Campos (`roberto-campos`)

- **Sensação:** polêmica aforística, lanterna na popa, gabinete brasileiro.
- **Arte:** calvície, bigode, óculos largos. Lanterna só como símbolo discreto,
  não cartoon.

### José Bonifácio de Andrada e Silva (`jose-bonifacio-de-andrada-e-silva`)

- **Sensação:** patriarca da independência, espada e cruz, gênio difícil.
- **Arte:** cabelos brancos, casaca, condecoração. Império como peso no peito.

### Visconde de Uruguai (`visconde-de-uruguai`)

- **Sensação:** gabinete Saquarema, frieza jurídica, centralização.
- **Arte:** rosto comprido, casaca, quase nenhum ouro. O homem do decreto.

### Carlos Lacerda (`carlos-lacerda`)

- **Sensação:** microfone de combate, brilho de imprensa, perigo da palavra.
- **Arte:** terno claro anos 50, óculos, microfone de pé. Boca a meio do
  ataque.

### Afonso Arinos de Melo Franco (`afonso-arinos-de-melo-franco`)

- **Sensação:** conciliação jurídica, colarinho, ouro discreto de ordem.
- **Arte:** terno fechado, óculos, papada de parlamentar. Persuasão, não
  tropa.

### Visconde de Mauá (`visconde-de-maua`)

- **Sensação:** ferrovia, risco, sépia de progresso material.
- **Arte:** costeletas, sobrecasaca, fundo de trilho ou relógio. Energia de
  empreendedor dentro do reino — o castelo o olha como útil, não como igual.

---

## Regras para gerar arte

1. Ler este briefing **e** a ficha JSON **e** `paleta.md` antes de qualquer
   sprite ou tile.
2. Um sentimento dominante por asset. Se o sprite tentar ser “herói + vítima +
   ícone pop”, cortar.
3. No castelo do Rei César, mesmo um NPC simpático da vila do reino deve
   parecer **menos seguro**. O espaço manda no humor.
4. Personagem `oposicao` no castelo: corpo fechado, olho no corredor, paleta
   um degrau mais escura.
5. Não inventar área nova, facção nova ou biografia do Rei César neste
   arquivo. Pedido do usuário primeiro.
6. SpriteCook / qualquer gerador: colar no prompt a **sensação** em uma frase
   + o objeto-âncora + “pixel art 3/4, estilo técnico Stardew Valley, paleta
   limitada, sem chibi, sem neon”.
