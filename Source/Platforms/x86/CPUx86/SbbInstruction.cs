﻿/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using System;
using System.Collections.Generic;
using System.Text;

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.CompilerFramework.Operands;
using IR = Mosa.Runtime.CompilerFramework.IR;
using System.Diagnostics;

namespace Mosa.Platforms.x86.CPUx86
{
    /// <summary>
    /// Representations the x86 sbb instruction.
    /// </summary>
    public sealed class SbbInstruction : TwoOperandInstruction
	{
		#region Data Members

		private static readonly OpCode R_C = new OpCode(new byte[] { 0x81 }, 3);
        private static readonly OpCode M_C = new OpCode(new byte[] { 0x81 }, 3);
        private static readonly OpCode R_R = new OpCode(new byte[] { 0x19 });
        private static readonly OpCode M_R = new OpCode(new byte[] { 0x19 });
        private static readonly OpCode R_M = new OpCode(new byte[] { 0x1B });

		#endregion // Data Members

		#region Properties

		/// <summary>
		/// Gets the instruction latency.
		/// </summary>
		/// <value>The latency.</value>
		public override int Latency { get { return 1; } }

		#endregion // Properties

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="source"></param>
        /// <param name="third"></param>
        /// <returns></returns>
        protected override OpCode ComputeOpCode(Operand destination, Operand source, Operand third)
        {
            if ((destination is RegisterOperand) && (source is ConstantOperand)) return R_C;
            if ((destination is MemoryOperand) && (source is ConstantOperand)) return M_C;
            if ((destination is RegisterOperand) && (source is RegisterOperand)) return R_R;
            if ((destination is MemoryOperand) && (source is RegisterOperand)) return M_R;
            if ((destination is RegisterOperand) && (source is MemoryOperand)) return R_M;
            throw new ArgumentException(@"No opcode for operand type.");
        }

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Sbb(context);
		}

        #endregion // Methods
    }
}
