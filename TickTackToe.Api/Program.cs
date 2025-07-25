using TickTackToe.Api.Data;
using TickTackToe.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration.GetConnectionString("Game");
builder.Services.AddSqlite<GameContext>(connString);

var app = builder.Build();

app.MapGameEndpoints();

app.Run();