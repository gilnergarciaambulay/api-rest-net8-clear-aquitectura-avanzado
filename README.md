# 🚀 MyApi – API RESTful en .NET 8 con Clean Architecture

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-Developer-blue?logo=csharp)
![Clean Architecture](https://img.shields.io/badge/Architecture-Clean-brightgreen)
![SQL Server](https://img.shields.io/badge/Database-SQL%20Server-red?logo=microsoftsqlserver)
![PostgreSQL](https://img.shields.io/badge/Database-PostgreSQL-blue?logo=postgresql)
![License](https://img.shields.io/badge/License-MIT-yellow)
![Status](https://img.shields.io/badge/Status-Active-success)

# MyApi

**MyApi** es una API RESTful desarrollada en **.NET 8**, diseñada siguiendo los principios de **Clean Architecture** para lograr un código modular, mantenible y escalable.  
La solución separa claramente las responsabilidades en capas independientes, promoviendo una arquitectura desacoplada y fácil de probar.

---

## 🏗️ Estructura del Proyecto

```bash
MyApi/
├── Core/              → 🧠 Dominio: entidades e interfaces base del negocio
├── Application/       → ⚙️ Aplicación: lógica de negocio y casos de uso
├── Infrastructure/    → 🧩 Infraestructura: persistencia y servicios externos
└── Presentation/      → 🌐 Presentación: controladores y endpoints HTTP
```

## 🏗️ Estructura del Proyecto: Detalle por cada capa
```bash
MyApi/
├── Core/                                ← 🧠 Capa de Dominio
│   ├── Entities/                        ← Entidades del dominio (modelos del negocio)
│   │   ├── User.cs                      ← Entidad del dominio
│   │   └── Contact.cs                   ← Entidad del dominio
│   └── Core.csproj                      ← Proyecto del dominio (sin dependencias)
│
├── Application/                         ← ⚙️ Capa de Aplicación
│   ├── DTOs/                            ← Objetos de transferencia (para entrada/salida)
│   │   ├── AuthResponseDTO.cs           ← DTO para respuestas de autenticación
│   │   ├── LoginRequestDTO.cs           ← DTO para solicitudes de login
│   │   └── ContactDTO.cs                ← DTO de Contact, usado por servicios/controladores
│   ├── Interfaces/                      ← Contratos de la capa de aplicación
│   │   ├── IRepository/                 ← Contratos de repositorios (NO implementaciones)
│   │   │   ├── IContactRepository.cs    ← Contrato de repositorio para Contact
│   │   │   └── IUserRepository.cs       ← Contrato de repositorio para User
│   │   ├── IService/                    ← Contratos de servicios de aplicación
│   │   │   ├── IAuthService.cs          ← Servicio de autenticación
│   │   │   └── IContactService.cs       ← Servicio para Contact
│   ├── Services/                        ← Implementación de la lógica de aplicación
│   │   ├── AuthService.cs               ← Implementación de IAuthService
│   │   └── ContactService.cs            ← Implementación de IContactService
│   ├── DependencyInjection.cs           ← Registro de servicios de la aplicación
│   └── Application.csproj               ← Proyecto dependiente de Core
│
├── Infrastructure/                      ← 🧩 Capa de Infraestructura
│   ├── Persistence/                     ← Acceso a datos y persistencia
│   │   ├── SqlServer/                   ← Implementaciones para SQL Server
│   │   │   ├── SqlServerConnectionFactory.cs ← Crea conexiones SQL
│   │   │   └── Repositories/            ← Repositorios concretos
│   │   │       └── PedidosRepository.cs ← Implementa IPedidosRepository
│   │   ├── DatabaseSettings.cs          ← Configuración de conexión
│   ├── DependencyInjection.cs           ← Registro de repositorios y persistencia
│   └── Infrastructure.csproj            ← Proyecto dependiente de Core y Application
│
├── WebApi/                              ← 🌐 Capa de Presentación
│   ├── Controllers/                     ← Endpoints HTTP
│   │   ├── ContactController.cs         ← Controlador para Contact
│   │   └── UserController.cs            ← Controlador para User/Auth
│   ├── Models/                          ← Modelos exclusivos de la API (si existen)
│   │   └── ApiResponse.cs               ← Modelo estándar de respuesta
│   ├── Middleware/                      ← Middlewares personalizados ASP.NET
│   │   ├── RequestLoggingMiddleware.cs  ← Middleware para log de peticiones
│   │   └── ErrorHandlingMiddleware.cs   ← Manejo global de errores
│   ├── appsettings.json                 ← Configuración de la API
│   ├── Program.cs                       ← Configuración principal y ejecución
│   └── WebApi.csproj                    ← Proyecto ejecutable
│
├── Utilities/                           ← 🧰 Helpers y utilidades compartidas
│   ├── ErrorUtilities.cs                ← Funciones auxiliares para manejo de errores
│   └── Utilities.csproj                 ← Proyecto utilitario
│
└── MyApi.sln                            ← 💼 Solución principal

```

### 🔁 Dependencias entre capas

- **WebApi** depende de **Application**
- **Application** depende de **Core**
- **Infrastructure** implementa interfaces de **Core** 

---

## 🚀 Características Principales

- Arquitectura basada en principios **SOLID**  
- Separación clara de responsabilidades  
- Uso de **Inyección de Dependencias (DI)**  
- Integración con **Entity Framework Core** y **MediatR**  
- Validación de entrada con **FluentValidation**  
- Manejo global de errores y respuestas consistentes  
- Documentación interactiva con **Swagger / OpenAPI**  

---

## ⚙️ Requisitos Previos

- [.NET SDK 8.0](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) u otro motor compatible
- [Visual Studio 2022](https://visualstudio.microsoft.com/) o [Visual Studio Code](https://code.visualstudio.com/)

---

## 🧩 Ejecución del Proyecto

1. Clonar el repositorio  
   ```bash
   git clone https://github.com/gilnergarciaambulay/api-rest-net8-clear-aquitectura.git
   cd api-rest-net8-clear-aquitectura
   ```
2. Restaurar dependencias  
   ```bash
   dotnet restore
   ```

3. Aplicar migraciones (si corresponde)  
   ```bash
   dotnet ef database update
   ```

4. Ejecutar la API  
   ```bash
   dotnet run --project api-rest-net8-clear-aquitectura
   ```

5. Acceder a la documentación Swagger:  
   👉 [http://localhost:5000/swagger](http://localhost:5000/swagger)

---

## 🧠 Arquitectura Clean Overview

Cada capa tiene una responsabilidad clara:

| Capa | Rol | Descripción |
|------|------|-------------|
| **Core** | Dominio | Contiene las entidades, interfaces base y lógica empresarial pura. |
| **Application** | Aplicación | Define los casos de uso, servicios y lógica de negocio específica. |
| **Infrastructure** | Infraestructura | Implementa la persistencia, integración con bases de datos y servicios externos. |
| **WebApi** | Presentación | Expone endpoints HTTP y maneja solicitudes de clientes. |
| **Utilities** | Utilidades | Contiene funciones, extensiones o helpers reutilizables. |

---

## 🧰 Mejores Prácticas Implementadas

- Patrón **Repository y Unit of Work**  
- Validación y manejo de excepciones centralizados  
- DTOs y mapeos con **AutoMapper**  
- Configuración por entorno (Development, Staging, Production)  
- Registro y trazabilidad con **Serilog**  

---

## 📄 Licencia

Este proyecto se distribuye bajo la licencia **MIT**, lo que permite su libre uso y modificación.




