using System.Xml.Serialization;

namespace KardexIti.Models;

/// <summary>
/// Carreras válidas. El texto de [XmlEnum] es el que se escribe en el XML
/// y debe coincidir con el xs:enumeration de Kardex.xsd.
/// </summary>
public enum Carrera
{
    [XmlEnum("Ingeniería en Informática")]
    Informatica,

    [XmlEnum("Ingeniería en Sistemas Computacionales")]
    SistemasComputacionales,

    [XmlEnum("Ingeniería Industrial")]
    Industrial
}
