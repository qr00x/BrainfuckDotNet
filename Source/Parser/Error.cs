namespace BrainfuckDotNet.Parser
{
	internal class Error : IComparable
	{
		private readonly TokenLocation _location;

		private readonly string _message;

		public Error(TokenLocation location, string message)
		{
			_location = location;
			_message = message;
		}

		public override string ToString()
		{
			return $"[Row: {_location.Row} Col: {_location.Column}] {_message}";
		}

		public int CompareTo(object? obj)
		{
			var error = obj as Error;
			return error == null ? throw new ArgumentException() : _location.CompareTo(error._location);
		}
	}
}
