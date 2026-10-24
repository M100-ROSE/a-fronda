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

### 2026-10-22 — 23 personagens históricos criados com ficha JSON

Solicitação: o usuário explicou a divisão política do mundo do jogo (`reino` ×
`oposicao`) e pediu para pesquisar e adicionar 23 personagens históricos, todos com a
ficha JSON obrigatória.

O que foi feito:

1. Pesquisa dos dados biográficos de cada personagem (nascimento, aparência, ideais e
   personalidade), dividida em 4 lotes paralelos.
2. Criadas as 23 pastas em `game/arts/personas/<slug>/`, cada uma com `<slug>.json`,
   `<slug>-context/.gitkeep` e `<slug>-sprite/.gitkeep` (arquivos `.gitkeep` de 0 bytes).
3. Todas as fichas gravadas com `"ideal": "oposicao"` e os 7 campos validados (JSON em
   UTF-8 sem BOM, `ConvertFrom-Json` sem erro).
4. Documentadas no `contexto.md` a regra da divisão política (seção **Divisão política
   do mundo**) e a convenção do campo `idade` (idade no momento mais marcante da
   trajetória, não idade de morte).

Personagens criados (slug | nome | nascimento | idade | momento marcante usado):

| slug | nome | data_nascimento | idade | momento marcante |
| --- | --- | --- | --- | --- |
| carlos-marighella | Carlos Marighella | 1911-12-05 | 56 | Fundação da ALN / ruptura com o PCB, fev/1968 |
| karl-marx | Karl Marx | 1818-05-05 | 29 | Publicação do Manifesto Comunista, fev/1848 |
| friedrich-engels | Friedrich Engels | 1820-11-28 | 27 | Coautoria do Manifesto Comunista, fev/1848 |
| che-guevara | Che Guevara | 1928-06-14 | 39 | Campanha guerrilheira na Bolívia, 1967 |
| fidel-castro | Fidel Castro | 1926-08-13 | 32 | Vitória da Revolução e entrada em Havana, jan/1959 |
| leonel-brizola | Leonel Brizola | 1922-01-22 | 39 | Campanha da Legalidade no Palácio Piratini, ago–set/1961 |
| rosa-luxemburgo | Rosa Luxemburgo | 1871-03-05 | 47 | Fundação do KPD / Revolução Espartaquista, jan/1919 |
| mao-tse-tung | Mao Tsé-Tung | 1893-12-26 | 55 | Proclamação da República Popular da China, out/1949 |
| malcolm-x | Malcolm X | 1925-05-19 | 38 | Rompimento com a Nação do Islã e peregrinação a Meca, 1964 |
| rosa-parks | Rosa Parks | 1913-02-04 | 42 | Recusa de ceder o assento em Montgomery, dez/1955 |
| carlos-lamarca | Carlos Lamarca | 1937-10-27 | 31 | Deserção de Quitaúna com 63 fuzis, jan/1969 |
| iara-iavelberg | Iara Iavelberg | 1944-05-07 | 25 | Treinamento no Vale do Ribeira / formação teórica, início de 1970 |
| fred-hampton | Fred Hampton | 1948-08-30 | 21 | Presidência do capítulo de Illinois dos Panteras Negras e Coalizão Arco-Íris, set/1969 |
| huey-p-newton | Huey P. Newton | 1942-02-17 | 24 | Fundação do Partido Pantera Negra, out/1966 |
| martin-luther-king-jr | Martin Luther King Jr. | 1929-01-15 | 34 | Discurso "I Have a Dream", ago/1963 |
| tupac-shakur | Tupac Shakur | 1971-06-16 | 23 | Lançamento de *Me Against the World*, mar/1995 |
| vladimir-lenin | Vladimir Lênin | 1870-04-22 | 47 | Revolução de Outubro, nov/1917 |
| maximilien-de-robespierre | Maximilien de Robespierre | 1758-05-06 | 35 | Entrada no Comitê de Salvação Pública, jul/1793 |
| nelson-mandela | Nelson Mandela | 1918-07-18 | 75 | Posse como presidente da África do Sul, mai/1994 |
| mahatma-gandhi | Mahatma Gandhi | 1869-10-02 | 60 | Marcha do Sal, 1930 |
| thomas-sankara | Thomas Sankara | 1949-12-21 | 33 | Assunção da presidência de Alto Volta, ago/1983 |
| kwame-nkrumah | Kwame Nkrumah | 1909-09-21 | 47 | Independência de Gana, mar/1957 |
| patrice-lumumba | Patrice Lumumba | 1925-07-02 | 34 | Discurso da independência do Congo, jun/1960 |

