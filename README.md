# API de CRUD de Produtos

Esta é uma API com Tests desenvolvida em C# utilizando ASP.NET Core para realizar operações CRUD (Create, Read, Update, Delete) em uma entidade de Produto. Ela utiliza Entity Framework Core como ORM (Object-Relational Mapping) para interagir com o banco de dados SQL Server.

## Funcionalidades

- Criar um novo produto
- Obter todos os produtos, com opção de ordenação por diferentes campos da entidade, ascendente ou descendente
- Obter um produto por ID
- Obter uma lista de produtos por nome
- Atualizar um produto existente
- Excluir um produto

## Estrutura do Projeto

O projeto está estruturado da seguinte forma:

- **Controllers:** Contém os controladores da API para lidar com as requisições HTTP.
- **Data:** Contém o contexto do banco de dados, dtos, profiles, models e repository.
- **Models:** Contém as definições das entidades do domínio, como a classe Product.
- **Repository:** Contém as implementações das interfaces de repositório para acesso aos dados.

Além disso, a utilização de DTOs e Profiles permite uma comunicação eficiente entre as diferentes camadas da aplicação, garantindo a segurança e a integridade dos dados.

## Configuração do Banco de Dados

Este projeto utiliza o padrão **Code-First** para a criação do banco de dados. Isso significa que a estrutura do banco de dados é definida por meio das classes de entidade (como a classe Product), e o Entity Framework cria automaticamente o esquema do banco de dados com base nessas classes.

## Configuração e Execução

Para executar os comandos abaixo utilize o **Package Manager Console** (com visual studio: View->Other Windows->Package Manager Console)

1. Certifique-se de ter o .NET Core SDK instalado na sua máquina.
2. Configure a string de conexão com o banco de dados no arquivo `appsettings.Development.json` em **ProductConnection**.
3. Execute o camando `Add-Migration ProductMigration`, para criar a migrations da entidade Product.
4. Execute o camando `Update-Database`, para criar a tabela referente a entidade.
5. Execute o projeto para iniciar a aplicação. A API estará acessível em `https://localhost:7150`.

## Testes

O projeto contém testes unitários e de integração para garantir a qualidade do código e a funcionalidade da API. Os testes podem ser executados utilizando o comando `dotnet test`.

## Tecnologias Utilizadas

- .NET 6
- ASP.NET Core
- Entity Framework Core
- SQL Server
- xUnit (para testes)

## Experiência Adquirida 

Para o desenvolvimento desse projeto o maior desafio foi desenvolver os testes unitarios e de integração, divertido quando fazemos algo que gostamos.😄
