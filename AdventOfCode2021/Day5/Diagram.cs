using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.Day5
{
	class Diagram
	{
		private readonly List<LineSegment> _lineSegments;

		private Point BottomRight;

		public Diagram(List<string> lines)
		{
			_lineSegments = new List<LineSegment>();

			var maxX = 0;
			var maxY = 0;

			foreach(var line in lines)
			{
				var lineSegment = new LineSegment(line);
				
				if(lineSegment.First.X > maxX)
				{
					maxX = lineSegment.First.X;
				}
				
				if(lineSegment.Second.X > maxX)
				{
					maxX = lineSegment.Second.X;
				}

				if(lineSegment.First.Y > maxY)
				{
					maxY = lineSegment.First.Y;
				}

				if(lineSegment.Second.Y > maxY)
				{
					maxY = lineSegment.Second.Y;
				}

				_lineSegments.Add(lineSegment);
			}

			BottomRight = new Point(maxX, maxY);
		}

		public int OverlapsAllLines()
		{
			var allCoveredPoints = new List<Point>();

			foreach(var line in _lineSegments)
			{
				allCoveredPoints = allCoveredPoints.Concat(line.CoveredPoints).ToList();
			}

			return Overlaps(allCoveredPoints);
		}

		public int OverlapsStraightOnly()
		{
			var straighLines = _lineSegments.Where(x => x.IsStraight).ToList();

			var allCoveredPoints = new List<Point>();

			foreach(var line in straighLines)
			{
				allCoveredPoints = allCoveredPoints.Concat(line.CoveredPoints).ToList();
			}

			return Overlaps(allCoveredPoints);
		}

		private int Overlaps(List<Point> points)
		{
			var grid = new int[BottomRight.X + 1, BottomRight.Y + 1];
			var overlapsFound = 0;

			foreach (var point in points)
			{
				grid[point.X, point.Y]++;
			}

			/* find all elements marked at least twice */
			for (int row = 0; row < BottomRight.X + 1; row++)
			{
				for (int column = 0; column < BottomRight.Y + 1; column++)
				{
					if (grid[row, column] >= 2)
					{
						overlapsFound++;
					}
				}
			}

			return overlapsFound;
		}
	}	
}
