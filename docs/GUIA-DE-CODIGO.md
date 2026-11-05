# Cómo funciona Mesa de diario

## 1. Angular lee el formulario

En [app.html](../frontend/src/app/app.html), `#texto` es una referencia al elemento HTML. `texto.value` contiene lo que escribiste. Los paréntesis en `(submit)` vinculan un evento del navegador al código de Angular.

```html
<form (submit)="$event.preventDefault(); buscar(texto.value, categoria.value, tiempo.value)">
```

`$event` es el evento del formulario. `preventDefault()` evita la recarga de la página. Después, la plantilla llama a `buscar` en la clase `App`.

## 2. TypeScript guarda el estado y llama a la API

En [app.ts](../frontend/src/app/app.ts), `@Component({...})` conecta la clase con su plantilla. `inject(RecetasApi)` obtiene el servicio de Angular.

```typescript
readonly recetas = signal<Receta[]>([]);
```

`Receta[]` es un arreglo de recetas y `[]` es su valor inicial. Lees la señal con `recetas()` y escribes con `recetas.set(nuevosResultados)`. `readonly` protege la referencia a la señal. Su valor se puede actualizar.

`Subject` recibe cada búsqueda y `switchMap` cancela la suscripción a la anterior si todavía está pendiente. `catchError` convierte un fallo HTTP en un estado que la pantalla puede mostrar. La suscripción final actualiza las señales.

En [recetas-api.ts](../frontend/src/app/recetas-api.ts), `HttpParams` codifica los filtros como parámetros de la URL. `HttpClient.get<Receta[]>()` indica a TypeScript qué estructura espera el frontend. El servidor entrega esos datos como JSON.

## 3. C# recibe y aplica los filtros

[Program.cs](../backend/Recetario.Api/Program.cs) registra la ruta con `MapGet`. ASP.NET Core toma los parámetros de la URL. En `string?` e `int?`, el signo `?` permite recibir `null` cuando el parámetro se omite. `Results.Ok(...)` devuelve HTTP 200 y serializa los datos como JSON.

En [Recetario.cs](../backend/Recetario.Api/Recetario.cs), `record` declara el modelo de datos. Esta expresión LINQ revisa si algún ingrediente coincide con el texto:

```csharp
receta.Ingredientes.Any(ingrediente => Contiene(ingrediente, consulta))
```

`ingrediente => ...` es una función lambda. Recibe un ingrediente y devuelve `true` si coincide. `Where` conserva las recetas que cumplen todos los filtros activos y `ToArray()` materializa el resultado. `CompareInfo` permite que **limon** coincida con **limón**.

## 4. Angular muestra la respuesta

```html
@for (receta of recetas(); track receta.id) {
  <h3>{{ receta.nombre }}</h3>
}
```

`@for` crea un bloque por receta. `track receta.id` identifica cada elemento por su ID. `{{ receta.nombre }}` muestra texto. `@if` y `@switch` eligen qué mostrar según el estado de la búsqueda. En `[attr.aria-label]`, los corchetes vinculan un atributo HTML con una expresión.

## Practicar con el código

1. Agrega una receta al arreglo `Catalogo` con un ID único y prueba encontrarla por ingrediente.
2. Agrega una opción de 45 minutos en el selector y verifica que el filtro funcione. El backend ya acepta cualquier tiempo máximo entero positivo.
3. Con la API en ejecución, abre [una búsqueda de ejemplo](http://localhost:5086/api/recetas?q=limon&categoria=Frescas&maxMinutos=15) y compara su JSON con la interfaz `Receta` en TypeScript.

[Plantillas de Angular](https://angular.dev/essentials/templates) · [HttpClient](https://angular.dev/guide/http/making-requests) · [Minimal APIs de ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis?view=aspnetcore-10.0)
