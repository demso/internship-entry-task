using TickTackToe.Api.Endpoints;

namespace TickTacToe.Tests;

[TestFixture]
public class Tests {
    [SetUp]
    public void Setup() { }

    [Test] //public static bool CheckVirticalWinCondition(int row, int col, bool playerType, int winCodition, int boardSize, bool[] board) 
    public void TestVerticalWinCondition() {
        Assert.That(GameEndpoints.CheckVirticalWinCondition(1, 1, true, 3, 3, new []{ 
            0, 1, 0, 
            0, 1, 0, 
            0, 1, 0 
        }));
        Assert.That(GameEndpoints.CheckVirticalWinCondition(2, 2, true, 3, 5, new []{ 
            0, 0, 1, 0, 1, 
            0, 0, 0, 0, 0,
            0, 0, 1, 0, 0,
            0, 0, 1, 0, 0,
            1, 0, 1, 0, 0 
        }));
    }

    [Test]
    public void TestHorizontalWinCondition() {
        Assert.That(GameEndpoints.CheckHorizontalWinCondition(1, 1, true, 3, 3, new []{ 
            0, 0, 0, 
            1, 1, 1, 
            0, 0, 0 
        }));
        Assert.That(GameEndpoints.CheckHorizontalWinCondition(2, 2, true, 4, 5, new []{ 
            0, 0, 1, 0, 1, 
            0, 0, 0, 0, 0,
            1, 1, 1, 1, 0,
            0, 0, 0, 0, 0,
            1, 0, 1, 0, 0 
        }));
    }
    
    [Test]
    public void TestDiagonalWinCondition() {
        Assert.That(GameEndpoints.CheckDiagonalWinCondition(1, 1, true, 3, 3, new []{ 
            1, 0, 0, 
            0, 1, 0, 
            0, 0, 1 
        }));
        Assert.That(GameEndpoints.CheckDiagonalWinCondition(1, 1, true, 3, 3, new []{ 
            0, 0, 1, 
            0, 1, 0, 
            1, 0, 1 
        }));
        Assert.That(GameEndpoints.CheckDiagonalWinCondition(2, 2, true, 4, 5, new []{ 
            1, 0, 1, 0, 1, 
            0, 1, 0, 0, 0,
            0, 0, 1, 1, 0,
            0, 0, 0, 1, 0,
            1, 0, 1, 0, 0 
        }));
        Assert.That(GameEndpoints.CheckDiagonalWinCondition(2, 2, true, 3, 5, new []{ 
            1, 0, 1, 0, 1, 
            0, 0, 0, 1, 0,
            0, 0, 1, 1, 0,
            0, 0, 0, 0, 0,
            1, 0, 1, 0, 0 
        }));
    }
}