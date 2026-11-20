using KardexIti.Data;
using KardexIti.Services;

namespace KardexIti.UI;

/// <summary>Menú interactivo. Solo maneja entrada y salida en consola y delega el trabajo a los servicios.</summary>
public sealed class MenuConsola(
    RutasArchivos rutas,
    KardexSerializer serializer,
    XsdValidator validador,
    KardexParser parser)
{
    public void Ejecutar()
    {
        while (true)
        {
            MostrarOpciones();
            var opcion = Console.ReadLine()?.Trim();
            Console.WriteLine();

            try
            {
                switch (opcion)
                {
                    case "1": GenerarYSerializar(); break;
                    case "2": ValidarArchivo(); break;
                    case "3": PruebaErrorControlado(); break;
                    case "4": ConsultarKardex(); break;
                    case "0": case null: return;
                    default: Console.WriteLine("Opción no válida."); break;
                }
            }
            catch (Exception ex)
            {
                EscribirColor($"Error inesperado: {ex.Message}", ConsoleColor.Red);
            }

            Console.WriteLine();
        }
    }

    private static void MostrarOpciones()
    {
        Console.WriteLine("==============================================");
        Console.WriteLine("   Sistema de Control de Kardex Académico ITI");
        Console.WriteLine("==============================================");
        Console.WriteLine(" 1. Generar y serializar kardex_valido.xml");
        Console.WriteLine(" 2. Validar un archivo XML contra Kardex.xsd");
        Console.WriteLine(" 3. Prueba de error controlado (kardex_invalido.xml)");
        Console.WriteLine(" 4. Consultar promedio y materias aprobadas");
        Console.WriteLine(" 0. Salir");
        Console.Write("Seleccione una opción: ");
    }

    // Operación 1
    private void GenerarYSerializar()
    {
        var expediente = KardexSeed.CrearEjemplo();
        serializer.Serializar(expediente, rutas.KardexValido);

        Console.WriteLine($"Expediente de {expediente.NombreCompleto} ({expediente.NumeroControl}) " +
                          $"con {expediente.Materias.Count} materias.");
        Console.WriteLine($"Archivo generado: {rutas.KardexValido}");

        // Comprobación inmediata de que lo serializado cumple el esquema.
        MostrarResultado(validador.Validar(rutas.KardexValido));
    }

    // Operación 2
    private void ValidarArchivo()
    {
        Console.Write("Ruta del archivo XML (Enter = kardex_valido.xml): ");
        var entrada = Console.ReadLine()?.Trim().Trim('"');
        var ruta = string.IsNullOrEmpty(entrada) ? rutas.KardexValido : rutas.Resolver(entrada);

        MostrarResultado(validador.Validar(ruta));
    }

    // Operación 3
    private void PruebaErrorControlado()
    {
        Console.WriteLine($"Validando archivo con errores intencionales: {rutas.KardexInvalido}");
        MostrarResultado(validador.Validar(rutas.KardexInvalido));
    }

    // Operación 4
    private void ConsultarKardex()
    {
        if (!File.Exists(rutas.KardexValido))
        {
            EscribirColor("No existe kardex_valido.xml. Ejecute primero la opción 1.", ConsoleColor.Yellow);
            return;
        }

        var resumen = parser.Consultar(rutas.KardexValido);

        Console.WriteLine($"Alumno:          {resumen.NombreCompleto}");
        Console.WriteLine($"No. de control:  {resumen.NumeroControl}");
        Console.WriteLine($"Carrera:         {resumen.Carrera}");
        Console.WriteLine($"Semestre:        {resumen.Semestre}");
        Console.WriteLine($"Materias:        {resumen.TotalMaterias}");
        EscribirColor($"Promedio general: {resumen.PromedioGeneral:0.00}", ConsoleColor.Cyan);

        Console.WriteLine();
        Console.WriteLine($"Materias aprobadas ({resumen.MateriasAprobadas.Count}):");
        Console.WriteLine($"  {"Clave",-10}{"Materia",-40}{"Créd.",6}{"Calif.",8}");
        foreach (var m in resumen.MateriasAprobadas)
            Console.WriteLine($"  {m.Clave,-10}{m.Nombre,-40}{m.Creditos,6}{m.Calificacion,8:0.0}");
    }

    private static void MostrarResultado(ValidationResult resultado)
    {
        if (resultado.EsValido)
        {
            EscribirColor($"VÁLIDO: {Path.GetFileName(resultado.Archivo)} cumple con Kardex.xsd.", ConsoleColor.Green);
            return;
        }

        EscribirColor($"INVÁLIDO: {Path.GetFileName(resultado.Archivo)} tiene {resultado.Errores.Count} error(es).",
                      ConsoleColor.Red);
        foreach (var e in resultado.Errores)
        {
            EscribirColor($"  [{e.Severidad}] Línea {e.Linea}, posición {e.Posicion}", ConsoleColor.Yellow);
            Console.WriteLine($"    {e.Mensaje}");
        }
    }

    private static void EscribirColor(string texto, ConsoleColor color)
    {
        var anterior = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine(texto);
        Console.ForegroundColor = anterior;
    }
}