Observações da pesquisa:

- **Che Guevara:** a Wikipédia em português traz 1928-05-14; foi usada `1928-06-14`
  (data do registro, mais aceita academicamente).
- **Lênin:** `1870-04-22` é a data no calendário gregoriano; o Wikidata lista
  `1870-04-10` (calendário juliano).

Arquivos alterados:

- `game/arts/personas/<slug>/<slug>.json` — 23 fichas criadas
- `game/arts/personas/<slug>/<slug>-context/.gitkeep` — 23 arquivos criados (0 bytes)
- `game/arts/personas/<slug>/<slug>-sprite/.gitkeep` — 23 arquivos criados (0 bytes)
- `contexto.md` — seção **Divisão política do mundo**, nota do campo `idade` e item de
  personagens cadastrados
- `historico.md` — esta entrada

Pendências:

- [ ] Gerar os sprites de cada personagem (pastas `<slug>-sprite/` ainda vazias).
- [ ] Desenhar os mapas/reinos em `game/arts/mapa/`.
- [ ] Decidir se/quais personagens do lado `reino` serão criados.

---

### 2026-10-22 — 23 personagens do lado reino

Solicitação: o usuário pediu para adicionar 23 personagens do lado `reino` — nomes
conservadores, liberais e figuras da direita brasileira, completando o outro lado da
divisão política do mundo do jogo.

O que foi feito:

1. Pesquisa dos dados biográficos de cada personagem, dividida em 4 lotes paralelos
   (nascimento, aparência, ideais e personalidade), seguindo o schema obrigatório.
2. Criadas as 23 pastas em `game/arts/personas/<slug>/`, cada uma com `<slug>.json`,
   `<slug>-context/.gitkeep` e `<slug>-sprite/.gitkeep` (`.gitkeep` de 0 bytes).
3. Todas as fichas gravadas com `"ideal": "reino"` e os 7 campos validados (JSON em
   UTF-8 sem BOM, `ConvertFrom-Json` sem erro).
4. **Correção de estrutura:** 5 personagens brasileiros
   (`afonso-arinos-de-melo-franco`, `carlos-lacerda`,
   `jose-bonifacio-de-andrada-e-silva`, `visconde-de-maua` e `visconde-de-uruguai`)
   tiveram as pastas `<slug>-context/` e `<slug>-sprite/` criadas por engano na **raiz**
   de `game/arts/personas/`. Os 10 diretórios foram movidos para dentro da pasta de cada
   personagem, sem sobra de diretórios vazios e sem alterar o conteúdo dos JSONs.
5. O total do projeto passa a **46 personagens** (23 `oposicao` × 23 `reino`).

Personagens criados (slug | nome | nascimento | idade | momento marcante usado):

