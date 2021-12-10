using System;
using System.Collections.Generic;
using System.IO;
namespace AdventOfCode2021
{
	class Input
	{
		private readonly string _inputFileName;

		private readonly FileInfo _inputFileInfo;

		public Input(string filename)
		{
			_inputFileName = filename;

			_inputFileInfo = new FileInfo(_inputFileName);

			if (!_inputFileInfo.Exists) throw new ArgumentException($"file {filename} not found.");
		}

		public Stream Stream()
		{
			return _inputFileInfo.Open(FileMode.Open);
		}

		public T GetLinesString<T>() where T : ICollection<string>, new()
		{
			var result = new T();

			var lines = File.ReadAllLines(_inputFileName);

			foreach(var line in lines)
			{
				result.Add(line);
			}

			return result;
		}

		public T GetLinesInt<T>() where T : ICollection<int>, new()
		{
			var result = new T();

			var lines = File.ReadAllLines(_inputFileName);

			foreach (var line in lines)
			{
				result.Add(Int32.Parse(line));
			}

			return result;
		}


	}
}
