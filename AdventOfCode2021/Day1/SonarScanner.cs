using System.Collections.Generic;

namespace AdventOfCode2021.Day1
{
	class SonarScanner
	{
		private readonly List<int> _measurements;

		public SonarScanner(List<int> measurements)
		{
			_measurements = measurements;
		}

		public int IncreaseCount()
		{
			var increases = 0;

			for(int i = 1; i < _measurements.Count; i++)
			{
				var previous = _measurements[i - 1];
				var current = _measurements[i];

				if (current > previous) increases++;
			}

			return increases;
		}

		public int SumOfThreeSlidingWindowCount()
		{
			var increases = 0;

			for (int i = 1; i < _measurements.Count - 2; i++)
			{
				var first = _measurements[i - 1];
				var second = _measurements[i];
				var third = _measurements[i + 1];

				var previousSum = first + second + third;

				var currentFirst = _measurements[i];
				var currentSecond = _measurements[i + 1];
				var currentThird = _measurements[i + 2];

				var currentSum = currentFirst + currentSecond + currentThird;

				if (currentSum > previousSum) increases++;
			}

			return increases;
		}

	}
}