| slug | nome | data_nascimento | idade | momento marcante |
| --- | --- | --- | --- | --- |
| edmund-burke | Edmund Burke | 1729-01-12 | 61 | Publicação de *Reflexões sobre a Revolução na França*, 1790 |
| michael-oakeshott | Michael Oakeshott | 1901-12-11 | 60 | Publicação de *Rationalism in Politics*, 1962 |
| roger-scruton | Roger Scruton | 1944-02-27 | 36 | Publicação de *The Meaning of Conservatism*, 1980 |
| alexis-de-tocqueville | Alexis de Tocqueville | 1805-07-29 | 29 | Publicação do vol. 1 de *A Democracia na América*, 1835 |
| winston-churchill | Winston Churchill | 1874-11-30 | 65 | Posse como primeiro-ministro, mai/1940 |
| margaret-thatcher | Margaret Thatcher | 1925-10-13 | 53 | Posse como primeira-ministra, mai/1979 |
| ronald-reagan | Ronald Reagan | 1911-02-06 | 69 | Posse como 40º presidente dos EUA, jan/1981 |
| konrad-adenauer | Konrad Adenauer | 1876-01-05 | 73 | Eleição como 1º chanceler federal da RFA, set/1949 |
| robert-schuman | Robert Schuman | 1886-06-29 | 63 | Declaração Schuman, mai/1950 |
| john-howard | John Howard | 1939-07-26 | 56 | Posse como primeiro-ministro da Austrália, mar/1996 |
| brian-mulroney | Brian Mulroney | 1939-03-20 | 45 | Posse como primeiro-ministro do Canadá, set/1984 |
| barry-goldwater | Barry Goldwater | 1909-01-02 | 55 | Nomeação republicana à presidência, jul/1964 |
| friedrich-hayek | Friedrich Hayek | 1899-05-08 | 75 | Prêmio Nobel de Economia, out/1974 |
| milton-friedman | Milton Friedman | 1912-07-31 | 64 | Prêmio Nobel de Economia, out/1976 |
| ludwig-von-mises | Ludwig von Mises | 1881-09-29 | 38 | Artigo sobre o cálculo econômico no socialismo, 1920 |
| thomas-sowell | Thomas Sowell | 1930-06-30 | 50 | *Knowledge and Decisions* e ingresso na Hoover Institution, 1980 |
| walter-williams | Walter Williams | 1936-03-31 | 44 | Entrada na George Mason University e coluna sindicada, 1980 |
| roberto-campos | Roberto Campos | 1917-04-17 | 47 | Ministro do Planejamento e coordenação do PAEG, 1964 |
| jose-bonifacio-de-andrada-e-silva | José Bonifácio de Andrada e Silva | 1763-06-13 | 59 | Independência do Brasil, set/1822 |
| visconde-de-uruguai | Visconde de Uruguai | 1807-10-04 | 44 | Intervenção no Prata contra Oribe e Rosas, 1851–1852 |
| carlos-lacerda | Carlos Lacerda | 1914-04-30 | 40 | Atentado da Rua Tonelero e crise que levou ao suicídio de Vargas, 1954 |
| afonso-arinos-de-melo-franco | Afonso Arinos de Melo Franco | 1905-11-27 | 45 | Lei Afonso Arinos contra a discriminação racial, 1951 |
| visconde-de-maua | Visconde de Mauá | 1813-12-28 | 40 | Inauguração da primeira ferrovia brasileira, 1854 |

Observações da pesquisa (divergências factuais):

- **Edmund Burke:** `1729-01-12` é a data no estilo novo (gregoriano); no estilo antigo
  (juliano) corresponde a 1º/jan/1729. Foi usada a data gregoriana.
- **Barry Goldwater:** `1909-01-02`; algumas fontes antigas citam 01/01/1909. Foi usada
  a data mais difundida nas fontes biográficas.
- **Visconde de Uruguai:** `1807-10-04`; a Wikipédia em inglês traz 1808. Foi usada a
  data das fontes em português.
- **Lênin e Che Guevara:** divergências já registradas na entrada dos 23 personagens de
  `oposicao` (Lênin: gregoriano × juliano; Che: data de registro × Wikipédia em português).

Arquivos alterados:

- `game/arts/personas/<slug>/<slug>.json` — 23 fichas criadas (lado `reino`)
- `game/arts/personas/<slug>/<slug>-context/.gitkeep` — 23 arquivos criados (0 bytes)
- `game/arts/personas/<slug>/<slug>-sprite/.gitkeep` — 23 arquivos criados (0 bytes)
- `game/arts/personas/<slug>/<slug>-context/` e `<slug>-sprite/` — 10 diretórios
  movidos da raiz de `personas/` para dentro da pasta do personagem (correção de
  estrutura dos 5 personagens brasileiros)
