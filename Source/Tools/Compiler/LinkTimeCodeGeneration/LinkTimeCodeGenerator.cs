/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections.Generic;

using Mosa.Runtime.CompilerFramework;

namespace Mosa.Tools.Compiler.LinkTimeCodeGeneration
{
	/// <summary>
	/// Performs link time code generation for various parts of mosacl.
	/// </summary>
	public sealed class LinkTimeCodeGenerator
	{
		#region Data Members

		/// <summary>
		/// Holds the compiler generated type.
		/// </summary>
		private static CompilerGeneratedType compilerGeneratedType;

		#endregion // Data Members

		#region Methods

		/// <summary>
		/// Link time code generator used to compile dynamically created methods during link time.
		/// </summary>
		/// <param name="compiler">The assembly compiler used to compile this method.</param>
		/// <param name="methodName">The name of the created method.</param>
		/// <returns></returns>
		/// <exception cref="System.ArgumentNullException"><paramref name="compiler"/> or <paramref name="methodName"/>  is null.</exception>
		/// <exception cref="System.ArgumentException"><paramref name="methodName"/> is invalid.</exception>
		public static CompilerGeneratedMethod Compile(AssemblyCompiler compiler, string methodName)
		{
			return Compile(compiler, methodName, null);
		}

		/// <summary>
		/// Link time code generator used to compile dynamically created methods during link time.
		/// </summary>
		/// <param name="compiler">The assembly compiler used to compile this method.</param>
		/// <param name="methodName">The name of the created method.</param>
		/// <param name="instructionSet">The instruction set.</param>
		/// <returns></returns>
		/// <exception cref="System.ArgumentNullException"><paramref name="compiler"/>, <paramref name="methodName"/> or <paramref name="instructionSet"/> is null.</exception>
		/// <exception cref="System.ArgumentException"><paramref name="methodName"/> is invalid.</exception>
		public static CompilerGeneratedMethod Compile(AssemblyCompiler compiler, string methodName, InstructionSet instructionSet)
		{
			if (compiler == null)
				throw new ArgumentNullException(@"compiler");
			if (methodName == null)
				throw new ArgumentNullException(@"methodName");
			if (methodName.Length == 0)
				throw new ArgumentException(@"Invalid method name.");

			// Create the type if we need to.
			if (compilerGeneratedType == null)
				compilerGeneratedType = new CompilerGeneratedType(compiler.Assembly, @"Mosa.Tools.Compiler", @"LinkerGenerated");

			// Create the method
			// HACK: <$> prevents the method from being called from CIL
			CompilerGeneratedMethod method = new CompilerGeneratedMethod(compiler.Assembly, "<$>" + methodName, compilerGeneratedType);
			compilerGeneratedType.Methods.Add(method);

			LinkerMethodCompiler methodCompiler = new LinkerMethodCompiler(compiler, method, instructionSet);
			methodCompiler.Compile();
			return method;
		}

		#endregion // Methods
	}
}
