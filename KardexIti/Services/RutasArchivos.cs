namespace KardexIti.Services;

/// <summary>Centraliza las rutas de los archivos que usa la aplicación.</summary>
public sealed class RutasArchivos(string directorioBase)
{
    public string DirectorioBase { get; } = directorioBase;
    public string Esquema => Path.Combine(DirectorioBase, "Resources", "Kardex.xsd");
    public string KardexValido => Path.Combine(DirectorioBase, "kardex_valido.xml");
    public string KardexInvalido => Path.Combine(DirectorioBase, "Resources", "kardex_invalido.xml");

    /// <summary>
    /// Resuelve una ruta escrita por el usuario. Si es relativa, prueba primero el
    /// directorio actual y luego el directorio de la aplicación.
    /// </summary>
    public string Resolver(string ruta)
    {
        if (Path.IsPathRooted(ruta) || File.Exists(ruta)) return Path.GetFullPath(ruta);
        return Path.Combine(DirectorioBase, ruta);
    }
}
