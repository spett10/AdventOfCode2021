using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2021.Day2
{
	class Pilot
	{
		private readonly List<string> _commands;
		public Pilot(List<string> commands)
		{
			_commands = commands;
		}

		public int Navigate()
		{
			int horizontal = 0;
			int depth = 0;

			foreach(var command in _commands)
			{
				var (distance, direction) = ParseCommand(command);

				switch (direction)
				{
					case Direction.Up:
						depth += -distance;
						break;
					case Direction.Down:
						depth += distance;
						break;
					case Direction.Forward:
						horizontal += distance;
						break;
				}
			}

			return depth * horizontal; 
		}

		public int Aim()
		{
			int horizontal = 0;
			int depth = 0;
			int aim = 0;

			foreach (var command in _commands)
			{
				var (distance, direction) = ParseCommand(command);

				switch (direction)
				{
					case Direction.Up:
						aim -= distance;
						break;
					case Direction.Down:
						aim += distance;
						break;
					case Direction.Forward:
						horizontal += distance;
						depth += (aim * distance);
						break;
				}
			}

			return depth * horizontal;
		}

		private (int, Direction) ParseCommand(string command)
		{
			var split = command.Split(" ");

			if (split.Length != 2) throw new ArgumentException($"expected format <string> <int> but split on \" \" returned length = {split.Length}'");

			var parsedDirection = split[0].ToLower();
			Direction direction;

			if (parsedDirection.Equals("forward"))
			{
				direction = Direction.Forward;
			}
			else if (parsedDirection.Equals("up"))
			{
				direction = Direction.Up;
			}
			else if (parsedDirection.Equals("down"))
			{
				direction = Direction.Down;
			}
			else
			{
				throw new ArgumentException($"got unknown command: {parsedDirection}");
			}

			int distance = Int32.Parse(split[1]);

			return (distance, direction);
		}

		private enum Direction
		{
			Forward,
			Up,
			Down
		}


	}
}
