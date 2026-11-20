using Ontologia;

// Exemplo: um vocabulário pequeno de vendas.
var vocabulario = new Vocabulario()
    .Adicionar("Cliente", "comprador", "tomador", "consumidor")
    .Adicionar("Pedido", "compra", "ordem", "pedidos")
    .Adicionar("Produto", "item", "mercadoria", "itens")
    .Conectar("Cliente", "faz", "Pedido")
    .Conectar("Pedido", "contém", "Produto");

Console.WriteLine("1) Sinônimos resolvidos para o conceito");
foreach (var termo in new[] { "comprador", "Tomador", "mercadoria", "pedido" })
    Console.WriteLine($"   {termo} -> {vocabulario.Resolver(termo)}");

Console.WriteLine("\n2) Conexões de um conceito");
foreach (var (direcao, relacao, outro) in vocabulario.Relacionados("Pedido"))
    Console.WriteLine($"   {direcao} {relacao} {outro}");

Console.WriteLine("\n3) Conceitos citados em perguntas");
foreach (var pergunta in new[]
{
    "quais pedidos do comprador?",
    "o item da compra ainda está em estoque?",
})
    Console.WriteLine($"   \"{pergunta}\" -> {string.Join(", ", vocabulario.ConceitosNoTexto(pergunta))}");
