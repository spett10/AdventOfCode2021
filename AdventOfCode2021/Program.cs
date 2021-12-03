using AdventOfCode2021.Day1;
using System;
using System.Collections.Generic;

namespace AdventOfCode2021
{
	class Program
	{
		static void Main(string[] args)
		{
			Day1();
		}

		private static void Day1()
		{
			var input = new Input(@"Day1/input.txt");
			var measurements = input.GetLinesInt<List<int>>();
			var sonarscanner = new SonarScanner(measurements);
			Console.WriteLine($"measurement increases = {sonarscanner.IncreaseCount()}");
		}
	}
}
