using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Fronda.Auth;
using Fronda.Auth.Armazenamento;
using Fronda.Auth.Hash;
using Fronda.Core;
using Fronda.Font;
using Fronda.Rendering;
using Fronda.UI;

namespace Fronda.Scenes;

/// <summary>
/// Tela de login/cadastro conforme game/code/docs/design-tela-login.md:
/// painel 360×480 centrado @ (300,30), fundo noite com silhueta do horizonte,
/// CTA primário lado oposição, CTA desligado até escolher lado, selo de erro
/// como placa sólida Erro. Estados: entrar ↔ cadastrar; submit chama
/// IAuthService (AuthService real da leva 2 — back-end local em JSON).
/// Leva 3: painel/títulos dissolvem na entrada da cena, caret piscando,
/// nudge nos lados, guardas de erro com trem; êxito troca para BemVindoScene.
/// </summary>
public sealed class LoginScene : Scene
{
    private enum Modo
    {
        Entrar,
        Cadastrar,
    }

    // -------- widgets da tela (posições em Organizar)
    private readonly Panel _painel = new();
    private readonly BotaoTab _tabEntrar = new() { Texto = "entrar" };
    private readonly BotaoTab _tabCadastrar = new() { Texto = "cadastrar" };
    private readonly Label _lblUsuario = new() { Texto = "usuário" };
    private readonly Label _lblSenha = new() { Texto = "senha" };
    private readonly Label _lblConfirma = new() { Texto = "confirmar senha" };
    private readonly Label _lblLados = new() { Texto = "escolha seu lado", Cor = Palette.TextoMudo };
    private readonly TextBox _campoUsuario = new() { Campo = "usuario", Placeholder = "nome do jogador", LimiteCaracteres = 20 };
    private readonly TextBox _campoSenha = new() { Campo = "senha", Placeholder = "senha", MascaraSenha = true };
    private readonly TextBox _campoConfirma = new() { Campo = "confirmaSenha", Placeholder = "repita a senha", MascaraSenha = true };
    private readonly ToggleGroup _grupoLados = new();
    private readonly BotaoCta _cta = new();
    private readonly LinkButton _link = new();
    private readonly ErrorBanner _banner = new();

    // -------- estado da cena
    private Modo _modo = Modo.Entrar;
    private readonly IAuthService _auth =
        new AuthService(new Pbkdf2Hasher(), new JsonContasRepository());
    private Task<Resultado>? _tarefa;
    private bool _carregando;

    // -------- transições pixel-art (leva 3): dissolução discreta em degraus
    private const int FramesPorDegrau = 3; // 4 degraus × 3 frames ≈ 200ms @60fps
    private const int Degraus = 4;
    private const int FramesPulso = 40; // pulso do filete do título: 2 degraus

    /// <summary>Degrau atual da dissolução da cena (0 = noite, 4 = cor inteira).</summary>
    private int _degrau = 1;
    private int _framesNoDegrau;
    private int _framesCena; // relógio de frames da cena (pulso do título)
    private bool _saindo; // êxito: dissolve para a noite e troca de cena
    private Jogador? _jogador; // contrato da leva 2: { Id, Usuario, Lado }

    private string? _erroCampo;
    private Widget? _foco;

    public LoginScene(GameApp jogo) : base(jogo)
    {
        _campoUsuario.FiltroCaractere = c => "abcdefghijklmnopqrstuvwxyz0123456789_-".Contains(c);
        _campoSenha.LimiteCaracteres = 32;
        _campoConfirma.LimiteCaracteres = 32;

        // digitar limpa o erro daquele campo ("corrigido" some, spec §3.5)
        _campoUsuario.AoDigitar = () => { if (_erroCampo == "usuario") LimparErro(); };
        _campoSenha.AoDigitar = () => { if (_erroCampo == "senha") LimparErro(); };
        _campoConfirma.AoDigitar = () => { if (_erroCampo == "confirmaSenha") LimparErro(); };

        _tabEntrar.AoClicar = () => TrocarModo(Modo.Entrar);
        _tabCadastrar.AoClicar = () => TrocarModo(Modo.Cadastrar);
        _grupoLados.BotaoOposicao.AoClicar = () => _grupoLados.Selecionar(Lado.Oposicao);
        _grupoLados.BotaoReino.AoClicar = () => _grupoLados.Selecionar(Lado.Reino);
        _cta.AoClicar = Submeter;
        _link.AoClicar = () => TrocarModo(_modo == Modo.Entrar ? Modo.Cadastrar : Modo.Entrar);

        Focar(_campoUsuario);
    }

