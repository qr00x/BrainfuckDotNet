using System;
using System.Collections.Generic;
using System.Text;

namespace BrainFuckDotNet.Parser
{
	internal class Preprocessor
	{
		public string Process(string code)
		{
			var sb = new StringBuilder(code);
			sb.Replace("\t", "");
			sb.Replace("\r\n", "");
			sb.Replace(" ", "");
			return sb.ToString();
		}
	}
}
