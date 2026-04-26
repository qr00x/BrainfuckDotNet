using System.Text;
using BrainfuckDotNet.Parser;

namespace BrainFuckDotNet.Parser
{
	internal class BrainFuckParser
	{
		public static char[] Keywords = ['+', '-', '<', '>', '[', ']', ',', ',']; 

		public LexerResult Parse(string codeBody)
		{
			var result = new LexerResult();
			var rowNumber = 1;
			var colNumber = 1;
			
			for (var i = 0; i < codeBody.Length; i++)
			{
				var c = codeBody[i];
				if (TryParse(c, result, rowNumber, colNumber))
				{
					continue;
				}

				if (c == '\r')
				{ 
					rowNumber++;
					colNumber = 1;
					i++;
				}
				else
				{
					if (c == ' ')
					{
						colNumber++;
					}
					else
					{
						var comment = new StringBuilder();
						var j = i;

						while (codeBody[j] != '\r')
						{
							comment.Append(codeBody[j++]);
						}

						i = j - 1;
						result.AddToken(comment.ToString(), TokenType.Comment, rowNumber, colNumber);
					}
				}
			}

			return result;
		}

		private static bool TryParse(char c, LexerResult result, int rowNumber, int colNumber)
		{
			switch (c)
			{
				case '[': result.AddToken(c.ToString(), TokenType.LoopBegin, rowNumber, colNumber); return true;
				case ']': result.AddToken(c.ToString(), TokenType.LoopEnd, rowNumber, colNumber); return true;
				case '+': result.AddToken(c.ToString(), TokenType.Inc, rowNumber, colNumber); return true;
				case '-': result.AddToken(c.ToString(), TokenType.Dec, rowNumber, colNumber); return true;
				case '<': result.AddToken(c.ToString(), TokenType.Left, rowNumber, colNumber); return true;
				case '>': result.AddToken(c.ToString(), TokenType.Right, rowNumber, colNumber); return true;
				case ',': result.AddToken(c.ToString(), TokenType.In, rowNumber, colNumber); return true;
				case '.': result.AddToken(c.ToString(), TokenType.Out, rowNumber, colNumber); return true;
				default: return false;
			}
		}
	}
}
