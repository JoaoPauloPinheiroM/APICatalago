# APICatalago

## Vis�o Geral
API RESTful para gerenciamento de produtos e categorias, utilizando .NET 8, Entity Framework Core e MySQL. O projeto segue boas pr�ticas de arquitetura, como uso de Unit of Work, Repository Pattern, AutoMapper e filtros globais para tratamento de exce��es e logging customizado.

## Estrutura de Pastas
APICatalago/
?
??? Controllers/
??? Data/                # Contexto do banco de dados
??? DTOs/
?   ??? Mappings/
??? Filters/
?   ??? Extensions/
??? Logging/
??? Models/
??? Pagination/
??? Repositories/
?   ??? Interfaces/
??? Services/
??? Extensions/          # (Opcional, se houver)
??? Program.cs
??? README.md
### Comando para criar as pastas (PowerShell)
mkdir Controllers, Data, DTOs\Mappings, Filters\Extensions, Logging, Models, Pagination, Repositories\Interfaces, Services, Extensions
## Tecnologias Utilizadas
- .NET 8
- Entity Framework Core
- MySQL
- AutoMapper
- Swagger (OpenAPI)
- Newtonsoft.Json

## Como Executar
1. Configure a string de conex�o no `appsettings.json`.
2. Execute as migra��es do banco de dados (caso necess�rio).
3. Rode o projeto com `dotnet run`.
4. Acesse a documenta��o Swagger em `/swagger`.

## Funcionalidades Principais
- CRUD de Produtos e Categorias
- Pagina��o de produtos
- Filtros globais para exce��es e logging
- Logging customizado em arquivo
- Mapeamento DTO <-> Entidade
- Valida��o de modelos

## Pontos de Aten��o / Limita��es
- O caminho do arquivo de log est� fixo em `D:\Macoratti\APICatalago\logs\log.txt`.
- O logger customizado s� grava logs do n�vel configurado (n�o grava n�veis superiores).
- N�o h� autentica��o/autoriza��o implementada.
- O tratamento de erros pode ser expandido para cen�rios mais complexos.

## Sugest�es de Melhorias Futuras
- Tornar o caminho do log configur�vel via appsettings.
- Permitir logging para n�veis superiores ao configurado (ex: LogLevel.Warning grava Error/Fatal).
- Implementar autentica��o e autoriza��o (JWT, Identity, etc).
- Adicionar testes automatizados (unit�rios e integra��o).
- Implementar versionamento de API.
- Melhorar a documenta��o dos endpoints no Swagger.
- Adicionar cache para consultas frequentes.
- Internacionaliza��o das mensagens de erro.

---

Para d�vidas ou sugest�es, consulte os coment�rios no c�digo ou abra uma issue.
