using BrainFuckDotNet.Abstract;
using System.Reflection;
using System.Reflection.Emit;

namespace BrainFuckDotNet.Compiler
{
	internal class ILEmitter
	{
		public void Emit(AbstractSyntaxTree syntaxTree, ILGenerator il)
		{
			il.DeclareLocal(typeof(byte[]));
			il.DeclareLocal(typeof(int));

			il.Emit(OpCodes.Ldc_I4, 30_000);
			il.Emit(OpCodes.Newarr, typeof(byte));
			il.Emit(OpCodes.Stloc_0);

			il.Emit(OpCodes.Ldc_I4_0);
			il.Emit(OpCodes.Stloc_1);

			foreach (var node in syntaxTree.Nodes)
			{
				EmitNode(il, node);
			}

			il.Emit(OpCodes.Ret);

		}

		private void EmitNode(ILGenerator il, ISyntaxNode node)
		{
			if (node.Type == NodeType.Instruction)
			{
				EmitInstruction(il, (Instraction)node);
			}
			else if (node.Type == NodeType.Loop)
			{
				EmitLoop(il, (Loop)node);
			}
		}

		private void EmitInstruction(ILGenerator il, Instraction instraction)
		{
			switch (instraction.OpCode)
			{
				case Operation.Inc:
					EmitInc(il);
					return;
				case Operation.Dec:
					EmitDec(il);
					return;
				case Operation.Right:
					EmitRight(il);
					return;
				case Operation.Left:
					EmitLeft(il);
					return;
				case Operation.In:
					EmitIn(il);
					return;
				case Operation.Out:
					EmitOut(il);
					return;
			}
		}

		private void EmitLoop(ILGenerator il, Loop loop)
		{
			var label = il.DefineLabel();
			var conditionCheckLabel = il.DefineLabel();
			il.Emit(OpCodes.Br, conditionCheckLabel);
			il.MarkLabel(label);

			foreach (var node in loop.Node)
			{
				EmitNode(il, node);
			}

			il.MarkLabel(conditionCheckLabel);
			il.Emit(OpCodes.Ldloc_0);
			il.Emit(OpCodes.Ldloc_1);
			il.Emit(OpCodes.Ldelem_U1);
			il.Emit(OpCodes.Brtrue_S, label);
		}

		private void EmitInc(ILGenerator il)
		{
			il.Emit(OpCodes.Ldloc_0);
			il.Emit(OpCodes.Ldloc_1);

			il.Emit(OpCodes.Ldloc_0);
			il.Emit(OpCodes.Ldloc_1);
			il.Emit(OpCodes.Ldelem_U1);

			il.Emit(OpCodes.Ldc_I4_1);
			il.Emit(OpCodes.Add);
			il.Emit(OpCodes.Conv_U1);
			il.Emit(OpCodes.Stelem_I1);
		}

		private void EmitDec(ILGenerator il)
		{
			il.Emit(OpCodes.Ldloc_0);
			il.Emit(OpCodes.Ldloc_1);

			il.Emit(OpCodes.Ldloc_0);
			il.Emit(OpCodes.Ldloc_1);
			il.Emit(OpCodes.Ldelem_U1);

			il.Emit(OpCodes.Ldc_I4_1);
			il.Emit(OpCodes.Sub);
			il.Emit(OpCodes.Conv_U1);
			il.Emit(OpCodes.Stelem_I1);
		}

		private void EmitLeft(ILGenerator il)
		{
			il.Emit(OpCodes.Ldloc_1);
			il.Emit(OpCodes.Ldc_I4_1);
			il.Emit(OpCodes.Sub);

			il.Emit(OpCodes.Stloc_1);
		}

		private void EmitRight(ILGenerator il)
		{
			il.Emit(OpCodes.Ldloc_1);
			il.Emit(OpCodes.Ldc_I4_1);
			il.Emit(OpCodes.Add);

			il.Emit(OpCodes.Stloc_1);
		}

		private void EmitIn(ILGenerator il)
		{
			il.Emit(OpCodes.Ldloc_0);
			il.Emit(OpCodes.Ldloc_1);

			var read = typeof(Console).GetMethod(nameof(Console.Read));
			if (read == null)
				throw new InvalidOperationException();
				
			il.Emit(OpCodes.Call, read);
			il.Emit(OpCodes.Stelem_I1);
		}

		private void EmitOut(ILGenerator il)
		{
			il.Emit(OpCodes.Ldloc_0);
			il.Emit(OpCodes.Ldloc_1);
			il.Emit(OpCodes.Ldelem_U1);
			
			var write = typeof(Console).GetMethod(nameof(Console.Write), BindingFlags.Public | BindingFlags.Static, [typeof(char)]);
			if (write == null)
				throw new InvalidOperationException();
				
			il.Emit(OpCodes.Call, write);
			il.Emit(OpCodes.Nop);
		}
	}
}