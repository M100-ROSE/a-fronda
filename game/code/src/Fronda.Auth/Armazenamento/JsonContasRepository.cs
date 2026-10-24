using System.Text.Json;
using System.Text.Json.Serialization;

namespace Fronda.Auth.Armazenamento;

/// <summary>
/// Persistência das contas em um único arquivo JSON. Reflexões:
/// - caminho injetável por construtor (testes); padrão do app: %APPDATA%\Fronda\usuarios.json;
/// - diretório criado se não existir;
/// - gravação atômica: escreve “nome.tmp” no mesmo diretório e move por cima do final;
/// - leitura tolerante: arquivo ausente ou corrompido devolve lista vazia;
/// - thread-safe por lock de processo (um só jogo grava no arquivo).
/// </summary>
public sealed class JsonContasRepository : IContasRepository
{
    private static readonly JsonSerializerOptions Opcoes = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
        // Lado persiste como texto: "reino" | "oposicao" (mesmo mapeamento do wire futuro).
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower) },
    };

    private static string CaminhoPadrao() => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "Fronda", "usuarios.json");

    private readonly string _caminho;
    private readonly object _trava = new();

    public JsonContasRepository() : this(CaminhoPadrao()) { }

    /// <summary>Para testes e cenários alternativos: caminho explícito do arquivo.</summary>
    public JsonContasRepository(string caminho) => _caminho = caminho;

    public Conta? BuscarPorUsuario(string usuario)
    {
        lock (_trava)
        {
            return LerContas().FirstOrDefault(c => c.Usuario == usuario);
        }
    }

    public Conta Adicionar(Conta conta)
    {
        lock (_trava)
        {
            var contas = LerContas();
            conta.Id = contas.Count == 0 ? 1 : contas.Max(c => c.Id) + 1;
            contas.Add(conta);
            Gravar(contas);
            return conta;
        }
    }

    private List<Conta> LerContas()
    {
        if (!File.Exists(_caminho)) return [];
        try
        {
            return JsonSerializer.Deserialize<List<Conta>>(File.ReadAllText(_caminho), Opcoes) ?? [];
        }
        catch (JsonException)
        {
            return []; // arquivo corrompido: começa vazio (a próxima gravação recria)
        }
    }

    private void Gravar(List<Conta> contas)
    {
        var diretorio = Path.GetDirectoryName(_caminho);
        if (!string.IsNullOrEmpty(diretorio)) Directory.CreateDirectory(diretorio);

        var temporario = _caminho + ".tmp";
        try
        {
            File.WriteAllText(temporario, JsonSerializer.Serialize(contas, Opcoes));
            File.Move(temporario, _caminho, overwrite: true);
        }
        catch (IOException)
        {
            if (File.Exists(temporario)) File.Delete(temporario);
            throw;
        }
    }
}