    // ================================================================ update

    public override void Atualizar(GameTime tempo, KeyboardState teclado, KeyboardState tecladoAntigo, MouseState mouse, MouseState mouseAntigo)
    {
        // êxito: dissolver para a noite e trocar para BemVindoScene (leva 3)
        if (_saindo)
        {
            DissolverWidgets(); // sem Organizar aqui: espalha o degrau manualmente
            if (AvancarDissolucao(-1)) Jogo.TrocarCena(new BemVindoScene(Jogo, _jogador!));
            return;
        }
        _framesCena++;

        // resultado do serviço termina no frame seguinte (evita corrida real de rede)
        ConsumirTarefa();

        // estados de desabilitação (loading etc.)
        bool cad = _modo == Modo.Cadastrar;
        _carregando = _tarefa is not null;
        _campoUsuario.Desabilitado = _carregando;
        _campoSenha.Desabilitado = _carregando;
        _campoConfirma.Desabilitado = _carregando;
        _tabEntrar.Desabilitado = _carregando;
        _tabCadastrar.Desabilitado = _carregando;
        _grupoLados.BotaoOposicao.Desabilitado = _carregando;
        _grupoLados.BotaoReino.Desabilitado = _carregando;
        _link.Desabilitado = _carregando;
        _cta.ModoCadastro = cad;
        _cta.Grupo = _grupoLados;
        _cta.Desabilitado = _carregando || (cad ? !_grupoLados.TemEscolha
            : _campoUsuario.Texto.Length == 0 || _campoSenha.Texto.Length == 0);
        _cta.Texto = _carregando ? "enviando…"
            : cad ? (_grupoLados.TemEscolha ? "criar conta" : "escolha seu lado")
            : "entrar";
        _link.Texto = cad ? "já tenho conta" : "criar conta";
        _tabEntrar.Ativa = _modo == Modo.Entrar;
        _tabCadastrar.Ativa = _modo == Modo.Cadastrar;

        // hover / pressão
        foreach (var w in Interativos())
            w.AtualizarEntrada(mouse, mouseAntigo);

        bool shift = teclado.IsKeyDown(Keys.LeftShift) || teclado.IsKeyDown(Keys.RightShift);

        foreach (var tecla in Entrada.TeclasNovas(teclado, tecladoAntigo))
        {
            if (tecla == Keys.Tab)
            {
                CorrerFoco(!shift);
                continue;
            }
            if (tecla == Keys.Escape)
            {
                if (cad) TrocarModo(Modo.Entrar); // Esc volta para entrar (spec §5)
                continue;
            }
            if (tecla == Keys.Enter)
            {
                if (!_cta.Desabilitado) Submeter();
                continue;
            }
            if ((tecla == Keys.Left || tecla == Keys.Right) && _foco is FactionButton fb)
            {
                // ←/→ alternam a seleção sem sair do grupo (spec §5)
                var outro = fb.Lado == Lado.Oposicao ? _grupoLados.BotaoReino : _grupoLados.BotaoOposicao;
                _grupoLados.Selecionar(outro.Lado);
                Focar(outro);
                continue;
            }

            char? c = Entrada.TeclaParaChar(tecla, shift);
            if (c is null)
            {
                _ = _foco?.ReceberTecla(tecla);
                continue;
            }
            // campo usuário força minúscula (a-z0-9_-)
            if (_foco == _campoUsuario) c = char.ToLowerInvariant(c.Value);
            _foco?.ReceberTexto(c.Value);
        }

        // clique de mouse dá foco ao widget sob o cursor (tabs mudam de modo)
        if (mouse.LeftButton == ButtonState.Pressed && mouseAntigo.LeftButton == ButtonState.Released)
        {
            var alvo = Interativos().FirstOrDefault(w => w is not BotaoTab && w.PodeFocar && w.ContemPonto(mouse.Position));
            if (alvo is not null) Focar(alvo);
        }

        Organizar();

        // entrada da cena: dissolve o painel/títulos em degraus (leva 3)
        AvancarDissolucao(+1);
    }

