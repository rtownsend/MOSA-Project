/*
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

namespace Mosa.Runtime.Linker.Elf64.Sections
{
    /// <summary>
    /// 
    /// </summary>
    public class Elf64StringTableSection : Elf64Section
    {
        /// <summary>
        /// 
        /// </summary>
        protected static List<byte> stringTable = new List<byte>();

        /// <summary>
        /// Gets the length of the section in bytes.
        /// </summary>
        /// <value>The length of the section in bytes.</value>
        public override long Length
        {
            get
            {
                return stringTable.Count;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Elf64CodeSection"/> class.
        /// </summary>
        public Elf64StringTableSection()
            : base(Mosa.Runtime.Linker.SectionKind.Text, @".shstrtab", IntPtr.Zero)
        {
            header.Type = Elf64SectionType.StringTable;
            header.Flags = (Elf64SectionAttribute)0;
        }

        /// <summary>
        /// Writes the specified fs.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public override void Write(System.IO.BinaryWriter writer)
        {
            header.Offset = (uint)writer.BaseStream.Position;
            byte initial = (byte)'\0';
            writer.Write(initial);
            writer.Write(stringTable.ToArray());
        }

        /// <summary>
        /// Writes the _header.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public override void WriteHeader(System.IO.BinaryWriter writer)
        {
            Header.Size = (uint)Length;
            Header.Write(writer);
        }

        /// <summary>
        /// Adds the string.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static uint AddString(string text)
        {
            if (text.Length == 0)
                return (uint)(stringTable.Count + 1);

            uint index = (uint)stringTable.Count;

            foreach (char c in text)
                stringTable.Add((byte)c);
            stringTable.Add((byte)'\0');

            return index + 1;
        }
    }
}
