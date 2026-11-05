using System.Globalization;

namespace Recetario.Api;

// Un record agrupa los datos que ASP.NET Core convierte a JSON.
public sealed record Receta(
    string Id, string Nombre, string Categoria, int Minutos, int Porciones,
    string Icono, string Descripcion, string[] Ingredientes, string[] Pasos);

public static class CatalogoRecetas
{
    private static readonly CompareInfo Comparador = CultureInfo.GetCultureInfo("es-CL").CompareInfo;
    private const CompareOptions Opciones = CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace;

    private static readonly Receta[] Catalogo =
    [
        new("ensalada-garbanzos", "Ensalada de garbanzos", "Frescas", 15, 2, "🥗",
            "Crujiente, fresca y aliñada con limón. Ideal para un almuerzo rápido.",
            ["1 taza de garbanzos cocidos", "1 pepino", "1 tomate", "½ limón", "Aceite de oliva y sal"],
            ["Pica el pepino y el tomate en cubos.", "Mezcla con los garbanzos escurridos.", "Aliña con limón, aceite y sal."]),
        new("pasta-pesto", "Pasta al pesto", "Principales", 25, 2, "🍝",
            "Pasta con albahaca fresca, nueces y un toque de parmesano.",
            ["200 g de pasta", "1 taza de albahaca", "¼ taza de nueces", "¼ taza de parmesano", "Aceite de oliva"],
            ["Cuece la pasta según el envase.", "Tritura la albahaca, las nueces y el parmesano con aceite.", "Mezcla el pesto con la pasta y un chorrito de agua de cocción."]),
        new("sopa-zapallo", "Crema de zapallo", "Principales", 40, 4, "🥣",
            "Una crema suave de zapallo y zanahoria para los días fríos.",
            ["600 g de zapallo", "1 zanahoria", "1 cebolla", "750 ml de caldo de verduras"],
            ["Pica las verduras y sofríe la cebolla.", "Agrega el zapallo, la zanahoria y el caldo. Cocina hasta que estén tiernos.", "Tritura hasta obtener una crema y ajusta la sal."]),
        new("tostadas-palta", "Tostadas con palta", "Frescas", 10, 2, "🥑",
            "Palta cremosa con limón y tomate sobre pan tostado.",
            ["1 palta", "4 rebanadas de pan", "1 tomate", "½ limón", "Sal y pimienta"],
            ["Tuesta el pan.", "Muele la palta con limón, sal y pimienta.", "Unta sobre el pan y agrega el tomate en rodajas."]),
        new("yogur-frutos", "Vasitos de yogur y frutos rojos", "Postres", 10, 2, "🫐",
            "Capas de yogur, fruta y granola para un postre fácil.",
            ["1 taza de yogur natural", "½ taza de frutos rojos", "¼ taza de granola", "1 cucharada de miel"],
            ["Reparte el yogur en dos vasos.", "Agrega fruta y miel.", "Termina con la granola justo antes de servir."]),
        new("manzana-canela", "Manzanas asadas con canela", "Postres", 35, 4, "🍎",
            "Manzanas tibias al horno con avena y canela.",
            ["4 manzanas", "4 cucharadas de avena", "1 cucharadita de canela", "2 cucharadas de miel"],
            ["Corta las manzanas por la mitad y quita las semillas.", "Rellena con avena, canela y miel.", "Hornea a 190 °C durante 25 a 30 minutos, hasta que estén blandas."])
    ];

    public static Receta[] Buscar(string? texto, string? categoria, int? maxMinutos)
    {
        var consulta = texto?.Trim() ?? "";
        var filtro = categoria?.Trim();

        // Where conserva las recetas que cumplen todos los filtros.
        return Catalogo.Where(receta =>
            (consulta.Length == 0 ||
             Contiene(receta.Nombre, consulta) ||
             receta.Ingredientes.Any(ingrediente => Contiene(ingrediente, consulta))) &&
            (string.IsNullOrEmpty(filtro) || Comparador.Compare(receta.Categoria, filtro, Opciones) == 0) &&
            (maxMinutos is null || receta.Minutos <= maxMinutos.Value))
            .ToArray();
    }

    private static bool Contiene(string texto, string consulta) =>
        Comparador.IndexOf(texto, consulta, Opciones) >= 0;
}