    /// <summary>
    /// Avança a dissolução discreta da cena (leva 3): +1 = surgir (0→4),
    /// -1 = sumir (4→0). Devolve true quando a saída chegou ao fim (0).
    /// </summary>
    private bool AvancarDissolucao(int direcao)
    {
        if (++_framesNoDegrau < FramesPorDegrau) return false;
        _framesNoDegrau = 0;
        if (direcao < 0)
        {
            if (_degrau > 0) _degrau--;
            return _degrau == 0;
        }
        if (_degrau < Degraus) _degrau++;
        return false;
    }

    /// <summary>Espalha o degrau da cena por todos os widgets visíveis (estado
    /// de animação é da instância — nada estático mutável).</summary>
    private void DissolverWidgets()
    {
        int d = _degrau;
        _painel.DegrauDissolucao = d;
        _tabEntrar.DegrauDissolucao = d;
        _tabCadastrar.DegrauDissolucao = d;
        _lblUsuario.DegrauDissolucao = d;
        _lblSenha.DegrauDissolucao = d;
        _lblConfirma.DegrauDissolucao = d;
        _lblLados.DegrauDissolucao = d;
        _campoUsuario.DegrauDissolucao = d;
        _campoSenha.DegrauDissolucao = d;
        _campoConfirma.DegrauDissolucao = d;
        _grupoLados.BotaoOposicao.DegrauDissolucao = d;
        _grupoLados.BotaoReino.DegrauDissolucao = d;
        _cta.DegrauDissolucao = d;
        _link.DegrauDissolucao = d;
        _banner.DegrauDissolucao = d;
    }

    private void ConsumirTarefa()
    {
        if (_tarefa is null || !_tarefa.IsCompleted) return;
        var t = _tarefa;
        _tarefa = null;
        Resultado? res = null;
        string? falha = null;
        try { res = t.Result; }
        catch (Exception) { falha = "erro inesperado - tente de novo"; }
        if (res is not null) ProcessarResultado(res);
        else MostrarErro(null, "erro_geral", falha ?? "erro inesperado");
    }

    // ================================================================ helpers de estado

    private void TrocarModo(Modo novo)
    {
        if (_modo == novo) return;
        _modo = novo;
        // resposta em curso do modo anterior é descartada com o form (stub da leva 1)
        _tarefa = null;
        _carregando = false;
        // troca de modo redefine o formulário (spec §2.3): campos, erros e lado
        _campoUsuario.Texto = "";
        _campoSenha.Texto = "";
        _campoConfirma.Texto = "";
        _grupoLados.Limpar();
        LimparErro();
        Focar(_campoUsuario);
    }

    private void Focar(Widget w)
    {
        if (_foco is not null) _foco.Focado = false;
        _foco = w;
        w.Focado = true;
    }

    private IEnumerable<Widget> Interativos()
    {
        yield return _tabEntrar;
        yield return _tabCadastrar;
        yield return _campoUsuario;
        yield return _campoSenha;
        if (_modo == Modo.Cadastrar)
        {
            yield return _campoConfirma;
            yield return _grupoLados.BotaoOposicao;
            yield return _grupoLados.BotaoReino;
        }
        yield return _cta;
        yield return _link;
    }

    private void CorrerFoco(bool avancar)
    {
        // tabs ficam fora da ordem de Tab (spec §5); seguem o fluxo clicável
        var focaveis = Interativos().Where(w => w is not BotaoTab && w.PodeFocar).ToList();
        if (focaveis.Count == 0) return;
        int idx = focaveis.IndexOf(_foco!);
        int passo = avancar ? 1 : -1;
        idx = ((idx + passo) % focaveis.Count + focaveis.Count) % focaveis.Count;
        Focar(focaveis[idx]);
    }

    private void MostrarErro(string? campo, string codigo, string mensagem)
    {
        _erroCampo = campo;
        _banner.Mostrar(mensagem);
    }

    private void LimparErro()
    {
        _erroCampo = null;
        _banner.Esconder();
    }

    // ================================================================ submit

