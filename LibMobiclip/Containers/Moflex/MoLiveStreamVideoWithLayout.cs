﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using LibMobiclip.Utils;

namespace LibMobiclip.Containers.Moflex
{
	public class MoLiveStreamVideoWithLayout : MoLiveStreamVideo
	{
		public uint ImageLayout;
		public uint ImageRotation;

        public override int Read(byte[] Data, int Offset)
        {
            if (Data == null || Data.Length == 0) return -1;
            int offset = Offset;
            StreamIndex = Data[offset++];
            if (offset >= Data.Length) return -1;
            CodecId = Data[offset++];
            if (Data.Length - offset < 0xA) return -1;
            FpsRate = IOUtil.ReadU16BE(Data, offset);
            FpsScale = IOUtil.ReadU16BE(Data, offset + 2);
            Width = IOUtil.ReadU16BE(Data, offset + 4);
            Height = IOUtil.ReadU16BE(Data, offset + 6);
            PelRatioRate = Data[offset + 8];
            PelRatioRate = Data[offset + 9];
            offset += 0xA;
            if (offset >= Data.Length) return -1;
            ImageLayout = (uint)(Data[offset] & 0xF);
            ImageRotation = (uint)(Data[offset] >> 4);
            offset++;
            return offset;
        }

        public override void Write(Stream Destination)
        {
            base.Write(Destination);
            Destination.WriteByte((byte)((ImageLayout & 0xF) | ((ImageRotation & 0xF) << 4)));
        }
	}
}