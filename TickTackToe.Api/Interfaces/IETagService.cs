using TickTackToe.Api.Entities.Game;

namespace TickTackToe.Api.Interfaces;

public interface IETagService {
    public string GenerateETag(Game game);
}