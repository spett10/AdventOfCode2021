using System;
using System.Collections.Generic;
using System.Linq;

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

		public int LifeSupportRating()
		{
			/* TODO: unroll these functiosn that are almost identical and do a one-pass? */
			var oxygenGeneratorRating = OxygenGeneratorRating();
			var co2ScrubberRating = CO2ScrubberRating();
			return oxygenGeneratorRating * co2ScrubberRating;
		}

		private int OxygenGeneratorRating()
		{
			int indexes = _readings.First().Length;

			if (indexes < 1) throw new InvalidOperationException("empty reading");

			var workingSet = _readings;

			for(int i = 0; i < indexes; i++)
			{
				var sliceResult = Slice(workingSet, i, 1);

				if (!string.IsNullOrEmpty(sliceResult.OnlyOneLeft))
				{
					return Convert.ToInt32(sliceResult.OnlyOneLeft, 2);
				}

				workingSet = sliceResult.MostCommon;
			}

			throw new ArgumentException($"Could not determine {nameof(OxygenGeneratorRating)}");
		}

		private int CO2ScrubberRating()
		{
			int indexes = _readings.First().Length;

			if (indexes < 1) throw new InvalidOperationException("empty reading");

			var workingSet = _readings;

			for(int i = 0; i < indexes; i++)
			{
				var sliceResult = Slice(workingSet, i, 0);

				if (!string.IsNullOrEmpty(sliceResult.OnlyOneLeft))
				{
					return Convert.ToInt32(sliceResult.OnlyOneLeft, 2);
				}

				workingSet = sliceResult.LeastCommon;
			}

			throw new ArgumentException($"could not determine {nameof(CO2ScrubberRating)}");
		}

		private class SliceResult
		{
			public List<string> MostCommon { get; set; }
			public List<string> LeastCommon { get; set; }
			public string OnlyOneLeft { get; set; }
		}

		private SliceResult Slice(List<string> readings, int index, int tieBreaker)
		{
			if (readings == null || readings.Count < 1) throw new ArgumentException($"{nameof(readings)} null or empty");

			var readingSize = readings.First().Length;

			if (index >= readingSize || index < 0) throw new ArgumentException($"{nameof(index)} out of bounds");

			if (readings.Count == 1) return new SliceResult { OnlyOneLeft = readings.First() };

			var ones = new List<string>();
			var zeroes = new List<string>();

			foreach(var reading in readings)
			{
				var element = reading.ElementAt<char>(index);

				if(element == '1')
				{
					ones.Add(reading);
				}
				else if(element == '0')
				{
					zeroes.Add(reading);
				}
				else
				{
					throw new ArgumentException($"expected '0' or '1' but found {element}");
				}
			}

			if (ones.Count == 1 && zeroes.Count == 1)
			{
				if(tieBreaker == 0)
				{
					return new SliceResult { OnlyOneLeft = zeroes.First() };
				}
				else
				{
					return new SliceResult { OnlyOneLeft = ones.First() };
				}				
			}

			return ones.Count >= zeroes.Count ? new SliceResult { MostCommon = ones, LeastCommon = zeroes } :
												new SliceResult { MostCommon = zeroes, LeastCommon = ones };
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
