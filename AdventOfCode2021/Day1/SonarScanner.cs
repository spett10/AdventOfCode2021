using System;
using System.Collections.Generic;
using System.Text;

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


	}
}
