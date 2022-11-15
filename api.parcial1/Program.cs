using api.parcial1.Data;
using api.parcial1.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("PostgreSQLConnection");

builder.Services.AddDbContext<DbPrimerParcial>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapPost("/clientes/", async (Clientes clientes, DbPrimerParcial db) =>
{
    db.Clientes.Add(clientes);
    await db.SaveChangesAsync();

    return Results.Created($"/clientes/{clientes.Id}", clientes);
});

app.MapGet("/clientes/{id:int}", (int id, DbPrimerParcial db) =>
{

    var query = db.Clientes
                           .Where(s => s.Id == id)
                           .Include(s => s.Ciudades)
                           .FirstOrDefault();

    return Task.FromResult(query);
});

app.MapGet("/ListClientes/", (DbPrimerParcial db) =>
{
    var query = db.Clientes.Include(s => s.Ciudades).ToList();

    return Task.FromResult(query);
});

app.MapGet("/ListCiudades/", (DbPrimerParcial db) =>
{
    var query = db.Ciudades.ToList();

    return Task.FromResult(query);
});

app.MapPut("/clientes/{id:int}", async (int id, Clientes inputTodo, DbPrimerParcial db) =>
{
    var todo = await db.Clientes.FindAsync(id);

    if (todo is null) return Results.NotFound();

    todo.IdCiudad = inputTodo.IdCiudad;
    todo.Nombres = inputTodo.Nombres;
    todo.Apellidos = inputTodo.Apellidos;
    todo.Documento = inputTodo.Documento;
    todo.Telefono = inputTodo.Telefono;
    todo.Email = inputTodo.Email;
    todo.FechaNacimiento = inputTodo.FechaNacimiento;
    todo.Ciudad = inputTodo.Ciudad;
    todo.Nacionalidad = inputTodo.Nacionalidad;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/clientes/{id}", async (int id, DbPrimerParcial db) =>
{
    if (await db.Clientes.FindAsync(id) is Clientes todo)
    {
        db.Clientes.Remove(todo);
        await db.SaveChangesAsync();
        return Results.Ok(todo);
    }

    return Results.NotFound();
});

app.MapPost("/ciudades/", async (Ciudades ciudades, DbPrimerParcial db) =>
{
    db.Ciudades.Add(ciudades);
    await db.SaveChangesAsync();

    return Results.Created($"/ciudades/{ciudades.Id}", ciudades);
}); 

app.MapGet("/ciudades/{id:int}", (int id, DbPrimerParcial db) =>
{
    /*return await db.Clientes.FindAsync(id)
              is Clientes cliente ? Results.Ok(cliente) : Results.NotFound();*/

    var ciudad = db.Ciudades
                           .Where(s => s.Id == id)
                           .Include(s => s.Clientes)
                           .FirstOrDefault();

    return Task.FromResult(ciudad);
});

app.MapPut("/ciudades/{id:int}", async (int id, Ciudades inputTodo, DbPrimerParcial db) =>
{
    var todo = await db.Ciudades.FindAsync(id);

    if (todo is null) return Results.NotFound();

    todo.Ciudad = inputTodo.Ciudad;
    todo.Estado = inputTodo.Estado;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/ciudades/{id}", async (int id, DbPrimerParcial db) =>
{
    if (await db.Ciudades.FindAsync(id) is Ciudades todo)
    {
        db.Ciudades.Remove(todo);
        await db.SaveChangesAsync();
        return Results.Ok(todo);
    }

    return Results.NotFound();
});

app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}