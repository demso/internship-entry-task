using TickTackToe.Api.Data;
using TickTackToe.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration.GetConnectionString("Game");
builder.Services.AddSqlite<GameContext>(connString);
//builder.Services.AddScoped<GameContext>

var app = builder.Build();

app.MapGameEndpoints();

app.MigrateDb();

app.Run();