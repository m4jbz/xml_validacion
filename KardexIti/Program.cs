using System.Text;
using KardexIti.Services;
using KardexIti.UI;

Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

var rutas = new RutasArchivos(AppContext.BaseDirectory);
var menu = new MenuConsola(
    rutas,
    new KardexSerializer(),
    new XsdValidator(rutas.Esquema),
    new KardexParser());

menu.Ejecutar();
