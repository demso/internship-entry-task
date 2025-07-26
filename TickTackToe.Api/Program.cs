using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using TickTackToe.Api.Data;
using TickTackToe.Api.Endpoints;
using TickTackToe.Api.Interfaces;
using TickTackToe.Api.Repositories;

var builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration.GetConnectionString("Game");

builder.Services.Configure<JsonOptions>(options => options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddSqlite<GameContext>(connString);
builder.Services.AddScoped<IGameRepositoryAsync, GameRepositoryAsync>();

var app = builder.Build();

app.MapGameEndpoints();

await app.MigrateDbAsync();

app.Run();