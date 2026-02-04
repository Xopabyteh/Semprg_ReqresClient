# ReqresClient

A .NET 10 console application for interacting with the [Reqres API](https://reqres.in).

## About

- **API Client Generator**: [Microsoft Kiota](https://learn.microsoft.com/openapi/kiota/overview)
- **Code Generation**: All code is AI-generated using GitHub Copilot

## Quick Start

1. Set your API key:
   ```powershell
   [Environment]::SetEnvironmentVariable("quick_temporary_api_key", "your-api-key", "User")
   ```

2. Run:
   ```powershell
   dotnet run
   ```

## Features

Interactive menu-driven application with:
- User authentication (login/register)
- User CRUD operations
- Resource browsing