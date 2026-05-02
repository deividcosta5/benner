# Digital Microwave 🔥

Simulador de micro-ondas digital desenvolvido em ASP.NET Core com interface Web, atendendo aos requisitos de Orientação a Objetos, separação de camadas, programas pré-definidos e customizados com persistência em JSON, e Web API com autenticação Bearer Token.

## Tecnologias

- **Linguagem:** C# (.NET 8)
- **Framework Web:** ASP.NET Core MVC
- **Tempo Real:** SignalR
- **API:** ASP.NET Core Web API + JWT Bearer Token
- **Persistência:** JSON (programas customizados)
- **Testes:** xUnit + Moq
- **Frontend:** Bootstrap 5

## Estrutura do Projeto

```
DigitalMicrowave/
├── DigitalMicrowave.Business/     # Camada de negócio (modelos, serviços, validadores)
├── DigitalMicrowave.Web/          # Interface Web MVC com SignalR
├── DigitalMicrowave.Api/          # Web API com autenticação JWT (Nível 4)
└── DigitalMicrowave.Tests/        # Testes unitários xUnit
```

## Níveis implementados

| Nível | Descrição | Status |
|-------|-----------|--------|
| 1 | Interface Web, tempo/potência, início rápido, string de aquecimento, pausa/cancelar | ✅ |
| 2 | 5 Programas pré-definidos (Pipoca, Leite, Carne, Frango, Feijão) | ✅ |
| 3 | Cadastro de programas customizados com persistência em JSON | ✅ |
| 4 | Web API com Bearer Token, exception handling, logging | ✅ |

## Como instalar e usar

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download)

### Executar a Interface Web

```bash
cd DigitalMicrowave.Web
dotnet run
# Acesse: https://localhost:5001
```

### Executar a API (Nível 4)

```bash
cd DigitalMicrowave.Api
dotnet run
# Swagger UI: https://localhost:5003/swagger
```

**Credenciais da API:**
- Usuário: `admin`
- Senha: `admin123`

```bash
# 1. Obter token
POST /api/auth/login
{ "username": "admin", "password": "admin123" }

# 2. Usar o token nas demais requisições
Authorization: Bearer <token>
```

### Executar os testes

```bash
cd DigitalMicrowave.Tests
dotnet test
```

## Regras de negócio principais

- **Tempo:** 1 a 120 segundos (2 minutos). Tempos ≥ 60s são exibidos como MM:SS
- **Potência:** 1 a 10 (padrão: 10 quando não informada)
- **Início rápido:** sem tempo/potência → 30 segundos, potência 10
- **+30s:** pressionar iniciar com aquecimento em andamento adiciona 30 segundos
- **String de aquecimento:** potência define quantos caracteres por segundo
- **Pausa/Cancelar:** 1º clique pausa; 2º clique cancela e limpa
- **Programas pré-definidos:** não podem ser editados, excluídos ou ter tempo adicionado
- **Caractere customizado:** não pode ser `'.'` e não pode repetir entre programas

---

> This is a challenge by [Coodesh](https://coodesh.com/)
