using BrainFuckDotNet.Parser;

namespace BrainfuckDotNet.Parser
{
	internal class LexerResult
	{
		private List<Token> _tokens = [];

		public IEnumerable<Token> Tokens => _tokens;

		public void AddToken(string value, TokenType type, int row, int col)
		{
			_tokens.Add(new Token(value, type, new TokenLocation(row, col)));
		}

		public IEnumerable<Error> GetErrors()
		{
			var errors = new List<Error>();
			var stack = new Stack<Token>();
			foreach (var token in _tokens)
			{
				if (token.Type == TokenType.LoopBegin)
				{
					stack.Push(token);
				}

				if (token.Type == TokenType.LoopEnd)
				{
					if (stack.Count > 0)
					{
						stack.Pop();
					}
					else
					{
						stack.Push(token);
					}
				}

				if (token.Type == TokenType.Comment)
				{
					if (token.Value.ToCharArray().ContainsAny(BrainFuckParser.Keywords))
					{
						errors.Add(new Error(token.Location, "Comment can't contain opcode."));
					}
				}
			}

			while (stack.Count > 0)
			{
				var token = stack.Pop();
				if (token.Type == TokenType.LoopBegin)
				{
					errors.Add(new Error(token.Location, "Missing closing loop opcode."));
				}

				if (token.Type == TokenType.LoopEnd)
				{
					errors.Add(new Error(token.Location, "Missing opening loop opcode."));
				}
			}

			errors.Sort();
			return errors;
		}
	}
}
