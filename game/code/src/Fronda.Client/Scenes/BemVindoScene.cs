using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Fronda.Auth;
using Fronda.Core;
using Fronda.Rendering;

namespace Fronda.Scenes;

/// <summary>
/// Tela de pós-login/cadastro (spec §6.1, leva 3): fundo noite + "…conectado
/// ao mundo" em marfim + filete Ouro central — SEM cor de facção (a próxima
/// tela decide o clima do lado). Mostra o jogador autenticado (usuario +
/// lado, contrato Jogador da leva 2) e a nota "próxima tela do jogo".
/// Entra com dissolução discreta em 4 degraus (~200ms a 60fps); nada aqui é
/// interativo. Não é placeholder: substitui a mensagem estática da leva 1.
/// </summary>
public sealed class BemVindoScene : Scene
{
    private const int FramesPorDegrau = 3; // 4 degraus × 3 frames ≈ 200ms @60fps
    private const int Degraus = 4;

    private readonly Jogador _jogador;

    /// <summary>Estado de entrada: degrau atual da dissolução (0..4) e contador.</summary>
    private int _degrau = 1;
    private int _framesNoDegrau;

    public BemVindoScene(GameApp jogo, Jogador jogador) : base(jogo)
        => _jogador = jogador;

    public override void Atualizar(
        GameTime tempo, KeyboardState teclado, KeyboardState tecladoAntigo,
        MouseState mouse, MouseState mouseAntigo)
    {
        // cena sem interação: só a entrada em degraus acontece
        if (_degrau < Degraus && ++_framesNoDegrau >= FramesPorDegrau)
        {
            _framesNoDegrau = 0;
            _degrau++;
        }
    }

    public override void Desenhar(SpriteBatch lote, ShapeBatch formas)
    {
        var fonte = Jogo.Fonte;

        // tudo dissolve da noite: as cores misturam para o Fundo em degraus
        Color capa(Color cor) => Palette.DissolverPara(cor, Palette.Fundo, _degrau);

        var titulo = "...conectado ao mundo"; // três glifos "." (§6.1: "…" é só leva 3)
        fonte.Desenhar(lote, titulo, new Point(480 - fonte.LarguraTexto(titulo, 3) / 2, 216),
            capa(Palette.Texto), 3); // escala 3: 21px de altura, largura 378px

        // filete Ouro 1px × 120px central (§6.1)
        formas.Filete(420, 246, 120, 1, capa(Palette.Ouro));

        // jogador autenticado (contrato da leva 2: Id, Usuario normalizado, Lado)
        var usuario = $"jogador {_jogador.Usuario}";
        var u = fonte.MedirTexto(usuario, 2);
        fonte.Desenhar(lote, usuario, new Point(480 - u.X / 2, 286), capa(Palette.Texto), 2);

        var lado = $"lado {_jogador.Lado.ToString().ToLowerInvariant()}";
        var l = fonte.MedirTexto(lado, 2);
        fonte.Desenhar(lote, lado, new Point(480 - l.X / 2, 312), capa(Palette.TextoMudo), 2);

        var nota = "próxima tela do jogo";
        var n = fonte.MedirTexto(nota, 2);
        fonte.Desenhar(lote, nota, new Point(480 - n.X / 2, 356), capa(Palette.TextoMudo), 2);
    }
}
