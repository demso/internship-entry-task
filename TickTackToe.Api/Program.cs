using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using TickTackToe.Api.Data;
using TickTackToe.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration.GetConnectionString("Game");

builder.Services.Configure<JsonOptions>(options => options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddSqlite<GameContext>(connString);

var app = builder.Build();

app.MapGameEndpoints();

app.MigrateDb();

app.Run();