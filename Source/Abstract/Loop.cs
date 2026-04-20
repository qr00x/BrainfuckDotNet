namespace BrainFuckDotNet.Abstract
{
	internal class Loop : ISyntaxNode
	{
		private readonly List<ISyntaxNode> _nodes = [];

		public NodeType Type => NodeType.Loop;

		public IEnumerable<ISyntaxNode> Node => _nodes;

		public void AddInstructionNode(Operation operation)
		{
			_nodes.Add(new Instraction { OpCode = operation });
		}

		public void AddNode(ISyntaxNode node)
		{
			_nodes.Add(node);
		}
	}
}
