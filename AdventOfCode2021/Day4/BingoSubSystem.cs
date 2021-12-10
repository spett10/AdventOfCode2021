using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2021.Day4
{
	class BingoSubSystem
	{
		private readonly int[] _numbers;

		private readonly List<BingoBoard> _boards;

		public BingoSubSystem(string inputFileName)
		{
			_boards = new List<BingoBoard>();

			var lines = File.ReadAllLines(inputFileName);

			_numbers = lines.First().Split(",").Select(x => Convert.ToInt32(x)).ToArray();

			var allExceptNumbers = lines.Skip(2).ToList();
			var index = 0;
			const int rowsPerBoard = 5;

			var currentBoard = new BingoBoard(rowsPerBoard);

			foreach(var line in allExceptNumbers)
			{
				if((index + 1 )% 6 == 0)
				{
					_boards.Add(currentBoard);
					currentBoard = new BingoBoard(rowsPerBoard);

					index++;
					continue;
				}

				currentBoard.AddRow(line);

				index++;
			}

			/* add last board */
			_boards.Add(currentBoard);
		}

		public int PlayUntilFirstBoardWinsAndReportScore()
		{
			foreach(var number in _numbers)
			{
				foreach(var board in _boards)
				{
					var won = board.MarkAndCheck(number);
					if (won) return board.Score;
				}
			}

			return 0;
		}

		public int PlayUntilLastBoardWinsAndReportScore()
		{
			var boardCount = _boards.Count();
			var boardsWon = 0;

			foreach(var number in _numbers)
			{
				foreach(var board in _boards)
				{
					if (board.WonAlready) continue;

					var won = board.MarkAndCheck(number);
					if (won)
					{
						boardsWon++;

						if(boardsWon == boardCount)
						{
							/* Last board finally won */
							return board.Score;
						}
					}
				}
			}

			return 0;
		}

	}
}
