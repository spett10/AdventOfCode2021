using System;
using System.Collections.Generic;
using System.Numerics;

namespace AdventOfCode2021.Day6
{
	class OptimizedLanternFish
	{
		private BigInteger[] _states;

		public OptimizedLanternFish(List<int> states)
		{
			_states = new BigInteger[9];

			foreach(var state in states)
			{
				_states[state]++;
			}
		}

		public BigInteger SimulateDaysAndCount(int days)
		{
			if (days < 1) throw new ArgumentException($"{nameof(days)} must be positive");

			for (int i = 0; i < days; i++)
			{
				SimulateOneDay();
			}

			return _states[0] + _states[1] + _states[2] + _states[3] + _states[4] + _states[5] + _states[6] + _states[7] + _states[8];
		}

		private void SimulateOneDay()
		{
			/* zeroes become 6s and will add 8s to the end */
			var zeroes = _states[0];
			var newSpawns = zeroes;

			/* shuffle everything down */
			_states[0] = _states[1];
			_states[1] = _states[2];
			_states[2] = _states[3];
			_states[3] = _states[4];
			_states[4] = _states[5];
			_states[5] = _states[6];
			_states[6] = _states[7] + zeroes;
			_states[7] = _states[8];
			_states[8] = newSpawns;
		}
	}
}
