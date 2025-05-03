using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ProyectoSWebfront;
using ProyectoSWebfront.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Registra el componente App (definido en App.razor) como componente ra�z
// Lo asocia al elemento HTML con el id "app" en wwwroot/index.html
builder.RootComponents.Add<App>("#app");
// Registra el componente HeadOutlet despu�s del elemento <head> en el documento HTML
// Esto permite gestionar el contenido din�mico del encabezado HTML desde los componentes Blazor
// como t�tulos de p�gina o metadatos
builder.RootComponents.Add<HeadOutlet>("head::after");
// Este HttpClient se inicializa con la direcci�n base de la API gen�rica
// La URL base debe terminar con una barra "/" para que las rutas relativas funcionen correctamente
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("http://localhost:5281/api/")
});

builder.Services.AddScoped<ServicioEntidad>();
await builder.Build().RunAsync();
