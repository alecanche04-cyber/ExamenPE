using FoodCampus.Application.DTOs;
using FoodCampus.Application.Interfaces;
using FoodCampus.Application.UseCases;

namespace FoodCampus.API;

/// <summary>
/// Menú interactivo CRUD de consola para FoodCampus.
/// Depende solo de interfaces de Application y casos de uso; nunca de Infrastructure directamente.
/// </summary>
public class Menu(
    ConsultarRestaurantesUseCase consultarRestaurantes,
    RegistrarPedidoUseCase registrarPedido,
    ListarPedidosPorClienteUseCase listarPedidos,
    IRestauranteRepository restauranteRepo,
    IClienteRepository clienteRepo)
{
    public async Task EjecutarAsync()
    {
        while (true)
        {
            Console.WriteLine("\n╔══════════════════════════════╗");
            Console.WriteLine("║       FOODCAMPUS MENÚ        ║");
            Console.WriteLine("╠══════════════════════════════╣");
            Console.WriteLine("║  1. Listar Restaurantes      ║");
            Console.WriteLine("║  2. Agregar Restaurante      ║");
            Console.WriteLine("║  3. Registrar Pedido         ║");
            Console.WriteLine("║  4. Listar Pedidos x Cliente ║");
            Console.WriteLine("║  5. Salir                    ║");
            Console.WriteLine("╚══════════════════════════════╝");
            Console.Write("Selecciona una opción: ");

            switch (Console.ReadLine()?.Trim())
            {
                case "1": await ListarRestaurantesAsync(); break;
                case "2": await AgregarRestauranteAsync(); break;
                case "3": await RegistrarPedidoAsync(); break;
                case "4": await ListarPedidosPorClienteAsync(); break;
                case "5":
                    Console.WriteLine("\n¡Hasta luego!");
                    return;
                default:
                    Console.WriteLine("Opción inválida. Elige entre 1 y 5.");
                    break;
            }
        }
    }

    // ── Opción 1 ────────────────────────────────────────────────────────────

    private async Task ListarRestaurantesAsync()
    {
        var lista = (await consultarRestaurantes.ExecuteAllAsync()).ToList();

        Console.WriteLine("\n--- RESTAURANTES ---");
        if (lista.Count == 0)
        {
            Console.WriteLine("No hay restaurantes registrados.");
            return;
        }

        foreach (var r in lista)
            Console.WriteLine($"  [{r.Id}] {r.Nombre}  |  Apertura: {r.HorarioApertura:hh\\:mm}  |  Cierre: {r.HorarioCierre:hh\\:mm}");
    }

    // ── Opción 2 ────────────────────────────────────────────────────────────

    private async Task AgregarRestauranteAsync()
    {
        Console.WriteLine("\n--- AGREGAR RESTAURANTE ---");

        string nombre = LeerTexto("Nombre del restaurante: ");
        TimeSpan apertura = LeerHora("Horario de apertura (HH:mm): ");

        TimeSpan cierre;
        while (true)
        {
            cierre = LeerHora("Horario de cierre  (HH:mm): ");
            if (cierre > apertura) break;
            Console.WriteLine("El cierre debe ser posterior a la apertura. Intenta de nuevo.");
        }

        try
        {
            var resultado = await restauranteRepo.CreateAsync(new RestauranteDTO
            {
                Nombre = nombre,
                HorarioApertura = apertura,
                HorarioCierre = cierre,
            });
            Console.WriteLine($"✓ Restaurante '{resultado.Nombre}' creado con Id {resultado.Id}.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al crear restaurante: {ex.Message}");
        }
    }

    // ── Opción 3 ────────────────────────────────────────────────────────────

    private async Task RegistrarPedidoAsync()
    {
        Console.WriteLine("\n--- REGISTRAR PEDIDO ---");

        // Elegir restaurante
        await ListarRestaurantesAsync();
        int restauranteId = LeerEntero("Id del restaurante: ", min: 1);

        // Elegir o crear cliente
        int clienteId = LeerEntero("Id del cliente (0 = crear nuevo): ", min: 0);
        if (clienteId == 0)
        {
            string nombre = LeerTexto("Nombre del cliente: ");
            string correo = LeerCorreo("Correo del cliente: ");

            try
            {
                var nuevo = await clienteRepo.CreateAsync(new ClienteDTO { Nombre = nombre, Correo = correo });
                clienteId = nuevo.Id;
                Console.WriteLine($"✓ Cliente creado con Id {clienteId}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear cliente: {ex.Message}");
                return;
            }
        }

        // Capturar detalles del pedido
        var detalles = new List<DetallePedidoDTO>();
        Console.WriteLine("Agrega productos (escribe 'listo' como nombre para finalizar):");

        while (true)
        {
            string producto = LeerTexto("  Producto ('listo' para terminar): ");
            if (producto.Equals("listo", StringComparison.OrdinalIgnoreCase)) break;

            int cantidad    = LeerEntero("  Cantidad: ",        min: 1);
            decimal precio  = LeerDecimal("  Precio unitario: ", min: 0.01m);

            detalles.Add(new DetallePedidoDTO
            {
                Producto       = producto,
                Cantidad       = cantidad,
                PrecioUnitario = precio,
            });
        }

        if (detalles.Count == 0)
        {
            Console.WriteLine("El pedido debe tener al menos un producto. Operación cancelada.");
            return;
        }

        try
        {
            var resultado = await registrarPedido.ExecuteAsync(new PedidoDTO
            {
                Fecha        = DateTime.UtcNow,
                ClienteId    = clienteId,
                RestauranteId = restauranteId,
                Detalles     = detalles,
            });
            Console.WriteLine($"✓ Pedido registrado con Id {resultado.Id}.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al registrar pedido: {ex.Message}");
        }
    }

    // ── Opción 4 ────────────────────────────────────────────────────────────

    private async Task ListarPedidosPorClienteAsync()
    {
        Console.WriteLine("\n--- PEDIDOS POR CLIENTE ---");
        int clienteId = LeerEntero("Id del cliente: ", min: 1);

        try
        {
            var pedidos = (await listarPedidos.ExecuteAsync(clienteId)).ToList();

            if (pedidos.Count == 0)
            {
                Console.WriteLine("Este cliente no tiene pedidos registrados.");
                return;
            }

            foreach (var p in pedidos)
            {
                Console.WriteLine($"\n  Pedido #{p.Id}  |  Fecha: {p.Fecha:dd/MM/yyyy HH:mm}  |  Restaurante Id: {p.RestauranteId}");
                foreach (var d in p.Detalles)
                    Console.WriteLine($"    - {d.Producto}  x{d.Cantidad}  @  ${d.PrecioUnitario:F2}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    // ── Helpers de lectura con validación ───────────────────────────────────

    private static string LeerTexto(string mensaje)
    {
        while (true)
        {
            Console.Write(mensaje);
            var valor = Console.ReadLine()?.Trim();
            if (!string.IsNullOrWhiteSpace(valor)) return valor;
            Console.WriteLine("El valor no puede estar vacío.");
        }
    }

    private static int LeerEntero(string mensaje, int min = int.MinValue)
    {
        while (true)
        {
            Console.Write(mensaje);
            if (int.TryParse(Console.ReadLine(), out int valor) && valor >= min)
                return valor;
            Console.WriteLine($"Ingresa un número entero mayor o igual a {min}.");
        }
    }

    private static decimal LeerDecimal(string mensaje, decimal min = 0)
    {
        while (true)
        {
            Console.Write(mensaje);
            if (decimal.TryParse(Console.ReadLine(), out decimal valor) && valor >= min)
                return valor;
            Console.WriteLine($"Ingresa un número mayor o igual a {min}.");
        }
    }

    private static TimeSpan LeerHora(string mensaje)
    {
        while (true)
        {
            Console.Write(mensaje);
            var input = Console.ReadLine()?.Trim() ?? string.Empty;
            // Aceptar solo formato HH:mm y validar rango válido para SQL Server time (< 24h)
            if (TimeSpan.TryParseExact(input, @"hh\:mm", null, out TimeSpan valor)
                || TimeSpan.TryParseExact(input, @"h\:mm", null, out valor))
            {
                if (valor >= TimeSpan.Zero && valor < TimeSpan.FromHours(24))
                    return valor;
            }
            Console.WriteLine("Formato inválido. Usa HH:mm en rango 00:00 a 23:59 (ej. 08:00 o 14:30).");
        }
    }

    private static string LeerCorreo(string mensaje)
    {
        while (true)
        {
            Console.Write(mensaje);
            var correo = Console.ReadLine()?.Trim() ?? string.Empty;
            // Validación básica: debe contener @ y al menos un punto después del @
            int at = correo.IndexOf('@');
            if (at > 0 && correo.IndexOf('.', at) > at + 1)
                return correo;
            Console.WriteLine("Correo inválido. Formato esperado: usuario@dominio.ext");
        }
    }
}
