using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using TickTackToe.Api.Data;
using TickTackToe.Api.Endpoints;
using TickTackToe.Api.Interfaces;
using TickTackToe.Api.Repositories;
using TickTackToe.Api.Services;

var builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration.GetConnectionString("Game");

builder.Services
    .Configure<JsonOptions>(options => options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()))
    .AddSqlite<GameContext>(connString)
    .AddScoped<IGameRepositoryAsync, GameRepositoryAsync>()
    .AddSingleton<IETagService, ETagService>()
    .AddSingleton<IGameService, GameService>();

var app = builder.Build();

app.MapGameEndpoints();

await app.MigrateDbAsync();

app.Run();

public partial class Program {}