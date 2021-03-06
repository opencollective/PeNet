﻿using System;
using System.Linq;
using System.Text;
using PeNet.Utilities;

namespace PeNet.Structures
{
    /// <summary>
    ///     The IMAGE_DEBUG_DIRECTORY hold debug information
    ///     about the PE file.
    /// </summary>
    public class IMAGE_DEBUG_DIRECTORY : AbstractStructure
    {
        private CvInfoPdb70 _cvInfoPdb70 = null;

        /// <summary>
        ///     Create a new IMAGE_DEBUG_DIRECTORY object.
        /// </summary>
        /// <param name="buff">PE binary as byte array.</param>
        /// <param name="offset">Offset to the debug struct in the binary.</param>
        public IMAGE_DEBUG_DIRECTORY(byte[] buff, uint offset)
            : base(buff, offset)
        {
        }

        /// <summary>
        ///     Characteristics of the debug information.
        /// </summary>
        public uint Characteristics
        {
            get => Buff.BytesToUInt32(Offset);
            set => Buff.SetUInt32(Offset, value);
        }

        /// <summary>
        ///     Time and date stamp
        /// </summary>
        public uint TimeDateStamp
        {
            get => Buff.BytesToUInt32(Offset + 0x4);
            set => Buff.SetUInt32(Offset + 0x4, value);
        }

        /// <summary>
        ///     Major Version.
        /// </summary>
        public ushort MajorVersion
        {
            get => Buff.BytesToUInt16(Offset + 0x8);
            set => Buff.SetUInt16(Offset + 0x8, value);
        }

        /// <summary>
        ///     Minor Version.
        /// </summary>
        public ushort MinorVersion
        {
            get => Buff.BytesToUInt16(Offset + 0xa);
            set => Buff.SetUInt16(Offset + 0xa, value);
        }

        /// <summary>
        ///     Type
        ///     1: Coff
        ///     2: CV-PDB
        ///     9: Borland
        /// </summary>
        public uint Type
        {
            get => Buff.BytesToUInt32(Offset + 0xc);
            set => Buff.SetUInt32(Offset + 0xc, value);
        }

        /// <summary>
        ///     Size of data.
        /// </summary>
        public uint SizeOfData
        {
            get => Buff.BytesToUInt32(Offset + 0x10);
            set => Buff.SetUInt32(Offset + 0x10, value);
        }

        /// <summary>
        ///     Address of raw data.
        /// </summary>
        public uint AddressOfRawData
        {
            get => Buff.BytesToUInt32(Offset + 0x14);
            set => Buff.SetUInt32(Offset + 0x14, value);
        }

        /// <summary>
        ///     Pointer to raw data.
        /// </summary>
        public uint PointerToRawData
        {
            get => Buff.BytesToUInt32(Offset + 0x18);
            set => Buff.SetUInt32(Offset + 0x18, value);
        }

        /// <summary>
        /// PDB information if the "Type" is IMAGE_DEBUG_TYPE_CODEVIEW.
        /// </summary>
        public CvInfoPdb70 CvInfoPdb70
        {
            get
            {
                if (Type != 2) // Type IMAGE_DEBUG_TYPE_CODEVIEW
                    return null;

                _cvInfoPdb70 ??= new CvInfoPdb70(
                    Buff, 
                    PointerToRawData);

                return _cvInfoPdb70;
            }
        }
    }
}