using BrainfuckDotNet.Parser;
using BrainFuckDotNet.Abstract;

namespace BrainfuckDotNet.Abstract
{
	internal class SyntaxTreeFactory
	{
		public AbstractSyntaxTree Create(LexerResult lexerResult)
		{
			var tree = new AbstractSyntaxTree();
			var nestingStack = new Stack<Loop>();

			foreach (var token in lexerResult.Tokens)
			{
				if (token.Type == TokenType.LoopBegin)
				{
					nestingStack.Push(new Loop());
					continue;
				}

				if (token.Type == TokenType.LoopEnd)
				{
					var loop = nestingStack.Pop();
					if (nestingStack.Count > 0)
					{
						nestingStack.Peek().AddNode(loop);
					}
					else
					{
						tree.AddNode(loop);
					}

					continue;
				}

				if (token.Type == TokenType.Comment)
				{
					continue;
				}

				if (nestingStack.Count > 0)
				{
					nestingStack.Peek().AddInstructionNode(TokenToOperation(token));
				}
				else
				{
					tree.AddNode(new Instraction() { OpCode = TokenToOperation(token) });
				}
			}

			return tree;
		}

		private Operation TokenToOperation(Token token)
		{
			switch (token.Type)
			{
				case TokenType.Inc: return Operation.Inc;
				case TokenType.Dec: return Operation.Dec;
				case TokenType.Right: return Operation.Right;
				case TokenType.Left: return Operation.Left;
				case TokenType.In: return Operation.In;
				case TokenType.Out: return Operation.Out;
				default:
					throw new InvalidOperationException("Syntax not supported.");

			}
		}
	}
}