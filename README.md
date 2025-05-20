# APICatalago

## Visão Geral
API RESTful para gerenciamento de produtos e categorias, utilizando .NET 8, Entity Framework Core e MySQL. O projeto segue boas práticas de arquitetura, como uso de Unit of Work, Repository Pattern, AutoMapper e filtros globais para tratamento de exceções e logging customizado.

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
1. Configure a string de conexão no `appsettings.json`.
2. Execute as migrações do banco de dados (caso necessário).
3. Rode o projeto com `dotnet run`.
4. Acesse a documentação Swagger em `/swagger`.

## Funcionalidades Principais
- CRUD de Produtos e Categorias
- Paginação de produtos
- Filtros globais para exceções e logging
- Logging customizado em arquivo
- Mapeamento DTO <-> Entidade
- Validação de modelos

## Pontos de Atenção / Limitações
- O caminho do arquivo de log está fixo em `D:\Macoratti\APICatalago\logs\log.txt`.
- O logger customizado só grava logs do nível configurado (não grava níveis superiores).
- Não há autenticação/autorização implementada.
- O tratamento de erros pode ser expandido para cenários mais complexos.

## Sugestões de Melhorias Futuras
- Tornar o caminho do log configurável via appsettings.
- Permitir logging para níveis superiores ao configurado (ex: LogLevel.Warning grava Error/Fatal).
- Implementar autenticação e autorização (JWT, Identity, etc).
- Adicionar testes automatizados (unitários e integração).
- Implementar versionamento de API.
- Melhorar a documentação dos endpoints no Swagger.
- Adicionar cache para consultas frequentes.
- Internacionalização das mensagens de erro.

---

Para dúvidas ou sugestões, consulte os comentários no código ou abra uma issue.
