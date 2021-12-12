using System;
using System.Collections.Generic;
using System.Linq;

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

		public int LowPointRiskLevel()
		{
			var lowPointCoordinates = FindLowPoints();

			/* we could have done this while visiting coordinates above and not need the array but i liked it for debugging */
			var riskLevel = 0;
			foreach(var coordinate in lowPointCoordinates)
			{
				var height = _map[coordinate.Item1, coordinate.Item2];
				riskLevel += height + 1;
			}

			return riskLevel;
		}

		private class Basin
		{
			public Tuple<int, int> LowPoint{ get; set; }
			public HashSet<Tuple<int, int>> BasinPoints { get; set; }

			public Basin(Tuple<int, int> lowPoint)
			{
				LowPoint = lowPoint;

				BasinPoints = new HashSet<Tuple<int, int>>();
			}

			public void AddToBasin(Tuple<int, int> point)
			{
				if (BasinPoints.Contains(point)) return; //Beware duplicates, our discovery mechanism does not take this into account so we guard here - we know points are unique on the grid.

				BasinPoints.Add(point);
			}

			public int Size()
			{
				/* includes the low point but the lowpoint is added to the basinpoints in first call */
				return BasinPoints.Count;
			}
		}


		private List<Tuple<int, int>> FindEdges()
		{
			List<Tuple<int, int>> edges = new List<Tuple<int, int>>();

			for (int row = 0; row < _rows; row++)
			{
				for (int column = 0; column < _columns; column++)
				{
					if(_map[row, column] == 9)
					{
						edges.Add(new Tuple<int, int>(row, column));
					}
				}
			}

			return edges;
		}

		public int FindBasins()
		{
			var lowPoints = FindLowPoints();
			var basins = new List<Basin>();

			foreach(var lowpoint in lowPoints)
			{
				basins.Add(new Basin(lowpoint));
			}

			var edges = FindEdges();

			foreach(var basin in basins)
			{
				var visited = new HashSet<Tuple<int, int>>(edges);
				DiscoverBasin(basin.LowPoint.Item1, basin.LowPoint.Item2, int.MaxValue, int.MaxValue, basin.AddToBasin, visited, true);
			}

			var threeLargestBasins = basins.OrderBy(x => x.Size()).Reverse().Take(3).ToList();

			return threeLargestBasins[0].Size() * threeLargestBasins[1].Size() * threeLargestBasins[2].Size();
		}

		private List<Tuple<int, int>> FindLowPoints()
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

			return lowPointCoordinates;
		}

		private delegate void BasinCallback(Tuple<int,int> point);

		private void DiscoverBasin(int row, int column, int previousRow, int previousColumn, BasinCallback addToBasinCallback, HashSet<Tuple<int,int>> visited, bool isLowPoint)
		{
			var value = _map[row, column];
			var current = new Tuple<int, int>(row, column);

			if (value == 9)
			{
				/* 9s are pre-added to visited */
				return;
			}

			if (!isLowPoint) //we only look at previous if we werent the lowpoint, since that is where we started. 
			{
				var previous = _map[previousRow, previousColumn];

				if (value < previous)
				{
					/* We dont add to visited here, another path could lead to us in a valid way. We know we will always stop eventually since each basin
					 * is surrounded by 9's, which are pre-added to visited.
					 */
					return; // we are climping up the basin so if it goes down we stop
				}
			}

			/* we are >= previous so we are in basin */
			addToBasinCallback(current);
			visited.Add(current);

			var neightbours = GetUnvisitedNeighbours(row, column, visited);
			foreach(var neighbour in neightbours)
			{
				DiscoverBasin(neighbour.Item1, neighbour.Item2, row, column, addToBasinCallback, visited, false); //recursive calls are never lowpoints for the current basin
			}
		}

		List<Tuple<int, int>> GetUnvisitedNeighbours(int row, int column, HashSet<Tuple<int, int>> visited)
		{
			var neightBours = new List<Tuple<int, int>>();

			// We cannot visit diagonally, so only points <-^v->

			/* point not on edge or corner */
			if(row > 0 && row < _rows - 1 && column > 0 && column < _columns - 1)
			{
				AddUnlessPrevious(neightBours, row - 1, column, visited);
				AddUnlessPrevious(neightBours, row, column - 1, visited);
				AddUnlessPrevious(neightBours, row, column + 1, visited);
				AddUnlessPrevious(neightBours, row + 1, column, visited);
			}
			else if(row == 0 && column > 0 && column < _columns - 1) /* top row no corners */
			{
				AddUnlessPrevious(neightBours, row, column - 1, visited);
				AddUnlessPrevious(neightBours, row, column + 1, visited);
				AddUnlessPrevious(neightBours, row + 1, column, visited);
			}
			else if(row == _rows - 1 && column > 0 && column < _columns - 1) /* bottom row no corners*/
			{
				AddUnlessPrevious(neightBours, row, column - 1, visited);
				AddUnlessPrevious(neightBours, row, column + 1, visited);
				AddUnlessPrevious(neightBours, row - 1, column, visited);
			}
			else if(row > 0 && row < _rows - 1 && column == 0) /* first column no corners */
			{
				AddUnlessPrevious(neightBours, row - 1, column, visited);
				AddUnlessPrevious(neightBours, row, column + 1, visited);
				AddUnlessPrevious(neightBours, row + 1, column, visited);
			}
			else if(row > 0 && row < _rows - 1 && column == _columns - 1) /* last column no corners */
			{
				AddUnlessPrevious(neightBours, row - 1, column, visited);
				AddUnlessPrevious(neightBours, row, column - 1, visited);
				AddUnlessPrevious(neightBours, row + 1, column, visited);
			}
			else if(row == 0 && column == 0) /* top left */
			{
				AddUnlessPrevious(neightBours, row + 1, column, visited);
				AddUnlessPrevious(neightBours, row, column + 1, visited);

			}
			else if(row == 0 && column == _columns - 1) /* top right */
			{
				AddUnlessPrevious(neightBours, row, column - 1, visited);
				AddUnlessPrevious(neightBours, row + 1, column, visited);
			}
			else if(row == _rows - 1 && column == 0) /* bottom left */
			{
				AddUnlessPrevious(neightBours, row - 1, column, visited);
				AddUnlessPrevious(neightBours, row, column + 1, visited);
			}
			else if(row == _rows - 1 && column == -1) /* bottom right */
			{
				AddUnlessPrevious(neightBours, row - 1, column, visited);
				AddUnlessPrevious(neightBours, row, column - 1, visited);
			}

			return neightBours;
		}	

		private void AddUnlessPrevious(List<Tuple<int, int>> list, int row, int column, HashSet<Tuple<int, int>> visited)
		{
			var current = new Tuple<int, int>(row, column);

			if(visited.Contains(current))
			{
				return;
			}

			list.Add(current);
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
