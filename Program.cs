using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.UseSwagger();

if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference(options =>
    {
        options.WithOpenApiRoutePattern("/swagger/v1/swagger.json");
    });
}

app.Run();
