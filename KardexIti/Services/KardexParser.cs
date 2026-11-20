using System.Globalization;
using System.Xml.Linq;

namespace KardexIti.Services;

public sealed record MateriaConsultada(string Clave, string Nombre, int Creditos, decimal Calificacion);

public sealed record ResumenKardex(
    string NumeroControl,
    string NombreCompleto,
    string Carrera,
    int Semestre,
    int TotalMaterias,
    decimal PromedioGeneral,
    IReadOnlyList<MateriaConsultada> MateriasAprobadas);

/// <summary>
/// Lee el XML con LINQ to XML, sin usar las clases del modelo,
/// calcula el promedio general y extrae las materias aprobadas.
/// </summary>
public sealed class KardexParser
{
    public ResumenKardex Consultar(string rutaXml)
    {
        var documento = XDocument.Load(rutaXml);
        var raiz = documento.Root ?? throw new InvalidDataException("El documento no tiene elemento raíz.");

        var materias = raiz.Element("Materias")?.Elements("Materia").ToList() ?? [];
        if (materias.Count == 0)
            throw new InvalidDataException("El expediente no contiene materias.");

        // Promedio general simple de todas las calificaciones.
        var promedio = materias.Average(m => LeerDecimal(m, "Calificacion"));

        var aprobadas = materias
            .Where(m => (bool)m.Element("Aprobada")!)
            .Select(m => new MateriaConsultada(
                (string?)m.Attribute("clave") ?? "",
                (string?)m.Element("NombreMateria") ?? "",
                (int)m.Element("Creditos")!,
                LeerDecimal(m, "Calificacion")))
            .ToList();

        return new ResumenKardex(
            (string?)raiz.Element("NumeroControl") ?? "",
            (string?)raiz.Element("NombreCompleto") ?? "",
            (string?)raiz.Element("Carrera") ?? "",
            (int)raiz.Element("Semestre")!,
            materias.Count,
            Math.Round(promedio, 2),
            aprobadas);
    }

    private static decimal LeerDecimal(XElement materia, string nombre) =>
        decimal.Parse(materia.Element(nombre)!.Value, CultureInfo.InvariantCulture);
}
