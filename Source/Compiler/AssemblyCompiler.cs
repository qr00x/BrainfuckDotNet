using BrainFuckDotNet.Abstract;
using System.Reflection;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;

namespace BrainFuckDotNet.Compiler
{
	internal class AssemblyCompiler
	{
		private readonly ILEmitter _ilEmitter;

		public AssemblyCompiler(ILEmitter ilEmitter)
		{
			_ilEmitter = ilEmitter;
		}

		public void Compile(AbstractSyntaxTree syntaxTree, string outputFileName)
		{
			var ab = new PersistedAssemblyBuilder(new AssemblyName(outputFileName), typeof(object).Assembly);
			var module = ab.DefineDynamicModule($"{outputFileName}Module");
			var tb = module.DefineType("Program", TypeAttributes.Public | TypeAttributes.Class);
			var entryPoint = tb.DefineMethod("Main",
				MethodAttributes.HideBySig | MethodAttributes.Public | MethodAttributes.Static);

			_ilEmitter.Emit(syntaxTree, entryPoint.GetILGenerator());

			tb.CreateType();
			var metadataBuilder = ab.GenerateMetadata(out BlobBuilder ilStream, out BlobBuilder fieldData);

			var peBuilder = new ManagedPEBuilder(
				header: PEHeaderBuilder.CreateExecutableHeader(),
				metadataRootBuilder: new MetadataRootBuilder(metadataBuilder),
				ilStream: ilStream,
				mappedFieldData: fieldData,
				entryPoint: MetadataTokens.MethodDefinitionHandle(entryPoint.MetadataToken));

			var peBlob = new BlobBuilder();
			peBuilder.Serialize(peBlob);

			var fullFilePath = Path.Combine(Directory.GetCurrentDirectory(), $"{outputFileName}.exe");
			using var fileStream = new FileStream(fullFilePath, FileMode.Create, FileAccess.Write);
			peBlob.WriteContentTo(fileStream);
		}
	}
}
