using System;
using System.Linq;

namespace AdventOfCode2021.Day4
{
	class BingoBoard
	{
		private readonly int[][] _board;
		private readonly bool[][] _checks;
		private int rowCount;

		public int Score { get; private set; }

		public BingoBoard(int numberOfRows)
		{
			_board = new int[numberOfRows][];
			_checks = new bool[numberOfRows][];
			rowCount = 0;

			Score = 0;
		}

		public void AddRow(string row)
		{
			_board[rowCount] = row.Replace("  ", " ").Split(" ").Where(x => !string.IsNullOrEmpty(x)).Select(x => Convert.ToInt32(x)).ToArray();

			_checks[rowCount] = new bool[_board[rowCount].Length]; //bool inits to false

			rowCount++;
		}

		public bool MarkAndCheck(int number)
		{
			for(int rows = 0; rows < _board.Length; rows++)
			{
				for (int j = 0; j < rowCount; j++)
				{
					if (number == _board[rows][j])
					{
						_checks[rows][j] = true;
					}
				}
			}

			var winning = Winning();

			if (winning)
			{
				Score = number * SumAllUnmarked();
			}

			return winning;
		}

		private int SumAllUnmarked()
		{
			int sum = 0;

			for(int rows = 0; rows < _board.Length; rows++)
			{
				for (int j = 0; j < rowCount; j++)
				{
					if (!_checks[rows][j])
					{
						sum += _board[rows][j];
					}
				}
			}

			return sum;
		}

		private bool Winning()
		{
			bool rowAllTrue = true;

			for (int rows = 0; rows < _board.Length; rows++) 
			{
				for(int j = 0; j < rowCount; j++)
				{
					rowAllTrue = rowAllTrue && _checks[rows][j];
				}

				if (rowAllTrue) return true;
				else rowAllTrue = true;
			}

			bool columnAllTrue = true;

			for(int columns = 0; columns < rowCount; columns++)
			{
				for (int j = 0; j < _board.Length; j++)
				{
					columnAllTrue = columnAllTrue && _checks[j][columns];
				}

				if (columnAllTrue) return true;
				else columnAllTrue = true;
			}

			return false;
		}
	}
}
