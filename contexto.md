# Contexto do Projeto — a-fronda

Este arquivo contém **apenas** as regras de negócio/convenções do projeto e a
explicação dos arquivos e pastas. O registro cronológico de tudo que foi feito fica
em [`historico.md`](historico.md).

Projeto: um jogo com arte em **pixel art** no idioma técnico do **Stardew
Valley** (vista 3/4, silhueta legível, contorno de 1 px, tiles com
personalidade, idle vivo) — sem copiar o aconchego seguro da fazenda.
Sensações oficiais da arte: [`inspo/briefing.md`](inspo/briefing.md). Cores:
[`inspo/paleta.md`](inspo/paleta.md). Engine, plataforma e mecânicas ainda não
foram definidas.

---

## Regras de negócio e do projeto

- **Idioma:** trabalhar e responder sempre em **português do Brasil**.
- **Registro obrigatório:** toda alteração no projeto (arquivos, pastas,
  configurações, agentes, assets) deve ser registrada em `historico.md` como uma nova
  entrada e, **na mesma tarefa**, commitada e enviada para `origin/main`
  (`git push origin main`). Nenhuma tarefa é considerada concluída antes disso.
- **Versionamento:** pastas vazias recebem um arquivo `.gitkeep` (vazio) para que o Git
  versione o diretório.
- **Convenção de pastas de personagens:** todo personagem fica em
  `game/arts/personas/<nome>/`, com a seguinte estrutura:
  ```
  game/arts/personas/<nome>/
    <nome>.json        → ficha do personagem (dados + descrições)
    <nome>-context/    → contexto/referência do personagem
    <nome>-sprite/     → sprites/spritesheets e animações
  ```
  - `<nome>.json` — ficha do personagem (dados + descrições), na raiz da pasta dele;
  - `<nome>-context/` — contexto/referência do personagem (descrição, referências,
    prompts, decisões);
  - `<nome>-sprite/` — sprites/spritesheets e animações geradas do personagem.

  Regra do `<nome>`: **minúsculas**, **sem acentos** e **sem espaços**; se tiver mais
  de uma palavra, usar hífen. Exemplo: `game/arts/personas/fox/fox.json`,
  `game/arts/personas/fox/fox-context/` e `game/arts/personas/fox/fox-sprite/`. Vale
  para todo personagem criado a partir de agora, sem precisar ser reafirmada a cada
  pedido.
- **Ficha JSON do personagem (obrigatória):** junto com cada personagem — na **mesma**
  tarefa, sempre — criar `game/arts/personas/<nome>/<nome>.json` com **todos** os 7
  campos abaixo, sempre presentes:
  ```json
  {
    "nome": "string",
    "idade": 0,
    "data_nascimento": "AAAA-MM-DD",
    "ideal": "reino",
    "descricao_visual": "string",
    "descricao_ideais": "string",
    "descricao_personalidade": "string"
  }
  ```
  - `nome` — nome do personagem como aparece no jogo.
  - `idade` — número inteiro em anos: a **idade do personagem no momento mais
    marcante da trajetória dele** (o marco é escolhido caso a caso e registrado no
    `historico.md`). **Não** é a idade de morte e **não** deve ser derivada
    automaticamente de `data_nascimento`. Exemplos: Che aos 39 na Bolívia em 1967;
    Lênin aos 47 em 1917; Marx aos 29 no Manifesto de 1848.
  - `data_nascimento` — data no formato `AAAA-MM-DD`; usar `null` se não houver data
    definida (universo sem calendário real).
  - `ideal` — apenas `"reino"` ou `"oposicao"` (minúsculo, sem acento; `oposicao`
    representa a oposição). Sem outros valores.
  - `descricao_visual` — aparência: silhueta, roupas, cores, marcas, paleta.
  - `descricao_ideais` — o que o personagem defende/persegue e por quê.
  - `descricao_personalidade` — temperamento, comportamento, medos, motivações.
  - Campos não definidos no momento da criação ficam como `null` (nunca inventar
    conteúdo); o arquivo é criado sempre com **todos** os campos presentes.
  - O JSON é criado **junto** com o personagem, na mesma tarefa; não é opcional.
- **Arte:** gerada com o **SpriteCook** (MCP já instalado no ambiente) — conta free,
  **40 créditos/mês**, assets com **uso comercial permitido**. Assets gerados vão para
  dentro de `game/arts/`: mapas em `game/arts/mapa/`, personagens em
  `game/arts/personas/<nome>/<nome>-sprite/`.
- **Time de agentes:**
  - **Bebell** — agente líder; recebe o pedido do usuário e delega.
  - **Asta** — back-end.
  - **Anna** — front-end.
  - **Day** — design.
  - **Monica** — pesquisa de referências visuais, tendências, assets e documentação.
  - **Dakota** — fechamento: registra no `historico.md`, commita e faz o push.
  - **Izak** — relatório final e opinião de cada agente sobre o que foi entregue.
