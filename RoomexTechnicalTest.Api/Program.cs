using Microsoft.AspNetCore.Mvc.ApiExplorer;
using RoomexTechnicalTest.Api.Behaviors;
using RoomexTechnicalTest.Application;
using RoomexTechnicalTest.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationServices();
builder.Services.AddValidationBehaviorService();

builder.Services.AddApiVersioning();
builder.Services.AddVersionedApiExplorer(options => {
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

var app = builder.Build();

var apiVersionDescriptionProvider = app.Services.GetService<IApiVersionDescriptionProvider>();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();

    app.UseSwaggerUI(options => {
        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions) {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
        }
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCustomExceptionHandler();

app.Run();

public partial class Program { }