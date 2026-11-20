using System.Xml.Serialization;

namespace KardexIti.Models;

/// <summary>
/// Raíz del documento. El orden de las propiedades define el orden de los elementos
/// en el XML y debe coincidir con el xs:sequence de Kardex.xsd.
/// </summary>
[XmlRoot("ExpedienteAcademico")]
public class ExpedienteAcademico
{
    [XmlAttribute("periodo")]
    public string Periodo { get; set; } = string.Empty;

    public string NumeroControl { get; set; } = string.Empty;

    public string NombreCompleto { get; set; } = string.Empty;

    public Carrera Carrera { get; set; }

    public int Semestre { get; set; }

    [XmlArray("Materias")]
    [XmlArrayItem("Materia")]
    public List<Materia> Materias { get; set; } = [];
}
