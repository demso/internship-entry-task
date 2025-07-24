using TickTackToe.Api.Endpoints;
using TickTackToe.Api.Handlers;

namespace TickTacToe.Tests;

[TestFixture]
public class Tests {
    [SetUp]
    public void Setup() { }

    [Test] //public static bool CheckVirticalWinCondition(int row, int col, bool playerType, int winCodition, int boardSize, bool[] board) 
    public void TestVerticalWinCondition() {
        Assert.That(GameHandler.CheckVerticalWinCondition(1, 1, true, 3, 3, new []{ 
            0, 1, 0, 
            0, 1, 0, 
            0, 1, 0 
        }));
        Assert.That(GameHandler.CheckVerticalWinCondition(2, 2, true, 3, 5, new []{ 
            0, 0, 1, 0, 1, 
            0, 0, 0, 0, 0,
            0, 0, 1, 0, 0,
            0, 0, 1, 0, 0,
            1, 0, 1, 0, 0 
        }));
    }

    [Test]
    public void TestHorizontalWinCondition() {
        Assert.That(GameHandler.CheckHorizontalWinCondition(1, 1, true, 3, 3, new []{ 
            0, 0, 0, 
            1, 1, 1, 
            0, 0, 0 
        }));
        Assert.That(GameHandler.CheckHorizontalWinCondition(2, 2, true, 4, 5, new []{ 
            0, 0, 1, 0, 1, 
            0, 0, 0, 0, 0,
            1, 1, 1, 1, 0,
            0, 0, 0, 0, 0,
            1, 0, 1, 0, 0 
        }));
    }
    
    [Test]
    public void TestDiagonalWinCondition() {
        Assert.That(GameHandler.CheckDiagonalWinCondition(1, 1, true, 3, 3, new []{ 
            1, 0, 0, 
            0, 1, 0, 
            0, 0, 1 
        }));
        Assert.That(GameHandler.CheckDiagonalWinCondition(1, 1, true, 3, 3, new []{ 
            0, 0, 1, 
            0, 1, 0, 
            1, 0, 1 
        }));
        Assert.That(GameHandler.CheckDiagonalWinCondition(2, 2, true, 4, 5, new []{ 
            1, 0, 1, 0, 1, 
            0, 1, 0, 0, 0,
            0, 0, 1, 1, 0,
            0, 0, 0, 1, 0,
            1, 0, 1, 0, 0 
        }));
        Assert.That(GameHandler.CheckDiagonalWinCondition(2, 2, true, 3, 5, new []{ 
            1, 0, 1, 0, 1, 
            0, 0, 0, 1, 0,
            0, 0, 1, 1, 0,
            0, 0, 0, 0, 0,
            1, 0, 1, 0, 0 
        }));
    }
}