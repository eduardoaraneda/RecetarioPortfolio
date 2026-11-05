# Mesa de diario

Buscador de recetas para portafolio con Angular 21, TypeScript y una API REST en C# con ASP.NET Core y .NET 10. Puedes buscar por nombre o ingrediente, combinar filtros de categoría y tiempo, y abrir los ingredientes y la preparación de cada receta.

El catálogo inicial incluye seis recetas en español. Vive en C# y funciona localmente con Node.js 24 y el SDK de .NET 10, sin servicios externos.

## Ejecutar en desarrollo

Desde esta carpeta, abre dos terminales.

**Terminal 1 · API**

```powershell
dotnet run --project backend/Recetario.Api --launch-profile http
```

**Terminal 2 · Angular**

```powershell
cd frontend
npm ci
npm start
```

Abre [Mesa de diario](http://localhost:4201). El proxy de Angular envía las llamadas a `/api` a ASP.NET Core en el puerto 5086. Prueba buscar **limon** sin tilde, elige **Frescas** y filtra por **15 minutos**.

## Código y guía

Abre `RecetarioPortfolio.slnx` en Visual Studio para estudiar y ejecutar la API con el perfil **http**. Angular se inicia desde la segunda terminal.

| Archivo | Responsabilidad |
| --- | --- |
| [app.html](frontend/src/app/app.html) | Plantilla del buscador, resultados y detalle. |
| [app.ts](frontend/src/app/app.ts) | Estado, eventos y ciclo de búsqueda. |
| [recetas-api.ts](frontend/src/app/recetas-api.ts) | Petición HTTP y tipo de respuesta. |
| [Program.cs](backend/Recetario.Api/Program.cs) | Configuración y ruta de la API. |
| [Recetario.cs](backend/Recetario.Api/Recetario.cs) | Modelo, catálogo y filtros. |

Lee la [guía de sintaxis de Angular y C#](docs/GUIA-DE-CODIGO.md) para seguir una búsqueda desde el formulario hasta el JSON de respuesta.

## Preparar una versión para alojar

Desde la raíz de este proyecto, compila ambos lados y copia Angular a la carpeta pública de la API publicada.

```powershell
npm --prefix frontend run build
dotnet publish backend/Recetario.Api -c Release -o dist/app
New-Item -ItemType Directory -Force dist/app/wwwroot
Copy-Item frontend/dist/recetario/browser/* dist/app/wwwroot -Recurse -Force
cd dist/app
dotnet Recetario.Api.dll --urls http://localhost:5086
```

Abre [localhost:5086](http://localhost:5086) para probar el paquete. Para una demo pública, aloja el contenido de `dist/app` en un servidor compatible con ASP.NET Core .NET 10. El portafolio estático puede enlazar a esa demo una vez que tenga URL.
