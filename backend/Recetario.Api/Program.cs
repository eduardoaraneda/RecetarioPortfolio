using Recetario.Api;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Cuando existe wwwroot, ASP.NET Core sirve también la compilación de Angular.
if (Directory.Exists(app.Environment.WebRootPath))
{
    app.UseDefaultFiles();
    app.UseStaticFiles();
}

// Los parámetros con ? son opcionales. ASP.NET Core los lee de la URL.
app.MapGet("/api/recetas", (string? q, string? categoria, int? maxMinutos) =>
{
    if (q?.Length > 100 || maxMinutos is <= 0)
    {
        return Results.BadRequest(new { mensaje = "Revisa el texto de búsqueda y el tiempo máximo." });
    }

    return Results.Ok(CatalogoRecetas.Buscar(q, categoria, maxMinutos));
});

app.Run();
