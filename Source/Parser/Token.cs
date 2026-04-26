using System;
using System.Collections.Generic;
using System.Text;

namespace BrainfuckDotNet.Parser
{
	internal enum TokenType
	{
		None,
		Inc,
		Dec,
		Left,
		Right,
		In,
		Out,
		LoopBegin,
		LoopEnd,
		Comment
	}

	internal class Token
	{
		public TokenType Type { get; }

		public string Value { get; }

		public TokenLocation Location { get; }

		public Token(string value, TokenType type, TokenLocation location)
		{
			Type = type;
			Location = location;
			Value = value;
		}
	}
}
