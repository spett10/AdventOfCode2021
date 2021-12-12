using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2021.Day10
{
	class SyntaxScorer
	{
		private List<string> _commands;

		public SyntaxScorer(List<string> commands)
		{
			/* TODO: incomplete are those we can parse without finding syntax error but with stuff left on the stack */
			this._commands = commands;
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
