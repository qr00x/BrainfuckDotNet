using System;
using System.Collections.Generic;
using System.Text;

namespace BrainfuckDotNet.Parser
{
	internal class TokenLocation : IComparable
	{
		public int Row { get; private set; }

		public int Column { get; private set; }

		public TokenLocation(int row, int column)
		{
			Row = row;
			Column = column;
		}

		public int CompareTo(object? obj)
		{
			if (obj is TokenLocation location)
			{
				if (Row != location.Row) return Row < location.Row ? -1 : 1;
				if (Column == location.Column)
				{
					return 0;
				}

				return Column < location.Column ? -1 : 1;
			}

			throw new ArgumentException();
		}
	}
}
