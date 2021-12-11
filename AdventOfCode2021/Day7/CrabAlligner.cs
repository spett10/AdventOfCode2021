using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2021.Day7
{
	class CrabAlligner
	{
		/* position and how many reside at it */
		private Dictionary<int, int> positions;
		private int MaxPosition;

		public CrabAlligner(List<int> startPositions)
		{
			positions = new Dictionary<int, int>();

			MaxPosition = 0;

			foreach(var pos in startPositions)
			{
				if (positions.ContainsKey(pos))
				{
					positions[pos] = positions[pos] + 1;
				}
				else
				{
					positions.Add(pos, 1);
				}

				if (pos > MaxPosition) MaxPosition = pos;
			}
		}

		public int CheapestCost()
		{
			int lowestCost = int.MaxValue;
			for (int i = 0; i <= MaxPosition; i++)
			{
				var currentCost = CostOfPosition(i);

				if (currentCost < lowestCost) lowestCost = currentCost;
			}

			return lowestCost;
		}

		private int CostOfPosition(int aligningPosition)
		{
			int cost = 0;

			foreach(var pos in positions)
			{
				int distance = Math.Abs(aligningPosition - pos.Key);
				cost += (distance * pos.Value);
			}

			return cost;
		}

	}
}
