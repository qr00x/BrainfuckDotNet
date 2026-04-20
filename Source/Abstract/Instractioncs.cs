
namespace BrainFuckDotNet.Abstract
{
	internal class Instraction : ISyntaxNode
	{
		public NodeType Type => NodeType.Instruction;

		public required Operation OpCode { get; init; }
	}
}
