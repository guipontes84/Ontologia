# AGENTS.md

Instruções para qualquer agente de IA (Codex, Claude ou outro) que trabalhe neste repositório. Leia também [docs/SDD.md](docs/SDD.md) para o desenho e [docs/VISAO.md](docs/VISAO.md) para o objetivo em linguagem simples.

## Projeto

Vocabulário de domínio em C#: conceitos, sinônimos e conexões, com busca de conceitos em textos. Solution `Ontologia.sln`, projeto `Ontologia.csproj`, em .NET 10, sem dependências externas.

Arquivos principais:
- `Vocabulario.cs`: classe principal.
- `Program.cs`: exemplo executável.
- `README.md`: uso, exemplos, benefícios e limitações.

## Comandos

```powershell
dotnet build Ontologia.sln
dotnet run --project .\Ontologia.csproj
```

Saída esperada do `dotnet run`: a seção 1 resolve "comprador" para "Cliente"; a seção 2 lista as conexões de "Pedido"; a seção 3 mostra os conceitos das duas perguntas. Se a saída mudar, atualize o README junto.

## Regras

- **Não publique nada externamente.** Não crie repositórios, não faça push e não publique posts sem confirmação explícita do usuário.
- **Não invente fatos.** Nomes de recursos, números e exemplos precisam ser verdadeiros para o código atual.
- **Textos em português do Brasil**, no tom do README e dos posts já existentes.
- **Mantenha o código sem dependências externas**, salvo decisão explícita do usuário.
- **Não versione `bin/` nem `obj/`.** Eles estão no `.gitignore`.

## Limitações conhecidas (não "corrija" sem pedido)

- A busca de conceitos é palavra por palavra. Termos compostos não são encontrados.
- Plurais precisam ser cadastrados como sinônimos.
- O vocabulário é montado no código; não há carregamento de arquivo.

## Convenções

- Código em C#. Nomes técnicos em inglês quando forem padrão do .NET; nomes do domínio em português (`Vocabulario`, `Conceito`, `Sinonimos`).
- Mensagens ao usuário em português.
- Antes de dizer que algo funciona, rode a build e o exemplo.

## Documentação padrão

| Arquivo | Público | Conteúdo |
|---|---|---|
| `README.md` | Quem vai usar | Uso, exemplos, benefícios e limitações. |
| `docs/SDD.md` | Quem vai desenvolver | Escopo, arquitetura, decisões e riscos. |
| `docs/VISAO.md` | Qualquer pessoa | Objetivo e resultados, sem termos técnicos. |
| `AGENTS.md` / `CLAUDE.md` | Agentes de IA | Comandos, regras e convenções. |

Ao mudar o comportamento do projeto, atualize o README e o SDD na mesma tarefa.
