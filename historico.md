# Histórico do Projeto — a-fronda

Registro cronológico de tudo que foi feito no projeto (decisões, configurações,
tarefas, commits), em ordem cronológica. As regras do projeto e o mapa de arquivos
ficam em `contexto.md`.

---

### 2026-10-22 — Instalação do SpriteCook (MCP + skills)

Solicitação: adicionar o SpriteCook ao ambiente.

O que foi feito:

1. Baixado e revisado o instalador oficial para Windows
   (`https://spritecook.ai/install-opencode.ps1`) antes de executá-lo.
2. Executado o instalador oficial, que realizou:
   - Criação do arquivo de configuração global `C:\Users\kaike\.config\opencode\opencode.json`
     com o servidor MCP remoto:
     ```json
     {
       "$schema": "https://opencode.ai/config.json",
       "mcp": {
         "spritecook": {
           "type": "remote",
           "url": "https://mcp.spritecook.ai/mcp/openai",
           "enabled": true
         }
       }
     }
     ```
   - Cópia de 10 skills de workflow do SpriteCook para
     `C:\Users\kaike\.config\opencode\skills`:
     - `spritecook-workflow-essentials`
     - `spritecook-generate-sprites`
     - `spritecook-animate-assets`
     - `spritecook-generate-tilesets`
     - `spritecook-use-dual-grid-tilesets`
     - `spritecook-build-ui-kits`
     - `spritecook-polish-sprites`
     - `spritecook-upload-assets`
     - `spritecook-use-presets`
     - `spritecook-use-assets-in-godot`

3. Após reiniciar o OpenCode, as ferramentas MCP do SpriteCook foram carregadas
   (44 ferramentas, entre elas `generate_game_art`, `animate_game_art`,
   `generate_tileset`, `remove_background`, `list_recent_assets`,
   `get_credit_balance`).

4. Validada a conexão chamando `get_credit_balance` — o OAuth já estava ativo
   (nenhum login manual extra foi necessário):
   - Conta: `unaware_retired_timid` (`k***@gmail.com`)
   - Plano: `free`, assinatura ativa
   - Créditos: **40 de 40**
   - Jobs simultâneos: 1
   - Versão mais recente das skills: `2026.10.20`

Observações importantes:

- **Não é necessário API key.** A autenticação é via OAuth no navegador,
  na primeira utilização de uma ferramenta protegida, vinculada à conta SpriteCook.
- A conta gratuita inclui 40 créditos por mês.
- Os assets gerados podem ser usados comercialmente.

Arquivos alterados:

- `C:\Users\kaike\.config\opencode\opencode.json` — criado com o servidor MCP `spritecook`
- `C:\Users\kaike\.config\opencode\skills\*` — 10 skills do SpriteCook instaladas

Pendências:

- [ ] Definir o primeiro asset a ser gerado para o projeto a-fronda.
- [ ] Escolher o diretório onde os assets gerados serão salvos.

---

### 2026-10-22 — Criação deste arquivo de contexto

Solicitação: responder em português do Brasil e registrar todas as ações em
`contexto.md` (Markdown).

O que foi feito:

1. Criado este arquivo `contexto.md` na raiz do projeto.
2. Registrado o histórico completo da instalação do SpriteCook.
3. Definida a estrutura de registro (seções, histórico, modelo de nova entrada).

Arquivos alterados:

- `contexto.md` — criado

Pendências:

- [ ] Manter este arquivo atualizado a cada nova tarefa.

### 2026-10-22 — Publicação no GitHub a cada atualização

Solicitação: sempre que houver uma atualização, enviá-la para o GitHub.

O que foi feito:

1. Verificado o repositório remoto já configurado:
   - `origin` → `https://github.com/M100-ROSE/a-fronda.git`
   - Branch local/remota: `main`
   - Autenticação: Gerenciador de Credenciais do Windows (`credential.helper = manager`),
     acesso confirmado com `git ls-remote origin`.
2. Adicionada a regra de Git em **Preferências de trabalho**.
3. Criado o primeiro commit deste projeto com o `contexto.md` e enviado para `origin/main`
   — push confirmado: `8e1f501..e3388af  main -> main`.

Arquivos alterados:

- `contexto.md` — nova regra e este registro

Pendências:

- [ ] Definir identidade do Git (`user.name` / `user.email`) — hoje estão como
      `Your Name <you@example.com>`, então os commits não ficam vinculados à conta do GitHub.

---

### 2026-10-22 — Criação do time de agentes (Bebell, Asta, Anna, Day e Dakota)

Solicitação: criar um agente chamado **Bebell** que recebe os pedidos do usuário e os
distribui para os subagentes — **Asta** (back-end, calma e sem estresse, paciente com a
Anna), **Anna** (front-end), **Day** (design, o mais fiel possível à referência) e
**Dakota** (organiza o que foi feito, sobe para o Git e atualiza o contexto).

O que foi feito:

