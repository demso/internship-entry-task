using TickTackToe.Api.Entities.Game;

namespace TickTackToe.Api.Enums;
//2:9:31
public class Move {
    public int Id { get; set; } 
    public int GameId { get; set; }
    public Game? Game { get; set; }
    public Player Player { get; set; }
    public int Row { get; set; }
    public int Column { get; set; }
}