    private void Submeter()
    {
        if (_carregando || _cta.Desabilitado) return;
        LimparErro();

        bool cad = _modo == Modo.Cadastrar;
        string usuario = _campoUsuario.Texto;
        string senha = _campoSenha.Texto;
        string confirma = _campoConfirma.Texto;

        // validações locais (mesmas regras que o back-end aplica server-side)
        if (usuario.Length < 3)
        {
            MostrarErro("usuario", "usuario_curto", "nome curto - 3 letras ou mais");
            return;
        }
        if (usuario.Length > 20)
        {
            MostrarErro("usuario", "usuario_longo", "nome longo - até 20 letras");
            return;
        }
        if (senha.Length < 8)
        {
            MostrarErro("senha", "senha_curta", "senha curta - mínimo 8");
            return;
        }
        if (cad)
        {
            if (confirma != senha)
            {
                MostrarErro("confirmaSenha", "senha_diferente", "as senhas não coincidem");
                return;
            }
            // lado obrigatório: se não há escolha, o CTA está desligado (§3.6)
        }

        _tarefa = cad
            ? _auth.Registrar(usuario, senha, confirma, _grupoLados.Selecionado!.Value)
            : _auth.Entrar(usuario, senha);
    }

    private void ProcessarResultado(Resultado res)
    {
        if (res.Sucesso)
        {
            // contrato da leva 2: Sucesso → Jogador { Id, Usuario, Lado }
            _jogador = res.Jogador ?? new Jogador { Id = 0, Usuario = "?", Lado = Lado.Reino };
            _saindo = true; // dissolve para a noite e troca para BemVindoScene (leva 3)
        }
        else
        {
            MostrarErro(res.ErroCampo, res.ErroCodigo ?? "erro_geral", res.ErroMensagem ?? "erro inesperado");
        }
    }

    // ================================================================ layout

    private void Organizar()
    {
        bool cad = _modo == Modo.Cadastrar;
        DissolverWidgets();

        _painel.Limites = new Rectangle(300, 30, 360, 480);
        _tabEntrar.Limites = new Rectangle(324, 100, 152, 24);
        _tabCadastrar.Limites = new Rectangle(484, 100, 152, 24);

        _campoUsuario.Visivel = true;
        _campoSenha.Visivel = true;
        _campoConfirma.Visivel = cad;
        _lblConfirma.Visivel = cad;
        _lblLados.Visivel = cad;
        _grupoLados.BotaoOposicao.Visivel = cad;
        _grupoLados.BotaoReino.Visivel = cad;

        int y = 134;
        Campo(_lblUsuario, _campoUsuario, ref y);
        Campo(_lblSenha, _campoSenha, ref y);
        if (cad) Campo(_lblConfirma, _campoConfirma, ref y);

        if (cad)
        {
            _lblLados.Limites = new Rectangle(324, y, 312, 14);
            y += 18;
            _grupoLados.BotaoOposicao.Limites = new Rectangle(324, y, 152, 56);
            _grupoLados.BotaoReino.Limites = new Rectangle(484, y, 152, 56);
            y += 56;
            BannerPorCampo("lado", ref y);
        }

        // CTA (modo entrar usa gap 24; cadastrar comprime por causa do painel)
        _cta.Limites = new Rectangle(324, Math.Min(y + (cad ? 6 : 24), 448), 312, 48);
        y = _cta.Limites.Bottom;
        BannerPorCampo(null, ref y); // erro de back-end (sem campo): sob o CTA

        _link.Limites = new Rectangle(324, Math.Min(y + 4, 490), 312, 18);
    }

    private void Campo(Label lbl, TextBox box, ref int y)
    {
        lbl.Limites = new Rectangle(324, y, 312, 14);
        box.Limites = new Rectangle(324, y + 18, 312, 40);
        box.DefinirErro(_erroCampo == box.Campo); // dispara o trem de 1px ao ligar (leva 3)
        y += 58;
        BannerPorCampo(box.Campo, ref y);
        if (_erroCampo != box.Campo) y += 10;
    }

    /// <summary>Se este é o campo com erro, posiciona o selo 4px abaixo dele e reserva o espaço.</summary>
    private void BannerPorCampo(string? campo, ref int y)
    {
        if (_erroCampo == campo)
        {
            _banner.Limites = new Rectangle(324, y + 4, 312, 24);
            y += 32; // 4 de gap + 24 de selo + 4 de folga
        }
    }

    // ================================================================ draw

    public override void Desenhar(SpriteBatch lote, ShapeBatch formas)
    {
        var fonte = Jogo.Fonte;

        DesenharHorizonte(formas);
        _painel.Desenhar(lote, formas, fonte);
        DesenharTitulo(lote, formas, fonte);

        // divisor vertical entre as tabs (dissolve com a cena)
        formas.FileteVertical(480, 106, 12, 1,
            Palette.DissolverPara(Palette.Borda, Palette.Painel, _degrau));
        _tabEntrar.Desenhar(lote, formas, fonte);
        _tabCadastrar.Desenhar(lote, formas, fonte);

        foreach (var w in WidgetsDeTela())
            w.Desenhar(lote, formas, fonte);

        _banner.Desenhar(lote, formas, fonte);
    }

