new GameBoard().RunGame();

class GameBoard
{
	public int[,] testBoard = new int[,] { { 0, 0, 0, 0}, { 0, 0, 0, 0}, { 0, 0, 0, 0}, { 0, 0, 0, 0} };

	public void RunGame()
	{
		SetOpenTile();
		SetOpenTile();
		Console.Clear();
		Console.WriteLine(ToString());
		var gameLoop = true;
		while (gameLoop)
		{

			Console.Write("move: ");
			var direction = Console.ReadLine();
			var spawnNewTile = false;

			switch(direction)
			{
				case "l":
					spawnNewTile = MoveLeft();
					break;
				case "r":
					spawnNewTile = MoveRight();
					break;
				case "d":
					spawnNewTile = MoveDown();
					break;
				case "u":
					spawnNewTile = MoveUp();
					break;
				case "x":
					gameLoop = false;
					return;
			}

			if (spawnNewTile) SetOpenTile();
			Console.Clear();
			Console.WriteLine(ToString());
			gameLoop = OpenTileExists() || PlayableMoveExists();
		}
		Console.WriteLine("Game Over");
	}

	public override string ToString() {
		string board = "";

		for (int i = 0; i < 4; i++)
		{
			board += "-------------------------\n";
			for (int j = 0; j < 4; j++)
			{
				var tileValue = testBoard[i,j].ToString().PadLeft(4, ' ');
				board += $"|{tileValue} ";
			}
			board += "|\n";
		}
		board += "-------------------------\n";

		return board;
	}

	public List<(int, int)> GetOpenTiles()
	{
		List<(int, int)> openTiles = [];

		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				if (testBoard[i,j] == 0)
				{
					openTiles.Add((i, j));
				}
			}
		}

		return openTiles;
	}

	public void SetOpenTile()
	{
		var openTiles = GetOpenTiles();
		var r = new Random();
		var tile = openTiles[r.Next(0, openTiles.Count())];
		testBoard[tile.Item1, tile.Item2] = (int)r.Next(1, 3) * 2;
	}

	public void UglyUpThePlace()
	{
		var r = new Random();

		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				testBoard[i,j] = r.Next(0,3) * 2;
			}
		}
	}

	public bool MoveLeft()
	{
		var moveCompleted = false;
		for (int i = 0; i < 4; i++)
			for (int j = 0; j < 3; j++)
			{
				if (
					(testBoard[i,j] != 0 && testBoard[i,j] == testBoard[i,j+1]) ||
					(testBoard[i,j] == 0 && testBoard[i,j+1] != 0))
				{
					testBoard[i,j] += testBoard[i,j+1];
					testBoard[i,j+1] = 0;
					moveCompleted = true;
				}
			}
		return moveCompleted;
	}

	public bool MoveRight()
	{
		var moveCompleted = false;
		for (int i = 0; i < 4; i++)
			for (int j = 3; j > 0; j--)
			{
				if (
					(testBoard[i,j] != 0 && testBoard[i,j] == testBoard[i,j-1]) ||
					(testBoard[i,j] == 0 && testBoard[i,j-1] != 0))
				{
					testBoard[i,j] += testBoard[i,j-1];
					testBoard[i,j-1] = 0;
					moveCompleted = true;
				}
			}
		return moveCompleted;
	}


	public bool MoveUp()
	{
		var moveCompleted = false;
		for (int i = 0; i < 3; i++)
			for (int j = 0; j < 4; j++)
			{
				if (
					(testBoard[i,j] == 0 && testBoard[i+1,j] != 0) ||
					(testBoard[i,j] != 0 && testBoard[i,j] == testBoard[i+1,j]))
				{
					testBoard[i,j] += testBoard[i+1,j];
					testBoard[i+1,j] = 0;
					moveCompleted = true;
				}
			}
		return moveCompleted;
	}

	public bool MoveDown()
	{
		var moveCompleted = false;
		for (int i = 3; i > 0; i--)
			for (int j = 0; j < 4; j++)
			{
				if (
					(testBoard[i,j] == 0 && testBoard[i-1,j] != 0) ||
					(testBoard[i,j] != 0 && testBoard[i,j] == testBoard[i-1,j]))
				{
					testBoard[i,j] += testBoard[i-1,j];
					testBoard[i-1,j] = 0;
					moveCompleted = true;
				}
			}
		return moveCompleted;
	}

	public bool OpenTileExists() => testBoard.Cast<int>().Any(o => o == 0);

	public bool PlayableMoveExists()
	{
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				if ((testBoard[i,j] != 0 && testBoard[i,j] == testBoard[i+1,j]) ||
					(testBoard[j,i] != 0 && testBoard[j,i] == testBoard[j,i+1]))
				{
					return true;
				}
			}

		}
		return false;
	}
}
