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
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Runtime.CompilerFramework.Operands
{
    /// <summary>
    /// Abstract base class for IR instruction operands.
    /// </summary>
    public abstract class Operand : IEquatable<Operand>
    {
        #region Static data members

        /// <summary>
        /// Undefined operand constant.
        /// </summary>
        public static readonly Operand Undefined = null;

        #endregion // Static data members

        #region Data members

        /// <summary>
        /// The namespace of the operand.
        /// </summary>
        protected SigType _type;

        /// <summary>
        /// Holds a list of instructions, which define this operand.
        /// </summary>
        private List<int> _definitions;

        /// <summary>
        /// Holds a list of instructions, which use this operand.
        /// </summary>
        private List<int> _uses;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of <see cref="Operand"/>.
        /// </summary>
        /// <param name="type">The type of the operand.</param>
        protected Operand(SigType type)
        {
            _type = type;
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Returns a list of instructions, which use this operand.
        /// </summary>
        public List<int> Definitions
        {
            get
            {
                if (_definitions == null)
                    _definitions = new List<int>();

                return _definitions;
            }
        }

        /// <summary>
        /// Determines if the operand is a register.
        /// </summary>
        public virtual bool IsRegister { get { return false; } }

        /// <summary>
        /// Determines if the operand is a stack local variable.
        /// </summary>
        public virtual bool IsStackLocal { get { return false; } }

        /// <summary>
        /// Returns the stack type of the operand.
        /// </summary>
        public StackTypeCode StackType
        {
            get
            {
                return StackTypeFromSigType(_type);
            }
        }

        /// <summary>
        /// Retrieves the stack type From a sig type.
        /// </summary>
        /// <param name="type">The signature type to convert to a stack type code.</param>
        /// <returns>The equivalent stack type code.</returns>
        public static StackTypeCode StackTypeFromSigType(SigType type)
        {
            StackTypeCode result = StackTypeCode.Unknown;
            switch (type.Type) {
                case CilElementType.Void:
                    break;

                case CilElementType.Boolean: result = StackTypeCode.Int32; break;
                case CilElementType.Char: result = StackTypeCode.Int32; break;
                case CilElementType.I1: result = StackTypeCode.Int32; break;
                case CilElementType.U1: result = StackTypeCode.Int32; break;
                case CilElementType.I2: result = StackTypeCode.Int32; break;
                case CilElementType.U2: result = StackTypeCode.Int32; break;
                case CilElementType.I4: result = StackTypeCode.Int32; break;
                case CilElementType.U4: result = StackTypeCode.Int32; break;
                case CilElementType.I8: result = StackTypeCode.Int64; break;
                case CilElementType.U8: result = StackTypeCode.Int64; break;
                case CilElementType.R4: result = StackTypeCode.F; break;
                case CilElementType.R8: result = StackTypeCode.F; break;
                case CilElementType.I: result = StackTypeCode.N; break;
                case CilElementType.U: result = StackTypeCode.N; break;
                case CilElementType.Ptr: result = StackTypeCode.Ptr; break;
                case CilElementType.ByRef: result = StackTypeCode.Ptr; break;
                case CilElementType.Object: result = StackTypeCode.O; break;
                case CilElementType.String: result = StackTypeCode.O; break;
                case CilElementType.ValueType: result = StackTypeCode.O; break;
                case CilElementType.Type: result = StackTypeCode.O; break;
                case CilElementType.Class: result = StackTypeCode.O; break;

                default:
                    throw new NotSupportedException(@"Can't transform SigType to StackTypeCode.");
            }

            return result;
        }

        /// <summary>
        /// Sigs the type of the type From stack.
        /// </summary>
        /// <param name="typeCode">The type code.</param>
        /// <returns></returns>
        public static SigType SigTypeFromStackType(StackTypeCode typeCode)
        {
            SigType result = null;
            switch (typeCode) {
                case StackTypeCode.Int32: result = new SigType(CilElementType.I4); break;
                case StackTypeCode.Int64: result = new SigType(CilElementType.I8); break;
                case StackTypeCode.F: result = new SigType(CilElementType.R8); break;
                case StackTypeCode.O: result = new SigType(CilElementType.Object); break;
                case StackTypeCode.N: result = new SigType(CilElementType.I); break;
                default:
                    throw new NotSupportedException(@"Can't convert stack type code to SigType.");
            }
            return result;
        }

        /// <summary>
        /// Returns the type of the operand.
        /// </summary>
        public SigType Type { get { return _type; } }

        /// <summary>
        /// Returns a list of instructions, which use this operand.
        /// </summary>
        public List<int> Uses
        {
            get
            {
                if (_uses == null)
                    _uses = new List<int>();

                return _uses;
            }
        }

        #endregion // Properties

        #region Methods

        /// <summary>
        /// Replaces this operand in all uses and defs with the given operand.
        /// </summary>
        /// <param name="replacement">The replacement operand.</param>
        /// <param name="instructionSet">The instruction set.</param>
        public void Replace(Operand replacement, InstructionSet instructionSet)
        {
            int opIdx;

            // Iterate all definition sites first
            foreach (int index in Definitions) {
                Context def = new Context(instructionSet, index);
                opIdx = 0;
                foreach (Operand r in def.Results) {
                    // Is this the operand?
                    if (ReferenceEquals(r, this))
                        def.SetResult(opIdx, replacement);

                    opIdx++;
                }
            }

            // Iterate all use sites
            foreach (int index in Uses) {
                Context instr = new Context(instructionSet, index);
                opIdx = 0;
                foreach (Operand r in instr.Operands) {
                    // Is this the operand?
                    if (ReferenceEquals(r, this))
                        instr.SetOperand(opIdx, replacement);

                    opIdx++;
                }
            }
        }

        #endregion // Methods

        #region Object Overrides

        /// <summary>
        /// Returns a string representation of <see cref="Operand"/>.
        /// </summary>
        /// <returns>A string representation of the operand.</returns>
        public override string ToString()
        {
            return String.Format("[Type: {0}]", _type);
        }

        #endregion // Object Overrides

        #region IEquatable<Operand> Members

        /// <summary>
        /// Compares with the given operand for equality.
        /// </summary>
        /// <param name="other">The other operand to compare with.</param>
        /// <returns>The return value is true if the operands are equal; false if not.</returns>
        public abstract bool Equals(Operand other);

        #endregion // IEquatable<Operand> Members
    }
}


