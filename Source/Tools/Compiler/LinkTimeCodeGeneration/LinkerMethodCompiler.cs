﻿/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *
 */

using System;
using System.Collections.Generic;

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Linker;
using Mosa.Runtime.Vm;

namespace Mosa.Tools.Compiler.LinkTimeCodeGeneration
{
	sealed class LinkerMethodCompiler : MethodCompilerBase
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="LinkerMethodCompiler"/> class.
		/// </summary>
		/// <param name="compiler">The assembly compiler executing this method compiler.</param>
		/// <param name="method">The metadata of the method to compile.</param>
		/// <param name="instructionSet">The instruction set.</param>
		/// <exception cref="System.ArgumentNullException"><paramref name="compiler"/>, <paramref name="method"/> or <paramref name="instructionSet"/> is null.</exception>
		public LinkerMethodCompiler(AssemblyCompiler compiler, RuntimeMethod method, InstructionSet instructionSet) :
			base(compiler.Pipeline.FindFirst<IAssemblyLinker>(), compiler.Architecture, compiler.Assembly, method.DeclaringType, method)
		{
			InstructionSet = instructionSet;
			this.Pipeline.AddRange(new IMethodCompilerStage[] {
				new BasicBlockBuilderStage(),
				new PlatformStubStage(),
				new SimpleTraceBlockOrderStage(),
				new CodeGenerationStage(),
            });
			compiler.Architecture.ExtendMethodCompilerPipeline(this.Pipeline);
		}

		#endregion // Construction
	}
}
