using System.Security.Cryptography;
using System.Text;
using TickTackToe.Api.Entities.Game;

namespace TickTackToe.Api.Services;

internal static class ETagService
{
    public static string GenerateETag(Game game)
    {
        var content = $"{game.Id}{game.Board}";
        var bytes = Encoding.UTF8.GetBytes(content);
        var hash = SHA256.HashData(bytes);
        return $"\"{Convert.ToBase64String(hash)}\"";
    }
}