1. Criados 5 agentes em Markdown (formato V2) em `.opencode/agents/`:
   - `bebell.md` — `primary`, orquestrador. Só lê e inspeciona; não edita nem roda
     comandos. Delega apenas para `asta`, `anna`, `day` e `dakota`.
   - `asta.md` — `subagent` de back-end. Postura calma, sem estresse, paciente na
     negociação de contratos de API com a Anna.
   - `anna.md` — `subagent` de front-end. Fiel ao design da Day e integrada ao
     contrato da Asta.
   - `day.md` — `subagent` de design. Extrai medidas/tokens da referência e entrega
     especificação implementável, registrando qualquer desvio.
   - `dakota.md` — `subagent` de fechamento. Revisa o diff, atualiza o `contexto.md`,
     commita e faz `git push origin main`.
2. Definido `default_agent: bebell` na configuração do projeto
   (`.opencode/opencode.json`), para que novas sessões já comecem com a Bebell.
3. Validada a descoberta dos agentes pela API (`GET /api/agent`): os 5 aparecem com
   modo e cor corretos.

Arquivos alterados:

- `.opencode/agents/bebell.md` — criado
- `.opencode/agents/asta.md` — criado
- `.opencode/agents/anna.md` — criado
- `.opencode/agents/day.md` — criado
- `.opencode/agents/dakota.md` — criado
- `.opencode/opencode.json` — criado com `default_agent: bebell`
- `contexto.md` — esta entrada e atualização da estrutura do projeto

Pendências:

- [ ] Reiniciar o OpenCode / abrir uma nova sessão para a Bebell assumir como agente padrão.
- [ ] Testar o fluxo completo com um pedido real (Bebell → Asta/Anna/Day → Dakota).

Observação: as cores dos agentes são definidas/ajustadas pelo próprio OpenCode e são
mantidas como estão — é apenas estético e não afeta o funcionamento.

---

### 2026-10-22 — Criação das pastas game e inspo

Solicitação: criar duas pastas na raiz do repositório — `game` (para o jogo/projeto) e
`inspo` (para referências visuais) — e registrar/commitar a tarefa no Git.

O que foi feito:

1. Criada a pasta `game/` na raiz do repositório.
2. Criada a pasta `inspo/` na raiz do repositório.
3. Como o Git não versiona diretórios vazios, adicionado um arquivo `.gitkeep`
   (vazio, 0 bytes) dentro de cada pasta, para que ambas apareçam no repositório.
4. Atualizada a seção **Estado atual do repositório** com as duas novas pastas.
5. Registrada esta entrada no histórico, seguida de commit e `git push origin main`.

Arquivos alterados:

- `game/.gitkeep` — criado (arquivo vazio, mantém a pasta no versionamento)
- `inspo/.gitkeep` — criado (arquivo vazio, mantém a pasta no versionamento)
- `contexto.md` — nova entrada no histórico e estrutura do repositório atualizada

Pendências:

- [ ] Preencher `game/` com o conteúdo do jogo/projeto (estrutura ainda não definida).
- [ ] Preencher `inspo/` com as referências visuais do projeto.

---

### 2026-10-22 — Regra fixa: registrar no contexto e publicar no GitHub

Solicitação: "sempre grave as alterações no contexto e suba ao github".

O que foi feito:

1. Reforçada a regra na seção **Preferências de trabalho**, como item explícito e
   inequívoco: toda alteração no projeto (arquivos, pastas, configurações, agentes,
   assets) deve gerar uma nova entrada de histórico neste arquivo e, na mesma tarefa,
   commit e `git push origin main`. Nenhuma tarefa é considerada concluída antes disso.
2. Registrada esta entrada no histórico, seguida de commit e envio para `origin/main`.

Arquivos alterados:

- `contexto.md` — regra fixa reforçada nas preferências e nova entrada de histórico

Pendências:

- Nenhuma pendência nova. Observação: a regra passa a valer para todas as tarefas
  futuras, sem exceção.

---

### 2026-10-22 — Estrutura de pastas do jogo (code e arts)

Solicitação: criar a pasta `code` e a pasta `arts` dentro de `game`, e dentro de `arts`
criar as pastas `mapa` e `personas`.

O que foi feito:

1. Criada a pasta `game/code/`.
2. Criada a pasta `game/arts/` com as subpastas `game/arts/mapa/` e `game/arts/personas/`.
3. Como o Git não versiona diretórios vazios, adicionado um arquivo `.gitkeep`
   (vazio, 0 bytes) em cada pasta nova — `game/code`, `game/arts/mapa` e
   `game/arts/personas`. O `game/.gitkeep` existente foi mantido como está.
4. Definida a convenção de organização: `game/code` guarda o código do jogo;
   `game/arts` guarda a arte, com `mapa` para tilesets/mapas e `personas` para
   personagens.
5. Atualizada a seção **Estado atual do repositório** com a nova árvore de pastas.
6. Registrada esta entrada no histórico, seguida de commit e `git push origin main`.

Arquivos alterados:

- `game/code/.gitkeep` — criado (arquivo vazio, mantém a pasta no versionamento)
- `game/arts/mapa/.gitkeep` — criado (arquivo vazio, mantém a pasta no versionamento)
- `game/arts/personas/.gitkeep` — criado (arquivo vazio, mantém a pasta no versionamento)
- `contexto.md` — nova entrada no histórico e estrutura do repositório atualizada

Pendências:

- [ ] Preencher `game/code` com a base do código do jogo.
- [ ] Preencher `game/arts/mapa` com os tilesets/mapas.
- [ ] Preencher `game/arts/personas` com os sprites de personagens.
- [ ] Preencher `inspo/` com as referências visuais do projeto.

---

### 2026-10-22 — Convenção de pastas para personagens

Solicitação: toda vez que for pedido para criar um personagem, criar dentro da pasta
dele a pasta `<nome>-context` e a pasta `<nome>-sprite`.

O que foi feito:

1. Definida a convenção de pastas para personagens na nova seção
   **Convenções do projeto**: `game/arts/personas/<nome>/` com as subpastas
   `<nome>-context/` e `<nome>-sprite/`; `<nome>` em minúsculas, sem acentos e sem
   espaços (hífen se tiver mais de uma palavra).
2. Confirmado com o usuário o formato `fox/fox-context` e `fox/fox-sprite`
   (`game/arts/personas/fox/fox-context/` e `game/arts/personas/fox/fox-sprite/`).
3. Nenhuma pasta de personagem criada nesta tarefa — fica aguardando o primeiro
   personagem.

Arquivos alterados:

- `contexto.md` — nova seção **Convenções do projeto** e esta entrada

Pendências:

- [ ] Criar o primeiro personagem seguindo a convenção, quando o usuário pedir.

---

### 2026-10-22 — Separação de contexto e histórico

Solicitação: o usuário pediu o `contexto.md` apenas com as regras de negócio e a
explicação dos arquivos; histórico movido para `historico.md`.

O que foi feito:

1. Criado `historico.md` com todo o histórico anterior preservado (8 entradas
   anteriores, sem resumo nem reescrita), mais o modelo de entrada.
2. `contexto.md` reescrito enxuto: regras de negócio do projeto, regras de negócio do
   jogo (ainda não definidas) e explicação dos arquivos e pastas.
3. Modelo de entrada movido para o `historico.md`.

Arquivos alterados:

- `contexto.md` — reescrito (sem histórico e sem modelo de entrada)
- `historico.md` — criado

Pendências:

- Nenhuma pendência nova (as antigas continuam nos seus blocos acima).

---

### 2026-10-22 — Restauração do agente Asta corrompido

Solicitação: não houve pedido do usuário — correção de integridade detectada no
fechamento da tarefa anterior.

O que foi feito:

1. Detectado que `.opencode/agents/asta.md` estava modificado no working tree, com a
   `description` corrompida ("regras de anco de dadnegócio, bos").
2. Confirmado por `git diff` que apenas essa linha divergia do HEAD.
3. Arquivo restaurado a partir do HEAD (`git restore -- .opencode/agents/asta.md`).
4. Nenhum agente foi alterado de comportamento — o conteúdo restaurado é o original
   commitado.

Arquivos alterados:

- `.opencode/agents/asta.md` — restaurado (sem alteração de conteúdo em relação ao HEAD)
- `historico.md` — esta entrada

Pendências:

- [ ] Investigar a origem da corrupção (alguma edição externa/concorrente tocou o
  arquivo) se voltar a acontecer.

---

### 2026-10-22 — Ficha JSON obrigatória para personagens

Solicitação: o usuário pediu que, ao criar um personagem, também seja criado um arquivo
JSON com as informações dele (nome, idade, data de nascimento, ideal — reino ou
oposição —, descrição visual, descrição dos ideais e descrição da personalidade).

O que foi feito:

1. Definido que o JSON fica na raiz da pasta do personagem
   (`game/arts/personas/<nome>/<nome>.json`), ao lado de `<nome>-context/` e
   `<nome>-sprite/`.
2. Documentado o schema no `contexto.md`: os 7 campos em snake_case
   (`nome`, `idade`, `data_nascimento`, `ideal`, `descricao_visual`,
   `descricao_ideais`, `descricao_personalidade`), com os tipos de cada um e o enum do
   `ideal` (`"reino"` ou `"oposicao"`).
3. Regra registrada no `contexto.md` (convenção de pastas de personagens + ficha JSON
   obrigatória) e linha correspondente na tabela de arquivos e pastas.
4. Nenhum personagem ou JSON criado nesta tarefa.

Arquivos alterados:

- `contexto.md` — regra de personagens e tabela de arquivos atualizadas
- `historico.md` — esta entrada

Pendências:

- [ ] Criar o primeiro personagem já com o `<nome>.json` preenchido conforme o schema.

---

## Modelo para novas entradas

```markdown
### AAAA-MM-DD — Título curto da tarefa

Solicitação: ...

O que foi feito:

1. ...

Arquivos alterados:

- `caminho/arquivo` — descrição da mudança

Pendências:

- [ ] ...
```