- **Identidade visual / paletas:** a fonte da verdade das cores é
  `inspo/paleta.md`. **`oposicao`** = vermelho, tijolo, ocre, oliva, carvão
  (calor, terra, bandeira, clandestinidade). **`reino`** = azul-marinho, ouro,
  marfim, grafite (ordem, instituição, hierarquia). Personagens históricos podem
  usar a rampa preto-e-branco. Login/cadastro é a única paleta que mistura os
  dois acentos, porque é a porta de escolha de lado. Não usar neon CSS
  (`#FF0000` / `#0000FF` / `#00FF00`) nem nomes que mintam o hex. Cor nova entra
  em `inspo/paleta.md` **antes** de ir para asset ou CSS.
- **Briefing de sensação:** a fonte da verdade do que cada arte deve **fazer
  sentir** é `inspo/briefing.md` (personagens, vilas, áreas, estilo Stardew
  Valley). Sprite e tileset leem briefing + paleta + ficha JSON.
- **Personagens já cadastrados:** 46 fichas em `game/arts/personas/` — **23** de
  `ideal` `oposicao` e **23** de `ideal` `reino` (lista completa no `historico.md`).

## Divisão política do mundo

O mundo do jogo é dividido em dois lados, definidos pelo usuário:

- **`reino`** — a direita política: quem quer manter o que já existe, com o poder
  centralizado e a hierarquia preservada.
- **`oposicao`** — as forças revolucionárias: quem quer descentralizar o poder e
  construir uma sociedade mais igualitária.

Regras:

- O campo `ideal` da ficha aceita **somente** esses dois valores: `"reino"` ou
  `"oposicao"` (minúsculo, sem acento). Nenhum outro valor é válido.
- O projeto tem hoje **46 personagens**: **23** com `ideal` `"oposicao"` e **23** com
  `ideal` `"reino"`.
- **Precisão importante sobre o lado `reino`:** os nomes desse lado foram escolhidos
  pelo usuário e reúnem conservadores, democratas-cristãos e também **liberais
  clássicos** (Hayek, Mises, Friedman, Sowell, Walter Williams e Roberto Campos), que
  defendem **Estado limitado e descentralização econômica**. Portanto, `ideal` é a
  **classificação política do jogo**, **não** uma afirmação de que todos os personagens
  do lado `reino` defendem poder centralizado: a definição acima descreve a tendência
  geral do lado, não cada ficha individualmente.
- Novos lados, facções ou subdivisões políticas só entram no projeto com **pedido
  explícito do usuário**. Nada deve ser inventado nesta área.

## Lugares do mundo (arte)

Áreas novas só entram com pedido explícito. Hoje:

- **Vila revolucionária** (`oposicao`) — calor, urgência, clandestinidade,
  dignidade pobre. Paleta quente. O jogador pode descansar o olho, não o risco.
- **Vila direita** (`reino`) — ordem, hierarquia, frieza educada, beleza que
  convence. Paleta fria. Cidade que acredita no rei; o medo não mora aqui.
- **Castelo do Rei César** — sede do poder central, **apoiado pelo `reino`**.
  Deve passar **perigo** (vigilância institucional), **insegurança** (nenhum
  canto é abrigo) e **prisão psicológica** (beleza, etiqueta e corredores que
  prendem mesmo com a porta aberta). Paleta de ambiente do reino; ouro como
  grade, não como tesouro. A biografia do Rei César **não** está definida —
  ele existe como pressão do espaço (brasão, trono, retratos).

## Regras de negócio do jogo

**Ainda não definidas.** Até agora nenhuma mecânica, regra de gameplay, progressão ou
balanceamento foi especificada. Nada deve ser inventado nesta seção sem pedido
explícito do usuário.

---

## Explicação dos arquivos e pastas

| Caminho | O que é / quem usa |
| --- | --- |
| `README.md` | Descrição curta do projeto. |
| `contexto.md` | Este arquivo: regras de negócio/convenções + mapa de arquivos e pastas. |
| `historico.md` | Registro cronológico de todas as tarefas do projeto (entrada por tarefa). |
| `.opencode/opencode.json` | Configuração do projeto (`default_agent: bebell`). |
| `.opencode/agents/bebell.md` | Agente líder: recebe o pedido e delega aos subagentes. |
| `.opencode/agents/asta.md` | Subagente de back-end. |
| `.opencode/agents/anna.md` | Subagente de front-end. |
| `.opencode/agents/day.md` | Subagente de design. |
| `.opencode/agents/dakota.md` | Subagente de fechamento (histórico, commit, push). |
| `.opencode/agents/monica.md` | Subagente de pesquisa de referências (parceira da Day). |
| `.opencode/agents/izak.md` | Subagente de relatório final (opinião de cada agente). |
| `game/` | Raiz do jogo. |
| `game/code/` | Código do jogo. |
| `game/arts/` | Arte do jogo. |
| `game/arts/mapa/` | Tilesets e mapas. |
| `game/arts/personas/` | Um diretório por personagem, no formato da convenção acima (com `<nome>.json`, `<nome>-context/` e `<nome>-sprite/`). |
| `game/arts/personas/<nome>/<nome>.json` | Ficha do personagem: nome, idade, nascimento, ideal e descrições (visual, ideais, personalidade). |
| `inspo/` | Referências visuais e materiais de inspiração. |
| `inspo/paleta.md` | Paletas oficiais do jogo (vilas, ambientes, personagens P&B, login). |
| `inspo/briefing.md` | Sensações da arte: estilo Stardew Valley, vilas, castelo do Rei César, cada personagem. |
