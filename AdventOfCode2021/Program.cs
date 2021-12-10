using AdventOfCode2021.Day1;
using AdventOfCode2021.Day2;
using AdventOfCode2021.Day3;
using AdventOfCode2021.Day4;
using System;
using System.Collections.Generic;

namespace AdventOfCode2021
{
	class Program
	{
		static void Main(string[] args)
		{
			Day4();
		}

		private static void Day4()
		{
			var testBingoSubSystem = new BingoSubSystem(@"Day4/testdata.txt");
			var testScore = testBingoSubSystem.PlayUntilFirstBoardWinsAndReportScore();
			Console.WriteLine($"test bingo board calculated score: {testScore}");

			var bingoSubSystemPart1 = new BingoSubSystem(@"Day4/input.txt");
			var score = bingoSubSystemPart1.PlayUntilFirstBoardWinsAndReportScore();
			Console.WriteLine($"bingo board calculated score: {score}");

			var bingoSubSystemPart2 = new BingoSubSystem(@"Day4/input.txt");
			var lastWinnerScore = bingoSubSystemPart2.PlayUntilLastBoardWinsAndReportScore();
			Console.WriteLine($"bingo board last board score: {lastWinnerScore}");
		}

		private static void Day3()
		{
			var testdata = new Input(@"Day3/testdata.txt");
			var testReadings = testdata.GetLinesString<List<string>>();
			var testDiagnostics = new BinaryDiagnostic(testReadings);
			Console.WriteLine($"power consumption test data = {testDiagnostics.PowerConsumption()}");

			var input = new Input(@"Day3/input.txt");
			var readings = input.GetLinesString<List<string>>();
			var diagnostics = new BinaryDiagnostic(readings);
			Console.WriteLine($"power consumption input = {diagnostics.PowerConsumption()}");

			var testLifeSupportReadings = new BinaryDiagnostic(testReadings).LifeSupportRating();
			Console.WriteLine($"life support rating test data = {testLifeSupportReadings}");

			var lifeSupportRating = new BinaryDiagnostic(readings).LifeSupportRating();
			Console.WriteLine($"life support rating = {lifeSupportRating}");
		}

		private static void Day2()
		{
			var testdata = new Input(@"Day2/testdata.txt");
			var testCommands = testdata.GetLinesString<List<string>>();
			var testpilot = new Pilot(testCommands);
			Console.WriteLine($"test position = {testpilot.Navigate()}");

			var input = new Input(@"Day2/input.txt");
			var commands = input.GetLinesString<List<string>>();
			var pilot = new Pilot(commands);
			Console.WriteLine($"position = {pilot.Navigate()}");
			Console.WriteLine($"aim = {pilot.Aim()}");
		}

		private static void Day1()
		{
			var input = new Input(@"Day1/input.txt");
			var measurements = input.GetLinesInt<List<int>>();
			var sonarscanner = new SonarScanner(measurements);
			Console.WriteLine($"measurement increases = {sonarscanner.IncreaseCount()}");
			Console.WriteLine($"sliding window increases = {sonarscanner.SumOfThreeSlidingWindowCount()}");
		}
	}
}
