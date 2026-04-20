namespace BrainFuckDotNet.Abstract
{
	internal class AbstractSyntaxTree
	{
		private readonly List<ISyntaxNode> _nodes = [];

		public IEnumerable<ISyntaxNode> Nodes => _nodes;

		public void AddNode(ISyntaxNode node)
		{
			_nodes.Add(node);
		}
	}
}