- `contexto.md` — contagem de personagens atualizada (46 / 23 / 23) e nota de precisão
  sobre os liberais clássicos no lado `reino`
- `historico.md` — esta entrada

Pendências:

- [ ] Gerar os sprites dos 46 personagens (pastas `<slug>-sprite/` ainda vazias).
- [ ] Desenhar os mapas/reinos em `game/arts/mapa/`.
- [ ] (Anterior, concluída) Decidir se/quais personagens do lado `reino` seriam criados —
  resolvida nesta tarefa, com 23 nomes escolhidos pelo usuário.

---

### 2026-10-23 — Revisão das paletas + agentes Monica/Izak + publicação

Solicitação: verificar se as paletas de cores estavam de acordo e passavam a
mensagem certa do contexto do jogo; em seguida registrar tudo no `historico.md`
e no `contexto.md` e subir as alterações para o GitHub.

O que foi feito:

1. Lido o rascunho em `inspo/paleta.md` contra a divisão política do mundo
   (`reino` × `oposicao`) e contra as fichas visuais dos personagens.
2. **Diagnóstico:** o rascunho **não** passava a mensagem do jogo.
   - Nomes mentiam o hex (ex.: “verde” = `#2094f3` azul; “azul claro” =
     `#90EE90` verde; “vermelho escuro” = `#800080` roxo).
   - As duas vilas compartilhavam os mesmos swatches (`#808080`, `#90EE90`,
     `#B22222`, `#DAA520`, `#00FF00`), então os lados políticos não se
     distinguiam.
   - Ambiente da oposição era um arco-íris Material Design.
   - Login/cadastro usava magenta `#FF00E9` e “gold” roxo, fora do universo.
   - A rampa P&B dos personagens repetia os nomes `Cinza` e `Branco`.
3. Reescrita de `inspo/paleta.md` como fonte da verdade, alinhada ao contexto:
   - **Vila revolucionária (`oposicao`):** vermelho bandeira `#C41E3A`, tijolo,
     ocre, oliva de guerrilha, terra, fumaça, papel sujo, carvão.
   - **Vila direita (`reino`):** azul-marinho `#0B1D4A`, azul real, ouro
     autoridade `#C9A227`, marfim, pedra, grafite.
   - **Ambientes** de cada vila (tileset): terra/tijolo/lampião vs
     mármore/ardósia/jardim aparado.
   - **Personagens P&B:** rampa de 8 degraus sem nomes repetidos.
   - **Login/cadastro:** palco escuro com os dois acentos de facção (única
     paleta que mistura os lados, porque é a porta de escolha).
   - Regras: nome = hex; sem neon CSS; cor nova entra neste arquivo antes de
     asset/CSS.
4. Atualizado o `contexto.md` com a regra de identidade visual, a linha da
   paleta na tabela de arquivos e os agentes **Monica** e **Izak** (já
   existentes em `.opencode/agents/`, mas ainda não documentados nas regras).
