using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2021.Day6
{
	class LanternFish
	{
		private List<int> _states;

		public LanternFish(List<int> states)
		{
			_states = states;
		}


		public int SimulateDaysAndCountFish(int days)
		{
			if (days < 1) throw new ArgumentException($"{nameof(days)} must be positive");

			for(int i = 0; i < days; i++)
			{
				SimulateOneDay();
			}

			return _states.Count;
		}

		private void SimulateOneDay()
		{
			/* A 0 becomes a 6 and adds a new 8 at the end of the list */
			/* all other numbers decrease by 1 if it was present at the start of the day */
			var newSpawns = new List<int>();

			for(int i = 0; i < _states.Count; i++)
			{
				if(_states[i] == 0)
				{
					newSpawns.Add(8);
					_states[i] = 6;
				}
				else
				{
					_states[i] = _states[i] - 1;
				}
			}

			_states = _states.Concat(newSpawns).ToList();
		}
	}
}
