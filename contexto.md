# Contexto do Projeto — a-fronda

Este arquivo registra o histórico de decisões, configurações e tarefas deste projeto.
Regra combinada: **tudo o que for feito deve ser registrado aqui, em Markdown.**

---

## Preferências de trabalho

- **Idioma:** responder sempre em português do Brasil.
- **Registro:** toda alteração, instalação, decisão ou tarefa executada deve ser anotada neste arquivo (`contexto.md`).
- **Formato:** Markdown, com seções por data/assunto e histórico em ordem cronológica.
- **Git:** toda atualização deve ser commitada e enviada para o GitHub (`origin`, branch `main`) na mesma tarefa.

---

## Estado atual do repositório

- Diretório: `C:\Users\kaike\Documents\a-fronda`
- Repositório Git: sim
- Conteúdo antes das alterações: apenas `README.md` (título `# a-fronda`)

---

## Histórico

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
