using System.Reflection;
using BrainFuckDotNet.Compiler;
using BrainFuckDotNet.Parser;

namespace BrainFuckDotNet
{
	public static class Program
	{
		public static void Main(string[] args)
		{
			var parser = new BrainFuckParser();
			var compiler = new AssemblyCompiler(new ILEmitter());

			var sourceFile = args[0];
			var targetFileName = args[1];

			var preprocessor = new Preprocessor();
			var code = preprocessor.Process(File.ReadAllText(sourceFile));

			compiler.Compile(parser.Parse(code), targetFileName);

			var name = "runtimeconfig.json";
			
			var runtimeConfig = Assembly.GetCallingAssembly().GetManifestResourceStream($"BrainfuckDotNet.Artifacts.{name}");
			if (runtimeConfig == null)
				throw new InvalidOperationException();

			var reader = new StreamReader(runtimeConfig);
			File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), $"{targetFileName}.{name}"), reader.ReadToEnd());
		}
	}
}