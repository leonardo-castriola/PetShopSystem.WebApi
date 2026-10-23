# 🐾 PetShop Management API

API RESTful desenvolvida em **.NET 10** para gerenciar pets e calcular automaticamente o valor do banho. O projeto usa ASP.NET Core Web API, Entity Framework Core, injeção de dependência e banco de dados SQLite.

---

## 🚀 Tecnologias

- **C# / .NET 10** — plataforma de desenvolvimento.
- **ASP.NET Core Web API** — construção da API REST e configuração do pipeline HTTP.
- **Entity Framework Core 10** — acesso a dados e gerenciamento de migrations.
- **SQLite** — banco de dados relacional local (`PetShop.db`).
- **Swagger / Swashbuckle** — documentação interativa disponível em ambiente de desenvolvimento.
- **xUnit** e **Microsoft.NET.Test.Sdk** — criação e execução dos testes automatizados.
- **Entity Framework Core InMemory** — banco em memória isolado para os testes, sem depender do SQLite local.
- **Coverlet** — coleta de dados de cobertura de código.

---

## ⚙️ Como executar

1. Abra `PetShopSystem.WebApi.slnx` no Visual Studio.
2. Execute o projeto com **F5** ou escolha o perfil `https` ou `http`.
3. A aplicação aplica automaticamente as migrations pendentes na inicialização.
4. No ambiente de desenvolvimento, acesse `/swagger` para explorar e chamar os endpoints.

Se preferir aplicar a migration manualmente pelo Console do Gerenciador de Pacotes do NuGet, selecione o projeto `PetShopSystem.WebApi` e execute:

```powershell
Update-Database
```

A string de conexão padrão está em `PetShopSystem.WebApi/appsettings.json` e cria o arquivo `PetShop.db`.

---

## 📌 Endpoints

A rota base é `/api/pet`.

| Método | Endpoint | Descrição | Respostas principais |
|:---:|:---|:---|:---:|
| `POST` | `/api/pet` | Cadastra um pet | `201 Created` / `400 Bad Request` |
| `GET` | `/api/pet` | Lista os pets cadastrados | `200 OK` |
| `GET` | `/api/pet/{id}` | Busca um pet pelo ID | `200 OK` / `400 Bad Request` / `404 Not Found` |
| `PUT` | `/api/pet/{id}` | Atualiza os dados de um pet | `200 OK` / `400 Bad Request` / `404 Not Found` |
| `DELETE` | `/api/pet/{id}` | Remove um pet | `204 No Content` / `400 Bad Request` / `404 Not Found` |
| `GET` | `/api/pet/{id}/calcular-banho` | Calcula o valor do banho | `200 OK` / `400 Bad Request` / `404 Not Found` |

### Dados do pet

O cadastro e a atualização recebem um JSON com nome, peso em quilogramas e raça. O ID é gerado pelo sistema.

```json
{
  "nome": "Mel",
  "peso": 8.5,
  "raca": 1
}
```

O nome deve ter entre 1 e 100 caracteres, o peso deve ser maior que zero e não ultrapassar 1000 kg, e a raça deve corresponder a um valor válido do enum `RacaPet`.

---

## 🧪 Como executar os testes

O projeto de testes `PetShopSystem.Tests` contém testes unitários para o cálculo do banho, usando um banco em memória isolado por teste. Na raiz do repositório, execute:

```bash
dotnet test PetShopSystem.slnx
```

Para executar somente o projeto de testes:

```bash
dotnet test PetShopSystem.Tests/PetShopSystem.Tests.csproj
```

No Visual Studio, abra a solução e use **Testar > Executar Todos os Testes**. Na primeira execução, o .NET restaura os pacotes NuGet; é necessário ter acesso ao feed configurado.

---

## 💡 Regra de cálculo do banho

O serviço `BanhoService` calcula o preço pela faixa de peso e, quando aplicável, soma o adicional da raça:

| Peso | Valor base |
|:---|---:|
| Até **10 kg** | `R$ 40,00` |
| Acima de **10 kg** até **25 kg** | `R$ 60,00` |
| Acima de **25 kg** | `R$ 80,00` |

Valores de `RacaPet`:

- `0` — Outra
- `1` — Poodle (**adicional de R$ 10,00**)
- `2` — Vira-Lata
- `3` — Golden Retriever

O endpoint de cálculo retorna o ID do pet e o valor calculado, por exemplo:

```json
{
  "petId": 1,
  "valorBanho": 50.00
}
```