5. Incluídos no mesmo envio os arquivos ainda não versionados do time:
   `.opencode/agents/monica.md`, `.opencode/agents/izak.md` e a atualização de
   `.opencode/agents/bebell.md` (delega também para Monica e Izak). Corrigidos
   backticks quebrados nesses três arquivos (`\asta\` → `` `asta` ``, `ead` →
   `read`, `\websearch\` → `` `websearch` ``).

Arquivos alterados:

- `inspo/paleta.md` — reescrito (paletas oficiais alinhadas ao contexto)
- `contexto.md` — identidade visual, tabela de arquivos, Monica e Izak
- `historico.md` — esta entrada
- `.opencode/agents/bebell.md` — orquestração passa a incluir Monica e Izak
- `.opencode/agents/monica.md` — criado (pesquisa de referências)
- `.opencode/agents/izak.md` — criado (relatório final)

Pendências:

- [ ] Aplicar as paletas oficiais nos primeiros tilesets (`game/arts/mapa/`) e
      nos sprites (`game/arts/personas/<slug>/<slug>-sprite/`).
- [ ] Quando houver tela de login/cadastro, usar só a paleta correspondente.

---

### 2026-10-23 — Briefing de sensações da arte (`inspo/briefing.md`)

Solicitação: criar `inspo/briefing.md` com as sensações que as artes de cada
personagem devem passar, o que as vilas devem passar, o estilo de pixel art
como o de Stardew Valley, e as áreas — em especial o **castelo do Rei César**,
apoiado pelo lado `reino`, com sentimento de perigo, de não estar seguro e de
prisão psicológica.

O que foi feito:

1. Cruzadas as 46 fichas JSON (visual, ideais, personalidade), `inspo/paleta.md`
   e a divisão política do mundo.
2. Criado `inspo/briefing.md` como fonte da verdade de **sensação** (não de
   biografia nem de hex):
   - **Estilo:** gramática do Stardew Valley (vista 3/4, silhueta, contorno
     1 px, idle vivo, tiles com personalidade, luz de hora do dia) **sem** o
     aconchego da fazenda, sem chibi, sem neon.
   - **Vila revolucionária:** calor, urgência, clandestinidade, dignidade pobre.
   - **Vila direita:** ordem, hierarquia, frieza educada; bela, mas não é o
     castelo.
   - **Castelo do Rei César:** área nova, apoiada pelo `reino`. Perigo
     institucional, insegurança (nenhum canto é save/abrigo) e prisão
     psicológica (etiqueta, retratos, corredores repetidos, ouro como grade).
     Sem biografia inventada do rei — ele é pressão espacial.
   - **46 personagens:** uma entrada cada, com sensação dominante e como a arte
     entrega (objeto-âncora, pose, luz).
3. Atualizado o `contexto.md`: estilo Stardew na abertura, regra do briefing,
   seção **Lugares do mundo (arte)** e linha na tabela de arquivos.

Arquivos alterados:

- `inspo/briefing.md` — criado
- `contexto.md` — briefing, lugares do mundo, estilo de arte
- `historico.md` — esta entrada

Pendências:

- [ ] Aplicar o briefing + paleta nos tilesets (`game/arts/mapa/`), inclusive o
      castelo do Rei César.
- [ ] Aplicar o briefing nos sprites (`game/arts/personas/<slug>/<slug>-sprite/`).
- [ ] Ficha do Rei César só quando o usuário pedir (biografia ainda indefinida).

---

### 2026-10-23 — Pasta de referências visuais (`inspo/visual.md`)

Solicitação: criar `inspo/visual.md` com links de imagens de pixel art medieval,
do estilo Stardew Valley, fotos dos personagens e o que mais for necessário
para inspirar sprites de mapas e de personagens.

O que foi feito:

1. Pesquisa de páginas reais (wiki do Stardew Valley, Steam/press kits, Wikimedia
   Commons, Wikipédia, Lospec, tileset CC0), com data de acesso 23 out 2026.
2. Criado `inspo/visual.md` com:
   - **Stardew:** Pelican Town, interiores, Mayor's Manor, Wizard's Tower,
     Adventurer's Guild, retratos/sprites de NPC (Lewis, Abigail, sprite sheet).
   - **Medieval / castelo:** Kingdom Two Crowns, Graveyard Keeper, Blasphemous,
     Eastward, tileset CC0 de vila, preview itch.io, Lospec.
   - **Mapeamento** vila revolucionária × vila do reino × Castelo do Rei César.
   - **46 personagens:** Wikipédia + busca Commons; arquivos `File:` conferidos
     para Che (Korda), Che+Fidel, Rosa Parks 1955, galerias de Marx, Mandela,
     Thatcher, Churchill 1940, etc.
   - Extra de figurino (kente, roca, tweed, muralha, tijolo).
   - Aviso de licença: jogo comercial = linguagem, não cópia; Commons = ler a
     página do arquivo.
3. Atualizados `contexto.md` (regra + tabela) e este histórico.

Arquivos alterados:

- `inspo/visual.md` — criado
- `contexto.md` — referência visual na abertura, regra e tabela
- `historico.md` — esta entrada

Pendências:

- [ ] Usar `visual.md` + briefing + paleta no primeiro lote de sprites/tilesets.
- [ ] Completar `File:` direto no Commons para personagens que hoje só têm busca
      (Marighella, Lamarca, Iara, etc.) quando o sprite for gerado.

---

### 2026-10-23 — Tela de login/cadastro em MonoGame (design + 3 levas: front, auth, polimento)

Solicitação: "crie uma tela de login/cadastro ... em levas, codigo frontend, adicione
backend e dai na terceira parte adicione mais detalhes no frontend". Executada em 4
levas pelos agentes (Day → Anna → Asta → Anna).

O que foi feito:

**Leva 0 — Design (Day):**

1. Criado `game/code/docs/design-tela-login.md` — spec visual completa da tela de
   login/cadastro: janela 960×540, painel 360×480 centrado, paleta de login de
   `inspo/paleta.md` usada exatamente, CTA primário no lado `oposicao` (`#C41E3A`),
   botão "criar conta" desligado até escolher lado, erro como placa sólida `#7A1418`
   com texto `#F3EDE0`, borda de input `#C41E3A`, foco `#E8DCC8`, ouro `#C9A227`
   restrito a 3 usos de marca, contorno carvão sancionado pelo briefing.

**Leva 1 — Frontend (Anna):**

2. Criada a solução em `game/code/`: `Fronda.slnx`, `README.md`, `.gitignore` (bin/,
   obj/, *.user) e o projeto `src/Fronda.Client/` (MonoGame 3.8.2.1105 via
   PackageReference, TFM net8.0), com um arquivo por classe:
   - `Program.cs`; `Core/` (GameApp, Scene, SceneManager, Entrada);
   - `Rendering/` (Palette, ShapeBatch);
   - `UI/` (Widget, Label, Button, FactionButton, BotaoCta, BotaoTab, LinkButton,
     TextBox, ToggleGroup, Panel, ErrorBanner);
   - `Font/PixelFont.cs` (glifos 5×7 procedurais em runtime, acentos PT-BR, sem
     Content Pipeline); `Scenes/LoginScene.cs`; `Auth/` (IAuthService,
     NullAuthService).
3. Tela completa: tabs entrar/cadastrar, usuário+senha (máscara), confirmação, escolha
   de lado reino×oposição, CTA desligado até escolher lado, validações locais, foco
   teclado+mouse.

**Leva 2 — Back-end (Asta):**

4. Criado `src/Fronda.Auth/` (class library net8.0, **sem** dependência de MonoGame;
   única dependência é Client→Auth):
   - `Modelos/` (Lado, Jogador, Resultado — definição única movida para cá);
   - `IAuthService.cs`, `Conta.cs`;
   - `Hash/` (IHasher, Pbkdf2Hasher — PBKDF2-HMAC-SHA256, 150k iterações, salt de
     16 B/conta, FixedTimeEquals);
   - `Armazenamento/` (IContasRepository, JsonContasRepository — persistência em
     `%APPDATA%\Fronda\usuarios.json`, gravação atômica .tmp+move, lock, tolerante a
     arquivo ausente/corrompido);
   - `AuthService.cs` com revalidação server-side (usuário 3–20 lowercase
     a-z0-9_-, senha ≥8, confirmação, lado válido, unicidade).
5. Cliente: ponto de composição em `LoginScene` trocado para o `AuthService` real
   (`NullAuthService` mantido como fallback, não default).
6. Criado `tests/Fronda.Auth.Tests/` (xunit, 25 testes, caminho de dados em %TEMP%).
   JSON de contas: id, usuario, hash, salt, lado ("reino"/"oposicao"), dataCriacao.

**Leva 3 — Polimento frontend (Anna):**

7. Caret piscando (30/30 frames); dissolução de entrada em 4 degraus discretos de cor
   (nota: BlendState.Opaque ⇒ alpha não afeta; fade é degrau de cor via
   `Palette.DissolverPara`); microfeedback nos botões de facção (nudge 1 px + filete
   intensificado); nova cena `Scenes/BemVindoScene.cs` com transição por dissolução
   interna (SceneManager só corta — documentado); contorno 1 px carvão `#1A1210` no
   painel (vila revolucionária nº 10, sancionada pelo briefing); placa de erro com
   2 degraus + shake 1 px/3 frames no input com erro; pulso 2 degraus no filete de
   ouro do título; glifo "…" dedicado no PixelFont.
8. Contrato ajustado no cliente: `credenciais_invalidas` com `ErroCampO` null (banner
   sob o CTA) e `usuario_invalido` mapeado.
9. Validação: `dotnet build` 0 erro/0 aviso e `dotnet test` 25/25. Sem mutações no
   back-end na leva 3.

Arquivos alterados:

- `game/code/docs/design-tela-login.md` — criado (spec da tela de login)
- `game/code/Fronda.slnx`, `game/code/README.md`, `game/code/.gitignore` — criados
- `game/code/src/Fronda.Client/` — criado (Program, Core/, Rendering/, UI/, Font/,
  Auth/, Scenes/)
- `game/code/src/Fronda.Auth/` — criado (Auth, Conta, Hash/, Modelos/, Armazenamento/)
- `game/code/tests/Fronda.Auth.Tests/` — criado (AuthService, JsonContasRepository,
  Pbkdf2Hasher, Suporte)
- `contexto.md` — engine definida como MonoGame (C#, DesktopGL) e tabela de
  arquivos/pastas atualizada
- `historico.md` — esta entrada

Pendências:

- [ ] Itens da spec §6 ainda não pedidos: hover com insígnia pulsando, costura do
      painel, fumaça/lampião no horizonte, dithering do placeholder.
- [ ] Fade na troca de modo entrar↔cadastrar.
- [ ] Migração futura do número de iterações PBKDF2 no formato do hash armazenado.
- [ ] Troca de cena pós-login é placeholder (`BemVindoScene`) — definir a cena real.
- [ ] Regras de negócio do jogo continuam indefinidas (nada a inventar).

---

### 2026-10-23 — Tela de login legível: bug da fonte invisível corrigido + spec de legibilidade implementada

Solicitação: "melhore a tela de login, cadê as coisas escritas e tudo mais?".
Diagnóstico do time: o texto **não renderizava** (bug real) e, por cima disso, a
legibilidade era ruim. Executada em 3 fases (Day → Anna → Dakota).

O que foi feito:

1. **Day — spec atualizada** (`game/code/docs/design-tela-login.md`):
   - Janela em **escala inteira ×2** (backbuffer lógico 960×540 desenhado em
     1920×1080 via transform matrix; fallback sancionado ×1 = 960×540).
   - Escalas de fonte por classe: título **5**, corpo/botões **3**,
     subtítulo/rodapé **2** (escala 1 interditada nesta tela; sem escala
     fracionária).
   - Re-layout do painel: padding 24→16, fluxo vertical, tabs assimétricas
     (128/192), facções 160×40 com insígnia no topo central, CTA 328×36, selo
     de erro 328×28.
   - Novos escritos: subtítulo "meia-noite: escolha o lado", rodapé
     "tab navega - enter envia - esc volta", placeholders novos; checklist de
     aceite atualizado (§2.2 re-layout, §2.4 contrato da fonte, §7 mensagens).
2. **Anna — causa raiz do bug encontrada e corrigida (fonte invisível):**
   - **Causa raiz:** `PixelFont.Desenhar`
     (`game/code/src/Fronda.Client/Font/PixelFont.cs`) pintava somente pixels
     `'#'`, mas **todos os glifos usam `'X'` como pixel sólido** — nenhum pixel
     era desenhado e a fonte inteira era invisível. Corrigido para aceitar
     `'X'` **ou** `'#'`.
   - Helpers novos no `PixelFont`: `AlturaSlot`, `TopoBaseNoSlot`,
     `TopoBaseCentradoNoSlot`, `TemAcento`, `TemCedilha`; a cedilha desce
     2·N **abaixo** da base (contrato de encaixe documentado no cabeçalho).
   - Implementação da spec de legibilidade:
     - `Core/GameApp.cs` — janela 1920×1080 (inteiros), transform matrix ×2,
       PointClamp, `PreferMultiSampling = false`, mouse ÷2 na entrega às
       cenas, fallback ×1 sancionado quando a altura 1080 não couber.
     - `UI/Label.cs`, `UI/Button.cs`, `UI/TextBox.cs` (escala 3, máscara `*`,
       caret 2×21), `UI/ErrorBanner.cs`, `UI/LinkButton.cs`,
       `UI/FactionButton.cs` (insígnia no topo central), `UI/BotaoCta.cs`.
     - `Scenes/LoginScene.cs` — `Organizar()` reescrito conforme §2.2,
       subtítulo, rodapé, placeholders novos, limiar de usuário 16,
       mensagens de erro novas por §7 ("nome de 3 letras", "no mínimo 8",
       "senhas não batem"), "escolha seu lado" em marfim.
     - `Scenes/BemVindoScene.cs` — "...conectado ao mundo" em escala 3.
   - Validação da Anna: `dotnet build` 0 erro/0 aviso; `dotnet test` 25/25
     (`Fronda.Auth.Tests`); screenshot da janela real com todos os textos
     legíveis.
3. **Dakota — fechamento:** revisão do diff (12 arquivos, todos relacionados;
   nenhum artefato de build no commit), `.gitignore` na raiz para a pasta
   temporária de reprodução, esta entrada, commit e push.

Arquivos alterados:

- `game/code/docs/design-tela-login.md` — spec atualizada (janela ×2, escalas,
  re-layout do painel, novos escritos, checklist de aceite)
- `game/code/src/Fronda.Client/Font/PixelFont.cs` — correção do bug ('X'/'#'),
  contrato de encaixe do pos.Y e helpers de slot/acento/cedilha
- `game/code/src/Fronda.Client/Core/GameApp.cs` — janela 1920×1080, matrix ×2,
  PointClamp, mouse ÷2, fallback ×1
- `game/code/src/Fronda.Client/UI/Label.cs`, `Button.cs`, `TextBox.cs`,
  `ErrorBanner.cs`, `LinkButton.cs`, `FactionButton.cs`, `BotaoCta.cs` —
  escalas/medidas da spec de legibilidade
- `game/code/src/Fronda.Client/Scenes/LoginScene.cs` — re-layout Organizar()
  (§2.2), subtítulo, rodapé, placeholders, mensagens de erro novas (§7)
- `game/code/src/Fronda.Client/Scenes/BemVindoScene.cs` — texto em escala 3
- `.gitignore` — criado na raiz: ignora `repro-fonte/` (projeto temporário de
  reprodução do bug da fonte, contém apenas `obj/` de build)

Pendências:

- [ ] `BemVindoScene` não foi validada ao vivo (exigiria criar uma conta real);
      o código segue o §6.1 da spec.
- [ ] Validar visualmente no monitor do usuário: rodar
      `dotnet run --project game/code/src/Fronda.Client` e confirmar que a
      tela de login mostra **todos** os textos.
- [ ] Decidir o destino da pasta `repro-fonte/` (hoje ignorada pelo Git):
      apagar quando não for mais útil ou guardar como teste de reprodução.

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
