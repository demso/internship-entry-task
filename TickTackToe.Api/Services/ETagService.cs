using System.Security.Cryptography;
using System.Text;
using TickTackToe.Api.Entities.Game;
using TickTackToe.Api.Interfaces;

namespace TickTackToe.Api.Services;

internal class ETagService : IETagService
{
    public string GenerateETag(Game game)
    {
        var content = $"{game.Id}{game.Board}";
        var bytes = Encoding.UTF8.GetBytes(content);
        var hash = SHA256.HashData(bytes);
        return $"\"{Convert.ToBase64String(hash)}\"";
    }
}