using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2021.Day3
{
	class BinaryDiagnostic
	{
		private readonly List<string> _readings;

		public BinaryDiagnostic(List<string> readings)
		{
			_readings = readings;
		}

		public int PowerConsumption()
		{
			var bits = _readings.First().Length;
			var bitCount = new int[bits];

			foreach(var reading in _readings)
			{
				Read(reading, bitCount);
			}

			/* each index has the count of 1's for that position */
			int bitCountVertially = _readings.Count;
			var bitStringForMostCommon = "";
			var bitStringForLeastCommon = "";

			foreach(int ones in bitCount)
			{
				/* if more than half where ones, there were most 1's */
				if(ones > bitCountVertially / 2)
				{
					bitStringForMostCommon += "1";
					bitStringForLeastCommon += "0";
				}
				else
				{
					bitStringForMostCommon += "0";
					bitStringForLeastCommon += "1";
				}
			}

			var bitStringForMostCommonAsInteger = Convert.ToInt32(bitStringForMostCommon, 2);
			var bitStringForLeastCommonAsInteger = Convert.ToInt32(bitStringForLeastCommon, 2);

			return bitStringForLeastCommonAsInteger * bitStringForMostCommonAsInteger;
		}

		private void Read(string reading, int[] bitcount)
		{
			int index = 0;
			foreach(char c in reading)
			{
				if (c == 49)
				{
					bitcount[index]++;
				}
				else if(c == 48)
				{
					// No op for now, perhaps for part 2 it will be relevant? 
				}
				else
				{
					throw new ArgumentException($"Expected '1' or '0' but found {c}");
				}

				index++;
			}
		}
	}
}
