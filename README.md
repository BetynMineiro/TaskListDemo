# TaskListDemo

**TaskListDemo** é uma aplicação em .NET projetada para gerenciar listas de tarefas. Este projeto utiliza uma arquitetura modular e boas práticas para garantir escalabilidade, manutenção e organização. Além disso, a documentação da API é disponibilizada utilizando o **Scalar.NET**, uma alternativa moderna ao Swagger.

---

## Estrutura do Projeto

O projeto é organizado nas seguintes camadas:

- **Task.Api**: Camada responsável por expor os endpoints da API.
- **Task.Application**: Implementa a lógica de aplicação, como casos de uso e serviços.
- **Task.CrossCutting**: Contém componentes transversais, como utilitários e configurações.
- **Task.Domain**: Define as entidades de domínio e interfaces para repositórios.
- **Task.MongoDbAdapter**: Implementa a persistência de dados usando MongoDB.

---

## Tecnologias Utilizadas

- **Linguagem**: C#
- **Banco de Dados**: MongoDB
- **Documentação da API**: Scalar.NET
- **Testes**: xUnit, FluentAssertions
- **Containerização**: Docker

---

## Uso do Scalar.NET

O **Scalar.NET** é utilizado neste projeto como ferramenta de documentação e interação com a API, substituindo o Swagger. Ele fornece uma interface mais moderna e intuitiva para explorar e testar os endpoints.

### Benefícios do Scalar.NET

- **Interface Moderna**: Visual mais limpo e intuitivo para desenvolvedores.
- **Facilidade de Testes**: Permite testar os endpoints diretamente pela interface.
- **Customização**: Personalizável para atender às necessidades do projeto.

### Acessando o Scalar.NET

1. Após iniciar a aplicação (veja a seção [Como Executar o Projeto](#como-executar-o-projeto)):
   - A interface do Scalar.NET estará disponível em:
     ```plaintext
     http://localhost:5000/docs
     ```
     (ou a porta configurada no ambiente).

2. Na interface, você poderá:
   - Visualizar todos os endpoints disponíveis.
   - Testar endpoints diretamente na interface.
   - Consultar detalhes de cada endpoint, como parâmetros e respostas.

### Configuração no Projeto

O Scalar.NET está configurado no arquivo `Program.cs`:

1. **Adicionando o Scalar.NET como Serviço**:
   ```csharp
   builder.Services.AddScalar();
