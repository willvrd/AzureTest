# AzureTest - .NET 10 API Cloud Lab

A technical laboratory focused on implementing a robust **.NET 10** architecture, advanced **Azure Blob Storage** integration, and full automation via **CI/CD**.

## 🚀 Project Purpose
This repository validates modern design patterns and cloud deployment workflows, prioritizing security, scalability, and system traceability.

## 🛠️ Tech Stack
* **Backend:** ASP.NET Core 10.0 (Web API).
* **Database & Storage:** Entity Framework Core with **Azure Blob Storage** integration.
* **DevOps:** **GitHub Actions** pipelines utilizing **OIDC (OpenID Connect)** for passwordless Azure authentication.
* **Observability:** Implementation of audit fields and custom logging for cloud transactions.

## 🌐 Demo & Frontend
> [!IMPORTANT]
> This project includes a **minimalist visualization interface** (HTML5/CSS3) designed exclusively to verify service integrity and correct asset rendering from Azure. It is not intended to be a full frontend implementation.

## 🏗️ Architecture & Best Practices
* **Pattern-Oriented:** Decoupled services and static extension methods for DTO transformations (Transformers).
* **Environment Management:** Multi-environment configuration (Development/Staging) through dynamic profiles in `launchSettings.json`.
* **Resiliency:** File management logic with built-in Rollbacks during external storage failures.

## ⚖️ License
This project is licensed under the **MIT License**.

---
**Author:** [William Verde](https://github.com/willvrd)