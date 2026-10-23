# 🐾 PetShop Management API

API RESTful desenvolvida em **.NET** para o gerenciamento de Pets e cálculo automatizado de serviços de banho, aplicando boas práticas de arquitetura em camadas, Injeção de Dependência (DI) e persistência de dados relacional.

---

## 🚀 Tecnologias Utilizadas

* **C# / .NET 10** — Plataforma principal de desenvolvimento.
* **ASP.NET Core Web API** — Construção dos endpoints RESTful e configuração de middlewares.
* **Entity Framework Core (EF Core)** — ORM para mapeamento objeto-relacional e controle de *Migrations*.
* **SQLite** — Banco de dados relacional leve e embarcado (`PetShop.db`).
* **Swagger (Swashbuckle)** — Documentação interativa e testes dos endpoints da API.

---

## ⚙️ Como Rodar o Projeto

1. **Abra a solução no Visual Studio:**
   * Abra o arquivo `PetShopSystem.WebApi.slnx` (ou a pasta do projeto) no Visual Studio.
2. **Aplique as Migrations (opcional via Console):**
   * No menu superior, acesse **Ferramentas > Gerenciador de Pacotes do NuGet > Console do Gerenciador de Pacotes** e execute:
     ```powershell
     Update-Database
     ```
   * *(Nota: A aplicação também já está configurada para aplicar as migrations pendentes automaticamente ao iniciar).*
3. **Execute a aplicação:**
   * Pressione **F5** (ou clique no botão **https / http** no topo do Visual Studio).
   * O navegador abrirá automaticamente na interface interativa do **Swagger** (`/swagger/index.html`).

---

## 📌 Principais Endpoints

A rota base do controlador é `/api/pet`:

| Método HTTP | Endpoint | Descrição | Retorno Principal |
| :---: | :--- | :--- | :---: |
| `POST` | `/api/pet` | Cadastra um novo Pet no banco de dados | `201 Created` |
| `GET` | `/api/pet` | Lista todos os Pets cadastrados | `200 OK` |
| `GET` | `/api/pet/{id}` | Busca os dados de um Pet específico pelo ID | `200 OK` / `404 Not Found` |
| `PUT` | `/api/pet/{id}` | Atualiza os dados de um Pet existente | `200 OK` / `404 Not Found` |
| `DELETE` | `/api/pet/{id}` | Remove um Pet do sistema pelo ID | `204 No Content` |
| `GET` | `/api/pet/{id}/calcular-banho` | Calcula o valor do banho com base no peso e raça do Pet | `200 OK` / `404 Not Found` |

---

## 💡 Regra de Negócio — Cálculo do Banho

O serviço `BanhoService` calcula o valor do banho seguindo as regras abaixo:

* **Por Faixa de Peso:**
  * Até **10 kg**: `R$ 40,00`
  * Acima de **10 kg** até **25 kg**: `R$ 60,00`
  * Acima de **25 kg**: `R$ 80,00`
* **Adicional por Raça (`RacaPet`):**
  * `0` = Outra | `1` = Poodle | `2` = Vira-Lata | `3` = Golden Retriever
  * Caso a raça seja **Poodle (`1`)**, é acrescido um valor adicional de **`R$ 10,00`**.
