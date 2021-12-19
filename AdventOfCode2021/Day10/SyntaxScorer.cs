using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace AdventOfCode2021.Day10
{
	class SyntaxScorer
	{
		private List<string> _commands;

		public SyntaxScorer(List<string> commands)
		{
			this._commands = commands;
		}


		public BigInteger AutoComplete()
		{

			var incomplete = _commands.Where(x => ScoreLine(x) == 0).ToList();

			var completingLines = new List<Tuple<BigInteger, string>>();

			foreach(var line in incomplete)
			{
				var (lineScore, completing) = CompleteLine(line);

				completingLines.Add(new Tuple<BigInteger, string>(lineScore, completing));
			}

			var sorted = completingLines.OrderBy(x => x.Item1).ToArray();

			var middleElement = (sorted.Length - 1) / 2;

			return sorted[middleElement].Item1;
		}


		private (BigInteger, string) CompleteLine(string line)
		{
			/* Push and pop until we run out of things to push */
			var chars = line.ToCharArray();

			var stack = new Stack<char>();

			foreach (var character in chars)
			{
				switch (character)
				{
					case '(':
					case '[':
					case '{':
					case '<':
						stack.Push(character);
						break;

					case ')':
						var a = stack.Pop();
						if (a != '(') throw new ArgumentException($"Expected '(' but found {a}");
						break;

					case ']':
						var b = stack.Pop();
						if (b != '[') throw new ArgumentException($"Expected '[' but found {b}");
						break;

					case '}':
						var c = stack.Pop();
						if (c != '{') throw new ArgumentException($"Expected closing tuborg but found {c}");
						break;

					case '>':
						var d = stack.Pop();
						if (d != '<') throw new ArgumentException($"Expected '<' but found {d}");
						break;

					default:
						throw new ArgumentException($"found illegal char: {character}");
				}
			}

			/* we are now left with what we should complete */
			var done = false;
			var completingString = new List<char>();

			while (!done)
			{
				var popped = stack.Pop();

				switch (popped)
				{
					case '(':
						completingString.Add(')');
						break;
					case '[':
						completingString.Add(']');
						break;
					case '{':
						completingString.Add('}');
						break;
					case '<':
						completingString.Add('>');
						break;
				}
				done = stack.Count == 0;
			}

			BigInteger score = 0;

			foreach(var c in completingString)
			{
				score *= 5;

				if (c == ')') score += 1;
				if (c == ']') score += 2;
				if (c == '}') score += 3;
				if (c == '>') score += 4;
			}

			return (score, new string(completingString.ToArray()));
		}

		public int SyntaxCheck()
		{
			int score = 0;

			foreach(var line in _commands)
			{
				score += ScoreLine(line);
			}

			return score;
		}

		private int ScoreLine(string line)
		{
			var chars = line.ToCharArray();

			var stack = new Stack<char>();

			foreach(var character in chars)
			{
				switch (character)
				{
					case '(':
					case '[':
					case '{':
					case '<':
						stack.Push(character);
						break;

					case ')':
						var a = stack.Pop();
						if (a != '(') return 3;
						break;

					case ']':
						var b = stack.Pop();
						if (b != '[') return 57;
						break;

					case '}':
						var c = stack.Pop();
						if (c != '{') return 1197;
						break;

					case '>':
						var d = stack.Pop();
						if (d != '<') return 25137;
						break;

					default:
						throw new ArgumentException($"found illegal char: {character}");
				}
			}
			return 0; //simply incomplete. 
		}



	}
}
