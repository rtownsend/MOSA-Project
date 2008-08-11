/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Mosa.Runtime.CompilerFramework.IL
{
    public class DupInstruction : UnaryInstruction
    {
        #region Construction

        public DupInstruction(OpCode code)
            : base(code, 2)
        {
            Debug.Assert(OpCode.Dup == code);
            if (OpCode.Dup != code)
                throw new ArgumentException(@"Invalid opcode.", @"code");
        }

        #endregion // Construction

        #region Methods

        public override void Validate(MethodCompilerBase compiler)
        {
            base.Validate(compiler);
            Operand[] ops = this.Operands;
            SetResult(0, ops[0]);
            SetResult(1, ops[0]);
        }

        public sealed override void Visit(IILVisitor visitor)
        {
            visitor.Dup(this);
        }

        #endregion // Methods
    }
}