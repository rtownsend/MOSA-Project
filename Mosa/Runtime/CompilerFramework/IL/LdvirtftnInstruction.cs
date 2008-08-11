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
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Vm;

namespace Mosa.Runtime.CompilerFramework.IL
{
    public class LdvirtftnInstruction : UnaryInstruction
    {
        #region Data members

        /// <summary>
        /// Member reference of the loaded function.
        /// </summary>
        private RuntimeMethod _function;

        #endregion // Data members

        #region Construction

        public LdvirtftnInstruction(OpCode code)
            : base(code, 1)
        {
            Debug.Assert(OpCode.Ldvirtftn == code);
            if (OpCode.Ldvirtftn != code)
                throw new ArgumentException(@"Invalid opcode.", @"code");
        }

        #endregion // Construction

        #region Methods

        public override void Decode(IInstructionDecoder decoder)
        {
            // Decode the base class
            base.Decode(decoder);

            TokenTypes token = decoder.DecodeToken();
            throw new NotImplementedException();
//            _function = MetadataMemberReference.FromToken(decoder.Metadata, token);

            // Setup the result
            // FIXME: Function ptr
//            _results[0] = CreateResultOperand(NativeTypeReference.NativeInt);
        }

        public sealed override void Visit(IILVisitor visitor)
        {
            visitor.Ldvirtftn(this);
        }

        #endregion // Methods
    }
}