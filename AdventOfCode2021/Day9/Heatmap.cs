using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2021.Day9
{
	class Heatmap
	{
		private int[,] _map;
		private int _rows;
		private int _columns;


		public Heatmap(List<string> lines)
		{
			_columns = lines.First().Length;
			_rows = lines.Count;

			_map = new int[_rows, _columns];

			for (int row = 0; row < _rows; row++)
			{
				var lineAsChars = lines[row].ToCharArray();

				for (int column = 0; column < _columns; column++)
				{
					var c = lineAsChars[column].ToString();
					_map[row, column] = Convert.ToInt32(c);
				}
			}
		}

		public int FindLowPoints()
		{
			List<Tuple<int, int>> lowPointCoordinates = new List<Tuple<int, int>>();

			for (int row = 0; row < _rows; row++)
			{
				for (int column = 0; column < _columns; column++)
				{
					// We check outlies first to ensure we are not looking out of bounds. But branch prediction woul fail most times here since most points are 
					// not corners or edges. 
					if (IsCornerLowPoint(row, column))
					{
						lowPointCoordinates.Add(new Tuple<int, int>(row, column));
					}
					else if (IsEdgeLowPoint(row, column))
					{
						lowPointCoordinates.Add(new Tuple<int, int>(row, column));
					}
					else
					{
						if (IsLowPoint(row, column))
						{
							lowPointCoordinates.Add(new Tuple<int, int>(row, column));
						}
					}
				}
			}

			/* we could have done this while visiting coordinates above and not need the array but i liked it for debugging */
			var riskLevel = 0;
			foreach(var coordinate in lowPointCoordinates)
			{
				var height = _map[coordinate.Item1, coordinate.Item2];
				riskLevel += height + 1;
			}

			return riskLevel;
		}


		private bool IsLowPoint(int rows, int columns)
		{
			var value = _map[rows, columns];

			/* we are not a "middle" point */
			if (rows == 0 || rows == _rows - 1 || columns == 0 || columns == _columns - 1) return false;

			if (value >= UpperLeft(rows, columns)) return false;

			if (value >= UpperMiddle(rows, columns)) return false;

			if (value >= UpperRight(rows, columns)) return false;

			if (value >= MiddleLeft(rows, columns)) return false;

			if (value >= MiddleRight(rows, columns)) return false;

			if (value >= LowerLeft(rows, columns)) return false;

			if (value >= LowerMiddle(rows, columns)) return false;

			if (value >= LowerRight(rows, columns)) return false;

			return true;
		}

		private bool IsCornerLowPoint(int rows, int columns)
		{
			var value = _map[rows, columns];

			/* top left */
			if (rows == 0 && columns == 0)
			{
				var middleRight = MiddleRight(rows, columns);
				var lowerRight = LowerRight(rows, columns);
				var lowerMiddle = LowerMiddle(rows, columns);

				if (value < middleRight && value < lowerRight && value < lowerMiddle) return true;
			}

			/* top right */
			if (rows == 0 && columns == _columns - 1)
			{
				var middleLeft = MiddleLeft(rows, columns);
				var lowerLeft = LowerLeft(rows, columns);
				var lowerMiddle = LowerMiddle(rows, columns);

				if (value < middleLeft && value < lowerLeft && value < lowerMiddle) return true;
			}

			/* bottom left */
			if (rows == _rows - 1 && columns == 0)
			{
				var upperMiddle = UpperMiddle(rows, columns);
				var upperRight = UpperRight(rows, columns);
				var middleRight = MiddleRight(rows, columns);

				if (value < upperMiddle && value < upperRight && value < middleRight) return true;
			}

			/* bottom right */
			if (rows == _rows - 1 && columns == _columns - 1)
			{
				var middleLeft = MiddleLeft(rows, columns);
				var upperLeft = UpperLeft(rows, columns);
				var upperMiddle = UpperMiddle(rows, columns);

				if (value < middleLeft && value < upperLeft && value < upperMiddle) return true;
			}

			return false;
		}

		/* not checking corners */
		private bool IsEdgeLowPoint(int rows, int columns)
		{
			var value = _map[rows, columns];

			/* top row */
			if (rows == 0 && columns > 0 && columns < _columns - 1)
			{
				var middleLeft = MiddleLeft(rows, columns);
				var middleRight = MiddleRight(rows, columns);
				var lowerLeft = LowerLeft(rows, columns);
				var lowerMiddle = LowerMiddle(rows, columns);
				var lowerRight = LowerRight(rows, columns);

				if (value < middleLeft &&
					value < middleRight &&
					value < lowerLeft &&
					value < lowerMiddle &&
					value < lowerRight) return true;
			}

			/* bottom row */
			if (rows == _rows - 1 && columns > 0 && columns < _columns - 1)
			{
				var middleLeft = MiddleLeft(rows, columns);
				var middleRight = MiddleRight(rows, columns);
				var upperLeft = UpperLeft(rows, columns);
				var upperMiddle = UpperMiddle(rows, columns);
				var upperRight = UpperRight(rows, columns);

				if (value < middleLeft &&
					value < middleRight &&
					value < upperLeft &&
					value < upperMiddle &&
					value < upperRight) return true;
			}

			/* first column */
			if (rows > 0 && rows < _rows - 1 && columns == 0)
			{
				var upperMiddle = UpperMiddle(rows, columns);
				var upperRight = UpperRight(rows, columns);
				var middleLeft = MiddleRight(rows, columns);
				var lowerMiddle = LowerMiddle(rows, columns);
				var lowerRight = LowerRight(rows, columns);

				if (value < upperMiddle &&
					value < upperRight &&
					value < middleLeft &&
					value < lowerMiddle &&
					value < lowerRight) return true;
			}

			/* last columns */
			if (rows > 0 && rows < _rows - 1 && columns == _columns - 1)
			{
				var upperMiddle = UpperMiddle(rows, columns);
				var upperLeft = UpperLeft(rows, columns);
				var middleLeft = MiddleLeft(rows, columns);
				var lowerLeft = LowerLeft(rows, columns);
				var lowerMiddle = LowerMiddle(rows, columns);

				if (value < upperMiddle &&
					value < upperLeft &&
					value < middleLeft &&
					value < lowerLeft &&
					value < lowerMiddle) return true;
			}

			return false;
		}

		private int UpperLeft(int rows, int columns) => _map[rows - 1, columns - 1];
		private int UpperMiddle(int rows, int columns) => _map[rows - 1, columns];
		private int UpperRight(int rows, int columns) => _map[rows - 1, columns + 1];
		private int MiddleLeft(int rows, int columns) => _map[rows, columns - 1];
		private int MiddleRight(int rows, int columns) => _map[rows, columns + 1];
		private int LowerLeft(int rows, int columns) => _map[rows + 1, columns - 1];
		private int LowerMiddle(int rows, int columns) => _map[rows + 1, columns];
		private int LowerRight(int rows, int columns) => _map[rows + 1, columns + 1];
	}
}
