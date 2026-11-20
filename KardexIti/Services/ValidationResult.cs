namespace KardexIti.Services;

public sealed record ErrorValidacion(int Linea, int Posicion, string Severidad, string Mensaje);

public sealed class ValidationResult(string archivo)
{
    public string Archivo { get; } = archivo;
    public List<ErrorValidacion> Errores { get; } = [];
    public bool EsValido => Errores.Count == 0;
}
