# Contexto do Projeto — a-fronda

Este arquivo contém **apenas** as regras de negócio/convenções do projeto e a
explicação dos arquivos e pastas. O registro cronológico de tudo que foi feito fica
em [`historico.md`](historico.md).

Projeto: um jogo com arte em **pixel art**. Engine, plataforma e mecânicas ainda não
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
  `game/arts/personas/<nome>/`, com duas subpastas:
  - `<nome>-context/` — contexto/referência do personagem (descrição, referências,
    prompts, decisões);
  - `<nome>-sprite/` — sprites/spritesheets e animações geradas do personagem.

  Regra do `<nome>`: **minúsculas**, **sem acentos** e **sem espaços**; se tiver mais
  de uma palavra, usar hífen. Exemplo: `game/arts/personas/fox/fox-context/` e
  `game/arts/personas/fox/fox-sprite/`. Vale para todo personagem criado a partir de
  agora, sem precisar ser reafirmada a cada pedido.
- **Arte:** gerada com o **SpriteCook** (MCP já instalado no ambiente) — conta free,
  **40 créditos/mês**, assets com **uso comercial permitido**. Assets gerados vão para
  dentro de `game/arts/`: mapas em `game/arts/mapa/`, personagens em
  `game/arts/personas/<nome>/<nome>-sprite/`.
- **Time de agentes:**
  - **Bebell** — agente líder; recebe o pedido do usuário e delega.
  - **Asta** — back-end.
  - **Anna** — front-end.
  - **Day** — design.
  - **Dakota** — fechamento: registra no `historico.md`, commita e faz o push.

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
| `game/` | Raiz do jogo. |
| `game/code/` | Código do jogo. |
| `game/arts/` | Arte do jogo. |
| `game/arts/mapa/` | Tilesets e mapas. |
| `game/arts/personas/` | Um diretório por personagem, no formato da convenção acima. |
| `inspo/` | Referências visuais e materiais de inspiração. |
