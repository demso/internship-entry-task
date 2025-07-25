using TickTackToe.Api.Endpoints;
using TickTackToe.Api.Handlers;

namespace TickTacToe.Tests;

[TestFixture]
public class Tests {
    [SetUp]
    public void Setup() { }

    [Test] //public static bool CheckVirticalWinCondition(int row, int col, bool playerType, int winCodition, int boardSize, bool[] board) 
    public void TestVerticalWinCondition() {
        Assert.That(GameHandler.CheckVerticalWinCondition(1, 1, "X", 3, 3, new []{ 
            "", "X", "", 
            "O", "X", "", 
            "", "X", ""
        }));
        Assert.That(GameHandler.CheckVerticalWinCondition(2, 2, "X", 3, 5, new []{ 
            "",  "", "X", "", "X", 
            "O",  "", "",  "", "", 
            "",  "", "X", "", "", 
            "",  "", "X", "", "O", 
            "X", "", "X", "", ""
        }));
    }

    [Test]
    public void TestHorizontalWinCondition() {
        Assert.That(GameHandler.CheckHorizontalWinCondition(1, 1, "O", 3, 3, new []{ 
            "",  "",  "X",
            "O", "O", "O",
            "X",  "",  ""
        }));
        Assert.That(GameHandler.CheckHorizontalWinCondition(2, 2, "X", 4, 5, new []{ 
            "",  "",  "",  "O",  "",
            "",  "",  "",  "",  "",
            "X",  "X",  "X",  "X",  "",
            "",  "",  "",  "",  "",
            "",  "",  "",  "",  "" 
        }));
    }
    
    [Test]
    public void TestDiagonalWinCondition() {
        Assert.That(GameHandler.CheckDiagonalWinCondition(1, 1, "O", 3, 3, new []{ 
            "", "", "O",
            "", "O", "",
            "O", "", ""
        }));
        Assert.That(GameHandler.CheckDiagonalWinCondition(1, 1, "X", 3, 3, new []{ 
            "X", "", "X",
            "", "X", "",
            "O", "", "X"
        }));
        Assert.That(GameHandler.CheckDiagonalWinCondition(2, 2, "X", 4, 5, new []{ 
            "",  "",  "",  "",  "",
            "",  "",  "",  "X",  "",
            "",  "",  "X",  "",  "",
            "",  "X",  "",  "",  "",
            "X",  "",  "",  "",  ""
        }));
        Assert.That(GameHandler.CheckDiagonalWinCondition(2, 2,"X", 3, 5, new []{ 
            "", "", "", "", "",
            "", "X", "", "", "",
            "", "", "X", "", "",
            "", "", "", "X", "",
            "", "", "", "", ""
        }));
    }
}