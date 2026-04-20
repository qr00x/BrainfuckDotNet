using BrainFuckDotNet.Abstract;

namespace BrainFuckDotNet.Parser
{
	internal class BrainFuckParser
	{
		public AbstractSyntaxTree Parse(string codeBody)
		{
			var tree = new AbstractSyntaxTree();
			var nestingStack = new Stack<Loop>();

			foreach (var i in codeBody)
			{
				if (i == '[')
				{
					nestingStack.Push(new Loop());
					continue;
				}

				if (i == ']')
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

				var operationn = ParseOperation(i);
				if (nestingStack.Count > 0)
				{
					nestingStack.Peek().AddInstructionNode(operationn);
				}
				else
				{
					tree.AddNode(new Instraction() { OpCode = operationn });
				}
			}

			return tree;
		}

		private Operation ParseOperation(char op)
		{
			switch (op)
			{
				case '+': return Operation.Inc;
				case '-': return Operation.Dec;
				case '>': return Operation.Right;
				case '<': return Operation.Left;
				case ',': return Operation.In;
				case '.': return Operation.Out;
				default:
					throw new InvalidOperationException("Syntax not supported.");

			}
		}
	}
}
