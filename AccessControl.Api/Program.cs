using AccessControl.Api.Middlewares;
using AccessControl.Application;
using AccessControl.Persistence;
using AccessControl.Persistence.Seeds;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddApplicationServices();
builder.Services.AddPersistenceServices(builder.Configuration);

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    // Interfaz Swagger UI sobre el documento OpenAPI generado por .NET (/openapi/v1.json)
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "AccessControl.Api v1");
    });
}

await DataBaseSeeder.SeedAsync(app.Services);

// Traduce las excepciones que escapan de los casos de uso (p. ej. validaciones de FluentValidation
// lanzadas por el SimpleMediator) al mismo formato ApiResponse que usan los controladores
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
