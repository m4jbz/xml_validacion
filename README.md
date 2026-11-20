# KardexIti: validación de XML con XSD

Práctica 1, Tema 5 (Servicios web XML). Aplicación de consola en **C# / .NET 10** que gestiona el kardex académico de un alumno del Instituto Tecnológico de Iguala usando XML:

- **Serializa** un expediente en memoria a `kardex_valido.xml` con `XmlSerializer`, sin concatenar cadenas.
- **Valida** cualquier archivo XML contra el esquema `Kardex.xsd` con `XmlReaderSettings` y reporta la línea, la posición y la descripción de cada error.
- **Demuestra** el rechazo de `kardex_invalido.xml`, que viola seis reglas del esquema.
- **Consulta** `kardex_valido.xml` con LINQ to XML: calcula el promedio general y muestra solo las materias aprobadas.

## Requisitos

- Windows con el **SDK de .NET 10**.
- Opcional: **Visual Studio 2026** para abrir la solución. Visual Studio 2019 no soporta .NET 10.

## Compilar y ejecutar

Desde Visual Studio: abrir `KardexIti.slnx` y usar *Depurar → Iniciar sin depurar* (`Ctrl+F5`).

Desde la terminal, en la raíz del repositorio:

```
dotnet build
dotnet run --project KardexIti
```

Al compilar, `Kardex.xsd` y `kardex_invalido.xml` se copian a la carpeta de salida (`KardexIti\bin\Debug\net10.0`), porque la aplicación busca sus archivos junto al ejecutable.

## Uso

```
==============================================
   Sistema de Control de Kardex Académico ITI
==============================================
 1. Generar y serializar kardex_valido.xml
 2. Validar un archivo XML contra Kardex.xsd
 3. Prueba de error controlado (kardex_invalido.xml)
 4. Consultar promedio y materias aprobadas
 0. Salir
```

| Opción | Qué hace |
|---|---|
| 1 | Crea el expediente de ejemplo, lo guarda como `kardex_valido.xml` en la carpeta de salida y lo valida en seguida. |
| 2 | Pide la ruta de un XML y lo valida. Con `Enter` valida `kardex_valido.xml`. Las rutas relativas se buscan en la carpeta actual y luego en la del ejecutable. |
| 3 | Valida `kardex_invalido.xml` y muestra sus seis errores. |
| 4 | Lee `kardex_valido.xml` y muestra los datos del alumno, el promedio general y las materias aprobadas. Requiere haber usado antes la opción 1. |

Para probar los archivos extra, elegir la opción 2 y escribir, por ejemplo:

```
pruebas\kardex_malformado.xml
pruebas\kardex_control_fuera_rango.xml
```

## Reglas del esquema

| Nodo | Regla |
|---|---|
| `@periodo` | Obligatorio, formato `ENE-JUN-AAAA` o `AGO-DIC-AAAA`. |
| `NumeroControl` | Año de ingreso (2 dígitos) + `67` + consecutivo de `0000` a `0400`. Patrón: `\d{2}67(0[0-3]\d{2}\|0400)`. |
| `NombreCompleto` | De 5 a 100 caracteres. |
| `Carrera` | Ingeniería en Informática, Ingeniería en Sistemas Computacionales o Ingeniería Industrial. |
| `Semestre` | Entero de 1 a 12. |
| `Materias` | De 1 a N elementos `Materia`; la `clave` no se puede repetir. |
| `@clave` | Obligatoria, formato `AAA-0000` (por ejemplo `SCD-1027`). |
| `Creditos` | Entero de 1 a 6. |
| `Calificacion` | Decimal de 0.0 a 100.0. |
| `Aprobada` | Booleano (`true`, `false`, `1`, `0`). |

## Archivos de prueba

| Archivo | Resultado esperado |
|---|---|
| `kardex_valido.xml` (generado por la opción 1) | Válido. |
| `KardexIti/Resources/kardex_invalido.xml` | 6 errores: número de control con letras, carrera inexistente, semestre 13, calificación 150, 8 créditos y `Aprobada` = `si`. |
| `pruebas/kardex_malformado.xml` | XML mal formado: `</Materia>` cierra a `<Materias>` en la línea 38. |
| `pruebas/kardex_control_fuera_rango.xml` | Número de control `22670401`, uno más que el máximo por generación. |

## Estructura

```
KardexIti.slnx                      Solución de Visual Studio 2026
KardexIti/
  Program.cs                        Punto de entrada y creación de servicios
  Models/                           ExpedienteAcademico, Materia y Carrera (atributos de serialización)
  Services/
    KardexSerializer.cs             Serialización con XmlSerializer
    XsdValidator.cs                 Validación contra el XSD (XmlReaderSettings + XmlSchemaSet)
    ValidationResult.cs             Errores con línea, posición y mensaje
    KardexParser.cs                 Consulta con LINQ to XML
    RutasArchivos.cs                Rutas del esquema y de los XML
  Data/KardexSeed.cs                Alumno de ejemplo con 5 materias
  UI/MenuConsola.cs                 Menú interactivo
  Resources/
    Kardex.xsd                      Esquema de validación
    kardex_invalido.xml             Documento con errores intencionales
pruebas/                            XML extra para la opción 2
reporte.typ                         Reporte de la práctica (Typst)
guia_kardex.html                    Guía que explica el código paso a paso
assets/                             Logos y capturas del reporte
```

