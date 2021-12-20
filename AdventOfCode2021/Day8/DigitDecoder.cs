using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.Day8
{
	class DigitDecoder
	{

		private List<List<string>> _inputValues;
		private List<List<string>> _outputValues;

		public DigitDecoder(List<string> lines)
		{
			_inputValues = new List<List<string>>();
			_outputValues = new List<List<string>>();

			foreach (var line in lines)
			{
				var split = line.Split("|");
				var input = split[0];
				var output = split[1];

				_inputValues.Add(input.Split(" ").Where(x => !string.IsNullOrEmpty(x)).ToList());
				_outputValues.Add(output.Split(" ").Where(x => !string.IsNullOrEmpty(x)).ToList());
			}
		}

		public int Decode()
		{
			var score = 0;

			var lineCount = _inputValues.Count;

			for(int line = 0; line < lineCount; line++)
			{
				score += AnalyzeLine(_inputValues[line], _outputValues[line]);
			}

			return score;
		}


		// we know that each digit appears once uniquely in input (left part before |) so we should be able to deterministically deduce everything. 
		// observe that in the output the digits will have the same characters, but they can be in permuted order. (bed = dbe for example).
		// they still encode the same digit. 
		private int AnalyzeLine(List<string> input, List<string> output)
		{
			// find the 1, 4, 7 and 8s, they have unique lengths. 
			var one = input.Where(x => x.Length == 2).FirstOrDefault();
			var four = input.Where(x => x.Length == 4).FirstOrDefault();
			var seven = input.Where(x => x.Length == 3).FirstOrDefault();
			var eight = input.Where(x => x.Length == 7).FirstOrDefault();

			var unique = new List<string>()
			{
				one, four, seven, eight
			};

			// All of these use the C and F grids. But we dont know the order (which one is C which one is F). But we get them for starters. 
			var uniqueCommon = GetCommon(unique);

			// All other digits use either 6 grids (0, 6, 9) or 5 grids (2, 3, 5). 
			var fiveGrids = input.Where(x => x.Length == 5).ToList();
			var sixGrids = input.Where(x => x.Length == 6).ToList();

			// Find 9. It is the only one that shares 4 characters with 4, and we already have 4.
			var nine = sixGrids.Where(x => GetCommon(new List<string> { x, four })?.Length == 4).FirstOrDefault();

			// We can now find 0 and 6, the others with 6 grids that are not the 9.
			var sixAndZero = sixGrids.Where(x => SameGrid(x, nine) == false).ToList();

			// 6 reveals which one the F grid is mapped to - it is the only one of length 6 that only uses F - because 0 and 9 use both F and C.
			var six = sixAndZero.Where(x => !x.Contains(uniqueCommon[0]) || !x.Contains(uniqueCommon[1])).First();
			var zero = sixAndZero.Where(x => SameGrid(x,six) == false).First();

			// the C is the one from uniqueCommon that is not F, so the one not in 6. Now we can distinguish C and F. 
			var c = uniqueCommon.Where(x => six.Contains(x) == false).First();
			var f = uniqueCommon.Where(x => x != c).First();

			// 2 is the only 5 grid that only uses C
			var two = fiveGrids.Where(x => x.Contains(c) && !x.Contains(f)).First();

			// 5 is the only 5 grid that only uses F
			var five = fiveGrids.Where(x => x.Contains(f) && !x.Contains(c)).First();

			// 3 is the only 5 grid that uses both
			var three = fiveGrids.Where(x => x.Contains(f) && x.Contains(c)).First();

			// and we now know them all. 
			var digitMap = new List<Tuple<string, int>>
			{
				new Tuple<string, int>(zero, 0),
				new Tuple<string, int>(one, 1),
				new Tuple<string, int>(two, 2),
				new Tuple<string, int>(three, 3),
				new Tuple<string, int>(four, 4),
				new Tuple<string, int>(five, 5),
				new Tuple<string, int>(six, 6),
				new Tuple<string, int>(seven, 7),
				new Tuple<string, int>(eight, 8),
				new Tuple<string, int>(nine, 9),
			};

			return EvaluateOutput(digitMap, output);
		}

		private int EvaluateOutput(List<Tuple<string, int>> digitMap, List<string> outputs)
		{
			string scoreDigits = "";

			foreach(var output in outputs)
			{
				scoreDigits += digitMap.Where(x => SameGrid(x.Item1, output) == true).First().Item2.ToString();
			}

			return Convert.ToInt32(scoreDigits);
		}

		private bool SameGrid(string left, string right)
		{
			if (left.Length != right.Length) return false;

			var leftCharacters = left.ToCharArray();
			var rightCharacters = right.ToCharArray();

			foreach(var c in leftCharacters)
			{
				if (!rightCharacters.Contains(c)) return false;
			}

			/* Same length, chars from one could all be found in the other, they contain same characters */
			return true;
		}

		private char[] GetCommon(List<string> input)
		{
			var ordered = input.OrderByDescending(x => x.Length);

			var candidateCharacters = ordered.First().ToCharArray();

			var others = ordered.Skip(1).ToList();

			foreach(var other in others)
			{
				var characters = other.ToCharArray();

				foreach(var ch in candidateCharacters)
				{
					if (!characters.Contains(ch))
					{
						candidateCharacters = candidateCharacters.Where(x => x != ch).ToArray();
					}
				}

			}

			return candidateCharacters;
		}

		public int CountEasyDigits()
		{
			int count = 0;

			foreach(var output in _outputValues)
			{
				foreach(var entry in output)
				{
					if (entry.Length == 2)
					{
						count++;
					}
					else if (entry.Length == 3)
					{
						count++;
					}
					else if (entry.Length == 4)
					{
						count++;
					}
					else if (entry.Length == 7)
					{
						count++;
					}
				}
			}

			return count;
		}
	}
}
