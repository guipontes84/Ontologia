using System.Globalization;
using System.Text;

namespace Ontologia;

/// <summary>
/// Vocabulário de um domínio: conceitos, sinônimos e conexões entre conceitos.
/// Sem dependências externas.
/// </summary>
public sealed class Vocabulario
{
    public record Conexao(string De, string Relacao, string Para);

    // termo normalizado -> conceito canônico (inclui o próprio nome e os sinônimos)
    private readonly Dictionary<string, string> _porTermo = new();
    // conceito canônico -> sinônimos cadastrados
    private readonly Dictionary<string, List<string>> _sinonimos = new();
    private readonly List<Conexao> _conexoes = new();

    /// <summary>Cadastra um conceito com seus sinônimos.</summary>
    public Vocabulario Adicionar(string conceito, params string[] sinonimos)
    {
        var canonico = conceito.Trim();
        _sinonimos[canonico] = sinonimos.Select(s => s.Trim()).ToList();
        _porTermo[Normalizar(canonico)] = canonico;
        foreach (var s in sinonimos)
            _porTermo[Normalizar(s)] = canonico;
        return this;
    }

    /// <summary>Cria uma conexão entre dois conceitos já cadastrados.</summary>
    public Vocabulario Conectar(string de, string relacao, string para)
    {
        _conexoes.Add(new Conexao(Canonico(de), relacao.Trim(), Canonico(para)));
        return this;
    }

    /// <summary>Devolve o conceito canônico para um nome ou sinônimo, ou null se não existir.</summary>
    public string? Resolver(string termo) =>
        _porTermo.TryGetValue(Normalizar(termo), out var canonico) ? canonico : null;

    public IReadOnlyList<string> Sinonimos(string conceito) =>
        _sinonimos.TryGetValue(Canonico(conceito), out var lista) ? lista : [];

    /// <summary>Conexões de um conceito: saindo dele (→) e chegando nele (←).</summary>
    public IEnumerable<(string Direcao, string Relacao, string Outro)> Relacionados(string conceito)
    {
        var c = Canonico(conceito);
        foreach (var x in _conexoes.Where(x => x.De == c))
            yield return ("→", x.Relacao, x.Para);
        foreach (var x in _conexoes.Where(x => x.Para == c))
            yield return ("←", x.Relacao, x.De);
    }

    /// <summary>Encontra os conceitos citados em um texto, palavra por palavra.</summary>
    public IReadOnlyList<string> ConceitosNoTexto(string texto)
    {
        var encontrados = new List<string>();
        foreach (var palavra in texto.Split(' ', StringSplitOptions.RemoveEmptyEntries))
        {
            var limpa = new string(palavra.Where(char.IsLetterOrDigit).ToArray());
            var conceito = Resolver(limpa);
            if (conceito is not null && !encontrados.Contains(conceito))
                encontrados.Add(conceito);
        }
        return encontrados;
    }

    private string Canonico(string termo) =>
        Resolver(termo) ?? throw new ArgumentException($"Conceito desconhecido: '{termo}'.");

    /// <summary>Minúsculas e sem acentos, para comparar "Comprador" com "comprador".</summary>
    public static string Normalizar(string texto)
    {
        var decomposto = texto.Trim().ToLowerInvariant().Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();
        foreach (var ch in decomposto)
            if (CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
                sb.Append(ch);
        return sb.ToString().Normalize(NormalizationForm.FormC);
    }
}
