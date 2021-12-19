using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.Day11
{
	class CavernOfOctopi
	{
		private int[,] _energyLevels;

		private int _rows;
		private int _columns;

		private int _flashCount;

		private bool _debugPrint = false;

		public CavernOfOctopi(List<string> energyLevels)
		{
			_rows = energyLevels.Count;
			_columns = energyLevels.First().Length;

			_energyLevels = new int[_rows, _columns];

			for(int row = 0; row < _rows; row++)
			{
				var levels = energyLevels[row].ToCharArray().Select(x => Convert.ToInt32(x.ToString())).ToArray();

				for(int column = 0; column < _columns; column++)
				{
					_energyLevels[row, column] = levels[column];
				}
			}

			_flashCount = 0;
		}

		private void AdvanceDay()
		{
			for(int row = 0; row < _rows; row++)
			{
				for(int column = 0; column < _columns; column++)
				{
					_energyLevels[row, column] += 1;
				}
			}
		}

		private void PrintState()
		{
			if (!_debugPrint) return;

			Console.WriteLine("\n");
			for(int row = 0; row < _rows; row++)
			{
				var currentRow = "";
				for(int column = 0; column < _columns; column++)
				{
					currentRow += _energyLevels[row, column].ToString();
				}
				Console.WriteLine(currentRow);
			}
			Console.WriteLine($"FlashCount = {_flashCount}");

			Console.WriteLine("\n");
		}

		public int FlashCountAfterDays(int days)
		{
			if (days < 1) throw new ArgumentException($"{nameof(days)} must be positive.");


			for(int day = 0; day < days; day++)
			{
				var hashFlashedThisDay = new HashSet<Tuple<int, int>>();

				PrintState();

				// All increment
				AdvanceDay();

				while (CanAnyFlash(hashFlashedThisDay))
				{
					TryFlashAll(hashFlashedThisDay);
				}

				ResetAllThatFlashed(hashFlashedThisDay);
			}

			return _flashCount;
		}

		private void FlashCount()
		{
			_flashCount++;
		}

		private void TryFlashAll(HashSet<Tuple<int, int>> hasFlashed)
		{
			for (int row = 0; row < _rows; row++)
			{
				for (int column = 0; column < _columns; column++)
				{
					if (_energyLevels[row, column] > 9) TryFlash(row, column, hasFlashed);
				}
			}
		}

		private void ResetAllThatFlashed(HashSet<Tuple<int, int>> hasFlashed)
		{
			foreach(var point in hasFlashed)
			{
				_energyLevels[point.Item1, point.Item2] = 0;
			}
		}

		private void TryFlash(int row, int column, HashSet<Tuple<int, int>> hasFlashed)
		{
			var current = new Tuple<int, int>(row, column);

			if (hasFlashed.Contains(current)) 
			{
				return; // can only flash once. 
			}

			hasFlashed.Add(current);
			_energyLevels[row, column] = 0;


			FlashCount();

			if(IsMiddlePoint(row, column))
			{
				IncrementUpperLeft(row, column);
				IncrementUpperMiddle(row, column);
				IncrementUpperRight(row, column);

				IncrementMiddleLeft(row, column);
				IncrementMiddleRight(row, column);

				IncrementLowerLeft(row, column);
				IncrementLowerMiddle(row, column);
				IncrementLowerRight(row, column);
			}
			else if(IsEdgePoint(row, column))
			{
				if(IsTopRow(row))
				{
					IncrementMiddleLeft(row, column);
					IncrementMiddleRight(row, column);

					IncrementLowerLeft(row, column);
					IncrementLowerMiddle(row, column);
					IncrementLowerRight(row, column);
				}
				else if(IsBottomRow(row))
				{
					IncrementMiddleLeft(row, column);
					IncrementMiddleRight(row, column);

					IncrementUpperLeft(row, column);
					IncrementUpperMiddle(row, column);
					IncrementUpperRight(row, column);
				}
				else if(IsFirstColumn(column))
				{
					IncrementUpperMiddle(row, column);
					IncrementUpperRight(row, column);

					IncrementMiddleRight(row, column);

					IncrementLowerMiddle(row, column);
					IncrementLowerRight(row, column);
				}
				else if(IsLastColumn(column))
				{
					IncrementUpperLeft(row, column);
					IncrementUpperMiddle(row, column);

					IncrementMiddleLeft(row, column);

					IncrementLowerLeft(row, column);
					IncrementLowerMiddle(row, column);
				}
			}
			else if(IsCornerPoint(row, column))
			{
				if(IsTopRow(row))
				{
					if(IsFirstColumn(column))
					{
						IncrementMiddleRight(row, column);
						IncrementLowerRight(row, column);
						IncrementLowerMiddle(row, column);
					}
					else if(IsLastColumn(column))
					{
						IncrementMiddleLeft(row, column);
						IncrementLowerLeft(row, column);
						IncrementLowerMiddle(row, column);
					}
				}
				else if(IsBottomRow(row))
				{
					if(IsFirstColumn(column))
					{
						IncrementUpperMiddle(row, column);
						IncrementUpperRight(row, column);
						IncrementMiddleRight(row, column);
					}
					else if(IsLastColumn(column))
					{
						IncrementMiddleLeft(row, column);
						IncrementUpperLeft(row, column);
						IncrementUpperMiddle(row, column);
					}
				}
			}
		}

		private bool IsEdgePoint(int row, int column)
		{
			return (
				(IsTopRow(row) || IsBottomRow(row)) && !IsFirstColumn(column) && !IsLastColumn(column)) ||
					((IsFirstColumn(column) || IsLastColumn(column)) && !IsTopRow(row) && !IsBottomRow(row));
		}

		private bool IsTopRow(int row)
		{
			return row == 0;
		}

		private bool IsBottomRow(int row)
		{
			return row == _rows - 1;
		}

		private bool IsFirstColumn(int column)
		{
			return column == 0;
		}

		private bool IsLastColumn(int column)
		{
			return column == _columns - 1;
		}

		private bool IsCornerPoint(int row, int column)
		{
			return (IsTopRow(row) && IsFirstColumn(column)) ||
				   (IsTopRow(row) && IsLastColumn(column)) ||
				   (IsBottomRow(row) && IsFirstColumn(column) ||
				   (IsBottomRow(row) && IsLastColumn(column)));
		}

		private bool IsMiddlePoint(int row, int column)
		{
			return !IsEdgePoint(row, column) && !IsCornerPoint(row, column);
		}

		private bool CanAnyFlash(HashSet<Tuple<int, int>> hashFlashedThisDay)
		{
			for (int row = 0; row < _rows; row++)
			{
				for (int column = 0; column < _columns; column++)
				{
					if (_energyLevels[row, column] > 9 && 
						!hashFlashedThisDay.Contains(new Tuple<int, int>(row, column))) return true;
				}
			}

			return false;
		}

		private void IncrementUpperLeft(int row, int column) => _energyLevels[row - 1, column - 1] += 1;
		private void IncrementUpperMiddle(int row, int column) => _energyLevels[row - 1, column] += 1;
		private void IncrementUpperRight(int row, int column) => _energyLevels[row - 1, column + 1] += 1;

		private void IncrementMiddleLeft(int row, int column) => _energyLevels[row, column - 1] += 1;
		private void IncrementMiddleRight(int row, int column) => _energyLevels[row, column + 1] += 1;

		private void IncrementLowerLeft(int row, int column) => _energyLevels[row + 1, column - 1] += 1;
		private void IncrementLowerMiddle(int row, int column) => _energyLevels[row + 1, column] += 1;
		private void IncrementLowerRight(int row, int column) => _energyLevels[row + 1, column + 1] += 1;
	}
}
