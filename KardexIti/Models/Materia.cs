using System.Xml.Serialization;

namespace KardexIti.Models;

public class Materia
{
    [XmlAttribute("clave")]
    public string Clave { get; set; } = string.Empty;

    public string NombreMateria { get; set; } = string.Empty;

    public int Creditos { get; set; }

    public decimal Calificacion { get; set; }

    public bool Aprobada { get; set; }
}
