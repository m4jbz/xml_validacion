using System.Xml;
using System.Xml.Schema;

namespace KardexIti.Services;

/// <summary>
/// Valida cualquier archivo XML contra Kardex.xsd y reporta la línea, la posición
/// y la descripción de cada error encontrado.
/// </summary>
public sealed class XsdValidator
{
    private readonly XmlSchemaSet _esquemas = new();

    public XsdValidator(string rutaEsquema)
    {
        _esquemas.Add(targetNamespace: null, rutaEsquema);
        _esquemas.Compile();
    }

    public ValidationResult Validar(string rutaXml)
    {
        var resultado = new ValidationResult(rutaXml);

        if (!File.Exists(rutaXml))
        {
            resultado.Errores.Add(new ErrorValidacion(0, 0, "Error", $"No existe el archivo \"{rutaXml}\"."));
            return resultado;
        }

        var configuracion = new XmlReaderSettings
        {
            ValidationType = ValidationType.Schema,
            Schemas = _esquemas,
            // Las advertencias también cuentan, por ejemplo una raíz que no está declarada en el esquema.
            ValidationFlags = XmlSchemaValidationFlags.ReportValidationWarnings
                            | XmlSchemaValidationFlags.ProcessIdentityConstraints
        };

        // Errores de estructura y de dominio: tipos, rangos, patrones, enumeraciones y cardinalidad.
        configuracion.ValidationEventHandler += (_, e) =>
            resultado.Errores.Add(new ErrorValidacion(
                e.Exception.LineNumber,
                e.Exception.LinePosition,
                e.Severity == XmlSeverityType.Error ? "Error" : "Advertencia",
                e.Message));

        try
        {
            using var reader = XmlReader.Create(rutaXml, configuracion);
            while (reader.Read()) { }
        }
        catch (XmlException ex)
        {
            // Errores de sintaxis: el documento no está bien formado y la lectura se detiene.
            resultado.Errores.Add(new ErrorValidacion(ex.LineNumber, ex.LinePosition, "XML mal formado", ex.Message));
        }

        return resultado;
    }
}
