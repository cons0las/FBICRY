﻿using System;
using System.Diagnostics;
using System.Security.Cryptography;

namespace HashLib
{
	class HMACBuildInAdapter : HMACBase
	{
		protected static readonly byte[] EMPTY = new byte[0];

		protected HMAC m_hmac;

		public HMACBuildInAdapter(HMAC a_hmac, int a_blockSize)
			: base(a_hmac.HashSize / 8, a_blockSize)
		{
			Debug.Assert(a_hmac != null);

			m_hmac = a_hmac;
		}

		public override string Name
		{
			get { return String.Format("{0}({1})", GetType().Name, m_hmac.GetType().Name); }
		}

		public override void Initialize()
		{
			m_hmac.Initialize();
			m_hmac.Key = Key;
			m_bTransforming = false;
		}

		public override HashResult TransformFinal()
		{
			if(!m_bTransforming)
			{
				Initialize();
				m_bTransforming = true;
			}

			m_hmac.TransformFinalBlock(EMPTY, 0, 0);
			byte[] result = m_hmac.Hash;

			Debug.Assert(result != null);
			Debug.Assert(result.Length == HashSize);

			Initialize();
			return new HashResult(result);
		}

		public override void TransformBytes(byte[] a_data, int a_index, int a_length)
		{
			Debug.Assert(a_data != null);
			Debug.Assert(a_index >= 0);
			Debug.Assert(a_length >= 0);
			Debug.Assert(a_index + a_length <= a_data.Length);

			if(!m_bTransforming)
			{
				Initialize();
				m_bTransforming = true;
			}

			m_hmac.TransformBlock(a_data, a_index, a_length, null, 0);
		}
	}
}