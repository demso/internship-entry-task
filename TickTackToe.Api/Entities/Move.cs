using TickTackToe.Api.Enums;

namespace TickTackToe.Api.Entities;

public class Move {
    public int Id { get; set; } 
    public int GameId { get; set; }
    public Game.Game? Game { get; set; }
    public Player Player { get; set; }
    public int Row { get; set; }
    public int Column { get; set; }
}