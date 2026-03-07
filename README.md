# FoodCampus

FoodCampus es un proyecto desarrollado bajo los principios de **Clean Architecture**, diseñado para ser escalable y fácil de mantener.

## Estructura del Proyecto

- **/db-scripts**: Scripts SQL para el esquema y datos iniciales.
- **/prompts**: Documentación de los prompts utilizados durante el desarrollo.
- **/src**: Código fuente dividido en capas:
  - **Domain**: Entidades puras y lógica de negocio.
  - **Application**: Casos de uso, DTOs e interfaces.
  - **Infrastructure**: Implementación de persistencia (EF Core) y repositorios.
  - **API (Consola)**: Aplicación cliente con menú interactivo e Inyección de Dependencias.

## Requisitos

- .NET 8.0 SDK o superior.
- SQL Server (opcional, configurado en Infrastructure).

## Instrucciones de Ejecución

1. Clonar el repositorio.
2. Navegar a la carpeta raíz del proyecto.
3. Ejecutar el proyecto de consola:
   ```bash
   dotnet run --project src/API/FoodCampus.API.csproj
   ```

## Notas de Desarrollo
- La capa **API** solo tiene referencia a **Application** para mantener la inversión de dependencias. Para registrar los servicios de **Infrastructure**, se recomienda el uso de ensamblados o una pequeña modificación según el entorno de despliegue.

## Enlace de Referencia
- [Ver video de referencia](https://youtu.be/h1a3JqkxV2s?si=-zW96fPyzlKKqAEH)
