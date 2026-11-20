using KardexIti.Models;

namespace KardexIti.Data;

/// <summary>Datos de ejemplo que se instancian en memoria para la operación de serialización.</summary>
public static class KardexSeed
{
    private const decimal CalificacionMinimaAprobatoria = 70m;

    public static ExpedienteAcademico CrearEjemplo() => new()
    {
        Periodo = "AGO-DIC-2026",
        NumeroControl = "22670154",
        NombreCompleto = "Juan Pérez López",
        Carrera = Carrera.SistemasComputacionales,
        Semestre = 5,
        Materias =
        [
            NuevaMateria("SCD-1027", "Tópicos Avanzados de Programación", 5, 92.5m),
            NuevaMateria("SCC-1017", "Métodos Numéricos", 4, 64.0m),
            NuevaMateria("AEF-1031", "Fundamentos de Bases de Datos", 5, 88.0m),
            NuevaMateria("SCD-1022", "Simulación", 5, 75.5m),
            NuevaMateria("ACF-0905", "Ecuaciones Diferenciales", 5, 58.0m)
        ]
    };

    private static Materia NuevaMateria(string clave, string nombre, int creditos, decimal calificacion) => new()
    {
        Clave = clave,
        NombreMateria = nombre,
        Creditos = creditos,
        Calificacion = calificacion,
        Aprobada = calificacion >= CalificacionMinimaAprobatoria
    };
}
