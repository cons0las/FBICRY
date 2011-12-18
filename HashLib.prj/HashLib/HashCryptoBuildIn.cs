﻿using System;
using System.Diagnostics;
using System.Security.Cryptography;

namespace HashLib
{
	class HashCryptoBuildIn : HashCrypto, ICryptoBuildIn
	{
		protected static readonly byte[] EMPTY = new byte[0];

		protected HashAlgorithm m_hashAlgorithm;

		public HashCryptoBuildIn(HashAlgorithm a_hashAlgorithm, int a_blockSize)
			: base(a_hashAlgorithm.HashSize / 8, a_blockSize)
		{
			Debug.Assert(a_hashAlgorithm != null);

			if(a_hashAlgorithm.CanReuseTransform == false)
				throw new NotImplementedException();
			if(a_hashAlgorithm.CanTransformMultipleBlocks == false)
				throw new NotImplementedException();

			m_hashAlgorithm = a_hashAlgorithm;
		}

		public override void Initialize()
		{
			m_hashAlgorithm.Initialize();
		}

		public override void TransformBytes(byte[] a_data, int a_index, int a_length)
		{
			Debug.Assert(a_data != null);
			Debug.Assert(a_index >= 0);
			Debug.Assert(a_length >= 0);
			Debug.Assert(a_index + a_length <= a_data.Length);

			m_hashAlgorithm.TransformBlock(a_data, a_index, a_length, null, 0);
		}

		public override HashResult TransformFinal()
		{
			m_hashAlgorithm.TransformFinalBlock(EMPTY, 0, 0);
			byte[] result = m_hashAlgorithm.Hash;

			Debug.Assert(result != null);
			Debug.Assert(result.Length == HashSize);

			Initialize();
			return new HashResult(result);
		}

		public virtual HMAC GetBuildHMAC()
		{
			throw new NotImplementedException();
		}
	}
}