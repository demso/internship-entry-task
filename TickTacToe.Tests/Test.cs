using TickTackToe.Api.Handlers;

namespace TickTacToe.Tests;

[TestFixture]
public class Test {
    [SetUp]
    public void Setup() { }

    [Test] 
    public void TestVerticalWinCondition() {
        
        Assert.That(GameHandler.CheckVerticalWinCondition(1, 1, "X", 3, 3, new string[][]{ 
            ["", "X", ""], 
            ["O", "X", ""], 
            ["", "X", ""]
        }));
        Assert.That(GameHandler.CheckVerticalWinCondition(2, 2, "X", 3, 5, new string[][]{ 
            ["",  "", "X", "", "X"], 
            ["O",  "", "",  "", ""], 
            ["",  "", "X", "", ""], 
            ["",  "", "X", "", "O"], 
            ["X", "", "X", "", ""]
        }));
    }

    [Test]
    public void TestHorizontalWinCondition() {
        Assert.That(GameHandler.CheckHorizontalWinCondition(1, 1, "O", 3, 3, new string[][]{ 
            ["",  "",  "X"],
            ["O", "O", "O"],
            ["X",  "",  ""]
        }));
        Assert.That(GameHandler.CheckHorizontalWinCondition(2, 2, "X", 4, 5, new string[][]{ 
            ["",  "",  "",  "O",  ""],
            ["",  "",  "",  "",  ""],
            ["X",  "X",  "X",  "X",  ""],
            ["",  "",  "",  "",  ""],
            ["",  "",  "",  "",  ""] 
        }));
    }
    
    [Test]
    public void TestDiagonalWinCondition() {
        Assert.That(GameHandler.CheckDiagonalWinCondition(1, 1, "O", 3, 3, new string[][]{ 
            ["", "", "O"],
            ["", "O", ""],
            ["O", "", ""]
        }));
        Assert.That(GameHandler.CheckDiagonalWinCondition(1, 1, "X", 3, 3, new string[][]{ 
            ["X", "", "X"],
            ["", "X", ""],
            ["O", "", "X"]
        }));
        Assert.That(GameHandler.CheckDiagonalWinCondition(2, 2, "X", 4, 5, new string[][]{ 
            ["",  "",  "",  "",  ""],
            ["",  "",  "",  "X",  ""],
            ["",  "",  "X",  "",  ""],
            ["",  "X",  "",  "",  ""],
            ["X",  "",  "",  "",  ""]
        }));
        Assert.That(GameHandler.CheckDiagonalWinCondition(2, 2,"X", 3, 5, new string[][]{ 
            ["", "", "", "", ""],
            ["", "X", "", "", ""],
            ["", "", "X", "", ""],
            ["", "", "", "X", ""],
            ["", "", "", "", ""]
        }));
    }

    [Test]
    public void OtherTests() {
        Assert.That(GameHandler.CheckDiagonalWinCondition(2, 1, "O", 3, 3, new string[][]{ 
            ["X", "X", ""], ["O", "X", ""], ["O", "O", ""]
        }));
    }
}