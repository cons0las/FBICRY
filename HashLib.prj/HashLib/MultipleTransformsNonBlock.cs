﻿using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace HashLib
{
	abstract class MultipleTransformNonBlock : Hash
	{
		private readonly List<ArraySegment<byte>> m_list = new List<ArraySegment<byte>>();

		public MultipleTransformNonBlock(int a_hashSize, int a_blockSize)
			: base(a_hashSize, a_blockSize)
		{
		}

		public override void Initialize()
		{
			m_list.Clear();
		}

		public override void TransformBytes(byte[] a_data, int a_index, int a_length)
		{
			Debug.Assert(a_data != null);
			Debug.Assert(a_index >= 0);
			Debug.Assert(a_length >= 0);
			Debug.Assert(a_index + a_length <= a_data.Length);

			m_list.Add(new ArraySegment<byte>(a_data, a_index, a_length));
		}

		public override HashResult TransformFinal()
		{
			HashResult result = ComputeBytes(Aggregate());
			Initialize();
			return result;
		}

		private byte[] Aggregate()
		{
			Debug.Assert(m_list != null);
			m_list.ForEach(seg => Debug.Assert(seg != null));

			int sum = 0;
			foreach(var seg in m_list)
				sum += seg.Count;

			var res = new byte[sum];

			int index = 0;

			foreach(var seg in m_list)
			{
				Array.Copy(seg.Array, seg.Offset, res, index, seg.Count);
				index += seg.Count;
			}

			return res;
		}
	}
}