using System.Reflection;
using BrainfuckDotNet.Abstract;
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
			var factory = new SyntaxTreeFactory();

			var sourceFile = args[0];
			var targetFileName = args[1];

			var lexerResult = parser.Parse(File.ReadAllText(sourceFile));
			var errors = lexerResult.GetErrors().ToList();

			if (errors.Count > 0)
			{
				foreach (var error in errors)
				{
					Console.WriteLine(error);	
				}

				return;
			}

			var syntaxTree = factory.Create(lexerResult);
			compiler.Compile(syntaxTree, targetFileName);

			var name = "runtimeconfig.json";
			
			var runtimeConfig = Assembly.GetCallingAssembly()
				.GetManifestResourceStream($"BrainfuckDotNet.Artifacts.{name}");
			
			if (runtimeConfig == null)
				throw new InvalidOperationException();

			var reader = new StreamReader(runtimeConfig);
			File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), $"{targetFileName}.{name}"), reader.ReadToEnd());
		}
	}
}