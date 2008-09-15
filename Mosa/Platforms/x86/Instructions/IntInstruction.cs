﻿using System;
using System.Collections.Generic;
using System.Text;

using Mosa.Runtime.CompilerFramework;
using IR = Mosa.Runtime.CompilerFramework.IR;

namespace Mosa.Platforms.x86
{
    sealed class IntInstruction : IR.OneOperandInstruction
    {
        public IntInstruction(Operand op) :
            base(op)
        {
        }

        public override string ToString()
        {
            return String.Format(@"x86 int {0}", this.Operand0);
        }

        protected override void Visit<ArgType>(IR.IIRVisitor<ArgType> visitor, ArgType arg)
        {
            IX86InstructionVisitor<ArgType> x86 = visitor as IX86InstructionVisitor<ArgType>;
            if (null != x86)
                x86.Int(this, arg);
            else
                base.Visit((IInstructionVisitor<ArgType>)visitor, arg);
        }
    }
}