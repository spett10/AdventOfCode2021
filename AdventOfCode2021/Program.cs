using AdventOfCode2021.Day1;
using AdventOfCode2021.Day10;
using AdventOfCode2021.Day2;
using AdventOfCode2021.Day3;
using AdventOfCode2021.Day4;
using AdventOfCode2021.Day5;
using AdventOfCode2021.Day6;
using AdventOfCode2021.Day7;
using AdventOfCode2021.Day9;
using System;
using System.Collections.Generic;

namespace AdventOfCode2021
{
	class Program
	{

		// TODO: make the below test cases? So we can refactor without regressing. If we ever bother. 

		static void Main(string[] args)
		{
			Day10();
		}
					
		private static void Day10()
		{
			var testData = new Input(@"Day10/testdata.txt").GetLinesString<List<string>>();
			var testSyntaxScorer = new SyntaxScorer(testData);
			var testScore = testSyntaxScorer.SyntaxCheck();
			Console.WriteLine($"test score syntax check: {testScore}");

			var input = new Input(@"Day10/input.txt").GetLinesString<List<string>>();
			var syntaxScorer = new SyntaxScorer(input);
			var score = syntaxScorer.SyntaxCheck();
			Console.WriteLine($"syntax score: {score}");

			var testAutoComplete = new SyntaxScorer(testData);
			var testAutoCompleteScore = testAutoComplete.AutoComplete();
			Console.WriteLine($"test auto complete: {testAutoCompleteScore}");

			var autoComplete = new SyntaxScorer(input);
			var autoCompleteScore = autoComplete.AutoComplete();
			Console.WriteLine($"auto complete score: {autoCompleteScore}");
		}

		private static void Day9()
		{
			var testData = new Input(@"Day9/testdata.txt").GetLinesString<List<string>>();
			var testHeatMap = new Heatmap(testData);
			var testRiskScore = testHeatMap.LowPointRiskLevel();
			Console.WriteLine($"Test data risk score: {testRiskScore}");

			var input = new Input(@"Day9/input.txt").GetLinesString<List<string>>();
			var heatmap = new Heatmap(input);
			var riskScore = heatmap.LowPointRiskLevel();
			Console.WriteLine($"risk score: {riskScore}");

			var testheatmapForBasin = new Heatmap(testData);
			var testBasinScore = testheatmapForBasin.FindBasins();
			Console.WriteLine($"test data basin score: {testBasinScore}");

			var heatMapForBasin = new Heatmap(input);
			var basinScore = heatMapForBasin.FindBasins();
			Console.WriteLine($"basin score: {basinScore}");
		}

		private static void Day7()
		{
			var testData = new Input(@"Day7/testdata.txt").GetSplitInt<List<int>>();
			var testCrabAligner = new CrabAlligner(testData);
			var testMinimalCost = testCrabAligner.CheapestCost();
			Console.WriteLine($"Test data minimal cost: {testMinimalCost}");

			var input = new Input(@"Day7/input.txt").GetSplitInt<List<int>>();
			var crabAligner = new CrabAlligner(input);
			var minimalCost = crabAligner.CheapestCost();
			Console.WriteLine($"minimal cost: {minimalCost}");

			var testVaryingCrabAligner = new CrabAlligner(testData);
			var testMinimalCostVarying = testVaryingCrabAligner.CheapestCostNotConstantRate();
			Console.WriteLine($"test minimal cost varying: {testMinimalCostVarying}");

			var varyingCrabAligner = new CrabAlligner(input);
			var minimalCostVarying = varyingCrabAligner.CheapestCostNotConstantRate();
			Console.WriteLine($"minimal cost varying: {minimalCostVarying}");
		}

		private static void Day6()
		{
			var testData = new Input(@"Day6/testdata.txt").GetSplitInt<List<int>>();
			var testLanternFish = new OptimizedLanternFish(testData);
			var testDataFishCount = testLanternFish.SimulateDaysAndCount(80);
			Console.WriteLine($"test data fish count: {testDataFishCount}");

			var input = new Input(@"Day6/input.txt").GetSplitInt<List<int>>();
			var lanternFish = new OptimizedLanternFish(input);
			var fishCount = lanternFish.SimulateDaysAndCount(80);
			Console.WriteLine($"fish count: {fishCount}");

			var testLanternFishForever = new OptimizedLanternFish(testData);
			var testForeverCount = testLanternFishForever.SimulateDaysAndCount(256);
			Console.WriteLine($"test forever count: {testForeverCount}");

			var lanternFishForever = new OptimizedLanternFish(input);
			var foreverCount = lanternFishForever.SimulateDaysAndCount(256);
			Console.WriteLine($"forever count: {foreverCount}");
		}

		private static void Day5()
		{
			var testdata = new Input(@"Day5/testdata.txt").GetLinesString<List<string>>();
			var testDiagram = new Diagram(testdata);
			var testOverlappingStraightOnly = testDiagram.OverlapsStraightOnly();
			Console.WriteLine($"test data overlapping points straight only: {testOverlappingStraightOnly}");
			var testDataAllPoint = testDiagram.OverlapsAllLines();
			Console.WriteLine($"test data all points : {testDataAllPoint}");

			var input = new Input(@"Day5/input.txt").GetLinesString<List<string>>();
			var diagram = new Diagram(input);
			var overlappingStraightOnly = diagram.OverlapsStraightOnly();
			Console.WriteLine($"overlapping points straight only : {overlappingStraightOnly}");
			var overlappingAll = diagram.OverlapsAllLines();
			Console.WriteLine($"overlapping all : {overlappingAll}");
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
