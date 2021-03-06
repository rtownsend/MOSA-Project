/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Alex Lyman <mail.alex.lyman@gmail.com>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.CompilerFramework.CIL;
using Mosa.Runtime.CompilerFramework.IR;
using Mosa.Runtime.Linker;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Vm;

namespace Mosa.Tools.Compiler
{
    /// <summary>
    /// Specializes <see cref="MethodCompilerBase"/> for AOT purposes.
    /// </summary>
    public sealed class AotMethodCompiler : MethodCompilerBase
    {
        #region Data Members

        /// <summary>
        /// Holds the aot compiler, which started this method compiler.
        /// </summary>
        private AotCompiler aotCompiler;

        #endregion // Data Members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="AotMethodCompiler"/> class.
        /// </summary>
        /// <param name="compiler">The AOT assembly compiler.</param>
        /// <param name="type">The type.</param>
        /// <param name="method">The method.</param>
        public AotMethodCompiler(AotCompiler compiler, RuntimeType type, RuntimeMethod method)
            : base(compiler.Pipeline.FindFirst<IAssemblyLinker>(), compiler.Architecture, compiler.Assembly, type, method)
        {
            aotCompiler = compiler;
            Pipeline.AddRange(new IMethodCompilerStage[] {
				new DecodingStage(),
				new InstructionLogger(),
				new BasicBlockBuilderStage(),
				new InstructionLogger(),
				new OperandDeterminationStage(),
				new InstructionLogger(),
				new CILTransformationStage(),
				new InstructionLogger(),
				//InstructionStatisticsStage.Instance,
				//new DominanceCalculationStage(),
				//new InstructionLogger(),
				//new EnterSSA(),
				//new InstructionLogger(),
				//new ConstantPropagationStage(),
				//new InstructionLogger(),
				//new ConstantFoldingStage(),
				//new StrengthReductionStage(),
				//new InstructionLogger(),
				//new LeaveSSA(),
				//new InstructionLogger(),
				new StackLayoutStage(),
				new PlatformStubStage(),
				//new InstructionLogger(),
				//new BlockReductionStage(),
				new LoopAwareBlockOrderStage(),
				new InstructionLogger(),
				//new SimpleTraceBlockOrderStage(),
				//new ReverseBlockOrderStage(),	
				//new LocalCSE(),
				new CodeGenerationStage(),
            });
        }

        #endregion // Construction

        #region MethodCompilerBase Overrides

        /// <summary>
        /// Called after the method compiler has finished compiling the method.
        /// </summary>
        protected override void EndCompile()
        {
            // If we're compiling a type initializer, run it immediately.
            MethodAttributes attrs = MethodAttributes.SpecialName | MethodAttributes.RTSpecialName | MethodAttributes.Static;
            if ((Method.Attributes & attrs) == attrs && Method.Name == ".cctor")
            {
                TypeInitializers.TypeInitializerSchedulerStage tiss = aotCompiler.Pipeline.FindFirst<TypeInitializers.TypeInitializerSchedulerStage>();
                tiss.Schedule(Method);
            }

            base.EndCompile();
        }

        #endregion // MethodCompilerBase Overrides
    }
}
