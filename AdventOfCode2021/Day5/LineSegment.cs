using System;
using System.Collections.Generic;

namespace AdventOfCode2021.Day5
{
	class LineSegment
	{
		public Point First { get; private set; }
		public Point Second { get; private set; }

		private bool _horizontal;
		private bool _vertical;

		public bool IsStraight => _horizontal || _vertical;

		public List<Point> CoveredPoints { get; private set; }

		public LineSegment(string lineSegment)
		{
			var coordinates = lineSegment.Split(" -> ");
			var firstPoint = coordinates[0].Split(",");
			var secondPoint = coordinates[1].Split(",");

			var x1 = Convert.ToInt32(firstPoint[0]);
			var y1 = Convert.ToInt32(firstPoint[1]);

			First = new Point(x1, y1);

			var x2 = Convert.ToInt32(secondPoint[0]);
			var y2 = Convert.ToInt32(secondPoint[1]);

			Second = new Point(x2, y2);

			if (First.X == Second.X) _horizontal = true;
			if (First.Y == Second.Y) _vertical = true;

			CoveredPoints = GetPoints();
		}

		private List<Point> GetPoints()
		{
			if(!_horizontal && !_vertical)
			{
				return new List<Point>();
			}

			if(_horizontal && _vertical)
			{
				return new List<Point> { First };
			}

			var coveredPoints = new List<Point>();

			if (_horizontal)
			{
				for (int y = Math.Min(First.Y, Second.Y); y <= Math.Max(First.Y, Second.Y); y++)
				{
					coveredPoints.Add(new Point(First.X, y));
				}
			}

			if (_vertical)
			{
				for(int x = Math.Min(First.X, Second.X); x <= Math.Max(First.X, Second.X); x++)
				{
					coveredPoints.Add(new Point(x, First.Y));
				}
			}

			return coveredPoints;
		}
	}
}