    private IEnumerable<Widget> WidgetsDeTela()
    {
        yield return _lblUsuario;
        yield return _lblSenha;
        if (_campoConfirma.Visivel) yield return _lblConfirma;
        yield return _campoUsuario;
        yield return _campoSenha;
        if (_campoConfirma.Visivel) yield return _campoConfirma;
        if (_lblLados.Visivel) yield return _lblLados;
        if (_grupoLados.BotaoOposicao.Visivel)
        {
            yield return _grupoLados.BotaoOposicao;
            yield return _grupoLados.BotaoReino;
        }
        yield return _cta;
        yield return _link;
    }

    /// <summary>Silhueta do horizonte (§2.2): telhados à esquerda, torre à direita.</summary>
    private void DesenharHorizonte(ShapeBatch formas)
    {
        // bloco esquerdo 200×72 @ (28,468): telhados apertados
        formas.RetanguloCheio(new Rectangle(28, 468, 200, 72), Palette.Painel);
        formas.RetanguloCheio(new Rectangle(70, 456, 60, 12), Palette.Painel);
        formas.RetanguloCheio(new Rectangle(148, 450, 46, 18), Palette.Painel);

        // bloco direito 120×120 @ (772,420): torre com filete de ouro a 4px do topo
        formas.RetanguloCheio(new Rectangle(772, 420, 120, 120), Palette.Painel);
        formas.Filete(772, 424, 120, 1, Palette.Ouro);
    }

    /// <summary>Título "a-fronda" 32px (escala 4) + fronda ouro 8×16 + filete ouro 1×120.
    /// Leva 3: título e logo dissolvem com a cena; o filete de ouro ganha um pulso
    /// sutil de 2 degraus (40 frames cheio / 40 atenuado, sem interpolação).</summary>
    private void DesenharTitulo(SpriteBatch lote, ShapeBatch formas, PixelFont fonte)
    {
        const string titulo = "a-fronda";
        int largura = fonte.LarguraTexto(titulo, 4);
        int x = 480 - largura / 2;
        var corTitulo = Palette.DissolverPara(Palette.Texto, Palette.Painel, _degrau);
        fonte.Desenhar(lote, titulo, new Point(x, 62), corTitulo, 4);

        // fronda 8×16 em ouro acoplada ao "a" final (detalhe do logo, §3.8)
        int fx = x + largura + 2;
        int fy = 62;
        DesenharFronda(formas, fx, fy);

        // filete ouro 1px × 120px centrado abaixo do título (y≈96) com pulso sutil:
        // 2 degraus (cheio / meio do pano), 40 frames cada, sem interpolação (leva 3)
        bool cheio = _framesCena / FramesPulso % 2 == 0;
        int degrauPulso = cheio ? Degraus : 2;
        int degrau = Math.Min(_degrau, degrauPulso);
        var corFilete = Palette.DissolverPara(Palette.Ouro, Palette.Painel, degrau);
        formas.Filete(420, 96, 120, 1, corFilete);
    }

    /// <summary>Fronda 8×16: caule 1px + três pares de folhas drooping (pixel puro).</summary>
    private void DesenharFronda(ShapeBatch formas, int x, int y)
    {
        var ouro = Palette.DissolverPara(Palette.Ouro, Palette.Painel, _degrau);
        formas.FileteVertical(x + 4, y + 2, 14, 1, ouro); // caule
        // pares de folhas (esquerda, direita) descendo para os lados
        formas.RetanguloCheio(new Rectangle(x + 2, y + 4, 2, 1), ouro);
        formas.RetanguloCheio(new Rectangle(x + 5, y + 5, 2, 1), ouro);
        formas.RetanguloCheio(new Rectangle(x + 1, y + 8, 3, 1), ouro);
        formas.RetanguloCheio(new Rectangle(x + 5, y + 9, 3, 1), ouro);
        formas.RetanguloCheio(new Rectangle(x, y + 12, 2, 1), ouro);
        formas.RetanguloCheio(new Rectangle(x + 6, y + 13, 2, 1), ouro);
    }
}
