# Registro de Prompts - FoodCampus

En este archivo se documentarán todos los prompts utilizados para la generación y mantenimiento del proyecto.

## Prompt Inicial
Generar la estructura de carpetas y archivos iniciales para un proyecto llamado "FoodCampus" en Visual Studio, siguiendo Clean Architecture.

## Prompt 2 - Capa Domain
Quiero que generes el código en C# para la capa Domain de un proyecto llamado FoodCampus, siguiendo Clean Architecture. 

Requisitos:
1. Crear las entidades principales: Restaurante, Pedido, DetallePedido y Cliente.
2. Cada entidad debe usar propiedades con validaciones en los setters utilizando la palabra clave `field` (C# 14).
   - Restaurante: validar que el horario de apertura sea menor al horario de cierre.
   - Pedido: validar que la fecha no sea futura y que tenga al menos un DetallePedido.
   - DetallePedido: validar que la cantidad sea mayor a 0 y el precio unitario mayor a 0.
   - Cliente: validar que el nombre no sea vacío y el correo tenga formato válido.
3. No incluir dependencias externas ni lógica de persistencia, solo clases puras de dominio.
4. Organizar las clases dentro de la carpeta `Domain/Entities`.
5. Crear una carpeta `Domain/Exceptions` con excepciones personalizadas para cada validación.

El resultado debe ser código limpio, comentado y listo para compilar en Visual Studio.

## Prompt 3 - Capa Application
Quiero que generes el código en C# para la capa Application de un proyecto llamado FoodCampus, siguiendo Clean Architecture. 

Requisitos:
1. Crear interfaces de repositorios en la carpeta Application/Interfaces:
   - IRestauranteRepository
   - IPedidoRepository
   - IClienteRepository
   Cada interfaz debe definir operaciones CRUD básicas y consultas específicas (ej. listar restaurantes disponibles).

2. Crear DTOs en la carpeta Application/DTOs:
   - RestauranteDTO (Id, Nombre, HorarioApertura, HorarioCierre)
   - PedidoDTO (Id, Fecha, ClienteId, Lista de Detalles)
   - DetallePedidoDTO (Id, PedidoId, Producto, Cantidad, PrecioUnitario)
   - ClienteDTO (Id, Nombre, Correo)

3. Crear casos de uso en la carpeta Application/UseCases:
   - RegistrarPedido (validar que tenga al menos un detalle y que el cliente exista).
   - ConsultarRestaurantes (listar todos los restaurantes disponibles).
   - ListarPedidosPorCliente (mostrar pedidos de un cliente específico).

4. Los casos de uso deben depender solo de las interfaces de repositorios y trabajar con DTOs, nunca con entidades directamente.
5. No incluir lógica de persistencia ni dependencias externas, solo contratos y lógica de aplicación.
6. Organizar el código limpio y comentado, listo para compilar en Visual Studio.

## Prompt 4 - Capa Infrastructure
Quiero que generes el código en C# para la capa Infrastructure de un proyecto llamado FoodCampus, siguiendo Clean Architecture y usando EF Core 10. 

Requisitos:
1. Crear el DbContext en la carpeta Infrastructure/Context con el nombre FoodCampusDbContext.
   - Incluir DbSet para Restaurante, Pedido, DetallePedido y Cliente.
   - Configurar las relaciones entre entidades (Pedido → Cliente, Pedido → DetallePedido, Restaurante → Pedido).
2. Crear configuraciones de entidades en la carpeta Infrastructure/Mappings usando Fluent API.
   - Validar restricciones como longitudes de texto, campos obligatorios y relaciones.
3. Crear repositorios concretos en la carpeta Infrastructure/Repositories que implementen las interfaces de Application.
   - RestauranteRepository
   - PedidoRepository
   - ClienteRepository
4. Configurar la inyección de dependencias en Infrastructure/DependencyInjection.cs para registrar los repositorios y el DbContext.
5. Preparar el proyecto para migraciones con EF Core (dotnet ef migrations add InitialCreate).
6. No incluir lógica de negocio, solo persistencia y configuración de acceso a datos.

## Prompt 5 - Infrastructure con conexión Somee
Genera el código en C# para la capa Infrastructure de un proyecto llamado FoodCampus, siguiendo Clean Architecture y usando EF Core 10 con SQL Server.

Requisitos:
1. Crear el DbContext en Infrastructure/Context con el nombre FoodCampusDbContext.
   - Incluir DbSet para Restaurante, Cliente, Pedido y DetallePedido.
   - Configurar relaciones entre entidades (Pedido → Cliente, Pedido → Restaurante, Pedido → DetallePedido).
   - Usar Fluent API en Infrastructure/Mappings para restricciones (longitudes, campos obligatorios, relaciones).

2. Crear repositorios concretos en Infrastructure/Repositories que implementen las interfaces de Application:
   - RestauranteRepository
   - ClienteRepository
   - PedidoRepository

3. Configurar la inyección de dependencias en Infrastructure/DependencyInjection.cs:
   - Registrar DbContext y repositorios.
   - Usar la siguiente cadena de conexión en appsettings.json:
     {
       "ConnectionStrings": {
         "FoodCampusDB": "Data Source=ExamenPE.mssql.somee.com;Initial Catalog=ExamenPE;User Id=AleUTM;Password=4qsRMippJN77K@3;"
       }
     }

4. Preparar el proyecto para migraciones con EF Core:
   - dotnet ef migrations add InitialCreate
   - dotnet ef database update
   Esto debe crear las tablas en la base de datos ExamenPE alojada en Somee.

5. El resultado debe ser código limpio, organizado y listo para compilar en Visual Studio, con conexión funcional a la base de datos en Somee.

## Prompt 6 - Verificar conexión a la base de datos
Revisa el proyecto, checa que si esté conectada a la base de datos correctamente y cómo yo puedo saber que sí existe la conexión.

## Prompt 7 - Corrección de errores de compilación
Ayúdame a corregir los errores. (Error CS8858: los DTOs eran `class` pero se usaba expresión `with {}` que solo funciona con `record`. Se convirtieron los 4 DTOs a `record`. Error CS1061: faltaba `using Microsoft.Extensions.Configuration` en Program.cs. Error CS0116: código de ejemplo fue pegado accidentalmente al final de RestaurantConfiguration.cs.)

## Prompt 8 - Agregar prompts al archivo prompts.md
Agrega todos los prompts al archivo prompts.md.

## Prompt 9 - Capa API con menú interactivo CRUD
Genera el código en C# para la capa API de un proyecto llamado FoodCampus, siguiendo Clean Architecture. 

Requisitos:
1. Crear un proyecto de consola en /src/API.
2. Implementar un menú interactivo CRUD con las siguientes opciones:
   - 1. Listar Restaurantes
   - 2. Agregar Restaurante
   - 3. Registrar Pedido (con detalles y cliente)
   - 4. Listar Pedidos por Cliente
   - 5. Salir
3. Usar inyección de dependencias para conectar Application con Infrastructure.
4. Los casos de uso deben invocar los repositorios concretos a través de las interfaces de Application.
5. La conexión a la base de datos debe usar la cadena en appsettings.json.
6. El menú debe validar entradas del usuario (ej. no permitir cantidades negativas, correos inválidos).
7. El resultado debe ser código limpio, organizado y listo para compilar en Visual Studio, capaz de ejecutar operaciones CRUD contra la base de datos en Somee.

## Prompt 10 - Verificar cadena de conexión a Somee
Ayúdame a buscar este JSON para los permisos que necesita el proyecto para conectarse a Somee:
{
  "ConnectionStrings": {
    "FoodCampusDB": "Data Source=ExamenPE.mssql.somee.com;Initial Catalog=ExamenPE;User Id=AleUTM;Password=4qsRMippJN77K@3;"
  }
}

## Prompt 11 - Corrección de errores en RestaurantConfiguration y Program.cs
Ayúdame a corregir los errores. (Error CS0116 y otros: código de prueba de conexión fue pegado accidentalmente al final de RestaurantConfiguration.cs. Se eliminó el código fuera de clase y se colocó correctamente en Program.cs con la prueba de conexión usando `db.Database.CanConnectAsync()`.)

## Prompt 12 - Agregar prompts finales al archivo prompts.md
Agrega los prompts finales al archivo prompts.md.
