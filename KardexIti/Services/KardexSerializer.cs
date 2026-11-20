using System.Text;
using System.Xml;
using System.Xml.Serialization;
using KardexIti.Models;

namespace KardexIti.Services;

/// <summary>Convierte objetos ExpedienteAcademico a XML y viceversa con XmlSerializer.</summary>
public sealed class KardexSerializer
{
    private static readonly XmlSerializer Serializer = new(typeof(ExpedienteAcademico));

    public void Serializar(ExpedienteAcademico expediente, string ruta)
    {
        var configuracion = new XmlWriterSettings
        {
            Indent = true,
            Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false)
        };

        // Espacios de nombres vacíos para no generar xmlns:xsi / xmlns:xsd en la raíz.
        var espacios = new XmlSerializerNamespaces();
        espacios.Add(string.Empty, string.Empty);

        using var writer = XmlWriter.Create(ruta, configuracion);
        Serializer.Serialize(writer, expediente, espacios);
    }

    public ExpedienteAcademico Deserializar(string ruta)
    {
        using var reader = XmlReader.Create(ruta);
        return (ExpedienteAcademico)(Serializer.Deserialize(reader)
            ?? throw new InvalidDataException("El archivo no contiene un expediente."));
    }
}
