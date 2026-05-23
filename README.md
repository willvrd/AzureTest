# AzureTest - .NET 10 API Cloud Lab

A technical laboratory focused on implementing a robust, modular **.NET 10** architecture, advanced **Azure Blob Storage** integration, and full automation via **CI/CD**.

## 🚀 Project Purpose
This repository validates modern design patterns and cloud deployment workflows, prioritizing security, structural scalability, and system traceability.

## 🛠️ Tech Stack
* **Backend:** ASP.NET Core 10.0 (Web API) with **API Versioning**.
* **Database & Storage:** Entity Framework Core with **Azure Blob Storage** integration.
* **DevOps:** **GitHub Actions** pipelines utilizing **OIDC (OpenID Connect)** for passwordless Azure authentication.
* **Observability:** Centralized global exception handlers and audit fields for cloud transactions.

## 🏗️ Architecture & Core Concepts

### 📦 Modular Domain Structure (Feature Folders)
The solution rejects monolithic, flat folder structures in favor of **Feature Isolation**. Code is organized by high-level domains inside the `Modules/` directory:
* **`Core/`**: Contains cross-cutting concerns (Global Exception Handlers, Middlewares, and static Dashboard Templates).
* **`Posts/`**: A self-contained vertical slice containing its own Controllers, Services, Entities, and DTOs.
* **`Media/`**: Isolated infrastructure layer handling cloud storage adapters.

### 🛣️ API Versioning Strategy
To prevent breaking changes in production/staging/development environments, the API implements strict **URL Versioning** powered by `Asp.Versioning.Http`:
* **Explicit Versioning:** `/api/v1/posts` (Route matching specific controller boundaries).
* **Next-Gen Ready:** Built to support concurrent `v2` deployments within the same domain slice without breaking changes.

### 📊 Consistent API Responses & Sorting
All endpoints follow a unified response contract using generic wrapper models (`ResponseWrapper<T>` and `PagedResponse<T>`):
* Unified wrapper: Data properties are consistently wrapped under a predictable root `"data"` object.
* Robust pagination metadata: Separated into a custom `"pagination"` object appended at the bottom of collection responses.
* Dynamic Safe Sorting: Built-in query sanitization supporting custom `orderField` and `orderWay` parameters mapped safely against SQL queries.

## 🌐 Demo & Frontend
> [!IMPORTANT]
> This project includes a **minimalist visualization interface** (HTML5/CSS3) housed inside the `Core/Templates` module. It is designed exclusively to verify service integrity, multi-environment configurations, and correct asset rendering from Azure Blob Storage. It is not intended to be a full frontend implementation.

## ⚖️ License
This project is licensed under the **MIT License**.

---
**Author:** [William Verde](https://github.com/willvrd)
